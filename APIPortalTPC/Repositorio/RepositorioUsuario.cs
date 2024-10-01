﻿using APIPortalTPC.Datos;              
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioUsuario(AccesoDatos CD)
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
        /// Se crea  un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="id">Id del Objeto Usuario a buscar</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> NuevoUsuario(Usuario U)
        {
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Usuario" +
                    "(Nombre_Usuario, Apellido_Paterno, Rut_Usuario_Sin_Digito, Digito_Verificador, Apellido_Materno, Correo_Usuario, Departamento_Usuario, Contraseña_Usuario, Tipo_Liberador, En_Vacaciones, Activado) " +
                    "VALUES (@Nombre_Usuario, @Apellido_Paterno, @Rut_Usuario_Sin_Digito, @Digito_Verificador, @Apellido_Materno, @Correo_Usuario, @Departamento_Usuario, @Contraseña_Usuario, @Tipo_Liberador, @En_Vacaciones, @Activado);" +
                    "SELECT SCOPE_IDENTITY() AS Id_Usuario";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre_Usuario", SqlDbType.VarChar, 50).Value = U.Nombre_Usuario;
                Comm.Parameters.Add("@Apellido_Paterno", SqlDbType.VarChar, 50).Value = U.Apellido_paterno;
                Comm.Parameters.Add("@Rut_Usuario_Sin_Digito", SqlDbType.Int).Value = U.Rut_Usuario_Sin_Digito;
                Comm.Parameters.Add("@Digito_Verificador", SqlDbType.VarChar, 10).Value = U.Digito_Verificador;
                Comm.Parameters.Add("@Apellido_Materno", SqlDbType.VarChar, 50).Value = U.Apellido_materno;
                Comm.Parameters.Add("@Correo_Usuario", SqlDbType.VarChar, 50).Value = U.Correo_Usuario;
                Comm.Parameters.Add("@Departamento_Usuario", SqlDbType.Int).Value = U.Departamento_Usuario;
                Comm.Parameters.Add("@Contraseña_Usuario", SqlDbType.VarChar, 50).Value = U.Contraseña_Usuario;
                Comm.Parameters.Add("@En_Vacaciones", SqlDbType.Bit).Value = U.En_Vacaciones;
                Comm.Parameters.Add("@Tipo_Liberador", SqlDbType.VarChar, 50).Value = U.Tipo_Liberador;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = U.Activado;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                int id = (int)idDecimal;
                U.Id_Usuario = id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Usuario " + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return U;
        }

        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> GetUsuario(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Usuario U = new();
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
                Comm.CommandText = "SELECT * FROM dbo.Usuario where Id_Usuario = @Id_Usuario";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = id;

                //permite regresar objetos de la base de datos para que se puedan leer
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    U.Nombre_Usuario = Convert.ToString(reader["Nombre_Usuario"]); ;
                    U.Apellido_paterno = Convert.ToString(reader["Apellido_Paterno"]); ;
                    U.Digito_Verificador = Convert.ToString(reader["Digito_Verificador"]); ;
                    U.Apellido_materno = Convert.ToString(reader["Apellido_Materno"]); ;
                    U.Correo_Usuario = Convert.ToString(reader["Correo_Usuario"]); ;
                    U.Contraseña_Usuario = Convert.ToString(reader["Contraseña_Usuario"]); ;
                    U.Departamento_Usuario= Convert.ToInt32(reader["Departamento_Usuario"]);
                    U.Tipo_Liberador = Convert.ToString(reader["Tipo_Liberador"]);
                    U.En_Vacaciones= Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario_Sin_Digito = Convert.ToInt32(reader["Rut_Usuario_Sin_Digito"]);
                    U.Activado = Convert.ToBoolean(reader["Activado"]); 
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario " + ex.Message);
            }
            finally
            {
                //Se cierran los objetos 
                reader.Close();
                Comm.Dispose();
                sql.Close();
                sql.Dispose();
            }
            return U;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna la lista de objetos Usuarios de la base de datos</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Usuario>> GetAllUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection sql = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Usuario"; // leer base datos 
                Comm.CommandType = CommandType.Text;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Usuario U = new();
                    U.Nombre_Usuario = Convert.ToString(reader["Nombre_Usuario"]);
                    U.Apellido_paterno = Convert.ToString(reader["Apellido_Paterno"]); 
                    U.Digito_Verificador = Convert.ToString(reader["Digito_Verificador"]); 
                    U.Apellido_materno = Convert.ToString(reader["Apellido_Materno"]); 
                    U.Correo_Usuario = Convert.ToString(reader["Correo_Usuario"]); 
                    U.Contraseña_Usuario = Convert.ToString(reader["Contraseña_Usuario"]); 
                    U.Departamento_Usuario = Convert.ToInt32(reader["Departamento_Usuario"]);
                    U.Tipo_Liberador = Convert.ToString(reader["Tipo_Liberador"]);
                    U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                    U.Rut_Usuario_Sin_Digito = Convert.ToInt32(reader["Rut_Usuario_Sin_Digito"]);
                    U.Activado = Convert.ToBoolean(reader["Activado"]); 
                    U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    lista.Add(U);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos tabla Usuario " + ex.Message);
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
        /// <param name="U">Objeto Usuario que se usa para reemplazar su homonimo dentro de la base de datos</param>
        /// <returns>Retorna el objeto Usuario modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> ModificarUsuario(Usuario U)
        {
            Usuario Umod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Usuario SET " +
                                    "Nombre_Usuario = @Nombre_Usuario, " +
                                    "Apellido_Paterno = @Apellido_Paterno, " +
                                    "Rut_Usuario_Sin_Digito = @Rut_Usuario_Sin_Digito, " +
                                    "Digito_Verificador = @Digito_Verificador, " +
                                    "Apellido_Materno = @Apellido_Materno, " +
                                    "Correo_Usuario = @Correo_Usuario, " +
                                    "Departamento_Usuario = @Departamento_Usuario, " +
                                    "Contraseña_Usuario = @Contraseña_Usuario, " +
                                    "Tipo_Liberador = @Tipo_Liberador, " +
                                    "En_Vacaciones = @En_Vacaciones, " +
                                    "Activado = @Activado " +
                                    "WHERE Id_Usuario = @Id_Usuario";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Nombre_Usuario", SqlDbType.VarChar, 50).Value = U.Nombre_Usuario;
                Comm.Parameters.Add("@Apellido_Paterno", SqlDbType.VarChar, 50).Value = U.Apellido_paterno;
                Comm.Parameters.Add("@Rut_Usuario_Sin_Digito", SqlDbType.Int).Value = U.Rut_Usuario_Sin_Digito;
                Comm.Parameters.Add("@Digito_Verificador", SqlDbType.VarChar, 10).Value = U.Digito_Verificador;
                Comm.Parameters.Add("@Apellido_Materno", SqlDbType.VarChar, 50).Value = U.Apellido_materno;
                Comm.Parameters.Add("@Correo_Usuario", SqlDbType.VarChar, 50).Value = U.Correo_Usuario;
                Comm.Parameters.Add("@Departamento_Usuario", SqlDbType.Int).Value = U.Departamento_Usuario;
                Comm.Parameters.Add("@Contraseña_Usuario", SqlDbType.VarChar, 50).Value = U.Contraseña_Usuario;
                Comm.Parameters.Add("@Tipo_Liberador", SqlDbType.VarChar, 50).Value = U.Tipo_Liberador;
                Comm.Parameters.Add("@En_Vacaciones", SqlDbType.Bit).Value = U.En_Vacaciones;
                Comm.Parameters.Add("@Activado", SqlDbType.Bit).Value = U.Activado;
                Comm.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = U.Id_Usuario;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Umod = await GetUsuario(Convert.ToInt32(reader["Id_Usuario"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando del Usuario " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return Umod;
        }

    }
}