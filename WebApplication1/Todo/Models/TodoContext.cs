using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Todo.Models;

public sealed class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}