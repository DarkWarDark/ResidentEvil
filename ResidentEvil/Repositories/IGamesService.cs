using ResidentEvil.Models;

public interface IGamesService
{
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<Game> GetGameByIdAsync(int id);
    void CreateGame(Game game);
    void UpdateGame(Game game);
    void DeleteGame(Game game);
    IEnumerable<Game> SearchGames(string term);
}