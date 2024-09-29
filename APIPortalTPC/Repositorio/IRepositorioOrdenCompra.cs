using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioOrdenCompra
    {
        public Task<Orden_de_compra> NuevoOC(Orden_de_compra OC);
        public Task<Orden_de_compra> GetOC(int id);
        public Task<IEnumerable<Orden_de_compra>> GetAllOC();
        public Task<Orden_de_compra> ModificarOC(Orden_de_compra OC);
    }
}
