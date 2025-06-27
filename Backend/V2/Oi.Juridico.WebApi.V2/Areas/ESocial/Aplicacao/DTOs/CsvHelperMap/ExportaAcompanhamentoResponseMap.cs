using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs.CsvHelperMap
{
    public class ExportaAcompanhamentoResponseMap : ClassMap<VEsAcompanhamentoDTO>
    {
        public ExportaAcompanhamentoResponseMap()
        {
            Map(row => row.TipoFormulario).Index(0).Convert(x => x.Value.TipoFormulario == "F_2500" ? $"S-2500" : x.Value.TipoFormulario == "F_2501" ? "S-2501" : string.Empty).Name("Formulário");
            Map(row => row.DescStatusFormulario).Index(1).Name("Status do Formulário");
            Map(row => row.TipoFormularioTipo).Index(2).Convert(x => x.Value.TipoFormularioTipo == 2 ? "Retificação" : "Original").Name("Tipo do Formulário");
            Map(row => row.PeriodoApuracao).Index(3).Name("Período de Apuração");
            Map(row => row.NrRecibo).Index(4).Convert(x => $"\t{x.Value.NrRecibo}").Name("Nº Recibo");
            Map(row => row.ExclusaoNrrecibo).Index(5).Convert(x => $"\t{x.Value.ExclusaoNrrecibo}").Name("Nº Recibo Exclusão 3500");
            Map(row => row.FinalizadoEscritorio).Index(6).Convert(x => x.Value.FinalizadoEscritorio == "S" ? "Finalizado" : "Pendente").Name("Status Escritório");
            Map(row => row.FinalizadoContador).Index(7).Convert(x => x.Value.FinalizadoContador == "S" ? "Finalizado" : "Pendente").Name("Status Contador");
            Map(row => row.LogCodUsuario).Index(8).Name("Login");
            Map(row => row.CodProcesso).Index(9).Name("Código Interno");
            Map(row => row.InfoprocessoNrproctrab).Index(10).Convert(x => $"\t{x.Value.InfoprocessoNrproctrab}").Name("Nº do Processo");
            Map(row => row.CodEstado).Index(11).Name("Cód. Estado");
            Map(row => row.NomComarca).Index(12).Name("Nome da Comarca");
            Map(row => row.NomTipoVara).Index(13).Name("Tipo Vara");
            Map(row => row.NomParte).Index(14).Name("Reclamantes Cadastrados");
            Map(row => row.CpfParte).Index(15).Convert(x => $"{x.Value.CpfParte.FormatCPF()}").Name("CPF");
            Map(row => row.NomParteEmpresa).Index(16).Name("Empresa do Grupo");
            Map(row => row.CgcParteEmpresa).Index(17).Convert(x => $"{x.Value.CgcParteEmpresa.FormatCNPJ()}").Name("CNPJ Empresa");
            Map(row => row.IndProprioTerceiro).Index(18).Convert(x => x.Value.IndProprioTerceiro == "P" ? $"Próprio" : x.Value.IndProprioTerceiro == "T" ? "Terceiro" : string.Empty).Name("Indicador Próprio/Terceiro");
            Map(row => row.IndProcessoAtivo).Index(19).Convert(x => x.Value.IndProcessoAtivo == "S" ? $"Sim" : x.Value.IndProcessoAtivo == "N" ? "Não" : string.Empty).Name("Indicador Processo Ativo");
            Map(row => row.LogDataOperacao).Index(20).Name("Data Operação");

        }
    }
}
