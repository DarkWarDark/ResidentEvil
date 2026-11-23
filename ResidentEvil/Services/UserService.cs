using ResidentEvil.Models;
using ResidentEvil.Repositories;
using ResidentEvil.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repository;
    public UserService(IRepositoryWrapper repository) => _repository = repository;

    public async Task<IEnumerable<Users>> GetAllUsersAsync() =>
        _repository.Users.FindAll().ToList();

    public async Task<Users> GetUserByIdAsync(int id) =>
        _repository.Users.FindByCondition(u => u.Id == id).FirstOrDefault();

    public async Task CreateUserAsync(Users user) { _repository.Users.Create(user); _repository.Save(); }
    public async Task UpdateUserAsync(Users user) { _repository.Users.Update(user); _repository.Save(); }
    public async Task DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        if (user != null) { _repository.Users.Delete(user); _repository.Save(); }
    }
}