using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Todo.Models;

public class TodoItemDTO
{
    public long Id { get; set; }
    [MaxLength(100)]
    [RegularExpression("\\w+")]
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}