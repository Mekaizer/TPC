

using Microsoft.AspNetCore.Http;

namespace BaseDatosTPC
{
    public class FormData
    {
        public IFormFile file { get; set; }
        public string Asunto { get; set; }
        public int iD_Bien_Servicio { get; set; }
        public string subject { get; set; }
        public int[] Lista { get; set; }

    }
}
