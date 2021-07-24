using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product
{
    public class Image
    {
        [Required]
        public string PublicId { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
