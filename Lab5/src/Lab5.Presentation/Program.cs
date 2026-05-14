using Lab5.Application.Extensions;
using Lab5.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string adminPassword = builder.Configuration["Atm:AdminPassword"] ?? "admin123";

builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices(adminPassword);

var app = builder.Build();

app.MapControllers();

app.Run();
