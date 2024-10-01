using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Ordenes_Estadisticas
    {
        [Key]
        public int Id_Orden_Estadistica { get; set; }
        public string? Nombre {  get; set; }

        public string? Codigo_Nave { get; set; }
        public int? Id_Centro_de_Costo { get; set; }

    }
}
