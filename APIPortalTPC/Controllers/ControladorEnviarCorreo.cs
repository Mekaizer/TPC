using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorEnviarCorreo : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceEnviarCorreo IEC;
        private readonly IRepositorioProveedores IRP;
        private readonly IRepositorioUsuario IRU;
        public ControladorEnviarCorreo(IRepositorioUsuario IRU, InterfaceEnviarCorreo IEC, IRepositorioProveedores IRP)
        {
            this.IEC = IEC;
            this.IRP = IRP;
            this.IRU = IRU;
        }



        [HttpPost("Proveedor{id:int}")]
        public async Task<ActionResult> EnviarCorreoProveedores(int id)
        {
            try

            {
                var lista = IRP.GetAllProveedoresBienServicio(id);
                string mensaje = "Otro mensaje";
                //   return Ok(await lista);
                foreach (var P in await lista)
                {
                    string? productos = P.ID_Bien_Servicio;

                    if (P.ID_Bien_Servicio.ToString() != null)
                    {
                        await IEC.CorreoProveedores(productos, P, mensaje);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error al revisar lista");
                    }


                }
                if (lista == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "no existen proveedores con el bien servicio ");
                }

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
