using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioDepartamento : IRepositorioDepartamento
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioDepartamento(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Departamento> NuevoDepartamento(Departamento D)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Departamento (Departamento) VALUES (@Departamento); SELECT SCOPE_IDENTITY() AS Id_Departamento";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = D.Descripcion;
                Comm.Parameters.Add("@Encargado", SqlDbType.VarChar,50).Value = D.Encargado;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = D.Nombre;
                D.Id_Departamento = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Departamento " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return D;
        }

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Departamento> GetDepartamento(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Departamento dep = new Departamento();
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
                Comm.CommandText = "SELECT * FROM dbo.Departamento where Id_Departamento = @Id_Departamento";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Departamento", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    dep.Descripcion = Convert.ToString(reader["Descripcion"]);
                    dep.Encargado = Convert.ToString(reader["Encargado"]);
                    dep.Nombre = Convert.ToString(reader["Nombre"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Departamento " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return dep;
        }
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Departamento>> GetAllDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Departamento"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Departamento dep = new Departamento();
                    dep.Descripcion = Convert.ToString(reader["Descripcion"]);
                    dep.Encargado = Convert.ToString(reader["Encargado"]);
                    dep.Nombre = Convert.ToString(reader["Nombre"]);
                    lista.Add(dep);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Departamento " + ex.Message);
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
        public async Task<Departamento> ModificarDepartamento(Departamento D)
        {
            Departamento Dmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Departamento SET Departamento = @Departamento WHERE Id_Departamento = @Id_Departamento";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = D.Descripcion;
                Comm.Parameters.Add("@Encargado", SqlDbType.VarChar, 50).Value = D.Encargado;
                Comm.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = D.Nombre;
                D.Id_Departamento = (int)await Comm.ExecuteScalarAsync();
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Dmod = await GetDepartamento(Convert.ToInt32(reader["Id_Departamento"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando el departamento " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Dmod;
        }

    }
}