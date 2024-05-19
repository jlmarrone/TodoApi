using TodoApi.Common.Persistence.Common.QueryObjects;
using TodoApi.Features.Todos.Domain;

namespace TodoApi.Features.Todos.Queries;

[Handler]
public sealed partial class GetAllTodos : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/todos",
                ([AsParameters] Query query, Handler handler, CancellationToken cancellationToken)
                    => handler.HandleAsync(query, cancellationToken))
            .Produces<IReadOnlyList<Presentation.Dto.Todo>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Todo));
    }

    public sealed record Query(bool? IsCompleted, int Page = 1, int PageSize = 25);

    private static async ValueTask<IReadOnlyList<Presentation.Dto.Todo>> HandleAsync(
        Query request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var todos = await dbContext.Todos
            .Where(x => request.IsCompleted == null || x.IsCompleted == request.IsCompleted)
            .Page(request.Page, request.PageSize)
            .Select(x => new Presentation.Dto.Todo
            {
                Id = x.Id,
                IsCompleted = x.IsCompleted,
                Text = x.Text
            })
            .ToListAsync(cancellationToken);

        return todos.AsReadOnly();
    }
}