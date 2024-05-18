using MediatR;

namespace TodoApi.Features.Todos.Domain.Events;

public record TodoCreatedEvent(Guid TodoId) : INotification;