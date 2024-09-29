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

    public class ControladorDepartamento : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioDepartamento RD;
        //Se inicializa la Interface Repositorio

        public ControladorDepartamento(IRepositorioDepartamento RD)
        {
            this.RD = RD;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RD.GetAllDepartamento());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Departamento: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RD.GetDepartamento(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Departamento: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<Departamento>> Nuevo(Departamento D)
        {
            try
            {
                if (D == null)
                    return BadRequest();

                Departamento nuevo = await RD.NuevoDepartamento(D);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Departamento>> Modificar(Departamento D, int id)
        {
            try
            {
                if (id != D.Id_Departamento)
                    return BadRequest("La Id no coincide");

                var Modificar = await RD.GetDepartamento(id);

                if (Modificar == null)
                    return NotFound($"Departamento con = {id} no encontrado");

                return await RD.ModificarDepartamento(D);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}