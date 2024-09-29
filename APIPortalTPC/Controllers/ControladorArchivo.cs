using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorArchivo : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioArchivo RA;
        //Se inicializa la Interface Repositorio

        public ControladorArchivo(IRepositorioArchivo RA)
        {
            this.RA = RA;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RA.GetAllArchivo());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el archivo: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RA.GetArchivo(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el archivo: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Archivo>> Nuevo(Archivo A)
        {
            try
            {
                if (A == null)
                    return BadRequest();

                Archivo nuevo = await RA.NuevoArchivo(A);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Archivo>> Modificar(Archivo A, int id)
        {
            try
            {
                if (id != A.Id_Archivo)
                    return BadRequest("La Id no coincide");

                var Modificar = await RA.GetArchivo(id);

                if (Modificar == null)
                    return NotFound($"Archivo con = {id} no encontrado");

                return await RA.ModificarArchivo(A);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}