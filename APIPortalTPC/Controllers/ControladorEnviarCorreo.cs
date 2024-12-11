using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
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
        private readonly IRepositorioLiberadores IRL;
        private readonly IRepositorioTicket IRT;
        private readonly IRepositorioOrdenCompra IROC;
        public ControladorEnviarCorreo(IRepositorioOrdenCompra IROC, IRepositorioTicket IRT,IRepositorioLiberadores IRL, IRepositorioUsuario IRU, InterfaceEnviarCorreo IEC, IRepositorioProveedores IRP)
        {
            this.IEC = IEC;
            this.IRP = IRP;
            this.IRU = IRU;
            this.IRL = IRL;
            this.IRT = IRT;
            this.IROC = IROC;
        }
        /// <summary>
        /// Metodo para enviar a varios proveedores (enviar archivo)
        /// </summary>
        /// <param name="LID"></param>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<IActionResult> EnviarVariosProveedores([FromForm] FormData formData)
        {
            try
            {
     
      
            var lis = formData.Lista;
                     Console.WriteLine(lis);
                     foreach (int ID in lis)
                     {
        
                    Proveedores P = await IRP.GetProveedor(ID);
                    //await IEC.CorreoProveedores(P, formData);
                }


                return Ok("Correos enviados con exito");
        
                }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }
        /// <summary>
        /// Metodo para enviar a un solo proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Proveedor/{id:int}")]
        public async Task<ActionResult> EnviarCorreoProveedores([FromForm] FormData formData, int id)
        {
            try
            {
                /*

                var lis = formData.Lista;
                Console.WriteLine(lis);
                foreach (int ID in lis)
                {


                    //await IEC.CorreoProveedores(P, formData.Asunto);
                }
                */


                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
            /*
             *   try
              {
                  var lista = IRP.GetAllProveedoresBienServicio(id);
                  string mensaje = "Otro mensaje";
                  //   return Ok(await lista);
                  foreach (var P in await lista)
                  {

                      if (P.ID_Bien_Servicio.ToString() != null)
                      {
                          await IEC.CorreoProveedores(P, mensaje);
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
             */

        }
        /// <summary>
        /// Metodo que envia correo al liberador del correo
        /// </summary>
        /// <returns></returns>
        [HttpPost("Liberadores")]
        public async Task<ActionResult> EnviarCorreoLiberadores()
        {
            string subject = "Recordatorio Urgente: Liberación de Órdenes de Compra Pendientes";
            var lista = await IRL.GetAll();
            foreach (var U in lista)
            {
                Usuario User = await IRU.GetUsuario(U.Id_Usuario);
                await IEC.CorreoLiberador(User, subject);
            }
          


            if (lista == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "no existen liberadores pendientes!!! :D ");
            }
       
            return Ok("Correos enviados con exito");

        }
        /// <summary>
        /// Metodo que recibe una lista con los ID de los Usuarios, para luego enviarles a sus correos que deben hacer liberaciones (no hay tabla)
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("VariosLiberadores")]
        public async Task<ActionResult> VariosLiberadores(ListaID LID)
        {
            int[] lista = LID.Lista;
            string subject = "Recordatorio Urgente: Liberación de Órdenes de Compra Pendientes";
            try
            {
                foreach(int i in lista) {
                    Usuario U = await IRU.GetUsuario(i);
                    bool enviado = false;
                    List<int> ldep = U.ListaIdDep;

                    foreach (int dep in ldep)
                    {
                        Liberadores L = await IRL.GetDep(dep);
                        U = await IRU.GetUsuario(L.Id_Usuario);
                        await IEC.CorreoLiberador(U, subject);
                        enviado = true;
                    }
                    if (enviado)
                    {
                        Liberadores lib = await IRL.Get(9);
                  
                        U = await IRU.GetUsuario(lib.Id_Usuario);
                        Console.WriteLine(U.Correo_Usuario + " Usuario " + U.Nombre_Completo);
                        await IEC.CorreoLiberador(U, subject);

                    }
                }
                //cambiar estado ticket!!!


                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron liberadores "+ex);
            }



        }
        


        /// <summary>
        /// Solicitantes para confirmar que tienen OC pendientes de recepcionar!
        /// </summary>
        /// <param name="LID"></param>
        /// <returns></returns>
        [HttpPost("VariosReceptores")]
        public async Task<ActionResult> VariosReceptores(ListaID LID)
        {
            int[] lista = LID.Lista;
          
        string subject = "TPC Confirmación de recepción";
            try
            {
                foreach (int i in lista)
                {

                    List<int> Id_LT = new List<int>();
                    Usuario U = await IRU.GetUsuario(i);
                    if (U.Activado)
                    {
                        int id = U.Id_Usuario;
                        var list = await IRT.TicketConOCPendientes(id);
                        Id_LT = (List<int>)list;
                        if (Id_LT.Count != 0)
                        {
                 
                            await IEC.CorreoRecepciones(U, subject, Id_LT);
                        }
                    }


                }
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron liberadores ");
            }



        }







    }
}
