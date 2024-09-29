using BaseDatosTPC;
using Microsoft.AspNetCore.Components;
using PortalAdquisicionTPC.Components.Servicios;
namespace PortalAdquisicionTPC.Components.Pages
{
    public class ListaBienServicioBase : ComponentBase
    {
        [Inject]
        public IServicioBS ServicioBS { get; set; }
        public IEnumerable<BienServicio> BS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BS = (await ServicioBS.GetAllServicio()).ToList();
        }
    }
}

/*


 namespace BlazorServer.Pages
{
    public class ListaAlumnosBase : ComponentBase
    {
        public IEnumerable<BienServicio> LBS { get; set; }

        protected override Task OnInitializedAsync()
        {
            CargarAlumnos();
            return base.OnInitializedAsync();
        }

        private void CargarAlumnos()
        {

            BienServicio bs = new BienServicio();
            bs.ID_Bien_Servicio = 1;
            bs.Bien_Servicio = "Casa";


            LBS = new List<BienServicio> { bs };

        }
    }

}*/