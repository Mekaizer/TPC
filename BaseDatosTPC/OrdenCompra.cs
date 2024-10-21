using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Clase que guarda los datos de las ordenes de compras
    /// </summary>
    public class OrdenCompra
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Orden_Compra { get; set; }
        /// <summary>
        /// Codigo de la orden de compra
        /// </summary>
        public int? Numero_OC {  get; set; }
        /// <summary>
        /// numero de la solped
        /// </summary>
        public int? Solped { get; set; }
        /// <summary>
        /// Id de la orden estadistica asociada
        /// </summary>
        public string? Id_OE { get; set; }
        /// <summary>
        /// Orden de posicionamiento entre varias ordenes de compras relacionadas
        /// </summary>
        public string? posicion { get; set; }
        public int? Id_Ticket { get; set; }
        public DateTime? Fecha_Recepcion { get; set; }
        public string? UsuarioRecepcionador { get; set; }

    }
}
