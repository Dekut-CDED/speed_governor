namespace Application.Interfaces
{
    public interface IUnitofWork
    {
        void Save();

        IAppUser AppUser { get; }
    }
}