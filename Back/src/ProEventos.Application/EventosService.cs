using System;
using System.Threading.Tasks;
using ProEventos.Application.Contrato;
using ProEventos.Domain;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Application
{
    public class EventosService : IEventosService
    {
        private readonly IEventosPersistence _eventosPersistence;
        private readonly IGeralPersistence _geralPersistence;

        public EventosService(IEventosPersistence eventosPersistence,
                              IGeralPersistence geralPersistence)
        {
            _eventosPersistence = eventosPersistence;
            _geralPersistence = geralPersistence;
        }

        public async Task<Evento> AddEventos(Evento eventoModel)
        {
            try
            {
                _geralPersistence.Add<Evento>(eventoModel);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    return await _eventosPersistence.GetEventoByIdAsync(eventoModel.Id, false);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEventos(int eventoId, Evento eventoModel)
        {
            try
            {
                var evento = await _eventosPersistence.GetEventoByIdAsync(eventoId, false);
                if (evento == null)
                {
                    return null;
                }

                eventoModel.Id = eventoId;

                _geralPersistence.Update(eventoModel);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    return await _eventosPersistence.GetEventoByIdAsync(eventoModel.Id, false);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Deleteventos(int eventoId)
        {
            try
            {
                var evento = await _eventosPersistence.GetEventoByIdAsync(eventoId, false);
                if (evento == null) throw new Exception("Evento não foi encontrado");

                _geralPersistence.Delete<Evento>(evento);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetAllEventosAsync(incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetAllEventosByTemaAsync(tema, incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetEventoByIdAsync(eventoId, incluirPalestrantes);
                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}