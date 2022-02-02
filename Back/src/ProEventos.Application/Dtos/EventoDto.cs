using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Display(Name = "local")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Local { get; set; }

        [Display(Name = "data")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string DataEvento { get; set; }

        [Display(Name = "tema")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Caracteres permitidos de 3 a 50!")]
        public string TemaEvento { get; set; }

        [Display(Name = "range")]
        [Range(1, 120000, ErrorMessage = "O {0} é de 1 a 120.000!")]
        public int QuantidadePessoas { get; set; }

        [Display(Name = "Imagem")]
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "{0} inválida! (gif|jpe?g|bmp|png)")]
        public string ImagemUrl { get; set; }

        [Display(Name = "telefone")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Phone(ErrorMessage = "O campo {0} está inválido!")]
        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [EmailAddress(ErrorMessage = "O campo {0} deve ser válido!")]
        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }

        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }

        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}