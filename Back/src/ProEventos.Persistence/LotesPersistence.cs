using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Persistence
{
    public class LotesPersistence : ILotesPersistence
    {
        private readonly ProEventosContext _context;
        public LotesPersistence(ProEventosContext context)
        {
            _context = context;
        }
        public async Task<Lote[]> GetAllLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes.AsNoTracking();

            query = query.Where(e => e.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes.AsNoTracking();

            query = query.Where(e => e.EventoId == eventoId && e.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}