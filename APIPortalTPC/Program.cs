
using APIPortalTPC.Datos;
using APIPortalTPC.Repositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
IConfiguration Configuration;
builder.Services.AddControllers();
//Todos los metodos clases deben ir aqui
builder.Services.AddScoped<IRepositorioArchivo, RepositorioArchivo>();
builder.Services.AddScoped<IRepositorioBienServicio, RepositorioBienServicio>();
builder.Services.AddScoped<IRepositorioCentroCosto, RepositorioCentroCosto>();
builder.Services.AddScoped<IRepositorioCotizacion, RepositorioCotizacion>();
builder.Services.AddScoped<IRepositorioDepartamento, RepositorioDepartamento>();
builder.Services.AddScoped<IRepositorioOrdenCompra,RepositorioOrdenCompra>();
builder.Services.AddScoped<IRepositorioOrdenesEstadisticas,RepositorioOrdenesEstadisticas>();
builder.Services.AddScoped<IRepositorioProveedores,RepositorioProveedores>();
builder.Services.AddScoped<IRepositorioReemplazos,RepositorioReemplazos>();
builder.Services.AddScoped<IRepositorioRelacion,RepositorioRelacion>();
builder.Services.AddScoped<IRepositorioTicket,RepositorioTicket>();
builder.Services.AddScoped<IRepositorioUsuario,RepositorioUsuario>();


var config = builder.Configuration;
var sqlConfig = new AccesoDatos(config.GetConnectionString("SQL"));
builder.Services.AddSingleton(sqlConfig);
//autorizaciones para que otros servicios puedan acceder
//se agrega la autentificacion para usar los tokes
//parametros para validar el token JWT
//Clases para validar los tokens

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = "https://localhost:5173/";
        options.Authority = "https://localhost:5173/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your_secret_key_here")
            )
        };
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();