using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioDepartamentoUsuario : IRepositorioDepartamentoUsuario
    {

        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioDepartamentoUsuario(AccesoDatos CD)
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
        /// Creacion de un
        /// </summary>
        /// <param name="DP"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DepartamentoUsuario> Nuevo(DepartamentoUsuario DP)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Relacion " +
                    "(Id_DepartamentoUsuarios,Id_Usuario,Id_Departamento) " +
                    "VALUES (@Id_DepartamentoUsuarios,@Id_Usuario,@Id_Departamento); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = DP.Id_Departamento;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = DP.Id_Usuario;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                DP.Id_DepartamentoUsuarios = (int)idDecimal;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla de Departamento Usuario " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return DP;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto DepartamentoUsuario a buscar</param>
        /// <returns>Retorna el objeto Relacion cuya Id se pide</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DepartamentoUsuario> Get(int id)
        {
            //Parametro para guardar el objeto a mostrar
            DepartamentoUsuario DP = new();
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
                Comm.CommandText = "SELECT * FROM dbo.DepartamentoUsuario " +
                    "where Id_DepartamentoUsuarios = @Id_DepartamentoUsuarios";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_DepartamentoUsuarios", SqlDbType.Int).Value = id;

                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    DP.Id_Usuario = Convert.ToString(reader["Id_Usuario"]);
                    DP.Id_DepartamentoUsuarios = Convert.ToInt32(reader["DepartamentoUsuario"]);
                    DP.Id_Departamento = Convert.ToString(reader["Id_Departamento"]);
                }
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
            return DP;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Relacion de la lsita</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<DepartamentoUsuario>> GetAll()
        {
            List<DepartamentoUsuario> lista = new List<DepartamentoUsuario>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.DepartamentoUsuario"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    DepartamentoUsuario DP = new();
                    DP.Id_Usuario = Convert.ToString(reader["Id_Usuario"]);
                    DP.Id_DepartamentoUsuarios = Convert.ToInt32(reader["Id_DepartamentoUsuarios"]);
                    DP.Id_Departamento = Convert.ToString(reader["Id_Departamento"]);
                    lista.Add(DP);
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

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="R">Objeto del tipo Relacion que se usará para modificar su homonimo por Id</param>
        /// <returns>Retorna el objeto Modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DepartamentoUsuario> Modificar(DepartamentoUsuario DP)
        {
            //int Id_DepartamentoUsuarios 
            //string Id_Usuario 
            //string Id_Departamento
            DepartamentoUsuario Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Relacion SET " +
                    "Id_DepartamentoUsuarios = @Id_DepartamentoUsuarios " +
                    "Id_Usuario = @Id_Usuario " +
                    "Id_Departamento = @Id_Departamento " +
                    "WHERE Id_DepartamentoUsuarios = @Id_DepartamentoUsuarios";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_DepartamentoUsuarios", SqlDbType.Int).Value = DP.Id_DepartamentoUsuarios;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = DP.Id_Departamento;
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = DP.Id_Usuario;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await Get(Convert.ToInt32(reader["Id_DepartamentoUsuarios"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el departamento Usuario " + ex.Message);
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
