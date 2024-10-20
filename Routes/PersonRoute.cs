using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Routes
{
    public static class PersonRoute
    {
        public static void PersonRoutes(this WebApplication app)
        {
            var route = app.MapGroup("person");

            route.MapGet("", async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync();
                return Results.Ok(people);
            });
            
            route.MapGet("{id:guid}", async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FindAsync(id);

                return person == null ? Results.NotFound() : Results.Ok(person);
            });

            route.MapPost("", async (PersonDTO dto, PersonContext context) =>
            {
                var personEntity = new PersonModel(dto.name);

                await context.People.AddAsync(personEntity);
                await context.SaveChangesAsync();

                return Results.Created($"/person/{personEntity.Id}", personEntity);
            });

            route.MapDelete("{id:guid}", async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FindAsync(id);

                if (person == null)
                    return Results.NotFound();

                context.People.Remove(person);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            route.MapPut("{id:guid}", async (Guid id, PersonDTO dto, PersonContext context) =>
            {
                var person = await context.People.FindAsync(id);

                if (person == null)
                    return Results.NotFound();

                person.UpdateName(dto.name);
                await context.SaveChangesAsync();

                return Results.Ok(person);
            });
        }
    }
}