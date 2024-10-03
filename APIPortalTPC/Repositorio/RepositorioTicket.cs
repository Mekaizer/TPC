using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioTicket : IRepositorioTicket
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioTicket(AccesoDatos CD)
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
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="T">Objeto del tipo Ticket que se va a agregar a la base de datos</param>
        /// <returns>Retorna el objeto a modificar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ticket> NewTicket(Ticket T)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Ticket " +
                    "(Id_OC,Estado,Fecha_Creacion_OC,Id_Usuario,Id_Proveedor,Fecha_OC_Enviada,Fecha_OC_Liberada) " +
                    "VALUES (@Id_OC,@Estado,@Fecha_Creacion_OC,@Id_Usuario,@Id_Proveedor,@Fecha_OC_Enviada,@Fecha_OC_Liberada); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Ticket";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_OC", SqlDbType.Int).Value = T.Id_OC;
                Comm.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = T.Estado;
                Comm.Parameters.Add("@Fecha_Creacion_OC", SqlDbType.DateTime).Value = T.Fecha_Creacion_OC;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = T.Id_Usuario;
                Comm.Parameters.Add("@Id_Proveedor", SqlDbType.Int).Value = T.ID_Proveedor;
                Comm.Parameters.Add("@Fecha_OC_Enviada", SqlDbType.DateTime).Value = T.Fecha_OC_Enviada;
                Comm.Parameters.Add("@Fecha_OC_Liberada", SqlDbType.DateTime).Value = T.Fecha_OC_Liberada;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                T.ID_Ticket  = (int)idDecimal;
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

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto Ticket a buscar</param>
        /// <returns>Retorna el objeto Ticket que se busca</returns>
        /// <exception cref="Exception"></exception>
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
                    T.Estado = (Convert.ToString(reader["Estado"])).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario= Convert.ToInt32(reader["ID_Cotizacion"]);
                    T.ID_Proveedor = Convert.ToInt32(reader["ID_Proveedor"]);
                    T.Fecha_Ingreso_OC = (DateTime)reader["Fecha_Ingreso_OC"];
                    T.Fecha_OC_Enviada = (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = (Convert.ToString(reader["Detalle"])).Trim();
                    T.ID_Ticket=Convert.ToInt32(reader["ID_Ticket"]);
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
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna lista con todos los objetos Ticket de la base de datos</returns>
        /// <exception cref="Exception"></exception>
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
                    T.Estado = (Convert.ToString(reader["Estado"])).Trim();
                    T.Fecha_Creacion_OC = (DateTime)reader["Fecha_Creacion_OC"];
                    T.Id_Usuario = Convert.ToInt32(reader["ID_Cotizacion"]);
                    T.ID_Proveedor = Convert.ToInt32(reader["ID_Proveedor"]);
                    T.Fecha_Ingreso_OC = (DateTime)reader["Fecha_Ingreso_OC"];
                    T.Fecha_OC_Enviada = (DateTime)reader["Fecha_OC_Enviada"];
                    T.Fecha_OC_Liberada = (DateTime)reader["Fecha_OC_Liberada"];
                    T.Detalle = (Convert.ToString(reader["Detalle"])).Trim();
                    T.ID_Ticket = Convert.ToInt32(reader["ID_Ticket"]);
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

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="T">Objeto del tipo Ticket que se quiere modificar</param>
        /// <returns>Retorna el objeto modificado</returns>
        /// <exception cref="Exception"></exception>
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
                Comm.CommandText = "UPDATE dbo.Ticket SET " +
                    "Id_OC = @Id_OC " +
                    "Estado = @Estado " +
                    "Fecha_Creacion_OC = @Fecha_Creacion_OC " +
                    "Id_Usuario = @Id_Usuario " +
                    "Id_Proveedor = @Id_Proveedor " +
                    "Fecha_OC_Enviada = @Fecha_OC_Enviada " +
                    "Fecha_OC_Liberada = @Fecha_OC_Liberada " +
                    "WHERE ID_Ticket = @ID_Ticket";
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