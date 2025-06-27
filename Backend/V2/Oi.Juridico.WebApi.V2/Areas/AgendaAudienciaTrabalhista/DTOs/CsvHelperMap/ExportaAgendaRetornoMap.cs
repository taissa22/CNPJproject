using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs.CsvHelperMap
{
    public class ExportaAgendaRetornoMap : ClassMap<VAgendaTrabalhistaExportarDTO>
    {
        //Map(row => row.CodProcesso).Index(1).Convert(x => $"\t{x.Value.CodProcesso}").Name("Código do Processo");
        public ExportaAgendaRetornoMap()
        {
            Map(row => row.DescricaoEstado).Index(1).Name("Estado");
            Map(row => row.DateAudiencia).Index(2).Convert(x => $"\t{x.Value.DateAudiencia!.Value.ToString("dd/MM/yyyy")}").Name("Data da Audiência");
            Map(row => row.HorarioAudiencia).Index(3).Convert(x => $"\t{x.Value.HorarioAudiencia!.Value.ToString("HH:mm:ss")}").Name("Hora da Audiência");
            Map(row => row.Comarca).Index(4).Name("Comarca");
            Map(row => row.CodVara).Index(5).Convert(x => $"\t{x.Value.CodVara}").Name("Vara");
            Map(row => row.TipoVara).Index(6).Name("Tipo de Vara");
            Map(row => row.TipoAudiencia).Index(7).Name("Tipo de Audiência");
            Map(row => row.DescLocalidade).Index(8).Name("Localidade da Audiência");
            Map(row => row.DescModalidade).Index(9).Name("Modalidade da Audiência");
            Map(row => row.Link).Index(10).Name("Link");
            Map(row => row.NumeroProcesso).Index(11).Convert(x => $"\t{x.Value.NumeroProcesso}").Name("Número do Processo");
            Map(row => row.CodProcesso).Index(12).Convert(x => $"\t{x.Value.CodProcesso}").Name("Código Interno");
            Map(row => row.ProcessoAtivo).Index(13).Convert(x => x.Value.ProcessoAtivo == "S" ? "Ativo" : "Inativo").Name("Status");
            Map(row => row.ClassificacaoProcesso).Index(14).Convert(x => x.Value.ClassificacaoProcesso == "P" ? "Próprio" : "Terceiro").Name("Classificação do Processo");
            Map(row => row.ClassificacaoHierarquica).Index(15).Convert(x => x.Value.ClassificacaoHierarquica == "U" ? "Único" : x.Value.ClassificacaoHierarquica == "P" ? "Primário" : "Secundário").Name("Classificação Hierárquica");
            Map(row => row.Estrategico).Index(16).Convert(x => x.Value.Estrategico == "S" ? "Sim" : "Não").Name("Processo Estratégico");
            Map(row => row.Reclamantes).Index(17).Name("Reclamantes");
            Map(row => row.EscritorioProcesso).Index(18).Name("Escritório");
            Map(row => row.Reclamadas).Index(19).Name("Reclamada");
            Map(row => row.Preposto).Index(20).Name("Preposto");

        }
    }
}
