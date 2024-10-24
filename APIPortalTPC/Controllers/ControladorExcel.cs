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
        [HttpPost("CeCo")]
        public async Task<ActionResult> ExcelCeCo()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop\excel.xlsx";


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
        [HttpPost("OE")]
        public async Task<ActionResult> ExcelOE()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop\cap.xls";


                List<OrdenesEstadisticas> lc = (await Excel.LeerExcelOE(path));
                
                foreach(OrdenesEstadisticas oe in lc)
                {
                OrdenesEstadisticas OEA=   await IRE.AjustarCodigo(oe);
                string res = await IRE.Existe(OEA.Codigo_OE);
                    if (res == "ok")
                        await IRE.NuevoOE(OEA);
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
