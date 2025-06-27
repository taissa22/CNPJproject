using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;


namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs.CsvHelperMap
{
    public class ExportaAcompanhamentoEnvio2501Map : ClassMap<AcompanhamentoEnvio2501DTO>
    {
        public ExportaAcompanhamentoEnvio2501Map()
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
            Map(row => row.Original_retificacao).Index(10).Name("Original/Retificação");
            Map(row => row.PeriodoApuracao).Index(11).Name("Período de Apuração");
            Map(row => row.Codigo).Index(12).Convert(x => $"\t{x.Value.Codigo}").Name("Código");
            Map(row => row.Ocorrencia).Index(13).Name("Ocorrência");
            Map(row => row.Acao).Index(14).Name("Ação");
            Map(row => row.Localizacao).Index(15).Convert(x => $"\t{x.Value.Localizacao}").Name("Localização");
            Map(row => row.IdExecucao).Index(16).Name("Id Execução");


        }
    }
}
