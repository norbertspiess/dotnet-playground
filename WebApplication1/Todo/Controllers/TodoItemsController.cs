using Microsoft.AspNetCore.Mvc;
using WebApplication1.Todo.Infrastructure;
using WebApplication1.Todo.Models;

namespace WebApplication1.Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodoItemsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> Get()
    {
        var todos = (await _unitOfWork.TodoItems.GetAllAsync()).Select(ItemToDto);
        return Ok(todos);
    }

    [HttpGet("{id:long:min(0)}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(id);
        return todoItem is null ? NotFound() : ItemToDto(todoItem);
    }

    [HttpPut("{id:long:min(0)}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        var todo = await _unitOfWork.TodoItems.GetByIdAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        todo.Name = dto.Name;
        todo.IsComplete = dto.IsComplete;

        await _unitOfWork.TodoItems.UpdateAsync(todo);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO dto)
    {
        var todoItem = new TodoItem
        {
            Name = dto.Name,
            IsComplete = dto.IsComplete,
            Secret = "hush",
        };

        await _unitOfWork.TodoItems.AddAsync(todoItem);

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDto(todoItem));
    }

    [HttpDelete("{id:long:min(0)}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var exists = await _unitOfWork.TodoItems.ExistsByIdAsync(id);
        if (!exists)
        {
            return NotFound();
        }

        await _unitOfWork.TodoItems.DeleteAsync(id);

        return NoContent();
    }

    private static TodoItemDTO ItemToDto(TodoItem todoItem) =>
        new()
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}