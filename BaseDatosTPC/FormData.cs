

using Microsoft.AspNetCore.Http;

namespace BaseDatosTPC
{
    public class FormData
    {
        public IFormFile Archivo { get; set; }
        public List<int> Lista { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public int id_Bien_Servicio { get; set; }

    }
}
