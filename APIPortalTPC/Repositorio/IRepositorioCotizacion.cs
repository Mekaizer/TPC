using BaseDatosTPC;
namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioCotizacion
    {
        public Task<IEnumerable<Cotizacion>> GetAllCotizacion();
        public Task<IEnumerable<Cotizacion>> GetCotizacion(int id);
        public Task<IEnumerable<Cotizacion>> NuevoCotizacion(Cotizacion cotizacion);
    }
}
