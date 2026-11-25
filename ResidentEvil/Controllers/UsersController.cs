using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using ResidentEvil.Repositories;
using ResidentEvil.Services;

namespace ResidentEvil.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class UsersController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public UsersController(IRepositoryWrapper repo) => _repo = repo;

        // All users
        public IActionResult Index()
        {
            var users = _repo.Users.FindAll().ToList();
            return View(users);
        }

        // Delete user
        [HttpPost]
        [ValidateAntiForgeryToken]
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