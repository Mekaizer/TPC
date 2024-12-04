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
        public async Task<ActionResult> EnviarVariosProveedores([FromForm] ListaID LID)
        {
            try
            {

                var lis = LID.Lista;
                Console.WriteLine(lis);
                foreach (int ID in lis) 
                {
                    Console.WriteLine(LID.iD_Bien_Servicio);
                    var P = await IRP.GetProveedor(ID);

                    //await IEC.CorreoProveedores(P, LID.Asunto);
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
        public async Task<ActionResult> EnviarCorreoProveedores(int id)
        {
            try
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

        }
        /// <summary>
        /// Metodo que envia correo al liberador del correo
        /// </summary>
        /// <returns></returns>
        [HttpPost("Liberadores")]
        public async Task<ActionResult> EnviarCorreoLiberadores()
        {
            string subject = "Recordatorio Urgente: Liberación de Órdenes de Compra Pendientes";
         /*   var[] lista = await IRU.GetAllUsuariosLiberadores();
            foreach (var U in lista)

                await IEC.CorreoLiberador(U, subject);


            if (lista == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "no existen liberadores pendientes!!! :D ");
            }
         */
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
            string subject = LID.Subject;
            try
            {
                Usuario U = await IRU.GetUsuario(lista[0]);
              
                List<int> ldep = U.ListaIdDep;
         
                foreach (int dep in ldep)
                {
                    Liberadores L = await IRL.GetDep(dep);
                    U = await IRU.GetUsuario(L.Id_Usuario);
                    await IEC.CorreoLiberador(U, subject);
                    Console.Write("Enviado Usuario " + U.Nombre_Completo);
                }


                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, " No se encontraron liberadores "+ex);
            }



        }
        


        /// <summary>
        /// Solicitantes con tabla
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
                    Usuario U = await IRU.GetUsuario(i);
                    var listaT = await IRT.GetAllTicketUsuario(i);
                    //sacar los ticket del usuario
                    foreach (Ticket T in (List<Ticket>)listaT)
                    {
                        Console.WriteLine(T.ID_Ticket);
                        int id_T = T.ID_Ticket;
                        var listaOC = await IROC.OCPendientes(id_T);
              
                        //tengo las OC del ticket del Usuario
                        //await IEC.CorreoRecepciones(U, subject,(List<OrdenCompra>)listaOC,id_T);
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
