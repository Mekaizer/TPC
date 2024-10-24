﻿using System.ComponentModel.DataAnnotations;

namespace ClasesBaseDatosTPC
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Apellido_paterno { get; set; }
        public int? Rut_Usuario_Sin_Digito { get; set; }
        public string? Digito_Verificador { get; set; }
        public string? Apellido_materno { get; set; }
        public string? Correo_Usuario { get; set; }
        public string? Contraseña_Usuario { get; set; }
        public bool? En_Vacaciones {  get; set; }
        public string? Tipo_Liberador {  get; set; }
        public bool? Activado { get; set; }
        public bool? Admin {  get; set; }
        public List<String>? ListaDepartamento { get; set; }
    }
}
