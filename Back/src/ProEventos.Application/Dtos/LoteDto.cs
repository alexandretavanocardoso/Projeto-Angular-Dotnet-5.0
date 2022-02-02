using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Nome { get; set; }

        public decimal Preco { get; set; }

        [Display(Name = "data")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string DataInicio { get; set; }

        [Display(Name = "data")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string DataFim { get; set; }

        [Range(1, 120000, ErrorMessage = "O {0} é de 1 a 120.000!")]
        public int Quantidade { get; set; }

        public int EventoId { get; set; }

        public EventoDto Evento { get; set; }
    }
}