using BaseDatosTPC;
using ClasesBaseDatosTPC;
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
                worksheet.Cells[1, 5].Value = "Ciclicidad";
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
    
        public async Task<string> DescargarExcel(List<Cotizacion> LC)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaCotizacion");

                // Encabezado
                worksheet.Cells[1, 1].Value = "ID Cotizacion";
                worksheet.Cells[1, 2].Value = "Solicitante";
                worksheet.Cells[1, 3].Value = "Fecha de Creacion de la cotizacion";
                worksheet.Cells[1, 4].Value = "Estado";
                worksheet.Cells[1, 5].Value = "Detalle";
                worksheet.Cells[1, 6].Value = "Solped";
                worksheet.Cells[1, 7].Value ="Bien y/o servicio";                
                int row = 2;
                foreach (var OC in LC)
                {
                    worksheet.Cells[row, 1].Value = OC.ID_Cotizacion;
                    worksheet.Cells[row, 2].Value = OC.Id_Solicitante;
                    worksheet.Cells[row, 3].Value = OC.Fecha_Creacion_Cotizacion;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "yyyy-MM-dd";
                    worksheet.Cells[row, 4].Value = OC.Estado;
                    worksheet.Cells[row, 5].Value = OC.Detalle;
                    worksheet.Cells[row, 6].Value = OC.Solped;
                    worksheet.Cells[row, 7].Value = OC.ID_Bien_Servicio;
                    row++;
                }
                string filePath = "C:/Users/drako/Desktop/ListaCotizacion.xlsx";

                // Guardar el archivo en la ruta especificada
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);



            }

                return "listo";
        }
        public async Task<string> DescargarExcel(List<Usuario> LU)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaUsuario");

                // Encabezado
                worksheet.Cells[1, 1].Value = "ID Usuario";
                worksheet.Cells[1, 2].Value = "Nombre Usuario";
                worksheet.Cells[1, 3].Value = "Apellido Paterno";
                worksheet.Cells[1, 4].Value = "Apellido Materno";
                worksheet.Cells[1, 5].Value = "Rut";
                worksheet.Cells[1, 6].Value = "Correo Usuario";
                worksheet.Cells[1, 7].Value = "Contraseña Usuario";
                worksheet.Cells[1, 8].Value = "Activado";
                worksheet.Cells[1, 9].Value = "Tipo Liberador";
                worksheet.Cells[1, 10].Value = "En vacaciones";
                worksheet.Cells[1, 11].Value = "Admin";
                int row = 2;
                foreach (var U in LU)
                {
                    worksheet.Cells[1, 1].Value = U.Id_Usuario;
                    worksheet.Cells[1, 2].Value = U.Nombre_Usuario;
                    worksheet.Cells[1, 3].Value = U.Apellido_paterno;
                    worksheet.Cells[1, 4].Value = U.Apellido_materno;
                    worksheet.Cells[1, 5].Value = U.Rut_Usuario;
                    worksheet.Cells[1, 6].Value = U.Correo_Usuario;
                    worksheet.Cells[1, 7].Value = U.Contraseña_Usuario;
                    worksheet.Cells[1, 8].Value = U.Activado;
                    worksheet.Cells[1, 9].Value = U.Tipo_Liberador;
                    worksheet.Cells[1, 10].Value = U.En_Vacaciones;
                    worksheet.Cells[1, 11].Value = U.Admin;
                    row++;
                }
                string filePath = "C:/Users/drako/Desktop/ListaUsuario.xlsx";

                // Guardar el archivo en la ruta especificada
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);


            }

            return "listo";
        }
        public async Task<string> DescargarExcel(List<BienServicio> LBS)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ListaBienServicio");

                // Encabezado
                worksheet.Cells[1, 1].Value = "ID Bien Servicio";
                worksheet.Cells[1, 2].Value = "Nombre Bien Servicio";

                int row = 2;
                foreach (var BS in LBS)
                {
                    worksheet.Cells[1, 1].Value = BS.ID_Bien_Servicio;
                    worksheet.Cells[1, 2].Value = BS.Bien_Servicio;
                    row++;
                }
                string filePath = "C:/Users/drako/Desktop/ListaBienServicio.xlsx";

                // Guardar el archivo en la ruta especificada
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);


            }

            return "listo";
        }
    }
}
