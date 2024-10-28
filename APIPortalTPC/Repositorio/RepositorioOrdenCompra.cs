using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioOrdenCompra : IRepositorioOrdenCompra
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioOrdenCompra(AccesoDatos CD)
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
        /// <param name="OC">Objeto OrdenCompra a añadir a la base de datos</param>
        /// <returns>El objeto OrdenCompra a añadir</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> NuevoOC(OrdenCompra OC)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Orden_de_Compra " +
                    "(Numero_OC,Solped,Id_OE,Posicion,Id_Ticket) " +
                    "VALUES (@Numero_OC,@Solped,@Id_OE,@Posicion,@Id_Ticket); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Orden_Compra";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Solped", SqlDbType.Int).Value = OC.Solped;
                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = OC.Id_OE;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar, 10).Value = OC.posicion;
                Comm.Parameters.Add("@Id_Ticket", SqlDbType.Int).Value = OC.Id_Ticket;

                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                OC.Id_Orden_Compra = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Orden de compra " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OC;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id que pertenece al objeto OrdenCompra a buscar</param>
        /// <returns>Retorna el objeto cuya Id coincide con el pedido</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> GetOC(int id)
        {
            //Parametro para guardar el objeto a mostrar
            OrdenCompra oc = new OrdenCompra();
            //Se realiza la conexion a la base de datos
            SqlConnection sql = conectar();
            //parametro que representa comando o instrucion en SQL para ejecutarse en una base de datos
            SqlCommand? Comm = null;
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
                Comm.CommandText = @"SELECT OC.*, OE.Nombre, U1.Nombre_Usuario AS PrimerUsuario ,U2.Nombre_Usuario AS SegundoUsuario , U3.Nombre_Usuario AS TercerUsuario 
                FROM dbo.Orden_de_Compra OC 
                LEFT OUTER JOIN dbo.Ordenes_estadisticas OE ON OE.Id_Orden_Estadistica = OC.ID_OE
                LEFT OUTER JOIN dbo.Usuario U1 ON OC.UsuarioRecepcionador = U1.Id_Usuario 
                LEFT OUTER JOIN dbo.Usuario U2 ON OC.SegundoUsuarioRecepcionador = U2.Id_Usuario 
                LEFT OUTER JOIN dbo.Usuario U3 ON OC.TercerUsuarioRecepcionador = U3.Id_Usuario
                where OC.Id_Orden_Compra = @Id_Orden_Compra";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Orden_Compra", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    oc.Numero_OC = Convert.ToInt32(reader["Numero_OC"]);
                    oc.Solped = reader["Solped"] is DBNull ? 0 : Convert.ToInt32(reader["Solped"]);
                    oc.Id_OE = reader["Nombre"] is DBNull ? " " : Convert.ToString(reader["Nombre"]).Trim();
                    oc.posicion = Convert.ToString(reader["Posicion"]).Trim();
                    oc.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    oc.Fecha_Recepcion = reader["Fecha_Recepcion"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_Recepcion"];
                    oc.UsuarioRecepcionador = Convert.ToString(reader["PrimerUsuario"]).Trim();
                    oc.SegundoUsuarioRecepcionador = Convert.ToString(reader["SegundoUsuario"]).Trim();
                    oc.TercerUsuarioRecepcionador = Convert.ToString(reader["TercerUsuario"]).Trim();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla OrdenCompra " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return oc;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna la lista con todos los objetos OrdenCompra</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<OrdenCompra>> GetAllOC()
        {
            List<OrdenCompra> lista = new List<OrdenCompra>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = @"SELECT OC.*, OE.Nombre, U1.Nombre_Usuario AS PrimerUsuario ,U2.Nombre_Usuario AS SegundoUsuario , U3.Nombre_Usuario AS TercerUsuario 
                FROM dbo.Orden_de_Compra OC
                LEFT OUTER JOIN dbo.Ordenes_estadisticas OE ON OE.Id_Orden_Estadistica = OC.ID_OE
                LEFT OUTER JOIN dbo.Usuario U1 ON OC.UsuarioRecepcionador = U1.Id_Usuario 
                LEFT OUTER JOIN dbo.Usuario U2 ON OC.SegundoUsuarioRecepcionador = U2.Id_Usuario 
                LEFT OUTER JOIN dbo.Usuario U3 ON OC.TercerUsuarioRecepcionador = U3.Id_Usuario"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    OrdenCompra oc = new();
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    oc.Numero_OC = Convert.ToInt32(reader["Numero_OC"]);
                    oc.Solped = reader["Solped"] is DBNull ? 0 : Convert.ToInt32(reader["Solped"]);
                    oc.Id_OE = reader["Nombre"] is DBNull ? " " : Convert.ToString(reader["Nombre"]).Trim();
                    oc.posicion = Convert.ToString(reader["Posicion"]).Trim();
                    oc.Id_Ticket = Convert.ToInt32(reader["Id_Ticket"]);
                    oc.Fecha_Recepcion = reader["Fecha_Recepcion"] is DBNull ? (DateTime?)null : (DateTime)reader["Fecha_Recepcion"];
                    oc.UsuarioRecepcionador = Convert.ToString(reader["PrimerUsuario"]).Trim();
                    oc.SegundoUsuarioRecepcionador = Convert.ToString(reader["SegundoUsuario"]).Trim();
                    oc.TercerUsuarioRecepcionador = Convert.ToString(reader["TercerUsuario"]).Trim();

                    lista.Add(oc);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotización " + ex.Message);
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
        /// <param name="OC">Objetivo del tipo OrdenCompra que va a modificarse en la base de datos</param>
        /// <returns>Retorna el objeto OrdenCompra modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrdenCompra> ModificarOC(OrdenCompra OC)
        {
            OrdenCompra ocmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Orden_de_Compra SET " +
                                   "Numero_OC = @Numero_OC, " +  // Add comma after Solped
                                   "Solped = @Solped, " +
                                   "Id_OE = @Id_OE, " +
                                   "Posicion = @Posicion, " +
                                   "Fecha_Recepcion = @Fecha_Recepcion, " +
                                   "UsuarioRecepcionador = @UsuarioRecepcionador " +
                                   "WHERE Id_OE = @Id_OE";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Solped", SqlDbType.Int).Value = OC.Solped;
                Comm.Parameters.Add("@Id_OE", SqlDbType.VarChar).Value = OC.Id_OE;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar).Value = OC.posicion;
                if (OC.Fecha_Recepcion.HasValue)
                    Comm.Parameters.Add("@Fecha_Recepcion", SqlDbType.DateTime).Value = OC.Fecha_Recepcion;
                else
                    Comm.Parameters.Add("@Fecha_Recepcion", SqlDbType.DateTime).Value = DBNull.Value;
                if (OC.UsuarioRecepcionador != null)
                    Comm.Parameters.Add("@UsuarioRecepcionador", SqlDbType.VarChar).Value = OC.UsuarioRecepcionador;
                else
                    Comm.Parameters.Add("@UsuarioRecepcionador", SqlDbType.VarChar).Value = DBNull.Value;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ocmod = await GetOC(Convert.ToInt32(reader["Id_OE"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la orden de compra " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ocmod;
        }

    }
}
