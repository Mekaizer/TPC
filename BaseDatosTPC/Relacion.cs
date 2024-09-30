using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Relacion
    {
        [Key]
        public int Id_Relacion { get; set; }
        public int? Id_Archivo { get; set; }
        public int? Id_Responsable { get; set; }
    }
}
