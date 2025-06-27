using CsvHelper.Configuration;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs.CsvHelperMap
{
    public class DownloadListaAgendamentosResponseMap : ClassMap<DownloadListaAgendamentosResponse>
    {
        public DownloadListaAgendamentosResponseMap()
        {
            Map(row => row.Id).Name("Id");
            Map(row => row.NomAgendamento).Name("Nome");
            Map(row => row.DscAgendamento).Name("Descrição");
            Map(row => row.DatCriacao).Name("Data Criação");
            Map(row => row.UsrCodUsuario).Name("Usuário");
            Map(row => row.DatUltExecucao).Name("Execução");
            Map(row => row.DatUltAlteracao).Name("Alteração");
            Map(row => row.Duracao).Name("Duração");
            Map(row => row.DataProxExecucao).Name("Próxima Execução");
            Map(row => row.IdTipAgendamento).Name("Id Tipo Agendamento");
            Map(row => row.TipoAgendamento).Name("Tipo Agendamento");

        }
    }
}
