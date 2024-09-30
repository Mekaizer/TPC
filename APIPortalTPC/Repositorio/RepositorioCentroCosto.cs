using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCentroCosto : IRepositorioCentroCosto
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioCentroCosto(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Centro_de_costo> GetCeCo(int IdCECO)
        {
            //Parametro para guardar el objeto a mostrar
            Centro_de_costo cc = new Centro_de_costo();
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
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo where Id_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = cc.Id_Ceco;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    cc.Nombre = Convert.ToString(reader["Nombre"]);
                    cc.Codigo_Ceco= Convert.ToString(reader["Codigo_Ceco"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Centro de costo " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return cc;
        }
        //Metodo que retorna una lista con los objetos
        public async Task<IEnumerable<Centro_de_costo>> GetAllCeCo()
        {
            List<Centro_de_costo> lista = new List<Centro_de_costo>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Centro_de_costo cc = new Centro_de_costo();
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_Ceco"]);
                    cc.Nombre = Convert.ToString(reader["Nombre"]);
                    cc.Codigo_Ceco = Convert.ToString(reader["Codigo_Ceco"]);
                    lista.Add(cc);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Centro de costo " + ex.Message);
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
        public async Task<Centro_de_costo> ModificarCeCo(Centro_de_costo CeCo)
        {
            Centro_de_costo ccmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Centro_de_costo SET Centro_de_costo = @Centro_de_costo WHERE Centro_de_costo = @Centro_de_costo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = ccmod.Id_Ceco;
                //Usar cuando se corrija el ingresar datos, porque por ahora no se como meter una clase
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = ccmod.Nombre;
                Comm.Parameters.Add("@Codigo_Ceco", SqlDbType.VarChar, 50).Value = ccmod.Codigo_Ceco;

                //Comm.Parameters.Add("@Bien_Servicio", SqlDbType.VarChar, 50).Value = "Pan";
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    ccmod = await GetCeCo(Convert.ToInt32(reader["Id_Ceco"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el Centro de costo " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ccmod;
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Centro_de_costo> Nuevo_CeCo(Centro_de_costo Ceco)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Centro_de_costo (Nombre,Centro_de_costo) VALUES (@Nombre,@Centro_de_costo); SELECT SCOPE_IDENTITY() AS Id_Ceco";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = Ceco.Nombre;
                Comm.Parameters.Add("@Codigo_Ceco", SqlDbType.VarChar, 50).Value = Ceco.Codigo_Ceco;
                Ceco.Id_Ceco = (int)await Comm.ExecuteScalarAsync();
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
            return Ceco;
        }
    }
}

