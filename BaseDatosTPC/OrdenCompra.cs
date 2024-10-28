﻿using System.ComponentModel.DataAnnotations;


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
        /// <summary>
        /// Id del ticket asociado a la orden de compra
        /// </summary>
        public int? Id_Ticket { get; set; }
        /// <summary>
        /// fecha que la orden de compra es liberada
        /// </summary>
        public DateTime? Fecha_Recepcion { get; set; }
        /// <summary>
        /// ID del liberador... en este caso generalmente será Carla
        /// </summary>
        public string? UsuarioRecepcionador { get; set; }
        /// <summary>
        /// Id del liberador del departamento
        /// </summary>
        public string? SegundoUsuarioRecepcionador { get; set; }
        /// <summary>
        /// En caso de haber un tercer liberador se guarda
        /// </summary>
        public string? TercerUsuarioRecepcionador { get; set; }

    }
}