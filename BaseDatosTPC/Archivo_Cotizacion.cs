using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Archivo_Cotizacion
    {
        [Key]
        public int Id_Archivo_Cotizacion {  get; set; }
        public int? Id_Cotizacion {  set; get; }

    }
}
