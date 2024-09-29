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

    public class ControladorCentroCosto : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCentroCosto RC;
        //Se inicializa la Interface Repositorio

        public ControladorCentroCosto(IRepositorioCentroCosto RC)
        {
            this.RC = RC;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RC.GetAllCeCo());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Centro de Costo: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RC.GetCeCo(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Centro de Costo: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Centro_de_costo>> Nuevo(Centro_de_costo A)
        {
            try
            {
                if (A == null)
                    return BadRequest();

                Centro_de_costo nuevo = await RC.Nuevo_CeCo(A);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Centro_de_costo>> Modificar(Centro_de_costo C, int id)
        {
            try
            {
                if (id != C.Id_Ceco)
                    return BadRequest("La Id no coincide");

                var Modificar = await RC.GetCeCo(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await RC.ModificarCeCo(C);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}