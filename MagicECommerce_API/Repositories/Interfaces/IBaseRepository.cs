namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> ExistsAsync(Guid id);
    }
}
