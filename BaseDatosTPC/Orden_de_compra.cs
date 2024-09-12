using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Orden_de_compra
    {
        [Key]
        public int Numero_OC {  get; set; }
        public string? Solped { get; set; }
        public string? Codigo_OE { get; set; }
        public string? posicion { get; set; }

    }
}
