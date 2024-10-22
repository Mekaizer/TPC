using APIPortalTPC.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ControladorExcel :ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly InterfaceExcel Excel;
        public ControladorExcel(InterfaceExcel Excel)
        {
            this.Excel = Excel;
        }

        public async Task<ActionResult> ExcelProveedores()
        {
            try
            {
                string path = @"C:\Users\drako\Desktop";

                return Ok(await Excel.LeerExcel(path));
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error: " + ex.Message);
            }
        }
    }
}
