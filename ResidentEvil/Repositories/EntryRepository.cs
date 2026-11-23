using ResidentEvil.Data;
using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(ApplicationDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}