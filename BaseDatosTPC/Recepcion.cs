﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Recepcion
    {
        public int Id_Recepcion {  get; set; }
        public int Id_Correo { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaRespuesta {  get; set; }
        public string? Respuesta { get; set; }
        public string? Comentarios { get; set; }

    }
}