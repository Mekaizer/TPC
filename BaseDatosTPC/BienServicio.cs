using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class BienServicio
    {

        [Key] 
        public int ID_Bien_Servicio {  get; set; }
        public string? Bien_Servicio { get; set;}

        
    }
}
