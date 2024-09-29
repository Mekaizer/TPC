using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioTicket : IRepositorioTicket
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioTicket(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Ticket> NewTicket(Ticket T)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Ticket (Ticket) VALUES (@Ticket); SELECT SCOPE_IDENTITY() AS ID_Ticket";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_OC", SqlDbType.Int).Value = T.Id_OC;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                Comm.Parameters.Add("@Fecha_Creacion_OC", SqlDbType.DateTime).Value = T.Fecha_Creacion_OC;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = T.Id_Usuario;
                Comm.Parameters.Add("@Id_Proveedor", SqlDbType.Int).Value = T.ID_Proveedor;
                Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = T.Fecha_OC_Enviada;
                Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = T.Fecha_OC_Liberada;
                T.ID_Ticket  = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Ticket " + ex.Message);
            }
            finally
            { 
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return T;
        }

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Ticket> GetTicket(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Ticket T = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Ticket where ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    T.Id_OC = Convert.ToInt32(reader["Id_OC"]);
                    T.Estado = Convert.ToString(reader["Estado"]);
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario= Convert.ToInt32(reader["ID_Cotizacion"]);
                    T.ID_Proveedor = Convert.ToInt32(reader["ID_Proveedor"]);
                    T.Fecha_Ingreso_OC = (DateTime)reader["Fecha_Ingreso_OC"];
                    T.Fecha_OC_Enviada = (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return T;
        }
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Ticket>> GetAllTicket()
        {
            List<Ticket> lista = new List<Ticket>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Ticket"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Ticket T = new();
                    T.Id_OC = Convert.ToInt32(reader["Id_OC"]);
                    T.Estado = Convert.ToString(reader["Estado"]);
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToInt32(reader["ID_Cotizacion"]);
                    T.ID_Proveedor = Convert.ToInt32(reader["ID_Proveedor"]);
                    T.Fecha_Ingreso_OC = (DateTime)reader["Fecha_Ingreso_OC"];
                    T.Fecha_OC_Enviada = (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = Convert.ToString(reader["Detalle"]);
                    lista.Add(T);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Ticket " + ex.Message);
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
        public async Task<Ticket> ModificarTicket(Ticket T)
        {
            Ticket Tmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Ticket SET Ticket = @Ticket WHERE ID_Ticket = @ID_Ticket";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_OC", SqlDbType.Int).Value = T.Id_OC;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                Comm.Parameters.Add("@Fecha_Creacion_OC", SqlDbType.DateTime).Value = T.Fecha_Creacion_OC;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = T.Id_Usuario;
                Comm.Parameters.Add("@Id_Proveedor", SqlDbType.Int).Value = T.ID_Proveedor;
                Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = T.Fecha_OC_Enviada;
                Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = T.Fecha_OC_Liberada;
                Comm.Parameters.Add("@ID_Ticket", SqlDbType.Int).Value = T.ID_Ticket;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Tmod = await GetTicket(Convert.ToInt32(reader["ID_Ticket"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Ticket " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Tmod;
        }

    }
}