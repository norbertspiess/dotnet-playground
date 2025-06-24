using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Infrastructure;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(long id);
    Task<bool> ExistsByIdAsync(long id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(long id);
}