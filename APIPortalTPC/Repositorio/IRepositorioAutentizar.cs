﻿using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioAutentizar
    {
        public Task<Usuario> ValidarCorreo(string correo, string pass);
    }
}