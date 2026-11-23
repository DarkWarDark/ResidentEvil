using ResidentEvil.Data;
using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(ApplicationDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Game> GetGamesBySearch(string searchTerm)
        {
            return FindByCondition(g => g.Name.Contains(searchTerm)).ToList(); 
        }
    }
}