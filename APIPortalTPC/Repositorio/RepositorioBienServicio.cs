using BaseDatosTPC;
using APIPortalTPC.Datos;
using System.Data.SqlClient;
using System.Data;
namespace APIPortalTPC.Repositorio
{
    public class RepositorioBienServicio : IRepositorioBienServicio
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioBienServicio(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<BienServicio> GetServicio(int id) {
            //Parametro para guardar el objeto a mostrar
            BienServicio bs = new BienServicio();
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
                Comm.CommandText = "SELECT * FROM dbo.Bien_Servicio where ID_Bien_Servicio = @ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = id;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    if (reader["ID_Bien_Servicio"] == System.DBNull.Value)
                    {
                        bs.ID_Bien_Servicio = 0;
                    }
                    else
                    {
                        bs.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    }
                    if (reader["Bien_Servicio"] == System.DBNull.Value)
                    {
                        bs.Bien_Servicio = " ";
                    }
                    else
                    {
                        bs.Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]);
                    }       
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return bs;
        }
        //Metodo que retorna una lista con los objetos
        public async Task<IEnumerable<BienServicio>> GetAllServicio()
        {
            List<BienServicio> lista = new List<BienServicio>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Bien_Servicio"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();
                //acontinuacion se procede a pasar los datos a una clase y luego se guardan en una lista
                while (reader.Read())
                {
                    BienServicio bs = new BienServicio();
                    if (reader["ID_Bien_Servicio"] == System.DBNull.Value)
                    {
                        bs.ID_Bien_Servicio = 0;
                    }
                    else
                    {
                        bs.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    }
                    if (reader["Bien_Servicio"] == System.DBNull.Value)
                    {
                        bs.Bien_Servicio = " ";
                    }
                    else
                    {
                        bs.Bien_Servicio = Convert.ToString(reader["Bien_Servicio"]);
                    }
                    lista.Add(bs);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Bien_Servicio " + ex.Message);
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
        public async Task<BienServicio> ModificarBien_Servicio(BienServicio bs)
        {
            BienServicio bsmodificado = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Bien_Servicio SET Bien_Servicio = @Bien_Servicio WHERE ID_Bien_Servicio = @ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = bs.ID_Bien_Servicio;
                Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = bs.Bien_Servicio;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    bsmodificado = await GetServicio(Convert.ToInt32(reader["ID_Bien_Servicio"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el bien/servicio " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return bsmodificado;
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<BienServicio> NuevoBienServicio(BienServicio bs)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Bien_Servicio (Bien_Servicio) VALUES (@Bien_Servicio); SELECT SCOPE_IDENTITY() AS ID_Bien_Servicio";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = bs.Bien_Servicio;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                int id = (int)idDecimal;
                bs.ID_Bien_Servicio = id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Bien_Servicio " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return bs;
        }
    }    
}
