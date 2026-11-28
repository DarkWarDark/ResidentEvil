namespace ResidentEvil.Controllers;

using Microsoft.AspNetCore.Mvc;
using ResidentEvil.Models;
using ResidentEvil.Services;
using Microsoft.AspNetCore.Authorization;

public class HomeController : Controller
{
    private readonly IEntryService _entryService;

    public HomeController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    public IActionResult Index() => View();
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
    public IActionResult RE4()
    {
        return View(); 
    }
    public IActionResult Requiem()
    {
        return View(); 
    }
    public IActionResult REVILLAGE()
    {
        return View();
    }
    public IActionResult RE3()
    {
        return View();
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
            if (string.IsNullOrEmpty(entry.PageName) || entry.PageName == "Details")
            {
                return RedirectToAction("Details", new { id = entry.Id });
            }

            return RedirectToAction(entry.PageName);
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