using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Ordenes_Estadisticas
    {
        [Key]
        public string? Codigo_OE { get; set; }

        public string? Nombre {  get; set; }

        public string? Codigo_Nave { get; set; }
        public string? Centro_de_Costo { get; set; }

    }
}
