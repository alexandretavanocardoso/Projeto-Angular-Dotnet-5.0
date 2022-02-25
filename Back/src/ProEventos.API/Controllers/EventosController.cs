using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventosController(IEventosService eventosService, IWebHostEnvironment webHostEnvironment)
        {
            _eventosService = eventosService;

            _webHostEnvironment = webHostEnvironment;
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

                if (await _eventosService.Deleteventos(id))
                {
                    DeleteImagem(evento.ImagemUrl);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Erro ao deletr evento");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar eventos. erro: {ex.Message}");
            }
        }

        [HttpPost("uploadImage")]
        public async Task<ActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    DeleteImagem(evento.ImagemUrl);
                    evento.ImagemUrl = await SaveImagem(file);
                }

                var eventoRetorno = await _eventosService.UpdateEventos(eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. erro: {ex.Message}");
            }
        }

        [NonAction] // não aparece na documentação do api
        public async Task<string> SaveImagem(IFormFile imagemFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imagemFile.FileName)
                                    .Take(10) // pega os 10 primeiros caracteres
                                    .ToArray()
                                ).Replace(' ', '-'); // trocando espaço branco por "-"

            // ajustando imagem pra nunca repetir o nome
            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imagemFile.FileName)}";

            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Image", imageName);

            // salvando imagem em partes
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imagemFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        [NonAction] // não aparece na documentação do api
        public void DeleteImagem(string imagem)
        {
            var imagemPath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Image", imagem);
            if (System.IO.File.Exists(imagemPath))
            {
                System.IO.File.Delete(imagemPath);
            }
        }
    }
}
