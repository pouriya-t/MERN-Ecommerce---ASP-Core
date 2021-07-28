using System.ComponentModel.DataAnnotations;

namespace Domain.Models.User
{
    public class Avatar
    {
        [Required]
        public string PublicId { get; set; }
        
        [Required]
        public string Url { get; set; }
    }
}