using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    internal class Centro_de_costo
    {
        [Key]
        public string Codigo_Ceco { get; set; }

        public string Nombre { get; set; }
    }
}
