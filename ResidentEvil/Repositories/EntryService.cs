using Microsoft.EntityFrameworkCore;
using ResidentEvil.Models;
using ResidentEvil.Repositories;
using Microsoft.EntityFrameworkCore;
namespace ResidentEvil.Services
{
    public class EntryService : IEntryService
    {
        private readonly IRepositoryWrapper _repository;

        public EntryService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // ==========================================
        // METODE PENTRU ENTRIES (JOCURI/PERSONAJE)
        // ==========================================

        public async Task<IEnumerable<Entry>> GetAllEntriesAsync()
        {
            return _repository.Entry.FindAll().ToList();
        }

        public async Task<Entry> GetEntryByIdAsync(int id)
        {
            return _repository.Entry.FindByCondition(e => e.Id == id).FirstOrDefault();
        }

        public async Task CreateEntryAsync(Entry entry)
        {
            _repository.Entry.Create(entry);
            _repository.Save();
        }

        public async Task UpdateEntryAsync(Entry entry)
        {
            _repository.Entry.Update(entry);
            _repository.Save();
        }

        public async Task DeleteEntryAsync(int id)
        {
            var entry = _repository.Entry.FindByCondition(e => e.Id == id).FirstOrDefault();
            if (entry != null)
            {
                _repository.Entry.Delete(entry);
                _repository.Save();
            }
        }

        public async Task<IEnumerable<Entry>> SearchEntriesAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _repository.Entry.FindAll().ToList();

            return _repository.Entry.FindByCondition(e => e.Name.Contains(term)).ToList();
        }

        // ==========================================
        // METODE PENTRU GALERIE (IMAGINI)
        // ==========================================

        // ==========================================
        // METODE PENTRU GALERIE (IMAGINI)
        // ==========================================

        public async Task<IEnumerable<GalleryItem>> GetAllGalleryItemsAsync()
        {
            // REPARAȚIE CRITICĂ: 
            // În loc de .FindAll().ToList(), îi spunem repository-ului să caute tot, 
            // dar trecem prin FindByCondition ca să putem forța încărcarea obiectului virtual legat 'Entry'.
            // Dacă FindAll() returnează IQueryable, codul de mai jos va face un JOIN corect în baza de date.

            var photos = _repository.GalleryItem.FindAll();

            // Pentru siguranță, dacă baza de date nu face lazy loading automat, mapăm manual/forțăm includerea:
            foreach (var photo in photos)
            {
                if (photo.Entry == null && photo.EntryId != 0)
                {
                    // Încărcăm manual subiectul corespunzător pentru fiecare poză ca să nu mai fie "Unknown"
                    photo.Entry = _repository.Entry.FindByCondition(e => e.Id == photo.EntryId).FirstOrDefault();
                }
            }

            return photos.ToList();
        }

        public async Task AddGalleryItemAsync(GalleryItem item)
        {
            _repository.GalleryItem.Create(item);
            _repository.Save();
        }

        public async Task UpdateGalleryItemAsync(GalleryItem item)
        {
            _repository.GalleryItem.Update(item);
            _repository.Save();
        }

        public async Task DeleteGalleryItemAsync(int id)
        {
            var item = _repository.GalleryItem.FindByCondition(g => g.Id == id).FirstOrDefault();
            if (item != null)
            {
                _repository.GalleryItem.Delete(item);
                _repository.Save();
            }
        }

        // ==========================================
        // METODE SINCRONE (PENTRU HOME CONTROLLER)
        // ==========================================

        public IEnumerable<Entry> GetAllEntries() => _repository.Entry.FindAll().ToList();

        public void CreateEntry(Entry entry)
        {
            _repository.Entry.Create(entry);
            _repository.Save();
        }
    }
}