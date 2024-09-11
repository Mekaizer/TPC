using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    internal class Departamento
    {
        [Key]
        public string Nombre { get; set; }

        public string? Descripcion {get;set;}

        public string? Encargado { get; set; }
    
    }
}
