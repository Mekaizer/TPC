using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ControladorExcel :ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceExcel Excel;
        private readonly IRepositorioCentroCosto IRC;
        private readonly IRepositorioOrdenesEstadisticas IRE;
        private readonly IRepositorioBienServicio IBS;
        private readonly IRepositorioProveedores IRP;
        private readonly IRepositorioBienServicio IRBS;
        private readonly IRepositorioOrdenCompra IROC;
        private readonly IRepositorioTicket IRT;
        public ControladorExcel(IRepositorioTicket IRT,IRepositorioOrdenCompra IROC,IRepositorioBienServicio IRBS,IRepositorioProveedores IRP , InterfaceExcel Excel, IRepositorioCentroCosto IRC, IRepositorioOrdenesEstadisticas IRE, IRepositorioBienServicio IBS)
        {
            this.Excel = Excel;
            this.IRC = IRC;
            this.IRE = IRE;
            this.IBS = IBS;
            this.IRP = IRP;
            this.IRBS = IRBS;
            this.IROC = IROC;
            this.IRT = IRT;
        }

        /// <summary>
        /// Metodo interno para transpasar los Provedores de una base a otra
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("Proveedores")]
        public async Task<ActionResult> ExcelProveedores([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {

                    List<Proveedores> lista = (await Excel.LeerProveedores(Archivo));
                    foreach (Proveedores p in lista)
                    {
                        string res = await IRP.Existe(p.Rut_Proveedor, p.ID_Bien_Servicio);
                        if (res == "ok")
                            await IRP.NuevoProveedor(p);
                    }

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Metodo que permite leer el excel con el formato que agrega un proveedor a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<ActionResult> ExcelProveedor([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {

                 Proveedores P =(await Excel.LeerExcelProveedor(Archivo));
                    P.ID_Bien_Servicio = "0";
                    //Se crea el bien y servicio
                    await IRP.NuevoProveedor(P);
                    return Ok(true);


               
              
                }
                catch (Exception ex)
                {
                    // Manejar excepciones generales
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }

            /// <summary>
            /// Metodo que lee el archivo de los CentroCosto para agregarlos a la base de datos, tambien añade las ordenes estadisticas!!!
            /// </summary>
            /// <returns></returns>
        [HttpPost("CeCo")]
        public async Task<ActionResult> ExcelCeCo([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {
                    {
                        //string path = @"C:\Users\drako\Desktop\cap.xlsx";
                        var original = (await IRC.GetAllCeCo());
                        foreach (CentroCosto c in original)
                        {
                            await IRC.EliminarCeCo(c.Id_Ceco);
                        }


                        List<CentroCosto> lc = (await Excel.LeerExcelCeCo(Archivo));
                        foreach (CentroCosto cc in lc)
                        {
                            string res = await IRC.Existe(cc.Codigo_Ceco);
                            if (res == "ok")
                                await IRC.Nuevo_CeCo(cc);

                            else
                            {
                                cc.Activado = true;
                                await IRC.ModificarCeCo(cc);
                            }
                        }

                        return Ok(true);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Metodo que lee un archivo excel que tiene orden de compra y lo actualiza en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("OCA")]
        public async Task<ActionResult> ActualizarOC([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {
                    //string path = @"C:\Users\drako\Desktop\OrdenCompra.xls";

                    return Ok(await Excel.ActualizarOC(Archivo));

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Metodo para agregar los Bien y Servicios mediante un excel que tenga dos columnas
        /// </summary>
        /// <returns></returns>
        [HttpPost("BS")]
        public async Task<ActionResult> ExcelBS([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();
                try
                {
                    //string path = @"C:\Users\drako\Desktop\BienServicio.xlsx";
                    List<BienServicio> lista = (await Excel.LeerBienServicio(Archivo));
                    foreach (BienServicio bs in lista)
                    {
                        string res = await IBS.Existe(bs.Bien_Servicio);
                        if (res == "ok")
                            await IBS.NuevoBienServicio(bs);
                    }

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }

            }
        }
        /// <summary>
        /// Añade las ordenes de compra a los tickets,
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("poss")]
        public async Task<ActionResult> ExcelPos([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] Archivo = memoryStream.ToArray();

                try
                {
                    //string path = @"C:\Desktop\BienServicio.xlsx";
                    List<OrdenCompra> lista = (await Excel.LeerExcelOC(Archivo));
                    foreach (OrdenCompra OC in lista)
                    {
                        Ticket T = await IRT.GetTicket((int)OC.Id_Ticket);
                        bool cont = true;
                        T.Estado = "Espera liberacion";
                        await IRT.ModificarTicket(T);
                        await IROC.NuevoOC(OC);



                    }

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
                }
            }
        }

      
    }
}
