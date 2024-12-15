﻿using APIPortalTPC.Repositorio;
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
        private readonly IRepositorioArchivo IRA;
        private readonly IRepositorioRelacion IRR;
        private readonly IRepositorioCotizacion IRC;
        public ControladorEnviarCorreo(IRepositorioCotizacion IRC,IRepositorioRelacion IRR,IRepositorioArchivo IRA,IRepositorioOrdenCompra IROC, IRepositorioTicket IRT,IRepositorioLiberadores IRL, IRepositorioUsuario IRU, InterfaceEnviarCorreo IEC, IRepositorioProveedores IRP)
        {
            this.IEC = IEC;
            this.IRP = IRP;
            this.IRU = IRU;
            this.IRL = IRL;
            this.IRA = IRA;
            this.IRT = IRT;
            this.IROC = IROC;
            this.IRR = IRR;
            this.IRC = IRC;
        }
        /// <summary>
        /// Metodo para enviar a varios proveedores (enviar archivo) las cotizaciones
        /// </summary>
        /// <param name="LID"></param>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<IActionResult> EnviarVariosProveedores([FromForm] FormData formData)
        {
            try
            {

                var lis = Array.ConvertAll<string, int>(formData.Proveedor.Split(','), Convert.ToInt32);
    
     
                     foreach (int ID in lis)
                     {
          
                    Proveedores P = await IRP.GetProveedor(ID);
     
     
                   //
                   //await IEC.CorreoProveedores(P, formData);
    
                     }

                //procede a guardar el correo
                if (formData.file == null || formData.file.Length == 0)
                {
                    return Ok("Correos enviados con exito"); // no se guarda archivo
                }

                using (var memoryStream = new MemoryStream())
                {
                    await formData.file.CopyToAsync(memoryStream);
                    //guardamos el archivo y cambiamos el estado de la cotizacion 
            
                    Archivo A = new Archivo();
                    A.ArchivoDoc = memoryStream.ToArray();
                    A.NombreDoc =formData.file.Name;
                    A = await IRA.NuevoArchivo(A);
                    //pedimos la cotizacion
                    Cotizacion Cot = await IRC.GetCotizacion(11);//formData.Id_Cotizacion
                    Cot.Estado = " Enviado";
                    Relacion R = new Relacion();
                    R.Id_Cotizacion=(Cot.ID_Cotizacion);

                    R.Id_Archivo = A.Id_Archivo;
                    Cot.Estado = "Enviado";
                    await IRC.ModificarCotizacion(Cot);
                    await IRR.NuevaRelacion(R);

                    //guardamos el correo
                }
                return Ok("Correos enviados con exito");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
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
                    int idT = 0;
                    foreach (int dep in ldep)
                    {

                        Liberadores L = await IRL.GetDep(dep);
                        U = await IRU.GetUsuario(L.Id_Usuario);
                        //await IEC.CorreoLiberador(U, subject);
                        enviado = true;
                    }
                    if (enviado)
                    {
                        Liberadores lib = await IRL.Get(9);
                    
                        U = await IRU.GetUsuario(lib.Id_Usuario);
                       // await IEC.CorreoLiberador(U, subject);
                        //cambiar estado ticket!!!
                        Ticket T = await IRT.GetTicket(1);

              

                        T.Estado = "OC Enviada";
                        await IRT.ModificarTicket(T);
                        Console.WriteLine(T.Estado);
                    
                    }
                }
         

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
                    //se saca la lista con los ID de OC relacionadas al ticket del usuario
                    List<int> Id_LT = new List<int>();
                    Usuario U = await IRU.GetUsuario(i);
                    if (U.Activado)
                    {
                        int id = U.Id_Usuario;
                        var list = await IRT.TicketConOCPendientes(id);
                        Id_LT = (List<int>)list;
                        if (Id_LT.Count != 0)
                        {
                 
                            //await IEC.CorreoRecepciones(U, subject, Id_LT);
                        }
                        //cambiar estado ticket
                   
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
