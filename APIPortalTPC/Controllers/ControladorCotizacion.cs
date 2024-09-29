using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft. AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorCotizacion : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCotizacion RC;
        //Se inicializa la Interface Repositorio

        public ControladorCotizacion(IRepositorioCotizacion RC)
        {
            this.RC = RC;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RC.GetAllCotizacion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RC.GetCotizacion(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Cotizacion>> Nuevo(Cotizacion c)
        {
            try
            {
                if (c == null)
                    return BadRequest();

                Cotizacion nuevac = await RC.NuevaCotizacion(c);
                return nuevac;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Cotizacion>> Modificar(Cotizacion c, int id)
        {
            try
            {
                if (id != c.ID_Cotizacion)
                    return BadRequest("La Id no coincide");

                var Modificar = await RC.GetCotizacion(id);

                if (Modificar == null)
                    return NotFound($"Cotizacion con = {id} no encontrado");

                return await RC.ModificarCotizacion(c);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}
