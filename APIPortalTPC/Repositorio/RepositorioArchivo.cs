using BaseDatosTPC;
using APIPortalTPC.Datos;
using System.Data.SqlClient;
using System.Data;
namespace APIPortalTPC.Repositorio
{
    public class RepositorioArchivo : IRepositorioArchivo
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;
        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioArchivo(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Archivo> GetArchivo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Archivo a = new Archivo();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand Comm = null;
            //parametro para leer los resultados de una consulta
            SqlDataReader reader = null;
            try
            {
                //Se crea la instancia con la conexion SQL para interactuar con la base de datos
                sql.Open();
                //se ejecuta la base de datos
                Comm = sql.CreateCommand();
                //se realiza la accion correspondiente en la base de datos
                //muestra los datos de la tabla correspondiente con sus condiciones
                Comm.CommandText = "SELECT * FROM dbo.Archivo where Id_Archivo = @Id_Archivo";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read()) {
                    a.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    a.Id_ArchivoCotizacion = Convert.ToInt32(reader["Id_Archivo"]);
                    a.IsPrincipal = Convert.ToBoolean(reader["IsPrincipal"]);
                    a.ArchivoDoc = (byte[])(reader["ArchivoDoc"]);
                    a.Grupo_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Archivo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return a;
        }
        //Metodo que retorna una lista con los Archivoa
        public async Task<IEnumerable<Archivo>> GetAllArchivo()
        {
            List<Archivo> lista = new List<Archivo>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Archivo"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Archivo a = new Archivo();
                    a.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    a.Id_ArchivoCotizacion = Convert.ToInt32(reader["Id_ArchivoCotizacion"]);
                    a.IsPrincipal = Convert.ToBoolean(reader["IsPrincipal"]);
                    a.ArchivoDoc = (byte[])reader["ArchivoDoc"];
                    a.Grupo_Archivo = Convert.ToInt32(reader["Grupo_Archivo"]);
                    lista.Add(a);
                }
            }   
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Archivo " + ex.Message);
            }
            finally
            {
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return lista;
        }
        //Pide un objetivo ya hecho para ser reemplazado por uno ya terminado
        public async Task<Archivo> ModificarArchivo(Archivo A)
        {
            Archivo Archmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Archivo SET Archivo = @Archivo WHERE Id_Archivo = @Id_Archivo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = A.Id_Archivo;
                Comm.Parameters.Add("@Id_ArchivoCotizacion", SqlDbType.Int, 50).Value = A.Id_ArchivoCotizacion;
                Comm.Parameters.Add("@IsPrincipal", SqlDbType.Bit, 50).Value = A.IsPrincipal;
                Comm.Parameters.Add("@ArchivoDoc", SqlDbType.VarBinary, -1).Value = A.ArchivoDoc;
                Comm.Parameters.Add("@Grupo_archivo",SqlDbType.Int).Value=A.Id_Archivo;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Archmod = await GetArchivo(Convert.ToInt32(reader["Id_Archivo"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Archivo " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Archmod;
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Archivo> NuevoArchivo(Archivo A)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Archivo (Archivo) VALUES (@Archivo); SELECT SCOPE_IDENTITY() AS Id_Archivo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_ArchivoCotizacion", SqlDbType.VarChar, 50).Value =A.Id_ArchivoCotizacion;
                Comm.Parameters.Add("@IsPrincipal", SqlDbType.Bit, 50).Value = A.IsPrincipal;
                Comm.Parameters.Add("@ArchivoDoc", SqlDbType.VarBinary, -1).Value = A.ArchivoDoc;
                Comm.Parameters.Add("@Id_ArchivoCotizacion", SqlDbType.VarChar, 50).Value = A.Id_ArchivoCotizacion;
                Comm.Parameters.Add("@Grupo_archivo", SqlDbType.Int).Value = A.Grupo_Archivo;
                A.Id_Archivo = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Archivo " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return A;
        }
    }

}

