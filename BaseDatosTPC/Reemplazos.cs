using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    internal class Reemplazos
    {
        public string? Rut_Usuario_Vacaciones { get; set; }

        public string? Rut_Usuario_Reemplazante { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha_Retorno { get; set; }

        [Key]
        public int ID_Reemplazos { get; set; }
    }
}
