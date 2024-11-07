﻿using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioCorreo
    {
        public Task<Correo> NuevoCorreo(Correo C);
        public Task<Correo> GetCorreo(int id);
        public Task<IEnumerable<Correo>> GetAllCorreo();
        public Task<Correo> ModificarCorreo(Correo C);
    }
}
