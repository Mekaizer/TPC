using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Centro_de_costo
    {
        [Key]
        public int Id_Ceco { get; set; }
        public string? Codigo_Ceco { get; set; }
        public string? Nombre { get; set; }
    }
}
