using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contrato;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Application
{
    public class LotesService : ILotesService
    {
        private readonly ILotesPersistence _lotesPersistence;
        private readonly IGeralPersistence _geralPersistence;
        private readonly IMapper _mapper;

        public LotesService(ILotesPersistence lotesPersistence,
                              IGeralPersistence geralPersistence,
                              IMapper mapper)
        {
            _lotesPersistence = lotesPersistence;
            _geralPersistence = geralPersistence;
            _mapper = mapper;
        }

        private async Task AddLote(int eventoId, LoteDto loteModel)
        {
            try
            {
                var lote = _mapper.Map<Lote>(loteModel);
                lote.EventoId = eventoId;

                _geralPersistence.Add<Lote>(lote);

                await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLote(int eventoId, LoteDto loteModel, Lote[] arraysLotes)
        {
            try
            {
                var lote = arraysLotes.FirstOrDefault(lote => lote.Id == loteModel.Id);
                loteModel.EventoId = eventoId;

                _mapper.Map(loteModel, lote);

                _geralPersistence.Update<Lote>(lote);

                await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lotes = await _lotesPersistence.GetAllLotesByEventoIdAsync(eventoId);
                        if (lotes == null) return null;

                        await UpdateLote(eventoId, model, lotes);
                    }
                }

                var loteRetorno = await _lotesPersistence.GetAllLotesByEventoIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesPersistence.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote não foi encontrado");

                _geralPersistence.Delete<Lote>(lote);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetAllLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotesPersistence.GetAllLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lotes = await _lotesPersistence.GetLoteByIdsAsync(eventoId, loteId);
                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDto>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}