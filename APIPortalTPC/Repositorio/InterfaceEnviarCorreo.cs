﻿using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface con metodos que envian especificos correos despues de cumplir ciertas condiciones
    /// </summary>
    public interface InterfaceEnviarCorreo
    {
        public Task<string> CorreoCotizacion(string productos, Proveedores P, string subject);
    }
}
