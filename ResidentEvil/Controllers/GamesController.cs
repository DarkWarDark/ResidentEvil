using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // ---------- GALLERY ----------

        [AllowAnonymous]
        public async Task<IActionResult> Gallery()
        {
            var photos = await _entryService.GetAllGalleryItemsAsync();
            return View(photos ?? new List<GalleryItem>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPhoto()
        {
            await LoadEntryDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPhoto(GalleryItem galleryItem)
        {
            ModelState.Remove("Entry");

            if (ModelState.IsValid)
            {
                await _entryService.AddGalleryItemAsync(galleryItem);
                return RedirectToAction(nameof(Gallery));
            }

            await LoadEntryDropdown(galleryItem.EntryId);
            return View(galleryItem);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPhoto(int id)
        {
            var allPhotos = await _entryService.GetAllGalleryItemsAsync();
            var photo = allPhotos.FirstOrDefault(p => p.Id == id);
            if (photo == null) return NotFound();

            await LoadEntryDropdown(photo.EntryId);
            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPhoto(int id, GalleryItem galleryItem)
        {
            if (id != galleryItem.Id) return NotFound();

            ModelState.Remove("Entry");
            if (!ModelState.IsValid)
            {
                await LoadEntryDropdown(galleryItem.EntryId);
                return View(galleryItem);
            }

            try
            {
                await _entryService.UpdateGalleryItemAsync(galleryItem);
                return RedirectToAction(nameof(Gallery));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update error: " + ex.Message);
                await LoadEntryDropdown(galleryItem.EntryId);
                return View(galleryItem);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            await _entryService.DeleteGalleryItemAsync(id);
            return RedirectToAction(nameof(Gallery));
        }

        // ---------- ROULETTE ----------

        [AllowAnonymous]
        public async Task<IActionResult> Roulette()
        {
            var data = await _entryService.GetAllEntriesAsync();
            var gamesOnly = data.Where(e => e.Category == "Game").ToList();
            return View(gamesOnly);
        }

        // ---------- ENTRIES CRUD (ADMIN) ----------

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Entry entry)
        {
            if (string.IsNullOrEmpty(entry.PageName)) entry.PageName = "Details";

            if (string.IsNullOrEmpty(entry.Name))
            {
                ModelState.AddModelError("", "Error: the name is required!");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                ModelState.AddModelError("", "Save error: " + ex.Message);
                return View(entry);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _entryService.DeleteEntryAsync(id);
            return RedirectToAction("Entries", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            var results = await _entryService.SearchEntriesAsync(term);
            return Json(results.Select(e => new { e.Id, e.Name, e.Category }));
        }

        // ---------- HELPERS ----------

        private async Task LoadEntryDropdown(int? selectedId = null)
        {
            var entries = await _entryService.GetAllEntriesAsync();
            ViewBag.EntryId = new SelectList(entries, "Id", "Name", selectedId);
        }
    }
}