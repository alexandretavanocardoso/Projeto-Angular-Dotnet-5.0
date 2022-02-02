using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contrato;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Application
{
    public class EventosService : IEventosService
    {
        private readonly IEventosPersistence _eventosPersistence;
        private readonly IGeralPersistence _geralPersistence;
        private readonly IMapper _mapper;

        public EventosService(IEventosPersistence eventosPersistence,
                              IGeralPersistence geralPersistence,
                              IMapper mapper)
        {
            _eventosPersistence = eventosPersistence;
            _geralPersistence = geralPersistence;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEventos(EventoDto eventoModel)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoModel);

                _geralPersistence.Add<Evento>(evento);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosPersistence.GetEventoByIdAsync(evento.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEventos(int eventoId, EventoDto eventoModel)
        {
            try
            {
                var evento = await _eventosPersistence.GetEventoByIdAsync(eventoId, false);
                if (evento == null)
                {
                    return null;
                }

                eventoModel.Id = eventoId;

                _mapper.Map(eventoModel, evento);

                _geralPersistence.Update<Evento>(evento);
                if (await _geralPersistence.SaveChangesAsync())
                {
                      var eventoRetorno = await _eventosPersistence.GetEventoByIdAsync(evento.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
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

        public async Task<EventoDto[]> GetAllEventosAsync(bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetAllEventosAsync(incluirPalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetAllEventosByTemaAsync(tema, incluirPalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistence.GetEventoByIdAsync(eventoId, incluirPalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}