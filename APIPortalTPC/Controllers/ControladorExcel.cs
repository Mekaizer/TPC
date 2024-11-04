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
        public ControladorExcel(InterfaceExcel Excel, IRepositorioCentroCosto IRC, IRepositorioOrdenesEstadisticas IRE)
        {
            this.Excel = Excel;
            this.IRC = IRC;
            this.IRE = IRE;
        }
        
        /// <summary>
        /// Metodo que permite leer el excel con el formato que agrega un proveedor a la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpPost("Proveedores")]
        public async Task<ActionResult> ExcelProveedores()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop\PRO4.xlsx";

                return Ok(await Excel.LeerExcelProveedor(path));
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
        public async Task<ActionResult> ExcelCeCo()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop\cap.xlsx";


                List<CentroCosto> lc= (await Excel.LeerExcelCeCo(path));
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
        [HttpPost("OC")]
        public async Task<ActionResult> ExcelOC()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop\OrdenCompra.xls";


                return Ok(await Excel.LeerExcelOC(path));
            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }

        }
    }
}
