using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using ClasesBaseDatosTPC;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioExcel : InterfaceExcel
    {
    public async Task<Proveedores> LeerExcelProveedor(string filePath)
    {
        // Establecer el contexto de la licencia
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        Proveedores P = new Proveedores();
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

                P.Rut_Proveedor = worksheet.Cells["C19"].Text; //RUT PROVEEDOR CELDA C19
                P.Razon_Social = worksheet.Cells["C17"].Text; // RAZON SOCIAL CELDA C17 
                P.Nombre_Fantasia = worksheet.Cells["C17"].Text;  // NOMBRE FANTASIA CELDA C17
                P.Direccion = worksheet.Cells["C23"].Text;   // DIRRECION  CELDA C23
                P.Comuna = worksheet.Cells["C25"].Text;  // COMUNA CELDA C25
                P.Correo_Proveedor = worksheet.Cells["C27"].Text;  // CORREO PROVEEDOR CELDA C27
                if (worksheet.Cells["C21"].Value != null) P.Telefono_Proveedor = worksheet.Cells["C21"].Value.ToString();  // TELEFONO PROVEEDOR CELDA C21
                P.Cargo_Representante = worksheet.Cells["C35"].Text;  // CARGO REPRESENTANTE CELDA C35
                P.Nombre_Representante = worksheet.Cells["C31"].Text;  // NOMBRE REPRESENTANTE CELDA C31
                P.Email_Representante = worksheet.Cells["F33"].Text;  // EMAIL DE REPRESENTANTE CELDA F33
                if (worksheet.Cells["C57"].Value != null) P.N_Cuenta = worksheet.Cells["C57"].Value.ToString();  // NUMERO DE CUENTA CELDA C57
                P.Banco = worksheet.Cells["C51"].Text;  // BANCO CELDA C51
                P.Swift = worksheet.Cells["C55"].Text;  // SWIFT 1 CELDA C55
                P.ID_Bien_Servicio = "1";
            }

            return P;

        }
        else
        {
            throw new Exception("Error a la hora de leer el excel");
        }
    }

    public async Task<List<CentroCosto>> LeerExcelCeCo(string filePath)
    {
        var centrosCostos = new List<CentroCosto>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];

            int rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                string Namea = "";
                    if (worksheet.Cells[row, 2].Value is string)
                {
                    Namea = worksheet.Cells[row, 2].Value.ToString();

                }

                var centroCosto = new CentroCosto
                {
                    Codigo_Ceco = worksheet.Cells[row, 1].Value.ToString(),
                        
                    Nombre = Namea
                };

                centrosCostos.Add(centroCosto);
            }
        }

        return centrosCostos;
    }


    public async Task<List<OrdenesEstadisticas>> LeerExcelOE(string filePath)
    {
        var ListaOE = new List<OrdenesEstadisticas>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        if (!File.Exists(filePath))
        {
                throw new Exception("Error en leer los datos");
        }

        // Inicializar NPOI para manejar archivos .xls
        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            // Crear el objeto Workbook para manejar el archivo
            IWorkbook workbook = new HSSFWorkbook(file);

            // Obtener la primera hoja de trabajo
            ISheet sheet = workbook.GetSheetAt(0);

            // Leer las columnas B, C, D, E hacia abajo desde la fila 5
            int rowCount = sheet.LastRowNum;
            for (int row = 4; row <= rowCount; row++) // Comienza desde fila 5 (row 4 en índice 0)
            {
                IRow currentRow = sheet.GetRow(row);
                if (currentRow == null) continue; // Saltar filas vacías

                // Leer cada columna y mostrar su contenido

                string colB = currentRow.GetCell(1)?.ToString() ?? ""; // Columna B
                string colC = currentRow.GetCell(2)?.ToString() ?? ""; // Columna C
                string colD = currentRow.GetCell(3)?.ToString() ?? ""; // Columna D

                // Verificar si todas las celdas están vacías, y detener el bucle si es necesario
                if (string.IsNullOrEmpty(colB)  && string.IsNullOrEmpty(colC) && string.IsNullOrEmpty(colD) )
                    break;
                else
                {
                    var OE = new OrdenesEstadisticas
                    {
                        Nombre = colC,

                        Codigo_OE = colB,

                        Id_Centro_de_Costo = colD

                    };
                    ListaOE.Add(OE);
                }

            }
        }
        return ListaOE;
    }
    }
}


/*
 *

 */