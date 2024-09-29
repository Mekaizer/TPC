using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioProveedores : IRepositorioProveedores
    {
        //Variable que guarda el string para la conexion con la base de datos
        private string Conexion;

        //Metodo que permite interactuar con la base de datos, aqui se guarda la conexion con la base de datos
        public RepositorioProveedores(AccesoDatos CD)
        {
            Conexion = CD.ConexionDatosSQL;
        }
        private SqlConnection conectar()
        {
            //Se realiza la conexion
            return new SqlConnection(Conexion);
        }
        //Se crea una en un nuevo objeto y se agrega a la base de datos
        public async Task<Proveedores> NuevoProveedor(Proveedores P)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Proveedores (Proveedores) VALUES (@Proveedores); SELECT SCOPE_IDENTITY() AS ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Rut_Proveedor", SqlDbType.VarChar,10).Value = P.Rut_Proveedor;
                Comm.Parameters.Add("@Razon_Social", SqlDbType.VarChar, 10).Value = P.Razon_Social;
                Comm.Parameters.Add("@Nombre_Fantasia", SqlDbType.VarChar,10).Value = P.Razon_Social;
                Comm.Parameters.Add("@ID_Bien_Servicio", SqlDbType.Int).Value = P.ID_Bien_Servicio;
                Comm.Parameters.Add("@Direccion", SqlDbType.VarChar, 50).Value = P.Razon_Social;
                Comm.Parameters.Add("@Comuna", SqlDbType.VarChar, 50).Value = P.Comuna;
                Comm.Parameters.Add("@Correo_Proveedor", SqlDbType.VarChar, 50).Value = P.Correo_Proveedor;
                Comm.Parameters.Add("@Telefono_Proveedor", SqlDbType.Int).Value = P.Telefono_Proveedor;
                Comm.Parameters.Add("@Nombre_Representante", SqlDbType.VarChar, 50).Value = P.Nombre_Representante;
                Comm.Parameters.Add("@Email_Representante", SqlDbType.VarChar, 50).Value = P.Email_Representante;
                Comm.Parameters.Add("@Bloqueado", SqlDbType.Bit).Value = P.Bloqueado;
                Comm.Parameters.Add("@N_Cuenta", SqlDbType.Int).Value = P.N_Cuenta;
                Comm.Parameters.Add("@Banco", SqlDbType.VarChar).Value = P.Banco;
                Comm.Parameters.Add("@Swift1", SqlDbType.VarChar).Value = P.Swift1;
                Comm.Parameters.Add("@Swift2", SqlDbType.VarChar).Value = P.Swift2;
                P.ID_Bien_Servicio = (int)await Comm.ExecuteScalarAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Proveedores " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return P;
        }

        //Metodo que permite conseguir un objeto usando su llave foranea
        public async Task<Proveedores> GetProveedor(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Proveedores P = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Proveedores where ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Proveedores", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    //Se asegura que no sean valores nulos, si es nulo se reemplaza por un valor valido
                    P.Rut_Proveedor = Convert.ToString(reader["Rut_Proveedor"]);
                    P.Razon_Social = Convert.ToString(reader["Razon_social"]);
                    P.Nombre_Fantasia = Convert.ToString(reader["Nombre_Fantasia"]);
                    P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    P.Direccion = Convert.ToString(reader["Direccion"]);
                    P.Comuna = Convert.ToString(reader["Comuna"]);
                    P.Correo_Proveedor = Convert.ToString(reader["Correo_Proveedor"]);
                    P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                    P.Cargo_Representante = Convert.ToString(reader["Cargo_Representante"]);
                    P.Nombre_Representante = Convert.ToString(reader["Nombre_Representante"]);
                    P.Email_Representante = Convert.ToString(reader["Email_Representante"]);
                    P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                    P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                    P.Banco = Convert.ToString(reader["Banco"]);
                    P.Swift1 = Convert.ToString(reader["Swift1"]);
                    P.Swift2 = Convert.ToString(reader["Swift2"]);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Proveedores " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return P;
        }
        //Metodo que retorna una lista con los objeto
        public async Task<IEnumerable<Proveedores>> GetAllProveedores()
        {
            List<Proveedores> lista = new List<Proveedores>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Proveedor"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Proveedores P = new();
                    P.Rut_Proveedor = Convert.ToString(reader["Rut_Proveedor"]);
                    P.Razon_Social = Convert.ToString(reader["Razon_social"]);
                    P.Nombre_Fantasia = Convert.ToString(reader["Nombre_Fantasia"]);
                    P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    P.Direccion = Convert.ToString(reader["Direccion"]);
                    P.Comuna = Convert.ToString(reader["Comuna"]);
                    P.Correo_Proveedor = Convert.ToString(reader["Correo_Proveedor"]);
                    P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                    P.Cargo_Representante = Convert.ToString(reader["Cargo_Representante"]);
                    P.Nombre_Representante = Convert.ToString(reader["Nombre_Representante"]);
                    P.Email_Representante = Convert.ToString(reader["Email_Representante"]);
                    P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                    P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                    P.Banco = Convert.ToString(reader["Banco"]);
                    P.Swift1 = Convert.ToString(reader["Swift1"]);
                    P.Swift2 = Convert.ToString(reader["Swift2"]);
                    lista.Add(P);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Proveedores " + ex.Message);
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
        public async Task<Proveedores> ModificarProveedor(Proveedores P)
        {
            Proveedores Pmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Proveedores SET Proveedores = @Proveedores WHERE ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                P.Rut_Proveedor = Convert.ToString(reader["Rut_Proveedor"]);
                P.Razon_Social = Convert.ToString(reader["Razon_social"]);
                P.Nombre_Fantasia = Convert.ToString(reader["Nombre_Fantasia"]);
                P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                P.Direccion = Convert.ToString(reader["Direccion"]);
                P.Comuna = Convert.ToString(reader["Comuna"]);
                P.Correo_Proveedor = Convert.ToString(reader["Correo_Proveedor"]);
                P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                P.Cargo_Representante = Convert.ToString(reader["Cargo_Representante"]);
                P.Nombre_Representante = Convert.ToString(reader["Nombre_Representante"]);
                P.Email_Representante = Convert.ToString(reader["Email_Representante"]);
                P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                P.Banco = Convert.ToString(reader["Banco"]);
                P.Swift1 = Convert.ToString(reader["Swift1"]);
                P.Swift2 = Convert.ToString(reader["Swift2"]);

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Pmod = await GetProveedor(Convert.ToInt32(reader["ID_Proveedores"]));
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
            return Pmod;
        }

    }
}