using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketStystemModules.Modules;
using TicketStystemModules.Services;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<TicketContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TicketDatabase")));
builder.Services.AddScoped<TicketService>();
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