using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence
{
    public interface IProEventosPersistence
    {
        // GERAL
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entity) where T : class;
        Task<bool> SaveChangesAsync();

        //EVENTOS
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes);
        Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes);

        //PALESTRANTES
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool incluirEventos);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool incluirEventos);
        Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool incluirEventos);

    }
}