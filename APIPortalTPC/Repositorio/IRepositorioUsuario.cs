using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioUsuario
    {
        public Task<Usuario> NuevoUsuario(Usuario U);
        public Task<Usuario> GetUsuario(int id );
        public Task<IEnumerable<Usuario>> GetAllUsuario();
        public Task<Usuario> ModificarUsuario(Usuario U);
        public Task<string> Existe(int rut, string correo );
        public Task<IEnumerable<Usuario>> GetAllUsuariosLiberadores();
    }
}
