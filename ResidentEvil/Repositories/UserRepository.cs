using ResidentEvil.Data;
using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    // Trebuie să moștenească IRepositoryBase<Users>
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public UserRepository(ApplicationDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}