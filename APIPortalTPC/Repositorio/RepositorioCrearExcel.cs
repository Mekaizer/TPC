using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using OfficeOpenXml;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCrearExcel : InterfaceCrearExcel
    {
        /// <summary>
        /// Metodo que va a imprimir un excel usando la lista de orden compra ya filtrada
        /// </summary>
        /// <param name="LOC"></param>
        /// <returns></returns>
        public async Task<string> DescargarExcel(List<OrdenCompra> LOC)
        {
            // Crear un nuevo archivo Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("OrdenCompras");

                // Encabezado
                worksheet.Cells[1, 1].Value = "Ticket";
                worksheet.Cells[1, 2].Value = "Solped";
                worksheet.Cells[1, 3].Value = "Orden Estadistico";
                worksheet.Cells[1, 4].Value = "Posición";
                worksheet.Cells[1, 5].Value = "Orden de compra";
                worksheet.Cells[1, 6].Value = "Fecha Recepcion";

                // Filas
                int row = 2;
                foreach (var OC in LOC)
                {
                    worksheet.Cells[row, 1].Value = OC.Id_Ticket;
                    worksheet.Cells[row, 2].Value = OC.Solped;
                    worksheet.Cells[row, 3].Value = OC.Numero_OC;
                    worksheet.Cells[row, 4].Value = OC.posicion;
                    worksheet.Cells[row, 5].Value = OC.Id_Orden_Compra;
                    worksheet.Cells[row, 6].Value = OC.Fecha_Recepcion;
                    worksheet.Cells[row, 6].Style.Numberformat.Format = "yyyy-MM-dd";
                    row++;
                }
                string filePath = "C:/Users/drako/Desktop/OrdenCompras.xlsx";

                // Guardar el archivo en la ruta especificada
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);

               
            }
            return "Archivo Excel guardado en: ";
        }
    }
}
