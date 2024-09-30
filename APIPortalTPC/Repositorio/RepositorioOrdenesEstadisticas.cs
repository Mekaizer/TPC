using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioOrdenesEstadisticas : IRepositorioOrdenesEstadisticas
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioOrdenesEstadisticas(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Ordenes_Estadisticas> NuevoOE(Ordenes_Estadisticas OE)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Ordenes_estadisticas " +
                    "(Nombre,Codigo_Nave,Centro_de_Costo,Id_Orden_Compra) " +
                    "VALUES (@Nombre,@Codigo_Nave,@Centro_de_Costo,@Id_Orden_Compra); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Orden_estadistica";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre", SqlDbType.Int).Value = OE.Nombre;
                Comm.Parameters.Add("@Codigo_Nave", SqlDbType.VarChar,50).Value = OE.Codigo_Nave;
                Comm.Parameters.Add("@Centro_de_Costo", SqlDbType.Int).Value = OE.Centro_de_Costo;
                Comm.Parameters.Add("@Id_Orden_Compra", SqlDbType.VarChar,50).Value = OE.Id_Orden_Compra;
                OE.Id_Orden_Estadistica  = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Cotizacion " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OE;
        }

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Ordenes_Estadisticas> GetOE(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Ordenes_Estadisticas OE = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Ordenes_estadisticas where Id_Orden_Estadistica = @Id_Orden_Estadistica";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Orden_Estadistica", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    OE.Nombre = Convert.ToString(reader["Nombre"]);
                    OE.Codigo_Nave = Convert.ToString(reader["Codigo_Nave"]);
                    OE.Centro_de_Costo = Convert.ToInt32(reader["Centro_de_Costo"]);
                    OE.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    OE.Id_Orden_Estadistica = Convert.ToInt32(reader["Id_Orden_Estadistica"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Cotizaciones " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return OE;
        }
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Ordenes_Estadisticas>> GetAllOE()
        {
            List<Ordenes_Estadisticas> lista = new List<Ordenes_Estadisticas>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Ordenes_Estadisticas"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Ordenes_Estadisticas OE = new();
                    OE.Nombre = Convert.ToString(reader["Nombre"]);
                    OE.Codigo_Nave = Convert.ToString(reader["Codigo_Nave"]);
                    OE.Centro_de_Costo = Convert.ToInt32(reader["Centro_de_Costo"]);
                    OE.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
                    OE.Id_Orden_Estadistica = Convert.ToInt32(reader["Id_Orden_Estadistica"]);
                    lista.Add(OE);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Ordenes Estadisticas " + ex.Message);
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
        public async Task<Ordenes_Estadisticas> ModificarOE(Ordenes_Estadisticas OE)
        {
            Ordenes_Estadisticas OEmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Ordenes_Estadisticas SET" +
                    "Nombre = @Nombre " +
                    "Codigo_Nave = @Codigo_Nave " +
                    "Centro_de_Costo = @Centro_de_Costo " +
                    "Id_Orden_Compra = @Id_Orden_Compra " +
                    "WHERE Id_Orden_Estadistica = @Id_Orden_Estadistica";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre", SqlDbType.Int).Value = OE.Nombre;
                Comm.Parameters.Add("@Codigo_Nave", SqlDbType.VarChar, 50).Value = OE.Codigo_Nave;
                Comm.Parameters.Add("@Centro_de_Costo", SqlDbType.Int).Value = OE.Centro_de_Costo;
                Comm.Parameters.Add("@Id_Orden_Compra", SqlDbType.VarChar, 50).Value = OE.Id_Orden_Compra;
                OE.Id_Orden_Estadistica = (int)await Comm.ExecuteScalarAsync();
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    OEmod = await GetOE(Convert.ToInt32(reader["Id_Orden_Estadistica"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la orden estadistica " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return OEmod;
        }

    }
}