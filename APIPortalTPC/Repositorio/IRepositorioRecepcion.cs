using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioRecepcion
    {
        public Task<Recepcion> NuevaRecepcion(Recepcion R);
        public Task<Recepcion> GetRecepcion(int id);
        public Task<IEnumerable<Recepcion>> GetAllRecepcion();
        public Task<Recepcion> ModificarRecepcion(Recepcion R);

    }
}
