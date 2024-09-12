
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
builder.Services.AddScoped<IRepositorioBienServicio, RepositorioBienServicio>();
var config = builder.Configuration;
var sqlConfig = new AccesoDatos(config.GetConnectionString("SQL"));
builder.Services.AddSingleton(sqlConfig);
//autorizaciones para que otros servicios puedan acceder
//se agrega la autentificacion para usar los tokes
//parametros para validar el token JWT
//Clases para validar los tokens

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWT:Issuer"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(config["JWT:ClaveSecreta"]))
    };
});
*/
var app = builder.Build();

/*
 * if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
*/

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();