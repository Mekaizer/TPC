﻿using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{/// <summary>
/// Clase que guarda los datos de las ordenes estadisticas
/// </summary>
    public class OrdenesEstadisticas
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Orden_Estadistica { get; set; }
        /// <summary>
        /// Nombre de las ordenes estadisticas
        /// </summary>
        public string? Nombre {  get; set; }
        /// <summary>
        /// Codigo de la nave asociada
        /// </summary>
        public string? Codigo_Nave { get; set; }
        /// <summary>
        /// Id del centro de costo asociado
        /// </summary>
        public string? Id_Centro_de_Costo { get; set; }

    }
}
