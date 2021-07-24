using API.Context;
using Application.Products.Commands;
using Domain.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Utility.DI
{
    public static class ConfigureDIServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {

            services.AddControllers()
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblyContaining<Edit>();
                });

            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            var assembly = AppDomain.CurrentDomain.Load("Application");
            services.AddMediatR(assembly);

            return services;
        }
    }
}
