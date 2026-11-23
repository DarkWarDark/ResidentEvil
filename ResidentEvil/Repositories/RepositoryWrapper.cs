using ResidentEvil.Data;
using ResidentEvil.Models;

namespace ResidentEvil.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _repoContext;

        private IUserRepository _users;
        private IGameRepository _game;
        private IEntryRepository _entry;
        private IProfilesRepository _profiles;
        private IGalleryRepository _galleryItem;

        public RepositoryWrapper(ApplicationDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        // O lăsăm activă aici doar pentru compatibilitate cu codul vechi (rezolvă cele 8 erori)
        public IUserRepository Users => _users ??= new UserRepository(_repoContext);

        public IGameRepository Game => _game ??= new GameRepository(_repoContext);

        public IEntryRepository Entry => _entry ??= new EntryRepository(_repoContext);

        public IProfilesRepository Profiles
        {
            get
            {
                if (_profiles == null)
                {
                    _profiles = new ProfilesRepository(_repoContext);
                }
                return _profiles;
            }
        }

        public IGalleryRepository GalleryItem
        {
            get
            {
                if (_galleryItem == null)
                {
                    _galleryItem = new GalleryRepository(_repoContext);
                }
                return _galleryItem;
            }
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}