using ResidentEvil.Models;
using ResidentEvil.Data;
namespace ResidentEvil.Repositories
{
    public class ProfilesRepository : RepositoryBase<Profiles>, IProfilesRepository
    {
        public ProfilesRepository(ApplicationDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}