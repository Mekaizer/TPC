using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface con metodos que envian especificos correos despues de cumplir ciertas condiciones
    /// </summary>
    public interface InterfaceEnviarCorreo
    {
        public Task<string> CorreoProveedores(string productos, Proveedores P, string subject);
        public Task<string> CorreoLiberador(Usuario U, string subject);
    }
}
