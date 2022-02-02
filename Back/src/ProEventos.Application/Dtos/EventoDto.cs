using System.Collections.Generic;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        public string TemaEvento { get; set; }

        public int QuantidadePessoas { get; set; }

        public string ImagemUrl { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }

        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }

        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}