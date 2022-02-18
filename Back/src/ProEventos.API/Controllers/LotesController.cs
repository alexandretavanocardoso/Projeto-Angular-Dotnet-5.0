using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Contrato;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence;
using ProEventos.Persistence.Contexto;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILotesService _lotesService;
        public LotesController(ILotesService lotesService)
        {
            _lotesService = lotesService;
        }

        [HttpGet("recuperarLotePorId")]
        public async Task<ActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await _lotesService.GetAllLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar lotes. erro: {ex.Message}");
            }
        }

        [HttpPut("saveLotes")]
        public async Task<ActionResult> Put(int eventoId, LoteDto[] models)
        {
            try
            {
                var lote = await _lotesService.SaveLotes(eventoId, models);
                if (lote == null) return NoContent();

                return Ok(lote);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar alterar lotes. erro: {ex.Message}");
            }
        }

        [HttpDelete("deletarLote")]
        public async Task<ActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesService.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                return await _lotesService.Delete(eventoId, loteId) ? 
                        Ok(new {message = "Deletado"}) :
                        throw new Exception("Erro ao deletr lote");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar lotes. erro: {ex.Message}");
            }
        }
    }
}
