using ResidentEvil.Data;

namespace ResidentEvil.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _repoContext;

        private IEntryRepository _entry;
        private IProfilesRepository _profiles;
        private IGalleryRepository _galleryItem;

        public RepositoryWrapper(ApplicationDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IEntryRepository Entry => _entry ??= new EntryRepository(_repoContext);
        public IProfilesRepository Profiles => _profiles ??= new ProfilesRepository(_repoContext);
        public IGalleryRepository GalleryItem => _galleryItem ??= new GalleryRepository(_repoContext);

        public void Save() => _repoContext.SaveChanges();
        public Task SaveAsync() => _repoContext.SaveChangesAsync();
    }
}