using System.ComponentModel.DataAnnotations;

namespace ClasesBaseDatosTPC
{
    public class Banco
    {
        [Key]
        public int Numero_Cuenta { get; set; }
        public string? Rut_Proveedor { get; set; }
        public string? Nombre_Banco { get; set; }
        public string? Swift1 { get; set; }
        public string? Swift2 { get; set; }


    }
}
