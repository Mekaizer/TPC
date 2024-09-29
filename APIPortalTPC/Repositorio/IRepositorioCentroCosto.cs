using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioCentroCosto
    {
        public Task<Centro_de_costo> Nuevo_CeCo(Centro_de_costo Ceco);
        public Task<Centro_de_costo> GetCeCo(int  IdCECO);
        public Task<IEnumerable<Centro_de_costo>> GetAllCeCo();
        public Task<Centro_de_costo> ModificarCeCo(Centro_de_costo CeCo);
    }
}
