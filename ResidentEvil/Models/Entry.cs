namespace ResidentEvil.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string? PageName { get; set; }
        public string Category { get; set; } // game/character/location

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}

