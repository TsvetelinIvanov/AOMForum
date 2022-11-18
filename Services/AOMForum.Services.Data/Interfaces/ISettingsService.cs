namespace AOMForum.Services.Data.Interfaces
{
    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();
    }
}