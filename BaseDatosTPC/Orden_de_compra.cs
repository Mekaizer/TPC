using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Orden_de_compra
    {
        [Key]
        public int Id_Orden_Compra { get; set; }
        public int Numero_OC {  get; set; }
        public int? Solped { get; set; }
        public int? Codigo_OE { get; set; }
        public string? posicion { get; set; }

    }
}
