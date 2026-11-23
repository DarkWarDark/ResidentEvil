using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResidentEvil.Models;
using ResidentEvil.Services;

namespace ResidentEvil.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGamesService _gamesService;
        private readonly IEntryService _entryService;

        public GamesController(IGamesService gamesService, IEntryService entryService)
        {
            _gamesService = gamesService;
            _entryService = entryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var entries = await _entryService.GetAllEntriesAsync();
            return View(entries);
        }

        // --- SECȚIUNEA GALERIE ---

        // --- SECȚIUNEA GALERIE (MODIFICATĂ COMPLET) ---

        [AllowAnonymous]
        public async Task<IActionResult> Gallery()
        {
            var photos = await _entryService.GetAllGalleryItemsAsync();
            if (photos == null)
            {
                photos = new List<GalleryItem>();
            }

            // Trimitem lista de poze către View
            return View(photos);
        }

        // GET: Games/AddPhoto
        [HttpGet]
        public async Task<IActionResult> AddPhoto()
        {
            var entries = await _entryService.GetAllEntriesAsync();
            ViewBag.EntryId = new SelectList(entries, "Id", "Name");
            return View();
        }

        // POST: Games/AddPhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(GalleryItem galleryItem)
        {
            // REPARAȚIE CRITICĂ: Eliminăm proprietatea virtuală de navigare din validare,
            // altfel ModelState va fi mereu INVALID și va bloca sau corupe salvarea câmpurilor de text!
            ModelState.Remove("Entry");

            if (ModelState.IsValid)
            {
                // Salvăm obiectul complet (conține Title, Description, ImageUrl, EntryId)
                await _entryService.AddGalleryItemAsync(galleryItem);
                return RedirectToAction(nameof(Gallery));
            }

            // Dacă totuși e vreo eroare reală în câmpuri, reîncărcăm dropdown-ul
            var entries = await _entryService.GetAllEntriesAsync();
            ViewBag.EntryId = new SelectList(entries, "Id", "Name", galleryItem.EntryId);
            return View(galleryItem);
        }
        public async Task<IActionResult> EditPhoto(int id)
        {
            var allPhotos = await _entryService.GetAllGalleryItemsAsync();
            var photo = allPhotos.FirstOrDefault(p => p.Id == id);

            if (photo == null) return NotFound();

            var entries = await _entryService.GetAllEntriesAsync();
            ViewBag.EntryId = new SelectList(entries, "Id", "Name", photo.EntryId);

            return View(photo);
        }

        // POST: Games/EditPhoto/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhoto(int id, GalleryItem galleryItem)
        {
            if (id != galleryItem.Id) return NotFound();

            try
            {
                // REPARAȚIA: Chemăm Update, nu Add!
                await _entryService.UpdateGalleryItemAsync(galleryItem);
                return RedirectToAction(nameof(Gallery));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la actualizare: " + ex.Message);
                var entries = await _entryService.GetAllEntriesAsync();
                ViewBag.EntryId = new SelectList(entries, "Id", "Name", galleryItem.EntryId);
                return View(galleryItem);
            }
        }

        // POST: Games/DeletePhoto/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            await _entryService.DeleteGalleryItemAsync(id);
            return RedirectToAction(nameof(Gallery));
        }
        // --- SECȚIUNEA RULETĂ ---

        [AllowAnonymous]
        public async Task<IActionResult> Roulette()
        {
            var data = await _entryService.GetAllEntriesAsync();
            var gamesOnly = data.Where(e => e.Category == "Game").ToList();
            return View(gamesOnly);
        }

        // --- OPERAȚIUNI CRUD ENTRIES (ADMIN) ---

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Entry entry)
        {
            if (string.IsNullOrEmpty(entry.PageName)) entry.PageName = "Details";

            if (string.IsNullOrEmpty(entry.Name))
            {
                ModelState.AddModelError("", "Eroare: Numele nu a fost completat!");
                return View(entry);
            }

            await _entryService.CreateEntryAsync(entry);
            return RedirectToAction("Entries", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var entry = await _entryService.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();
            return View(entry);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Entry entry)
        {
            if (id != entry.Id) return NotFound();

            if (string.IsNullOrEmpty(entry.PageName)) entry.PageName = "Details";

            try
            {
                await _entryService.UpdateEntryAsync(entry);
                return RedirectToAction("Entries", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la salvare: " + ex.Message);
                return View(entry);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _entryService.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();

            await _entryService.DeleteEntryAsync(id);
            return RedirectToAction("Entries", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var results = await _entryService.SearchEntriesAsync(term);
            return Json(results);
        }
    }
}