using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }

        [Display(Name = "nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Nome { get; set; }

        public string MiniCurriculo { get; set; }

        [Display(Name = "Imagem")]
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "{0} inválida! (gif|jpe?g|bmp|png)")]
        public string ImagemURL { get; set; }

        [Display(Name = "telefone")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Phone(ErrorMessage = "O campo {0} está inválido!")]
        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [EmailAddress(ErrorMessage = "O campo {0} deve ser válido!")]
        public string Email { get; set; }

        public IEnumerable<RedeSocialDto> RedeSociais { get; set; }

        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}