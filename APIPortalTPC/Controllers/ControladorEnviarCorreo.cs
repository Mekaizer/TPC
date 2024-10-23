using APIPortalTPC.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorEnviarCorreo : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceEnviarCorreo IEC;
        public ControladorEnviarCorreo(InterfaceEnviarCorreo IEC)
        {
            this.IEC = IEC;
        }
        [HttpPost("Cotizacion{id:int}")]
        public async Task<ActionResult> EnviarCorreo(int id)
        {
            try
            {


                await IEC.CorreoCotizacion();

                return Ok("Correos enviados con exito");
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }

            }

        }


    }
