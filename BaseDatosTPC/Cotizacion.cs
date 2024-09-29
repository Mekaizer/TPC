using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Cotizacion
    {
        [Key]
        public int ID_Cotizacion { get; set; }
        public int? Rut_Solicitante { get; set; }
        public DateTime? Fecha_Creacion_Cotizacion { get; set; }
        public string? Estado { get; set; }
        public int? Id_Proveedor { get; set; }
        public string? Detalle { get; set; }
        public int? Solped {  get; set; }
        public int? Id_Orden_Compra { get;set; }




    }
}
