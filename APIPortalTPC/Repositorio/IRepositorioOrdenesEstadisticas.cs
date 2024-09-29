using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioOrdenesEstadisticas
    {
        public Task<Ordenes_Estadisticas> NuevoOE(Ordenes_Estadisticas OE);
        public Task<Ordenes_Estadisticas> GetOE(int id);
        public Task<IEnumerable<Ordenes_Estadisticas>> GetAllOE();
        public Task<Ordenes_Estadisticas> ModificarOE(Ordenes_Estadisticas OE);
    }
}
