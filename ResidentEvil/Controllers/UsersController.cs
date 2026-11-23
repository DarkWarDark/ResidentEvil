using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using ResidentEvil.Repositories;
using ResidentEvil.Services;

namespace ResidentEvil.Controllers
{
    [Authorize(Roles = "Admin")] // Doar adminii umblă aici!
    public class UsersController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public UsersController(IRepositoryWrapper repo) => _repo = repo;

        // Afișează toți utilizatorii (Pagina CRUD nr. 2)
        public IActionResult Index()
        {
            var users = _repo.Users.FindAll().ToList();
            return View(users);
        }

        // Ștergere utilizator
        public IActionResult Delete(int id)
        {
            var user = _repo.Users.FindByCondition(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                _repo.Users.Delete(user);
                _repo.Save();
            }
            return RedirectToAction("Index");
        }
    }

}