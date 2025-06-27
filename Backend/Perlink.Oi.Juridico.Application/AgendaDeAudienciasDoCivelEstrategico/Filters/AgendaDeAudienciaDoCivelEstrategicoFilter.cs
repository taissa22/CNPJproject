using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Queries;
using System;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Filters
{
    public class AgendaDeAudienciaDoCivelEstrategicoFilter
    {
        public QueryOrder Estado { get; set; }
        public QueryOrder Comarca { get; set; }
        public QueryOrder Vara { get; set; }
        public QueryOrder TipoVara { get; set; }
        public QueryOrder DataAudiencia { get; set; }
        public QueryOrder HoraAudiencia { get; set; }
        public int Pagina { get; set; } = 1;
        public int Quantidade { get; set; } = 8;
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string EstadoId { get; set; }
        public int? ComarcaId { get; set; }
        public int? EmpresaGrupoId { get; set; }
        public int? EscritorioId { get; set; }
        public int? PrepostoId { get; set; }
        public int? ProcessoId { get; set; }
        public int? AssuntoId { get; set; }
        public string ClassificacaoProcessoId { get; set; }
        public string Closing { get; set; }
        public string ClientCo { get; set; }    
    }
}