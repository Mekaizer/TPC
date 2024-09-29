using BaseDatosTPC;
using ClasesBaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    public interface IRepositorioTicket
    {
        public Task<Ticket> NewTicket(Ticket T);
        public Task<Ticket> GetTicket(int id);
        public Task<IEnumerable<Ticket>> GetAllTicket();
        public Task<Ticket> ModificarTicket(Ticket T);
    }
}
