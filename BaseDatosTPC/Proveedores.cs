using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Proveedores
    {
        [Key]
        public int ID_Proveedores {  get; set; }
        public string? Rut_Proveedor {  get; set; }
        
        public string? Razon_Social { get; set; }

        public string? Nombre_Fantasia { get; set; }

        public int ID_Bien_Servicio { get; set; }

        public string? Direccion { get; set; }
        
        public string? Comuna { get; set; }
        
        public string? Correo_Proveedor { get; set; }

        public int? Telefono_Proveedor { get; set; }

        public string? Cargo_Representante { get; set; }    

        public string? Nombre_Representante { get; set; }

        public string? Email_Representante { get; set; }

        public bool Bloqueado { get; set; }

        public int Numero_Cuenta { get; set; }

        public string? Nombre_Banco { get; set; }

        public string? Swift1 { get; set; }

        public string? Swift2 { get; set; }

    }
}
