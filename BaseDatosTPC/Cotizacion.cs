using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    internal class Cotizacion
    {
        [Key]
        public int ID_Cotizacion { get; set; }
        public string? Rut_Solicitante { get; set; }
        public DateTime? Fecha_Creacion_Cotizacion { get; set; }
        public string? Estado { get; set; }

    }
}
