using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface InterfaceCrearExcel
    {
        public  Task<string> DescargarExcel(List<OrdenCompra> LOC);
        public Task<string> DescargarExcel(List<Cotizacion> LC);
        public Task<string> DescargarExcel(List<Usuario> LU);
    }
}
