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
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventosService;
        public EventosController(IEventosService eventosService)
        {
            _eventosService = eventosService;
        }

        [HttpGet("recuperarTodosEventos")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var eventos = await _eventosService.GetAllEventosAsync(true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. erro: {ex.Message}");
            }
        }

        [HttpGet("recuperarEventoPorId")]
        public async Task<ActionResult<Evento>> GetById(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. erro: {ex.Message}");
            }
        }

        [HttpGet("recuperarEventoPorTema")]
        public async Task<ActionResult<Evento>> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventosService.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. erro: {ex.Message}");
            }
        }

        [HttpPost("adicionarEvento")]
        public async Task<ActionResult> AddEvento(EventoDto model)
        {
            try
            {
                var evento = await _eventosService.AddEventos(model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. erro: {ex.Message}");
            }
        }

        [HttpPut("alterarEvento")]
        public async Task<ActionResult> AlterarEvento(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventosService.UpdateEventos(id, model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar alterar eventos. erro: {ex.Message}");
            }
        }

        [HttpDelete("deletarEvento")]
        public async Task<ActionResult> DeletarEvento(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent();

                return await _eventosService.Deleteventos(id) ? 
                        Ok("Deletado") :
                        throw new Exception("Erro ao deletr evento");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar eventos. erro: {ex.Message}");
            }
        }
    }
}
