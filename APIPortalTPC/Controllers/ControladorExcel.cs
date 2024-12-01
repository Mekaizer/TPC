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
        public ControladorExcel(IRepositorioProveedores IRP , InterfaceExcel Excel, IRepositorioCentroCosto IRC, IRepositorioOrdenesEstadisticas IRE, IRepositorioBienServicio IBS)
        {
            this.Excel = Excel;
            this.IRC = IRC;
            this.IRE = IRE;
            this.IBS = IBS;
            this.IRP = IRP;
        }

        [HttpPost("Proveedores")]
        public async Task<ActionResult> ExcelProveedores(FormDataArchivo FDA)
        {
            try
            {
                byte[] Archivo =  Subir(FDA);
                if (Archivo == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound , "Archivo vacio!");
                }

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

        /// <summary>
        /// Metodo que permite leer el excel con el formato que agrega un proveedor a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("Proveedor")]
        public async Task<ActionResult> ExcelProveedor(FormDataArchivo FDA)
        {
            try
            {
                //string path = @"C:\Users\drako\Desktop\PRO4.xlsx";
                byte[] Archivo = Subir(FDA);

                return Ok(await Excel.LeerExcelProveedor(Archivo));
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Metodo que lee el archivo de los CentroCosto para agregarlos a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("CeCo")]
        public async Task<ActionResult> ExcelCeCo(FormDataArchivo FDA)
        {
            try
            {
                //string path = @"C:\Users\drako\Desktop\cap.xlsx";
                byte[] Archivo = Subir(FDA);

                List<CentroCosto> lc= (await Excel.LeerExcelCeCo(Archivo));
                foreach(CentroCosto cc in lc)
                {
                    string res = await IRC.Existe(cc.Codigo_Ceco);
                    if (res == "ok")
                        await IRC.Nuevo_CeCo(cc);
                }

                return Ok(true);            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }
          
        }
        /// <summary>
        /// Metodo que lee un archivo excel que tiene orden de compra y lo actualiza en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("OCA")]
        public async Task<ActionResult> ActualizarOC(FormDataArchivo FDA)
        {
            try
            {
                //string path = @"C:\Users\drako\Desktop\OrdenCompra.xls";

                byte[] Archivo = Subir(FDA);
                return Ok(await Excel.ActualizarOC(Archivo));
            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }

        }
        /// <summary>
        /// Metodo para agregar los Bien y Servicios mediante un excel que tenga dos columnas
        /// </summary>
        /// <returns></returns>
        [HttpPost("BS")]
        public async Task<ActionResult> ExcelBS(FormDataArchivo FDA)
        {
            try
            {
                byte[] Archivo = Subir(FDA);
                //string path = @"C:\Users\drako\Desktop\BienServicio.xlsx";
                List<BienServicio> lista= (await Excel.LeerBienServicio(Archivo));
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

        public byte[] Subir(FormDataArchivo model)
        {
            byte[] Subido;
            if (model.Archivo == null || model.Archivo.Length == 0)
            {

                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                model.Archivo.CopyTo(memoryStream);
                Subido = memoryStream.ToArray();

            }

            return (Subido);
        }
    }
}
