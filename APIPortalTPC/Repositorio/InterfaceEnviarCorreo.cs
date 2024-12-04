using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface con metodos que envian especificos correos despues de cumplir ciertas condiciones
    /// </summary>
    public interface InterfaceEnviarCorreo
    {
        public Task<string> CorreoProveedores(Proveedores P, string subject);
        public Task<string> CorreoLiberador(Usuario U, string subject);
        public Task<string> CorreoRecepciones(Usuario U, string subject, List<OrdenCompra>loc, int Id_Ticket);
        public Task<string> CorreoUsuarioPass(Usuario U);
        public Task<string> RecuperarPass(Usuario U);
    }
}
