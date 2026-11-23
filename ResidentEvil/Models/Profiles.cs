using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; 

namespace ResidentEvil.Models
{
    public class Profiles
    {
        [Key]
        public int Id { get; set; }

        public string? Bio { get; set; }
        public string? FavoriteGame { get; set; }
        public string? AvatarUrl { get; set; }

       
        [Required]
        public string UsersId { get; set; }

        [ForeignKey("UsersId")]
        public virtual IdentityUser? User { get; set; }
    }
}