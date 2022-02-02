using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Contrato
{
    public interface IEventosService
    {
        Task<EventoDto> AddEventos(EventoDto eventoModel);
        Task<EventoDto> UpdateEventos(int eventoId, EventoDto eventoModel);
        Task<bool> Deleteventos(int eventoId);
         Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false);
        Task<EventoDto[]> GetAllEventosAsync(bool incluirPalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false);
    }
}