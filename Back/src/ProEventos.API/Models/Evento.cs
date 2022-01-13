namespace ProEventos.API.Models
{
    public class Evento
    {
        public int EventoId { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        public string TemaEvento { get; set; }

        public int QuantidadePessoas { get; set; }

        public string Lote { get; set; }

        public string ImagemUrl { get; set; }
    }
}