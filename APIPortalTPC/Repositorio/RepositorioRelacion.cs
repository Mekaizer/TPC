﻿using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioRelacion : IRepositorioRelacion
    {
       
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioRelacion(AccesoDatos CD)
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
        /// Se crea una en un nuevo objeto y se agrega a la base de datos
        /// </summary>
        /// <param name="R">Objeto Relacion que se va a añadir a la base de datos</param>
        /// <returns>Retorna el objeto Relacion que fue añadida</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Relacion> NuevaRelacion(Relacion R)
        {
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
            try
            {
                sql.Open();
                Comm = sql.CreateCommand();
                Comm.CommandText = "INSERT INTO Relacion " +
                    "(Id_Archivo,Id_Responsable1,Id_Responsable2) " +
                    "VALUES (@Id_Archivo,@Id_Responsable,@Id_Responsable2); " +
                    "SELECT SCOPE_IDENTITY() AS ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;
                Comm.Parameters.Add("@Id_Responsable1", SqlDbType.Int).Value = R.Id_Responsable1;
                Comm.Parameters.Add("@Id_Responsable2", SqlDbType.Int).Value = R.Id_Responsable2;
                decimal idDecimal = (decimal)await Comm.ExecuteScalarAsync();
                R.Id_Relacion = (int)idDecimal;
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
       
        /// <summary>
        /// Metodo que permite conseguir un objeto usando su llave foranea
        /// </summary>
        /// <param name="id">Id del objeto Relacion a buscar</param>
        /// <returns>Retorna el objeto Relacion cuya Id se pide</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Relacion> GetRelacion(int id)
        {
            //Parametro para guardar el objeto a mostrar
            Relacion R=new();
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
                Comm.CommandText = "SELECT * FROM dbo.Relacion " +
                    "where Id_Relacion = @Id_Relacion";
                Comm.CommandType = CommandType.Text;
                //se guarda el parametro 
                Comm.Parameters.Add("@Id_Relacion", SqlDbType.Int).Value = id;

                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    R.Id_Archivo = Convert.ToInt32(reader["Id_Archivo"]);
                    R.Id_Responsable1 = reader["Id_Responsable1"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Responsable1"]);
                    R.Id_Responsable2 = reader["Id_Responsable2"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Responsable2"]);
                    R.Id_Relacion = Convert.ToInt32(reader["Id_Relacion"]);
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
            return R;
        }
        /// <summary>
        /// Metodo que retorna una lista con los objeto
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos Relacion de la lsita</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Relacion>> GetAllRelacion()
        {
            List<Relacion> lista = new List<Relacion>();
            SqlConnection sql = conectar();
            SqlCommand? Comm = null;
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
                    R.Id_Responsable1 = reader["Id_Responsable1"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Responsable1"]);
                    R.Id_Responsable2 = reader["Id_Responsable2"] is DBNull ? 0 : Convert.ToInt32(reader["Id_Responsable2"]);
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

        /// <summary>
        /// Pide un objeto ya hecho para ser reemplazado por uno ya terminado
        /// </summary>
        /// <param name="R">Objeto del tipo Relacion que se usará para modificar su homonimo por Id</param>
        /// <returns>Retorna el objeto Modificado</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Relacion> ModificarRelacion(Relacion R)
        {
            Relacion Rmod = null;
            SqlConnection sqlConexion = conectar();
            SqlCommand? Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "UPDATE dbo.Relacion SET " +
                    "Id_Archivo = @Id_Archivo, " +
                    "Id_Responsable1 = @Id_Responsable1, " +
                    "Id_Responsable2 = @Id_Responsable2 " +
                    "WHERE ID_Relacion = @ID_Relacion";
                Comm.CommandType = CommandType.Text;
                Comm.Parameters.Add("@Id_Relacion", SqlDbType.Int).Value = R.Id_Relacion;
                Comm.Parameters.Add("@Id_Archivo", SqlDbType.Int).Value = R.Id_Archivo;
                if (R.Id_Responsable1.HasValue)
                    Comm.Parameters.Add("@Id_Responsable1", SqlDbType.Int).Value = R.Id_Responsable1;
                else
                    Comm.Parameters.Add("@Id_Responsable1", SqlDbType.Int).Value = DBNull.Value;

                if (R.Id_Responsable2.HasValue)
                    Comm.Parameters.Add("@Id_Responsable2", SqlDbType.Int).Value = R.Id_Responsable2;
                else
                    Comm.Parameters.Add("@Id_Responsable2", SqlDbType.Int).Value = DBNull.Value;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    Rmod = await GetRelacion(Convert.ToInt32(reader["ID_Relacion"]));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando la relacion " + ex.Message);
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