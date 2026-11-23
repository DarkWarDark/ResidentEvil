using ResidentEvil.Models;
using ResidentEvil.Repositories;

namespace ResidentEvil.Repositories
{

    public interface IGameRepository : IRepositoryBase<Game>
    {
        IEnumerable<Game> GetGamesBySearch(string searchTerm);
    }
}