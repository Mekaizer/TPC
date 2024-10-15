using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioOrdenCompra
    {
        public Task<OrdenCompra> NuevoOC(OrdenCompra OC);
        public Task<OrdenCompra> GetOC(int id);
        public Task<IEnumerable<OrdenCompra>> GetAllOC();
        public Task<OrdenCompra> ModificarOC(OrdenCompra OC);
    }
}
