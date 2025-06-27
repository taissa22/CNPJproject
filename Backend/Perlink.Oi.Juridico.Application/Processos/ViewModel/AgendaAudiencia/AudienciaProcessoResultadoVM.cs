
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AudienciaProcessoResultadoVM
    {
        [Ignore]
        public long CodProcesso { get; set; }

        [Ignore]
        public long SeqAudiencia { get; set; }

        [Name("Estado")]
        public string SiglaEstado { get; set; }

        [Name("Comarca")]
        public string Comarca { get; set; }

        [Name("Vara")]
        public int CodVara { get; set; }

        [Name("Tipo Vara")]
        public string TipoVara { get; set; }

        [Name("Data da Audiência")]
        public string DataAudiencia { get; set; }

        [Name("Horário da Audiência")]
        public string HorarioAudiencia { get; set; }

        [Name("Tipo de Audiência")]
        public string TipoAudiencia { get; set; }

        [Name("Preposto")]
        public string Preposto { get; set; }

        [Name("Escritório Audiência")]
        public string EscritorioAudiencia { get; set; }

        [Name("Advogado Escritório da Audiência")]
        public string AdvogadoAudiencia { get; set; }

        [Name("Preposto Acompanhante")]
        public string PrepostoAcompanhante { get; set; }

        [Name("Escritório Acompanhante")]
        public string EscritorioAcompanhante { get; set; }

        [Name("Advogado Acompanhante")]
        public string AdvogadoAcompanhante { get; set; }

        [Name("Tipo de Processo")]
        public string TipoProcesso { get; set; }

        [Name("Estratégico")]
        public string Estrategico { get; set; }

        [Name("Nº do Processo")]
        public string NumeroProcesso { get; set; }

        [Name("Classificação Hierárquica")]
        public string ClassificacaoHierarquica { get; set; }

        [Name("Empresa do Grupo")]
        public string EmpresaGrupo { get; set; }

        [Name("Endereço")]
        public string Endereco { get; set; }

        [Name("Escritório do Processo")]
        public string EscritorioProcesso { get; set; }

        [Ignore] 
        public string DddAdvogadoEscritorio { get; set; }

        [Ignore]
        public string NroAdvogadoEscritorio { get; set; }

        [Ignore]
        public string DddAdvogadoAcompanhante { get; set; }

        [Ignore]
        public string NroAdvogadoAcompanhante { get; set; }

        [Ignore]
        public string Migrado { get; set; }

        [Ignore]
        public IEnumerable<string> Pedidos { get; set; }

        [Ignore]
        public IEnumerable<string> Autores { get; set; }

        [Ignore]
        public IEnumerable<string> Reus { get; set; }

    }
}
