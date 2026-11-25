using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using ResidentEvil.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ResidentEvil.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly IRepositoryWrapper _repo;

        public ProfilesController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // VIEW PROFILE
        public IActionResult MyProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            var profile = _repo.Profiles.FindByCondition(p => p.UsersId == currentUserId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create");
            }

            return View(profile);
        }

        // CREATE PROFILE (GET)
        public IActionResult Create()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            var existing = _repo.Profiles.FindByCondition(p => p.UsersId == currentUserId).FirstOrDefault();
            if (existing != null) return RedirectToAction("MyProfile");

            return View();
        }

        // CREATE PROFILE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Profiles profile)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            var noulProfile = new Profiles
            {
                Bio = profile.Bio,
                FavoriteGame = profile.FavoriteGame,
                AvatarUrl = profile.AvatarUrl,
                UsersId = currentUserId // String to string
            };

            ModelState.Clear();

            try
            {
                _repo.Profiles.Create(noulProfile);
                _repo.Save();
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la salvare: " + ex.Message);
                return View(profile);
            }
        }

        // EDIT PROFILE (GET)
     
        public IActionResult Edit(int id)
        {
            var profile = _repo.Profiles.FindByCondition(p => p.Id == id).FirstOrDefault();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            if (profile == null || profile.UsersId != currentUserId)
            {
                return NotFound();
            }

            return View(profile);
        }

        // EDIT PROFILE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Profiles profile)
        {
            if (id != profile.Id) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            // Loading the profile from the database
            var existing = _repo.Profiles.FindByCondition(p => p.Id == id).FirstOrDefault();
            if (existing == null) return NotFound();
            if (existing.UsersId != currentUserId) return Forbid();

            // Copy only the fields the user is allowed to change
            existing.Bio = profile.Bio;
            existing.FavoriteGame = profile.FavoriteGame;
            existing.AvatarUrl = profile.AvatarUrl;

            try
            {
                _repo.Profiles.Update(existing);
                _repo.Save();
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la actualizare: " + ex.Message);
                return View(profile);
            }
        }
    }
}