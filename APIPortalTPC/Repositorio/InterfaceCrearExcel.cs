using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface InterfaceCrearExcel
    {
        public  Task<string> DescargarExcel(List<OrdenCompra> LOC);
        public Task<string> DescargarExcel(List<Cotizacion> LC);
    }
}
