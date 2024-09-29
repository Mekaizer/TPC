using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioReemplazos : IRepositorioReemplazos
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioReemplazos(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Reemplazos> NuevoReemplazos(Reemplazos R)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Reemplazos (Reemplazos) VALUES (@Reemplazos); SELECT SCOPE_IDENTITY() AS ID_Reemplazos";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Rut_Usuario_Vacaciones", SqlDbType.Int).Value = R.Rut_Usuario_Vacaciones;
                Comm.Parameters.Add("@Rut_Usuario_Reemplazante", SqlDbType.Int).Value = R.Rut_Usuario_Reemplazante;
                Comm.Parameters.Add("@Comentario", SqlDbType.VarChar).Value = R.Comentario;
                Comm.Parameters.Add("@Fecha_Retorno", SqlDbType.DateTime).Value = R.Fecha_Retorno;
                R.ID_Reemplazos = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Reemplazos " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return R;
        }

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Reemplazos> GetReemplazo(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Reemplazos R = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Reemplazos where ID_Reemplazos = @ID_Reemplazos";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Reemplazos", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    R.ID_Reemplazos = Convert.ToInt32(reader["ID_Reemplazos"]);
                    R.Rut_Usuario_Vacaciones = Convert.ToInt32(reader["Rut_Vacaciones"]);
                    R.Rut_Usuario_Reemplazante = Convert.ToInt32(reader["Rut_Reemplazante"]);
                    R.Comentario = Convert.ToString(reader["Comentario"]);
                    R.Fecha_Retorno = (DateTime)reader["Fecha_Retorno"];
                  
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Reemplazos " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return R;
        }
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Reemplazos>> GetAllRemplazos()
        {
            List<Reemplazos> lista = new List<Reemplazos>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Reemplazos"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Reemplazos R = new();
                    R.ID_Reemplazos = Convert.ToInt32(reader["ID_Reemplazos"]);
                    R.Rut_Usuario_Vacaciones = Convert.ToInt32(reader["Rut_Vacaciones"]);
                    R.Rut_Usuario_Reemplazante = Convert.ToInt32(reader["Rut_Reemplazante"]);
                    R.Comentario = Convert.ToString(reader["Comentario"]);
                    R.Fecha_Retorno = (DateTime)reader["Fecha_Retorno"];

                    lista.Add(R);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Reemplazos " + ex.Message);
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

        //Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        public async Task<Reemplazos> ModificarReemplazos(Reemplazos R)
        {
            Reemplazos Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Reemplazos SET Reemplazos = @Reemplazos WHERE ID_Reemplazos = @ID_Reemplazos";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@ID_Reemplazos", SqlDbType.Int).Value = R.ID_Reemplazos;
                Comm.Parameters.Add("@Rut_Usuario_Vacaciones", SqlDbType.Int).Value = R.Rut_Usuario_Vacaciones;
                Comm.Parameters.Add("@Rut_Usuario_Reemplazante", SqlDbType.Int).Value = R.Rut_Usuario_Reemplazante;
                Comm.Parameters.Add("@Comentario", SqlDbType.VarChar).Value = R.Comentario;
                Comm.Parameters.Add("@Fecha_Retorno", SqlDbType.DateTime).Value = R.Fecha_Retorno;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetReemplazo(Convert.ToInt32(reader["ID_Reemplazos"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el reemplazo " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Rmod;
        }

    }
}