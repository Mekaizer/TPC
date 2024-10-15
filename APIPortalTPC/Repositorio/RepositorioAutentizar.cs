using APIPortalTPC.Datos;
using ClasesBaseDatosTPC;
using System.Data;
using System.Data.SqlClient;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioAutentizar : IRepositorioAutentizar
    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioAutentizar(AccesoDatos CD)
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
        /// Retorna el Usuario validado si su contraseña y contraseña coinciden
        /// </summary>
        /// <param name="correo">Correo a buscar</param>
        /// <param name="pass">Contraseña a buscar</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Usuario> ValidarCorreo(string correo, string pass)
        {
            Usuario U = new();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            SqlDataReader? reader = null;

            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "SELECT * FROM dbo.Usuario " +
                                   "WHERE Contraseña_Usuario = @pass AND Correo_Usuario = @correo";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.AddWithValue("@correo", correo);
                Comm.Parameters.AddWithValue("@pass", pass);

                reader = await Comm.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        U.Nombre_Usuario = (Convert.ToString(reader["Nombre_Usuario"])).Trim();
                        U.Apellido_paterno = (Convert.ToString(reader["Apellido_Paterno"])).Trim();
                        U.Digito_Verificador = (Convert.ToString(reader["Digito_Verificador"])).Trim();
                        U.Apellido_materno = (Convert.ToString(reader["Apellido_Materno"])).Trim();
                        U.Correo_Usuario = (Convert.ToString(reader["Correo_Usuario"])).Trim();
                        U.Contraseña_Usuario = (Convert.ToString(reader["Contraseña_Usuario"])).Trim();
                        U.Departamento_Usuario = Convert.ToString(reader["Departamento_Usuario"]);
                        U.Tipo_Liberador = (Convert.ToString(reader["Tipo_Liberador"])).Trim();
                        U.En_Vacaciones = Convert.ToBoolean(reader["En_Vacaciones"]);
                        U.Rut_Usuario_Sin_Digito = Convert.ToInt32(reader["Rut_Usuario_Sin_Digito"]);
                        U.Activado = Convert.ToBoolean(reader["Activado"]);
                        U.Admin = Convert.ToBoolean(reader["Admin"]);
                        U.Id_Usuario = Convert.ToInt32(reader["Id_Usuario"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creando los datos en tabla Usuario " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado " + ex.Message);
            }
            finally
            {
                Comm?.Dispose();
                sql.Close();
                sql.Dispose();
            }
            

            return U;
        }

    }
}
