using Microsoft.EntityFrameworkCore;
using System;
using TicketStystemModules.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TicketContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-DB81SDF\\SQLEXPRESS;Database=InMemory;Trusted_Connection=True;TrustServerCertificate=True;");
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/tickets", async (string? title, TicketContext db) =>
{
    var query = db.Tickets.AsQueryable();

    if (!string.IsNullOrEmpty(title))
    {
        query = query.Where(t => t.Title.Contains(title));
    }

    var tickets = await query.OrderBy(t => t.CreatedAt).ToListAsync();
    return Results.Ok(tickets);
})
    .WithName("GetTickets")
    .WithOpenApi();

app.MapGet("/tickets/{id}", async(TicketContext db, Guid id) =>

    await db.Tickets.FindAsync(id) is Ticket ticket
        ? Results.Ok(ticket)
        : Results.NotFound()
)
    .WithName("GetTicketById")
    .WithOpenApi();

app.MapPost("/tickets", (TicketContext db, AddTicketDTO Data) =>
{
    Ticket newTicket = new Ticket();
    newTicket.Id = Guid.NewGuid();
    newTicket.Title = Data.Title;
    newTicket.Description = Data.Description;
    newTicket.CreatedAt = DateTime.Now;
    db.Tickets.Add(newTicket);
    db.SaveChanges();
    return Results.Created($"/tickets/{newTicket.Id}", newTicket);
})
    .WithName("AddTicket")
    .WithOpenApi();

app.MapPut("/tickets/{id}", (Guid id, EditTicketDTO ubdateTicket, TicketContext db) =>
{
    var existingTicket = db.Tickets.FirstOrDefault(t => t.Id == id);

    if (existingTicket is null) return Results.NotFound();

    existingTicket.Title = ubdateTicket.Title;
    existingTicket.Description = ubdateTicket.Description;
    db.Tickets.Update(existingTicket);
    db.SaveChanges();
    return Results.Ok(existingTicket);
})
    .WithName("UbdateTicket")
    .WithOpenApi();

app.MapDelete("/tickets/{id}", (Guid id, TicketContext db) =>
{
    var existingTicket = db.Tickets.FirstOrDefault(t => t.Id == id);

    if (existingTicket is null) return Results.NotFound();

    db.Tickets.Remove(existingTicket);
    db.SaveChanges();
    return Results.Ok(existingTicket);
})
    .WithName("DelateTicket")
    .WithOpenApi();

app.Run();

