using System.Text.Json.Serialization;
using APIPortalTPC.Datos;
using APIPortalTPC.Repositorio;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IRepositorioBienServicio, RepositorioBienServicio>();
var configuration = builder.Configuration;
var sqlConfig = new AccesoDatos("Server=DESKTOP-FHCBEIM;Database=BaseDatosPortalAquisicionesTPC;TrustServerCertificate=True;Trusted_Connection=True;");
builder.Services.AddSingleton(sqlConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
