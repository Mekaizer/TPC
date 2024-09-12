using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Ticket
    {
        [Key]
        public int ID_Ticket { get; set; }

        public int? Numero_OC { get; set; }

        public string? Estado { get; set; }

        public DateTime Fecha_Creacion_OC { get; set; }

        public string? Rut_Usuario { get; set; }

        public int? ID_Proveedor { get; set; }

        public DateTime Fecha_Ingreso_OC { get; set; }

        public DateTime Fecha_OC_Enviada { get; set; }
        public DateTime Fecha_OC_Liberada { get; set; }
        public DateTime Fecha_OC_Rechazada { get; set; }

        public string? Archivo { get; set; }

        public string? Detalle { get; set; }

    }
}
