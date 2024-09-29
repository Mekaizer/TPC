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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorBienServicio : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioBienServicio RBS;
        //Se inicializa la Interface Repositorio
        public ControladorBienServicio(IRepositorioBienServicio RBS)
        {
            this.RBS = RBS;
        }
        //Metodo para obtener todos los objetos de la tabla
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RBS.GetAllServicio());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }
        //Metodo para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado= await RBS.GetServicio(id);
                if (resultado == null)
                    return NotFound();
                
                return Ok(resultado) ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }

        //Metodo para crear nuevo objeto
        [HttpPost]
        public async Task<ActionResult<BienServicio>> Nuevo(BienServicio bs)
        {
            try
            {
                if (bs == null)
                    return BadRequest();

                BienServicio nuevoBS = await RBS.NuevoBienServicio(bs);
                return nuevoBS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de "+ex);
            }
        }

        //Metodo para modificar un objeto por ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BienServicio>> Modificar(BienServicio bs,int id)
        {
            try
            {
                if (id != bs.ID_Bien_Servicio)
                    return BadRequest("La Id no coincide");

                var bienServicioModificar = await RBS.GetServicio(id);

                if (bienServicioModificar == null)
                    return NotFound($"Bien o Servicio con = {id} no encontrado");

                return await RBS.ModificarBien_Servicio(bs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}

