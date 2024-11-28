using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Dml;
using NPOI.XWPF.UserModel;

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
        [HttpPost("Liberadores")]
        public async Task<ActionResult> EnviarCorreoLiberadores()
        {
            string subject = "Recordatorio Urgente: Liberación de Órdenes de Compra Pendientes";
            var lista = await IRU.GetAllUsuariosLiberadores();
            foreach (var U in lista)

                await IEC.CorreoLiberador(U, subject);


            if (lista == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "no existen liberadores pendientes!!! :D ");
            }

            return Ok("Correos enviados con exito");

        }
        /// <summary>
        /// Metodo que recibe una lista con los ID de los Usuarios, para luego enviarles a sus correos que deben hacer liberaciones
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("VariosLiberadores")]
        public async Task<ActionResult> VariosLiberadores(ListaID LID)
        {
            int[] lista = LID.Lista;
            string subject = LID.Subject;
            try
            {
                foreach (int i in lista)
                {
                    Usuario U = await IRU.GetUsuario(i);
                    await IEC.CorreoLiberador(U, subject);
                }
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron liberadores ");
            }



        }

        [HttpPost("Receptores")]
        public async Task<ActionResult> EnviarCorreoReceptores()
        {
            string subject = "TPC Confirmación recepción";
            var lista = await IRU.GetAllUsuario();
            foreach (var U in lista)
                await IEC.CorreoRecepciones(U, subject);

            if (lista == null)
                return StatusCode(StatusCodes.Status404NotFound, "No hay problemas de recepción");

            return Ok("Correos enviados con exito");
        }

        [HttpPost("VariosReceptores")]
        public async Task<ActionResult> VariosReceptores(ListaID LID)
        {
            int[] lista = LID.Lista;
            string subject = LID.Subject;
            try
            {
                foreach (int i in lista)
                {
                    Usuario U = await IRU.GetUsuario(i);
                    await IEC.CorreoRecepciones(U, subject);
                }
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron recepcionadores ");
            }

        }


    }
}
