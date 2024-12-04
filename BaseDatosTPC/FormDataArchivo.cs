

using Microsoft.AspNetCore.Http;

namespace BaseDatosTPC
{
    public class FormDataArchivo
    {
        public IFormFile Archivo { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public int[] Lista { get; set; }

        public int id_Bien_Servicio { get; set; }

    }
}
