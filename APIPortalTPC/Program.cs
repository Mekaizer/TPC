
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
builder.Services.AddScoped<IRepositorioAutentizar, RepositorioAutentizar>();
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
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioDepartamentoUsuario, RepositorioDepartamentoUsuario>();
builder.Services.AddScoped<InterfaceExcel, RepositorioExcel>();
builder.Services.AddScoped<InterfaceEnviarCorreo, RepositorioEnviarCorreo>();

var config = builder.Configuration;
var sqlConfig = new AccesoDatos(config.GetConnectionString("SQL"));
builder.Services.AddSingleton(sqlConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

/*Metodo para dar acceso al front-end
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();

    });
});

app.UseCors("NuevaPolitica");
 */


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



