using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : IPalestrantePersistence
    {
        private readonly ProEventosContext _context;
        public PalestrantePersistence(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool incluirEventos = false)
        {
           IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking()
                                       .Include(p => p.RedeSociais);

            if(incluirEventos == true){
                query = query.Include(p => p.PalestrantesEventos)
                             .ThenInclude(p => p.Evento);
            }

            query = query.OrderBy(p => p.Id)
                         .Where(p => p.Nome.ToLower()
                                           .Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool incluirEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking()
                                       .Include(p => p.RedeSociais);

            if(incluirEventos == true){
                query = query.Include(p => p.PalestrantesEventos)
                             .ThenInclude(p => p.Evento);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool incluirEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking()
                                       .Include(p => p.RedeSociais);

            if(incluirEventos == true){
                query = query.Include(p => p.PalestrantesEventos)
                             .ThenInclude(p => p.Evento);
            }

            query = query.OrderBy(p => p.Id)
                         .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}