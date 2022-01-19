using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contrato
{
    public interface IEventosService
    {
        Task<Evento> AddEventos(Evento eventoModel);
        Task<Evento> UpdateEventos(int eventoId, Evento eventoModel);
        Task<bool> Deleteventos(int eventoId);
         Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false);
    }
}