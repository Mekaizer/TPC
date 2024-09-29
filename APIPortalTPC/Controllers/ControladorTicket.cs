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

    public class ControladorTicket : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioTicket RT;
        //Se inicializa la Interface Repositorio

        public ControladorTicket(IRepositorioTicket RT)
        {
            this.RT = RT;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RT.GetAllTicket());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RT.GetTicket(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Ticket>> Nuevo(Ticket A)
        {
            try
            {
                if (A == null)
                    return BadRequest();

                Ticket nuevo = await RT.NewTicket(A);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Ticket>> Modificar(Ticket T, int id)
        {
            try
            {
                if (id != T.ID_Ticket)
                    return BadRequest("La Id no coincide");

                var Modificar = await RT.GetTicket(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await RT.ModificarTicket(T);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}