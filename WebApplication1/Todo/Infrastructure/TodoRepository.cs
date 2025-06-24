using Microsoft.EntityFrameworkCore;
using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Infrastructure;

public interface ITodoItemRepository : IRepository<TodoItem>;

public sealed class TodoItemRepository(TodoContext context)
    : Repository<TodoItem>(context), ITodoItemRepository;