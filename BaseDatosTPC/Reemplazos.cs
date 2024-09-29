using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Reemplazos
    {

        [Key]
        public int ID_Reemplazos { get; set; }
        public int? Rut_Usuario_Vacaciones { get; set; }

        public int? Rut_Usuario_Reemplazante { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha_Retorno { get; set; }

    }
}
