using MediatR;

namespace TodoApi.Common.Domain;

public abstract class BaseEntity
{
    public readonly List<INotification> StagedEvents = [];
}