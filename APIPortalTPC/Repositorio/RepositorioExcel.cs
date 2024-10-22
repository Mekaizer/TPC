using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using ClasesBaseDatosTPC;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioExcel : InterfaceExcel

    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioExcel(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }

        /// <summary>
        /// Metodo que realiza la conexión a la base de datos
        /// </summary>
        /// <returns>La conexión</returns>
        private SqlConnection conectar()
        {
            return new SqlConnection(Conexion);
        }

       
        public Task<string> LeerExcel(string filePath)
        {
            // Establecer el contexto de la licencia
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Ruta al archivo Excel
     
            // Verificar que el archivo exista
            if (File.Exists(filePath))
            {
                // Cargar el archivo Excel
                FileInfo fileInfo = new FileInfo(filePath);

                // Usar EPPlus para leer el archivo Excel
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    // Seleccionar la primera hoja del archivo
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Leer una celda específica (por ejemplo, B2)
                    string valorCeldaC19 = worksheet.Cells["C19"].Text; //RUT PROVEEDOR CELDA C19
                    string valorCeldaC17 = worksheet.Cells["C17"].Text; // RAZON SOCIAL CELDA C17 
                    string valorCeldaC13 = worksheet.Cells["C17"].Text;  // NOMBRE FANTASIA CELDA C17
                    string valorCeldaC23 = worksheet.Cells["C23"].Text;   // DIRRECION  CELDA C23
                    string valorCeldaC25 = worksheet.Cells["C25"].Text;  // COMUNA CELDA C25
                    string valorCeldaC27 = worksheet.Cells["C27"].Text;  // CORREO PROVEEDOR CELDA C27
                    string valorCeldaC21 = worksheet.Cells["C21"].Text;  // TELEFONO PROVEEDOR CELDA C21
                    string valorCeldaC35 = worksheet.Cells["C35"].Text;  // CARGO REPRESENTANTE CELDA C35
                    string valorCeldaC31 = worksheet.Cells["C31"].Text;  // NOMBRE REPRESENTANTE CELDA C31
                    string valorCeldaF33 = worksheet.Cells["F33"].Text;  // EMAIL DE REPRESENTANTE CELDA F33
                    string valorCeldaC57 = worksheet.Cells["C57"].Text;  // NUMERO DE CUENTA CELDA C57
                    string valorCeldaC51 = worksheet.Cells["C51"].Text;  // BANCO CELDA C51
                    string valorCeldaC55 = worksheet.Cells["C55"].Text;  // SWIFT 1 CELDA C55
                    string valorCeldaC67 = worksheet.Cells["C67"].Text;  // SWIFT 2 CELDA C67




                    // Imprimir el valor de la celda B2
                    throw new NotImplementedException();

                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    
        }

    }
}
