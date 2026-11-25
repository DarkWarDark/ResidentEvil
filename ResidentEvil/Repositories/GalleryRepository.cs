using Microsoft.EntityFrameworkCore;
using ResidentEvil.Data;
using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    public class GalleryRepository : RepositoryBase<GalleryItem>, IGalleryRepository
    {
        public GalleryRepository(ApplicationDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Task<List<GalleryItem>> GetAllWithEntryAsync() =>
            FindAll().Include(g => g.Entry).ToListAsync();
    }
}