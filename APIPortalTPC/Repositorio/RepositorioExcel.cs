using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Data;
using ClasesBaseDatosTPC;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioExcel

    {
        private string Conexion;

        /// <summary>
        /// Metodo que permite interactuar con la base de datos, aqui se guarda la dirección de la base de datos
        /// </summary>
        /// <param name="CD">Variable para guardar la conexion a la base de datos</param>
        public RepositorioExcel(AccesoDatos CD)
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
        public Task<IEnumerable<OrdenCompra>> LeerExcel()
        {
            List<OrdenCompra> lista = new List<OrdenCompra>();

            throw new NotImplementedException();
        }

    }
}
