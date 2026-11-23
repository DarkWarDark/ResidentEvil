using ResidentEvil.Models;
using ResidentEvil.Repositories;

namespace ResidentEvil.Services
{
    public class GamesService : IGamesService
    {
        private readonly IRepositoryWrapper _repository;

        public GamesService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // Folosim Task.FromResult pentru a simula async dacă Repository-ul nu este async
        public async Task<IEnumerable<Game>> GetAllGamesAsync() =>
            _repository.Game.FindAll().ToList();

        public async Task<Game> GetGameByIdAsync(int id) =>
            _repository.Game.FindByCondition(g => g.Id == id).FirstOrDefault();

        public void CreateGame(Game game)
        {
            _repository.Game.Create(game);
            _repository.Save();
        }

        public void UpdateGame(Game game)
        {
            _repository.Game.Update(game);
            _repository.Save();
        }

        public void DeleteGame(Game game)
        {
            _repository.Game.Delete(game);
            _repository.Save();
        }

        // AICI ERA PROBLEMA: Ne asigurăm că metoda de căutare folosește "Name"
        public IEnumerable<Game> SearchGames(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _repository.Game.FindAll().ToList();

            // Dacă GetGamesBySearch din Repository dă eroare, folosim varianta asta:
            return _repository.Game.FindByCondition(g => g.Name.Contains(term)).ToList();
        }
    }
}