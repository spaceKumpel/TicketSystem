var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var tickets = new List<Ticket>
{
    new Ticket { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Title = "First ticket", Description = "This is the first ticket", CreatedAt = DateTime.Now },
    new Ticket { Id = Guid.Parse("21211c73-56d0-4d1b-8fb2-a15c8e9315c2"), Title = "Second ticket", Description = "This is the second ticket", CreatedAt = DateTime.Now },
};

app.MapGet("/tickets", () => tickets)
    .WithName("GetTickets")
    .WithOpenApi();

app.MapGet("/tickets/{id}", (Guid id) =>
{
    var ticket = tickets.FirstOrDefault(t => t.Id == id);
    return ticket is not null ? Results.Ok(ticket) : Results.NotFound();
})
    .WithName("GetTicketById")
    .WithOpenApi();

app.MapPost("/tickets", (AddTicketDTO Data) =>
{
    Ticket newTicket = new Ticket();
    newTicket.Id = Guid.NewGuid();
    newTicket.Title = Data.Title;
    newTicket.Description = Data.Description;
    newTicket.CreatedAt = DateTime.Now;
    tickets.Add(newTicket);
    return Results.Created($"/tickets/{newTicket.Id}", newTicket);
})
    .WithName("AddTicket")
    .WithOpenApi();

app.MapPut("/tickets/{id}", (Guid id, EditTicketDTO ubdateTicket) =>
{
    var existingTicket = tickets.FirstOrDefault(t => t.Id == id);

    if (existingTicket is null) return Results.NotFound();
    
    existingTicket.Title = ubdateTicket.Title;
    existingTicket.Description = ubdateTicket.Description;
    return Results.Ok(existingTicket);
})
    .WithName("UbdateTicket")
    .WithOpenApi();

app.MapDelete("/tickets/{id}", (Guid id) =>
{
    var ticket = tickets.FirstOrDefault(t => t.Id == id);
    if (ticket is null) return Results.NotFound();
    tickets.Remove(ticket);
    return Results.NoContent();
})
    .WithName("DelateTicket")
    .WithOpenApi();

app.Run();

public class Ticket
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class AddTicketDTO
{
    public string Title { get; set; }
    public string Description { get; set; }

}
public class EditTicketDTO
{
    public string Title { get; set; }
    public string Description { get; set; }

}
