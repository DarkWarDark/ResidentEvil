using ResidentEvil.Models;
namespace ResidentEvil.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository Users { get; }
        IGameRepository Game { get; }
        IEntryRepository Entry { get; }
        IProfilesRepository Profiles { get; }
        IGalleryRepository GalleryItem { get; }

        void Save();
    }
}