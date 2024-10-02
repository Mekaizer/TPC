using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Guarda todas las cotizaciones
    /// </summary>
    public class Cotizacion
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int ID_Cotizacion { get; set; }
        /// <summary>
        /// Id del usuario que ha solicitado la solicitud
        /// </summary>
        public int? Id_Solicitante { get; set; }
        /// <summary>
        /// Guarda la fecha de creacion
        /// </summary>
        public DateTime? Fecha_Creacion_Cotizacion { get; set; }
        /// <summary>
        /// Etapa en la que se encuentra la cotizacion
        /// </summary>
        public string? Estado { get; set; }
        /// <summary>
        /// Id del proveedor asociado
        /// </summary>
        public int? Id_Proveedor { get; set; }
        /// <summary>
        /// descripcion de la cotizacion
        /// </summary>
        public string? Detalle { get; set; }
        /// <summary>
        /// Numero de solped
        /// </summary>
        public int? Solped {  get; set; }
        /// <summary>
        /// Id de la orden de compra asociada
        /// </summary>
        public int? Id_Orden_Compra { get;set; }




    }
}
