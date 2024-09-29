using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioProveedores
    {
        public Task<Proveedores> NuevoProveedor(Proveedores P);
        public Task<Proveedores> GetProveedor(int id);
        public Task<IEnumerable<Proveedores>> GetAllProveedores();
        public Task<Proveedores> ModificarProveedor(Proveedores P);
    }
}
