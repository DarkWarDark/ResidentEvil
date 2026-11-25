using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    public interface IGalleryRepository : IRepositoryBase<GalleryItem>
    {
        Task<List<GalleryItem>> GetAllWithEntryAsync();
    }
}