namespace TodoApi.Features.Todos.Presentation.Dto;

public sealed record Todo
{
    public Guid Id { get; init; }

    public string Text { get; init; } = string.Empty;
    
    public bool IsCompleted { get; init; }
}