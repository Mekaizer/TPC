using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioOrdenCompra : IRepositorioOrdenCompra
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioOrdenCompra(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Orden_de_compra> NuevoOC(Orden_de_compra OC)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Orden_de_compra " +
                    "(Numero_OC,Solped,Id_OE,Posicion) " +
                    "VALUES (@Numero_OC,@Solped,@Id_OE,@Posicion); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Orden_Compra";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Solped", SqlDbType.Int).Value = OC.Solped;
                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = OC.Id_OE;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar, 10).Value = OC.posicion;
                OC.Id_Orden_Compra = (int)await Comm.ExecuteScalarAsync();
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

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Orden_de_compra> GetOC(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Orden_de_compra oc = new Orden_de_compra();
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
                Comm.CommandText = "SELECT * FROM dbo.Orden_de_compra " +
                    "where Id_Orden_Compra = @Id_Orden_Compra";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Orden_Compra", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    oc.Solped = Convert.ToInt32(reader["Solped"]);
                    oc.Id_OE = Convert.ToInt32(reader["Id_OE"]);
                    oc.posicion = Convert.ToString(reader["Posicion"]);
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]); 
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Orden_de_compra " + ex.Message);
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
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Orden_de_compra>> GetAllOC()
        {
            List<Orden_de_compra> lista = new List<Orden_de_compra>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Orden_de_compra"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Orden_de_compra oc = new();
                    oc.Solped = Convert.ToInt32(reader["Solped"]);
                    oc.Id_OE = Convert.ToInt32(reader["Id_OE"]);
                    oc.posicion = Convert.ToString(reader["Posicion"]);
                    oc.Id_Orden_Compra = Convert.ToInt32(reader["Id_Orden_Compra"]);
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

        //Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        public async Task<Orden_de_compra> ModificarOC(Orden_de_compra OC)
        {
            Orden_de_compra ocmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Orden_de_compra SET " +
                    "Numero_OC = @Numero_OC " +
                    "Solped = @Solped " +
                    "Id_OE = @Id_OE " +
                    "Posicion = @Posicion " +
                    "WHERE Id_Orden_compra = @Id_Orden_compra";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Numero_OC", SqlDbType.Int).Value = OC.Numero_OC;
                Comm.Parameters.Add("@Solped", SqlDbType.Int).Value = OC.Solped;
                Comm.Parameters.Add("@Id_OE", SqlDbType.Int).Value = OC.Id_OE;
                Comm.Parameters.Add("@Posicion", SqlDbType.VarChar, 10).Value = OC.posicion;
                OC.Id_Orden_Compra = (int)await Comm.ExecuteScalarAsync();

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ocmod = await GetOC(Convert.ToInt32(reader["Id"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la cotización " + ex.Message);
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
