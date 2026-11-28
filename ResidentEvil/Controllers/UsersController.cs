using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using ResidentEvil.Repositories;

namespace ResidentEvil.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepositoryWrapper _repo;

        public UsersController(UserManager<IdentityUser> userManager, IRepositoryWrapper repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var list = new List<UserListItem>();
            foreach (var u in _userManager.Users.OrderBy(u => u.UserName).ToList())
            {
                list.Add(new UserListItem
                {
                    Id = u.Id,
                    UserName = u.UserName ?? "",
                    IsAdmin = await _userManager.IsInRoleAsync(u, "Admin")
                });
            }
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot change your own role.";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            else
                await _userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            // Remove the user's profile first
            var profile = _repo.Profiles.FindByCondition(p => p.UsersId == id).FirstOrDefault();
            if (profile != null)
            {
                _repo.Profiles.Delete(profile);
                await _repo.SaveAsync();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}