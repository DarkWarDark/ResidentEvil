using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResidentEvil.Models
{
    public class GalleryItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string? Caption { get; set; }

        // Legătura cu tabelul principal (Foreign Key)
        public int EntryId { get; set; }

        [ForeignKey("EntryId")]
        public virtual Entry? Entry { get; set; }
    }
}