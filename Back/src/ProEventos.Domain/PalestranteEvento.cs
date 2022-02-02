using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain
{
    [Table("PALESTRANTEEVENTO")]
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }

        public Palestrante Palestrante { get; set; }

        public int EventoId { get; set; }
        
        public Evento Evento { get; set; }
    }
}