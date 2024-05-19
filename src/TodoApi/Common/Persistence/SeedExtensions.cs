using Bogus;
using TodoApi.Features.Todos.Domain;

namespace TodoApi.Common.Persistence;

/// <summary>
/// Contains the extension method for seeding the database with initial data.
/// </summary>
public static class SeedExtensions
{
    private const int TodosTotal = 100000;
    
    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public static void SeedDatabase(this AppDbContext dbContext)
    {
        var hasAnyTodos = dbContext.Todos.Any();

        if (hasAnyTodos)
        {
            return;
        }

        var todoFaker = new Faker<Todo>()
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.Text, f => f.Hacker.Phrase())
            .RuleFor(t => t.IsCompleted, false);
        
        dbContext.Todos.AddRange(todoFaker.Generate(TodosTotal));
        
        dbContext.SaveChanges();
    }
}