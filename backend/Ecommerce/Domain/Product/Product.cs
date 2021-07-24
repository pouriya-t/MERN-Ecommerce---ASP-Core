using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;


namespace Domain.Product
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }



        [Required(ErrorMessage = "Please enter product price")]
        [Range(typeof(double), "0", "99999", ErrorMessage = "Product stock cannot exceed 5 digit")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Please enter product description")]
        public string Description { get; set; }

        public double Ratings { get; set; } = 0;

        public ICollection<Image> Images { get; set; }

        [Required(ErrorMessage = "Please select category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please product seller")]
        public string Seller { get; set; }

        [Required(ErrorMessage = "Please enter product stock")]
        [Range(typeof(double), "0", "99999", ErrorMessage = "Product stock cannot exceed 5 digit")]
        public double Stock { get; set; }

        public int NumOfReviews { get; set; } = 0;

        [Required]
        public ICollection<Review> Reviews { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
