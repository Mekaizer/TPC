using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioProveedores : IRepositorioProveedores
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioProveedores(AccesoDatos CD)
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
        /// <param name="id">Id del proveedor a buscar</param>
        /// <returns>Retorna objeto del tipo Proveedor con la Id solicitada</returns>
        /// <exception cref="Exception"></exception>
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
                Comm.CommandText = "SELECT * FROM dbo.Proveedores " +
                    "where ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@ID_Proveedores", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();

                P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                P.Swift1 = (Convert.ToString(reader["Swift1"])).Trim();
                P.Swift2 = (Convert.ToString(reader["Swift2"])).Trim();
                P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);

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
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna lista con todos los proveedores de la base de datos</returns>
        /// <exception cref="Exception"></exception>
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
                Comm.CommandText = "SELECT * FROM dbo.Proveedores"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Proveedores P = new();
                    P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                    P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                    P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                    P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                    P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                    P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                    P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                    P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                    P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                    P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                    P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                    P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                    P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                    P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                    P.Swift1 = (Convert.ToString(reader["Swift1"])).Trim();
                    P.Swift2 = (Convert.ToString(reader["Swift2"])).Trim();
                    P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);
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

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="P">Objeto del tipo Proveedor que se usara para modificar</param>
        /// <returns>Retorna el objeto Proveedores que se modifico</returns>
        /// <exception cref="Exception"></exception>
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
                Comm.CommandText = "UPDATE dbo.Proveedores SET " +
                    "Rut_Proveedor = @Rut_Proveedor " +
                    "Nombre_Fantasia = @Nombre_Fantasia " +
                    "ID_Bien_Servicio = @ID_Bien_Servicio " +
                    "Comuna = @Comuna " +
                    "Correo_Proveedor = @Correo_Proveedor " +
                    "Telefono_Proveedor = @Telefono_Proveedor " +
                    "Cargo_Representante = @Cargo_Representante " +
                    "Nombre_Representante = @Nombre_Representante " +
                    "Email_Representante = @Email_Representante " +
                    "Bloqueado = @Bloqueado " +
                    "N_Cuenta = @N_Cuenta " +
                    "Banco = @Banco " +
                    "Swift1 = @Swift1 " +
                    "Swift2 = @Swift2 " +
                    "WHERE ID_Proveedores = @ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                P.Rut_Proveedor = (Convert.ToString(reader["Rut_Proveedor"])).Trim();
                P.Razon_Social = (Convert.ToString(reader["Razon_social"])).Trim();
                P.Nombre_Fantasia = (Convert.ToString(reader["Nombre_Fantasia"])).Trim();
                P.ID_Bien_Servicio = Convert.ToInt32(reader["ID_Bien_Servicio"]);
                P.Direccion = (Convert.ToString(reader["Direccion"])).Trim();
                P.Comuna = (Convert.ToString(reader["Comuna"])).Trim();
                P.Correo_Proveedor = (Convert.ToString(reader["Correo_Proveedor"])).Trim();
                P.Telefono_Proveedor = Convert.ToInt32(reader["Telefono_Proveedor"]);
                P.Cargo_Representante = (Convert.ToString(reader["Cargo_Representante"])).Trim();
                P.Nombre_Representante = (Convert.ToString(reader["Nombre_Representante"])).Trim();
                P.Email_Representante = (Convert.ToString(reader["Email_Representante"])).Trim();
                P.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                P.N_Cuenta = Convert.ToInt32(reader["N_Cuenta"]);
                P.Banco = (Convert.ToString(reader["Banco"])).Trim();
                P.Swift1 = (Convert.ToString(reader["Swift1"])).Trim();
                P.Swift2 = (Convert.ToString(reader["Swift2"])).Trim();
                P.ID_Proveedores = Convert.ToInt32(reader["ID_Proveedores"]);
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
        /// <summary>
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="P">Objeto del tipo Proveedores que se agregará a la base de datos</param>
        /// <returns>Retorna el objeto Proveedores que se agrego a la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Proveedores> NuevoProveedor(Proveedores P)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Proveedores " +
                    "(Rut_Proveedor,Razon_Social,Nombre_Fantasia,ID_Bien_Servicio,Direccion,Comuna,Correo_Proveedor,Telefono_Proveedor,Nombre_Representante,Email_Representante,Bloqueado,N_Cuenta,Banco,Swift1,Swift2) " +
                    "VALUES (@Rut_Proveedor,@Razon_Social,@Nombre_Fantasia,@ID_Bien_Servicio,@Direccion,@Comuna,@Correo_Proveedor,@Telefono_Proveedor,@Nombre_Representante,@Email_Representante,@Bloqueado,@N_Cuenta,@Banco,@Swift1,@Swift2); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Proveedores";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Rut_Proveedor", SqlDbType.VarChar, 10).Value = P.Rut_Proveedor;
                Comm.Parameters.Add("@Razon_Social", SqlDbType.VarChar, 10).Value = P.Razon_Social;
                Comm.Parameters.Add("@Nombre_Fantasia", SqlDbType.VarChar, 10).Value = P.Razon_Social;
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
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                int id = (int)idDecimal;
                P.ID_Bien_Servicio = id;
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
        
        /// <summary>
        /// Metodo que permite ver si existe el proveedor de un bien o servicio en especifico
        /// </summary>
        /// <param name="id_bs">Id del biservicio relacionado al proveedor</param>
        /// <param name="rut">Rut del proveedor</param>
        /// <returns></returns>
        public async Task<string> Existe(int id_bs, string rut)
        {
            string res = "ok";
            using (SqlConnection sqlConnection = conectar())
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT TOP 1 1 FROM dbo.Proveedores WHERE Rut_Proveedor = @rut OR Id_Bien_Servicio = @id";
                    command.Parameters.AddWithValue("@rut", rut);
                    command.Parameters.AddWithValue("@id", id_bs);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows) 
                        {
                             res="Ese proveedor ya existe con ese bien o servicio";
                        }
                        return res;
                    }
                }
            }
            
        }


    }
}