using Microsoft.EntityFrameworkCore;
using TicketStystemModules.Modules;
using TicketStystemModules.Services;
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja bazy danych
builder.Services.AddDbContext<TicketContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-DB81SDF\\SQLEXPRESS;Database=InMemory;Trusted_Connection=True;TrustServerCertificate=True;");
});

// Konfiguracja serwisów
builder.Services.AddScoped<TicketService>();

// Konfiguracja kontrolerów i Swaggera
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();