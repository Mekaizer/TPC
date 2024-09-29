using System.ComponentModel.DataAnnotations;

namespace BaseDatosTPC
{
    public class Departamento
    {
        [Key]
        public int Id_Departamento { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion {get;set;}

        public string? Encargado { get; set; }
    
    }
}
