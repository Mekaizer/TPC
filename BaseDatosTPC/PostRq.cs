﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    /// <summary>
    /// Clase que se usa para validar la primera parte de iniciar sesion
    /// </summary>
    public class PostRq
    {
        public string? correo { get; set; } 
        public string? pass { get; set; }
    }
}
