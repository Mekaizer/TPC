using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y los repositoriosBien_Servicio para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ControladorBienServicio : ControllerBase
    {
        private readonly IRepositorioBienServicio RBS;
        public ControladorBienServicio(IRepositorioBienServicio RBS)
        {
            this.RBS = RBS;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllServicio()
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult> getServicio(int id)
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
        [HttpPost]
        public async Task<ActionResult<BienServicio>> NuevoBienServicio(BienServicio bs)
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en que?"+ex);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BienServicio>> ModificarBienServicio(BienServicio alumno,int id)
        {
            try
            {
                if (id != alumno.ID_Bien_Servicio)
                    return BadRequest("Alumno Id no coincide");

                var alumnoModificar = await RBS.GetServicio(id);

                if (alumnoModificar == null)
                    return NotFound($"Alumno con = {id} no encontrado");

                return await RBS.ModificarBien_Servicio(alumno);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
        }
}

