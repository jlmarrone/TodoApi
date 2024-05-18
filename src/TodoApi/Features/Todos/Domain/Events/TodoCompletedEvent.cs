using MediatR;

namespace TodoApi.Features.Todos.Domain.Events;

public record TodoCompletedEvent(Guid TodoId) : INotification;