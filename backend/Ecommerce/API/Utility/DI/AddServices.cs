using Application.Products.Commands;
using AspNetCore.Identity.Mongo;
using Domain.Interfaces.Repositories;
using Domain.Interfaces;
using Domain.Models.User;
using FluentValidation.AspNetCore;
using Persistence.Context;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using Domain.Interfaces.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Security.JwtGenerator;
using Domain.Interfaces.UserAccessor;
using Infrastructure.Security.UserAccessor;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.EmailService;
using Infrastructure.EmailService;
using Infrastructure.Helpers;
using Domain.Interfaces.PhotoAccessor;
using Infrastructure.PhotoAccessor;

namespace API.Utility.DI
{
    public static class ConfigureDIServices
    {


        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers()
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<EditProduct>();
                });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000").AllowCredentials();
                });
            });

            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole, ObjectId>(identity =>
             {
                 identity.Password.RequiredLength = 8;
                 //identity.Tokens.PasswordResetTokenProvider.
                 // other options
             },
            mongo =>
            {
                mongo.ConnectionString = "mongodb://127.0.0.1:27017/Ecommerce";
                // other options
            }).AddDefaultTokenProviders();


            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
            
            

            // add services settings for jwt bearer without cookie

            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.SaveToken = true;
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = configuration["JWT:ValidAudience"],
            //        ValidIssuer = configuration["JWT:ValidIssuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),
            //    };
            //});

            // add services settings for jwt with cookie

            services.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"]; // this token set in UserController
                        return Task.CompletedTask;
                    }

                };
            });



            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));


            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.AddSingleton<IEmailSender, EmailSender>();

            var assembly = AppDomain.CurrentDomain.Load("Application");
            services.AddMediatR(assembly);

            return services;
        }
    }
}
