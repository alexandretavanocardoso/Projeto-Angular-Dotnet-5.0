using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Persistence
{
    public class EventosPersistence : IEventosPersistence
    {
        private readonly ProEventosContext _context;
        public EventosPersistence(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false)
        {
           IQueryable<Evento> query = _context.Eventos
                                       .Include(e => e.Lotes)
                                       .Include(e => e.RedesSociais);

            if(incluirPalestrantes == true){
                query = query.Include(e => e.PalestrantesEventos)
                             .ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(e => e.TemaEvento.ToLower()
                                                 .Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                       .Include(e => e.Lotes)
                                       .Include(e => e.RedesSociais);

            if(incluirPalestrantes == true){
                query = query.Include(e => e.PalestrantesEventos)
                             .ThenInclude(e => e.PalestranteId);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                       .Include(e => e.Lotes)
                                       .Include(e => e.RedesSociais);

            if(incluirPalestrantes == true){
                query = query.Include(e => e.PalestrantesEventos)
                             .ThenInclude(e => e.PalestranteId);
            }

            query = query.OrderBy(e => e.Id)
                         .Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}