using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs.CsvHelperMap
{
    public class ExportaAcompanhamentoCriticasRetornoMap : ClassMap<AcompanhamentoCriticasRetornoDTO>
    {
        public ExportaAcompanhamentoCriticasRetornoMap()
        {
            Map(row => row.CodProcesso).Index(1).Convert(x => $"\t{x.Value.CodProcesso}").Name("Código do Processo");
            Map(row => row.NumProcesso).Index(2).Convert(x => $"\t{x.Value.NumProcesso}").Name("Número do Processo");
            Map(row => row.EmpreseGrupo).Index(3).Name("Empresa do Grupo");
            Map(row => row.Cpf).Index(4).Convert(x => $"{x.Value.Cpf.FormatCPF()}").Name("CPF do Reclamante");
            Map(row => row.NomeReclamante).Index(5).Name("Nome do Reclamante");
            Map(row => row.Escritorio).Index(6).Name("Escritório");
            Map(row => row.Contador).Index(7).Name("Contador");
            Map(row => row.LogDataOperacao).Index(8).Convert(x => $"\t{x.Value.LogDataOperacao}").Name("Data/Hora do Retorno");
            Map(row => row.Formulario).Index(9).Name("Formulário");
            Map(row => row.PeriodoApuracao).Index(10).Name("Período de Apuração");
            Map(row => row.Original_retificacao).Index(11).Name("Original/Retificação");
            Map(row => row.DescStatusFormulario).Index(12).Name("Status");
            Map(row => row.Codigo).Index(13).Convert(x => $"\t{x.Value.Codigo}").Name("Código");
            Map(row => row.Ocorrencia).Index(14).Convert(x => $"\t{x.Value.Ocorrencia.Replace("\n", "").Replace("\r", "")}").Name("Ocorrência");
            Map(row => row.Acao).Index(15).Convert(x => $"\t{x.Value.Acao}").Name("Ação");
            Map(row => row.Localizacao).Index(16).Convert(x => $"\t{x.Value.Localizacao}").Name("Localização");
            Map(row => row.IdExecucao).Index(17).Name("Id Execução");

        }
    }
}
