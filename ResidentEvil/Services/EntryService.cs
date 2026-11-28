using Microsoft.EntityFrameworkCore;
using ResidentEvil.Models;
using ResidentEvil.Repositories;

namespace ResidentEvil.Services
{
    public class EntryService : IEntryService
    {
        private readonly IRepositoryWrapper _repository;

        public EntryService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // ---------- ENTRIES ----------

        public async Task<IEnumerable<Entry>> GetAllEntriesAsync() =>
            await _repository.Entry.FindAll().ToListAsync();

        public async Task<IEnumerable<Entry>> GetFeaturedEntriesAsync() =>
            await _repository.Entry
                .FindByCondition(e => e.IsFeatured)
                .OrderByDescending(e => e.ReleaseYear)
                .ToListAsync();

        public async Task<Entry> GetEntryByIdAsync(int id) =>
            await _repository.Entry.FindByCondition(e => e.Id == id).FirstOrDefaultAsync();

        public async Task CreateEntryAsync(Entry entry)
        {
            _repository.Entry.Create(entry);
            await _repository.SaveAsync();
        }

        public async Task UpdateEntryAsync(Entry entry)
        {
            _repository.Entry.Update(entry);
            await _repository.SaveAsync();
        }

        public async Task DeleteEntryAsync(int id)
        {
            var entry = await GetEntryByIdAsync(id);
            if (entry != null)
            {
                _repository.Entry.Delete(entry);
                await _repository.SaveAsync();
            }
        }

        public async Task<IEnumerable<Entry>> SearchEntriesAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return await GetAllEntriesAsync();

            return await _repository.Entry
                .FindByCondition(e => e.Name.Contains(term))
                .ToListAsync();
        }

        // ---------- GALLERY ----------

        public async Task<IEnumerable<GalleryItem>> GetAllGalleryItemsAsync() =>
            await _repository.GalleryItem.GetAllWithEntryAsync();

        public async Task AddGalleryItemAsync(GalleryItem item)
        {
            _repository.GalleryItem.Create(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateGalleryItemAsync(GalleryItem item)
        {
            _repository.GalleryItem.Update(item);
            await _repository.SaveAsync();
        }

        public async Task DeleteGalleryItemAsync(int id)
        {
            var item = await _repository.GalleryItem
                .FindByCondition(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                _repository.GalleryItem.Delete(item);
                await _repository.SaveAsync();
            }
        }

        // SYNC 

        public IEnumerable<Entry> GetAllEntries() => _repository.Entry.FindAll().ToList();

        public void CreateEntry(Entry entry)
        {
            _repository.Entry.Create(entry);
            _repository.Save();
        }
    }
}