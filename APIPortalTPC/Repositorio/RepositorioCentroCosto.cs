using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioCentroCosto : IRepositorioCentroCosto
    {

        private readonly string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioCentroCosto(AccesoDatos CD)
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
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="IdCECO">Id a buscar para el Centro Costo</param>
        /// <returns>Retorna el objeto Centro_de_costo cuyo Id sea el dado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Centro_de_costo> GetCeCo(int IdCECO)
        {
            //Parametro para guardar el objeto a mostrar
            Centro_de_costo cc = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Centro_de_costo where Id_Ceco = @Id_Ceco";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Ceco", SqlDbType.Int).Value = cc.Id_Ceco;
                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_Ceo"]);
                    cc.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    cc.Codigo_Ceco = (Convert.ToString(reader["Codigo_Ceco"])).Trim();

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
        /// <summary>
        /// Metodo que retorna una lista con los objetos
        /// </summary>
        /// <returns>Retorna una lista con todos los Centro de Costo</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Centro_de_costo>> GetAllCeCo()
        {
            List<Centro_de_costo> lista = new List<Centro_de_costo>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
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
                    Centro_de_costo cc = new();
                    cc.Id_Ceco = Convert.ToInt32(reader["Id_Ceco"]);
                    cc.Nombre = (Convert.ToString(reader["Nombre"])).Trim();
                    cc.Codigo_Ceco = (Convert.ToString(reader["Codigo_Ceco"])).Trim();
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
        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="CeCo">Objeto del tipo Centro_de_Costo que se usará para reemplazar el Centro_de_costo antiguo</param>
        /// <returns>Regresa el centro_de_costo que va a reemplazar</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Centro_de_costo> ModificarCeCo(Centro_de_costo CeCo)
        {
            Centro_de_costo ccmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Centro_de_costo SET " +
                    "Nombre = @Nombre" +
                    "Codigo_Ceco = @Codigo_Ceco" +
                    "WHERE Id_Ceco = @Id_Ceco";
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
                reader?.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ccmod;
        }
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="Ceco">Objeto del tipo Centro_de_costo que se va a añadir a la base de datos</param>
        /// <returns>Regresa el objeto a añadirse</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Centro_de_costo> Nuevo_CeCo(Centro_de_costo Ceco)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Centro_de_costo (Nombre,Codigo_Ceco) " +
                    "VALUES (@Nombre,@Codigo_Ceco); " +
                    "SELECT SCOPE_IDENTITY() AS Id_Ceco";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = Ceco.Nombre;
                Comm.Parameters.Add("@Codigo_Ceco", SqlDbType.VarChar, 50).Value = Ceco.Codigo_Ceco;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                Ceco.Id_Ceco = (int)idDecimal;
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
        /// <summary>
        /// Metodo que se usa para asegura que no se repita el Centro de costo
        /// </summary>
        /// <param name="Ceco">Nombre a buscar</param>
        /// <returns></returns>
        public async Task<string> Existe(string Ceco)
        {
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Centro_de_costo WHERE Codigo_Ceco = @Codigo_Ceco";
                    command.Parameters.AddWithValue("@Codigo_Ceco", Ceco);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        return "El codigo CeCo ya existe";
                    }
                    else
                    {
                        reader.Close();
                        return "ok";
                    }
                }
            }
        }
    }
}

