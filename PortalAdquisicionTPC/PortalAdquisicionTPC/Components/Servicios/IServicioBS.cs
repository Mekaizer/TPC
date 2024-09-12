using BaseDatosTPC;

//Interface que se conecta la API con el server

namespace PortalAdquisicionTPC.Components.Servicios
{
    public interface IServicioBS
    {
        Task<IEnumerable<BienServicio>> GetAllServicio();
    }
}
 