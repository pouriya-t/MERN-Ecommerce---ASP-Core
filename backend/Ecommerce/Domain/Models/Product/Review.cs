using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Product
{
    public class Review
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
