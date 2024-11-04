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
using NPOI.SS.Formula.Functions;

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
        /// <summary>
        /// Metodo que lee el archivo excel que posee un proveedor, retorna un objeto Proveedores
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Metodo que lee el excel con los centros de costo para ser añadidos a una lista de objeto CentroCosto
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Metodo que lee el excel de ordenes de compras y actualiza la base de datos para cambiar los procesos
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> LeerExcelOC(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        if (!File.Exists(filePath))
        {
                throw new Exception("Error en leer los datos");
        }

        // Variables para almacenar datos de las columnas C, G y Q
        string columnaC ;
        string columnaG ;
        string columnaQ ;
        // Cargar el archivo Excel
        FileInfo fileInfo = new FileInfo(filePath);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Primera hoja del archivo

            // Leer las columnas C, G y Q al mismo tiempo (empieza desde la fila 5)
            int row = 5;
            while (worksheet.Cells[row, 3].Value != null || worksheet.Cells[row, 17].Value != null || worksheet.Cells[row,7].Value != null) // Columnas C (3) y Q (17)
            {
                    // Agregar el valor de la columna C (Documentos) o una cadena vacía si es nulo
                    columnaC = worksheet.Cells[row, 3].Value.ToString() ?? string.Empty;

                    //Agregar el valor de la columna G (Nombre de) o una cadena nula si es vacía si es nulo
                    columnaG= worksheet.Cells[row, 7].Text ?? string.Empty;

                    // Agregar el valor de la columna Q (Denominación StatLib) o una cadena vacía si es nulo
                    columnaQ = worksheet.Cells[row, 17].Text ?? string.Empty;
                    DateTime hoy = (DateTime.Today);
                    row++;

                    SqlConnection sqlConexion = conectar();
                    SqlCommand? Comm = null;
                    SqlDataReader reader = null;
                    try
                    {
                        sqlConexion.Open();
                        Comm = sqlConexion.CreateCommand();
                        Comm.CommandText = "UPDATE dbo.Ticket SET " +
                            "Estado =@Estado," +
                            "Fecha_OC_Liberada = @Fecha_OC_Liberada " +
                            "WHERE Numero_OC = @Numero_OC";
                        Comm.CommandType = CommandType.Text;
                        reader = await Comm.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = int.Parse(columnaC);
                            Comm.Parameters.Add("@Estado", SqlDbType.Int).Value = columnaQ;
                            if (columnaQ == "Liberación concluida")
                            {
                                Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.Int).Value = hoy;

                            }
                        }
                    }
                    catch(Exception ex) { 
                         throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
                    }
                    finally
                    {
                        reader?.Close();
                        Comm?.Dispose();
                        sqlConexion.Close();
                        sqlConexion.Dispose();
                    }

            }
        }

        return "listo";
        }
    }
}

