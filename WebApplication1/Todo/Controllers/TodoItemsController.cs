using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodoItemsController(TodoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> Get()
    {
        return await context.TodoItems
            .Select(t => ItemToDTO(t))
            .ToListAsync();
    }

    [HttpGet("{id:long:min(0)}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        return todoItem is null ? NotFound() : ItemToDTO(todoItem);
    }

    [HttpPut("{id:long:min(0)}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = dto.Name;
        todoItem.IsComplete = dto.IsComplete;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO dto)
    {
        var todoItem = new TodoItem()
        {
            Name = dto.Name,
            IsComplete = dto.IsComplete,
            Secret = "hush",
        };

        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
    }

    [HttpDelete("{id:long:min(0)}")]
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

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}