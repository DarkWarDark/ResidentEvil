using ResidentEvil.Models;

namespace ResidentEvil.Services
{
    public interface IEntryService
    {
        // Variantele noi pentru GamesController
        Task<IEnumerable<Entry>> GetAllEntriesAsync();
        Task<Entry> GetEntryByIdAsync(int id);
        Task CreateEntryAsync(Entry entry);
        Task UpdateEntryAsync(Entry entry);
        Task DeleteEntryAsync(int id);
        Task<IEnumerable<Entry>> SearchEntriesAsync(string term);
        Task<IEnumerable<GalleryItem>> GetAllGalleryItemsAsync();
        Task AddGalleryItemAsync(GalleryItem item);
        Task DeleteGalleryItemAsync(int id);
        Task UpdateGalleryItemAsync(GalleryItem item);

        // Variantele vechi pentru HomeController (să dispară erorile CS1061)
        IEnumerable<Entry> GetAllEntries();
        void CreateEntry(Entry entry);
    }
}