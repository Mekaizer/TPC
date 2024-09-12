using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorCotizacion : ControllerBase
    {
        private readonly IRepositorioCotizacion RC;
        public ControladorCotizacion(IRepositorioCotizacion RC)
        {
            this.RC = RC;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCotizacion()
        {
            try
            {
                return Ok(await RC.GetAllCotizacion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el servicio: " + ex.Message);
            }
        }
    }

}
