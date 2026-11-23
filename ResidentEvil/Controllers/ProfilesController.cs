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

        // --- VIZUALIZARE PROFIL ---
        public IActionResult MyProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            // Căutăm profilul comparând direct ca string
            var profile = _repo.Profiles.FindByCondition(p => p.UsersId == currentUserId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create");
            }

            return View(profile);
        }

        // --- CREARE PROFIL (GET) ---
        public IActionResult Create()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            var existing = _repo.Profiles.FindByCondition(p => p.UsersId == currentUserId).FirstOrDefault();
            if (existing != null) return RedirectToAction("MyProfile");

            return View();
        }

        // --- CREARE PROFIL (POST) ---
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
                UsersId = currentUserId // String la String
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

        // --- EDITARE PROFIL (GET) ---
     
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

        // --- EDITARE PROFIL (POST) ---

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Profiles profile)
        {
            if (id != profile.Id) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return RedirectToAction("Login", "Account");

            
            profile.UsersId = currentUserId;

            ModelState.Clear();

            try
            {
                _repo.Profiles.Update(profile);
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