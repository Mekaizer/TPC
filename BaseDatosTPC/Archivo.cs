using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Archivo
    {
        public int Id_Archivo { get; set; }
        public int? Id_ArchivoCotizacion { get; set; }
        public bool? IsPrincipal { get; set; }
        public byte? ArchivoDoc { get; set; }
    }
}
