using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    public class Ticket
    {
        [Key]
        public int ID_Ticket { get; set; }

        public int? Id_OC { get; set; }

        public string? Estado { get; set; }

        public DateTime Fecha_Creacion_OC { get; set; }

        public int? Id_Usuario { get; set; }

        public int? ID_Proveedor { get; set; }

        public DateTime Fecha_Ingreso_OC { get; set; }

        public DateTime Fecha_OC_Enviada { get; set; }
        
        public DateTime Fecha_OC_Liberada { get; set; }

        public string? Detalle { get; set; }

    }
}
