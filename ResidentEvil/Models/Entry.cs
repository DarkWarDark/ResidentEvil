namespace ResidentEvil.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string? PageName { get; set; }
        public string Category { get; set; } // game/caracter/locatie
    }
}

