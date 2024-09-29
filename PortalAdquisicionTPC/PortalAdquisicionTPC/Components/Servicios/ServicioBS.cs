using BaseDatosTPC;


namespace PortalAdquisicionTPC.Components.Servicios
{
    public class ServicioBS : IServicioBS
    {
        private readonly HttpClient HTTPC;
        public ServicioBS(HttpClient HTTPC)
        {
            this.HTTPC = HTTPC;  
        }
        public async Task<IEnumerable<BienServicio>> GetAllServicio()
        {
            return await HTTPC.GetFromJsonAsync<BienServicio[]>("API/ControladorBienServicio");
        }
        }
    }

