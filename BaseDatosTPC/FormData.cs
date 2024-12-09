

using Microsoft.AspNetCore.Http;

namespace BaseDatosTPC
{
    public class FormData
    {
        public IFormFile File { get; set; }
        public string Mensaje   { get; set; }
        public int bien_servicio { get; set; }
        public string asunto { get; set; }
        public int[] Proveedor { get; set; }

    }
}
