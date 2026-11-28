namespace ResidentEvil.Repositories
{
    public interface IRepositoryWrapper
    {
        IEntryRepository Entry { get; }
        IProfilesRepository Profiles { get; }
        IGalleryRepository GalleryItem { get; }

        void Save();
        Task SaveAsync();
    }
}