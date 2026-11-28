namespace ResidentEvil.Controllers;

using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using Microsoft.AspNetCore.Authorization;
using ResidentEvil.Services;

public class HomeController : Controller
{
    private readonly IEntryService _entryService;

    public HomeController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    public async Task<IActionResult> Index() => View(await _entryService.GetFeaturedEntriesAsync());
    public IActionResult Story() => View();
    public IActionResult Gallery() => View();
    public IActionResult Entries() => View(_entryService.GetAllEntries());
    public async Task<IActionResult> Details(int id)
    {
        // Searching by ID
        var entry = await _entryService.GetEntryByIdAsync(id);

        if (entry == null)
        {
            return NotFound();
        }

        return View(entry); // Views/Home/Details.cshtml
    }

    public async Task<IActionResult> SearchRedirect(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return RedirectToAction("Entries");
        }

        var entry = (await _entryService.GetAllEntriesAsync())
                    .FirstOrDefault(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        if (entry != null)
        {
            return RedirectToAction("Details", new { id = entry.Id });
        }

        TempData["Error"] = "File not found in Umbrella database!";
        return RedirectToAction("Entries");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateEntry(Entry entry)
    {
        if (ModelState.IsValid)
        {
            _entryService.CreateEntry(entry);
            return RedirectToAction("Entries");
        }
        return View("Entries", _entryService.GetAllEntries());
    }
}