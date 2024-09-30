using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioRelacion : IRepositorioRelacion
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioRelacion(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Relacion> NuevaRelacion(Relacion R)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Relacion " +
                    "(Id_Archivo,Id_Responsable) " +
                    "VALUES (@Id_Archivo,@Id_Responsable); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;
                Comm.Parameters.Add("@Id_Responsable", SqlDbType.Int).Value = R.Id_Responsable;
                R.Id_Relacion = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla de relaciones " + ex.Message);
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
        public async Task<Relacion> GetRelacion(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Relacion R=new();
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
                Comm.CommandText = "SELECT * FROM dbo.Relacion where ID_Relacion = @ID_Relacion";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Relacion", SqlDbType.Int).Value = id;

                reader = await Comm.ExecuteReaderAsync();
                R.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                R.Id_Responsable = Convert.ToInt32(reader["Id_Responsable"]);
                R.Id_Relacion = Convert.ToInt32(reader["Id_Relacion"]);

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Relacion " + ex.Message);
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
        public async Task<IEnumerable<Relacion>> GetAllRelacion()
        {
            List<Relacion> lista = new List<Relacion>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Relacion"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Relacion R = new();
                    R.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    R.Id_Responsable = Convert.ToInt32(reader["Id_Responsable"]);
                    R.Id_Relacion = Convert.ToInt32(reader["Id_Relacion"]);
                    lista.Add(R);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla de Relacion " + ex.Message);
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
        public async Task<Relacion> ModificarRelacion(Relacion R)
        {
            Relacion Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Relacion SET " +
                    "Id_Archivo = @Id_Archivo " +
                    "Id_Responsable = @Id_Responsable " +
                    "WHERE ID_Relacion = @ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Relacion", SqlDbType.Int).Value = R.Id_Relacion;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;
                Comm.Parameters.Add("@Id_Responsable", SqlDbType.Int).Value = R.Id_Responsable;


                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetRelacion(Convert.ToInt32(reader["ID_Relacion"]));
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
            return Rmod;
        }

    }
}