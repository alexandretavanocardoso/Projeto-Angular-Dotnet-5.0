using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class RedeSocialDto
    {
        public int Id { get; set; }

        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Nome { get; set; }

        [Display(Name = "url")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string URL { get; set; }

        public int? EventoId { get; set; }

        public EventoDto Evento { get; set; }

        public int PalestranteId { get; set; }

        public PalestranteDto Palestrenta { get; set; }
    }
}