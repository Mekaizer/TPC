using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesBaseDatosTPC
{
    internal class Usuario
    {
        [Key]
        public string Rut_Usuario_Sin_Digito { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Apellido_paterno { get; set; }
        public string? Digito_Verificador { get; set; }
        public string? Correo_Usuario { get; set; }
        public string? Departamento_Usuario { get; set; }
        public string? Contraseña_Usuario { get; set; }
        public bool En_Vacaciones {  get; set; }
        public string? Tipo_Liberador {  get; set; }

        /* Mouseque herramienta misteriosa para mas adelante
        * Disponible = Convert.ToBoolean(reader["Disponible"] ?? false); 
        */
    }
}
