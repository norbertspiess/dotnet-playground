using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodoItemsController(TodoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
    {
        return await context.TodoItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        return todoItem is null ? NotFound() : todoItem;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }
        
        if (!TodoItemExists(id))
        {
            return NotFound();
        }
        
        context.Entry(todoItem).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        context.TodoItems.Remove(todoItem);
        await context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool TodoItemExists(long id)
    {
        return context.TodoItems.Any(e => e.Id == id);
    }
}