using BaseDatosTPC;
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaOrdenCompras");

                // Encabezado
                worksheet.Cells[1, 1].Value = "Ticket";
                worksheet.Cells[1, 2].Value = "Numero de Orden Compra";
                worksheet.Cells[1, 3].Value = "Fecha de Recepcion";
                worksheet.Cells[1, 4].Value = "Texto";
                worksheet.Cells[1, 5].Value = "Ciclico?";
                worksheet.Cells[1, 6].Value = "Posicion";
                worksheet.Cells[1, 7].Value = "Cantidad"; 
                worksheet.Cells[1, 8].Value = "Moneda";
                worksheet.Cells[1, 9].Value = "Precio Neto";
                worksheet.Cells[1, 10].Value = "Proveedor";
                worksheet.Cells[1, 11].Value = "Material";
                worksheet.Cells[1, 12].Value = "Valor Neto";
                worksheet.Cells[1, 13].Value = "Recepcionada";
                // Filas
                int row = 2;
                foreach (var OC in LOC)
                {
                    worksheet.Cells[row, 1].Value = OC.Id_Ticket;
                    worksheet.Cells[row, 2].Value = OC.Numero_OC;
                    worksheet.Cells[row, 3].Value = OC.Fecha_Recepcion;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "yyyy-MM-dd";
                    worksheet.Cells[row, 4].Value = OC.Texto;
                    if (OC.IsCiclica == true)
                        worksheet.Cells[row, 5].Value = "Si";
                    else
                        worksheet.Cells[row, 5].Value = "No";
                    worksheet.Cells[row, 6].Value = OC.posicion;
                    worksheet.Cells[row, 7].Value = OC.Cantidad;
                    worksheet.Cells[row, 8].Value = OC.Mon;
                    worksheet.Cells[row, 9].Value = OC.PrcNeto;
                    worksheet.Cells[row, 10].Value = OC.Proveedor;
                    worksheet.Cells[row, 11].Value = OC.Material;
                    worksheet.Cells[row, 12].Value = OC.ValorNeto;
                    if(OC.Recepcion == true)
                        worksheet.Cells[row, 13].Value = "Si";
                    else
                        worksheet.Cells[row, 13].Value = "No";
                    row++;
                }
                string filePath = "C:/Users/drako/Desktop/ListaOrdenCompras.xlsx";

                // Guardar el archivo en la ruta especificada
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);

               
            }
            return "Archivo Excel guardado ";
        }
    
    
    
    }
}
