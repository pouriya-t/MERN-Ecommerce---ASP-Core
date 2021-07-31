using Domain.Models.Product;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Context
{

    public class SeedData
    {
        public static string User = "User";
        public static string Admin = "Admin";

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Admin))
            {
                await roleManager.CreateAsync(new ApplicationRole(Admin));
                await roleManager.CreateAsync(new ApplicationRole(User));
            }


            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com"
                    },
                    new ApplicationUser
                    {
                        UserName = "user@gmail.com",
                        Email = "user@gmail.com"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Admin123*");
                    if (user.UserName == "admin@gmail.com")
                    {
                        await userManager.AddToRoleAsync(user, Admin);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, User);
                    }
                }
            }
        }

        public static void SeedProducts(IMongoCollection<Product> productCollection
                                ,IMongoCollection<ApplicationUser> userCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                var user = userCollection.Find(u => u.Email == "admin@gmail.com").FirstOrDefault();
                productCollection.InsertManyAsync(GetPreconfigureProducts(user.Id.ToString()));
            }
        }

        private static IEnumerable<Product> GetPreconfigureProducts(string userId)
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "SanDisk Ultra 128GB SDXC UHS-I Memory Card up to 80MB/s",
                    Price = 45.89,
                    Description = "Ultra-fast cards (2) to take better pictures and Full HD videos (1) with your compact to mid-range point-and-shoot cameras and camcorders. With SanDisk Ultra SDXC UHS-I cards you’ll benefit from faster downloads, high capacity, and better performance to capture and store 128GB (5) of high quality pictures and Full HD video (1). Take advantage of ultra-fast read speeds of up to 80MB/s (3) to save time moving photos and videos from the card to your computer. From a leader in flash memory storage, SanDisk Ultra SDXC UHS-I cards are compatible with SDHC and SDXC digital devices, and come with a 10-year limited warranty (6).",
                    Ratings = 4.5,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            PublicId = "products/dsvbpny402gelwugv2le",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1608062030/products/dsvbpny402gelwugv2le.jpg"
                        },
                    },
                    Category = "Electronics",
                    Seller = "Ebay",
                    Stock = 50,
                    NumOfReviews = 32,
                    User = userId,
                    Reviews = new List<Review>{ }
                },
                new Product()
                {
                    Name = "CAN USB FD Adapter (GC-CAN-USB-FD)",
                    Price = 315.00,
                    Description = "Monitor a CAN network, write a CAN program and communicate with industrial, medical, automotive or other CAN based device. Connect CAN FD and CAN networks to a computer via USB with the CAN USB FD adapter.",
                    Ratings = 1.65,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            PublicId = "products/61oXGZ60GfL_fixco9",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1614877995/products/61oXGZ60GfL_fixco9.jpg"
                        },
                    },
                    Category = "Electronics",
                    Seller = "Amazon",
                    Stock = 0,
                    NumOfReviews = 2,
                    User = userId,
                    Reviews = new List<Review>{ }
                },
                new Product()
                {
                    Name = "CHARMOUNT Full Motion TV Wall Mount Swivel",
                    Price = 26.99,
                    Description = "CHARMOUNT TV MOUNT UNIVERSAL DESIGN - Has your TV been received? Tilted TV wall mount is for 26 - 55 TVs weight up to 88lbs 40 kg. Our tilt TV mount has a compatible faceplate that fits VESA 75X75mm (3x3). CHARMOUNT TV MOUNT UNIVERSAL DESIGN - Has your TV been received? Tilted TV wall mount is for 26 - 55 TVs weight up to 88lbs 40 kg. Our tilt TV mount has a compatible faceplate that fits VESA 75X75mm (3x3) CHARMOUNT TV MOUNT UNIVERSAL DESIGN - Has your TV been received? Tilted TV wall mount is for 26 - 55 TVs weight up to 88lbs 40 kg. Our tilt TV mount has a compatible faceplate that fits VESA 75X75mm (3x3).",
                    Ratings = 3.5,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            PublicId = "products/chairmount_nuubea",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1606231285/products/chairmount_nuubea.jpg"
                        },
                    },
                    Category = "Electronics",
                    Seller = "Ebay",
                    Stock = 1,
                    NumOfReviews = 12,
                    User = userId,
                    Reviews = new List<Review>{ }
                },
                new Product()
                {
                    Name = "Bose QuietComfort 35 II Wireless Bluetooth Headphones",
                    Price = 299.00,
                    Description = "What happens when you clear away the noisy distractions of the world? Concentration goes to the next level. You get deeper into your music, your work, or whatever you want to focus on. That’s the power of Bose QuietComfort 35 wireless headphones II. Put them on and get closer to what you’re most passionate about. And that’s just the beginning. QuietComfort 35 wireless headphones II are now enabled with Bose AR",
                    Ratings = 4.5,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            PublicId = "products/headphones_t2afnb",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1606231281/products/headphones_t2afnb.jpg"
                        },
                    },
                    Category = "Headphones",
                    Seller = "Amazon",
                    Stock = 11,
                    NumOfReviews = 112,
                    User = userId,
                    Reviews = new List<Review>{ }
                },
                new Product()
                    {
                    Name  = "Cable Boom Microphone - Volume Control for Playstation PS4 or Xbox",
                    Price = 27.99,
                    Description = "DESIGN INFO - 3.5mm male to 2.5mm male audio cable adapter with Upgraded Flexible, Detachable Boom Mic which also enables rotary Volume Control and Mute Switch. SteelFlex Arm for perfect microphone positioning. INPUT COMPATIBILITY - Devices supporting 3.5mm audio output such as gaming PS4 / Xbox One controller, PC, Laptop, iPhone and Android Phone.",
                    Ratings = 4.1,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            PublicId = "products/eahhtj1bkn1k9gjgd3hn",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1606233125/products/eahhtj1bkn1k9gjgd3hn.jpg"
                        },
                        new Image
                        {
                            PublicId = "products/headphones_t2afnb",
                            Url = "https://res.cloudinary.com/bookit/image/upload/v1606231281/products/headphones_t2afnb.jpg"
                        },
                    },
                    Category = "Accessories",
                    Seller =  "Amazon",
                    Stock = 1123,
                    NumOfReviews = 6,
                    User = userId,
                    Reviews = new List<Review>{ }
                },
            };
        }
    }
}

