using Microsoft.EntityFrameworkCore;
using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Infrastructure;

public interface IUnitOfWork
{
    IRepository<TodoItem> TodoItems { get; }
    Task SaveChangesAsync();
}

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IRepository<TodoItem> TodoItems { get; }

    public UnitOfWork(TodoContext context, ITodoItemRepository todoItems)
    {
        _context = context;
        TodoItems = todoItems;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}