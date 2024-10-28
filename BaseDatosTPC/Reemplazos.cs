using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Reemplazos
    {

        [Key]
        public int ID_Reemplazos { get; set; }
        public string? Id_Usuario_Vacaciones { get; set; }

        public string? Id_Usuario_Reemplazante { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha_Retorno { get; set; }
        public Boolean Valido { get; set; }


    }
}
