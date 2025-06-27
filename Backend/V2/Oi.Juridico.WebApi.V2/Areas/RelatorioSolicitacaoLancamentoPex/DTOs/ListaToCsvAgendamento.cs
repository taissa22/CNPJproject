using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using Oi.Juridico.Shared.V2.Tools;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs
{
    public class ListaToCsvAgendamento
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descrição { get; set; }  
        public string Data_Criação { get; set; }
        public string Usuário { get; set; }
        public string Execução { get; set; }
        public string Alteração { get; set; } 
        public string Duração { get; set; }
        public string Proxima_Execução { get; set; }
        public short Id_Agendamento { get; set; }

        public string? TIpo_Agendamento { get; set; }


        enum Colors { Red, Green, Blue, Yellow };

        public ListaToCsvAgendamento(AgPexRelSolic entity)
        {
            Id = entity.Id.ToString();
            Nome = entity.NomAgendamento;
            Descrição = entity.DscAgendamento;
            Data_Criação = entity.DatCriacao.ToString("dd/MM/yyyy");
            Usuário = entity.UsrCodUsuario ?? "";
            Execução = entity.DatUltExecucao == null ? "" : entity.DatUltExecucao.Value.ToString("dd/MM/yyyy");
            Alteração = entity.DatUltAlteracao.ToString("dd/MM/yyyy");
            Duração = "00:00"; //Util.CalculaHoras(entity.DatFimExecucao, entity.DatInicioExecucao);
            Proxima_Execução =  entity.DataProxExecucao.ToString("dd/MM/yyyy");
            Id_Agendamento = entity.TipAgendamento;
            TIpo_Agendamento = (Enum.GetName(typeof(TipoExecucaoRelatorio), Id_Agendamento)??" ").Replace("_"," ");





        }

        public ListaToCsvAgendamento(AgPexRelSolicHist entity)
        {
            Id = entity.Id.ToString();
            Nome = entity.NomAgendamento;
            Descrição = entity.DscAgendamento;
            Data_Criação = entity.DatCriacao.ToString("dd/MM/yyyy");
            Usuário = entity.UsrCodUsuario ?? "";
            Execução = entity.DatUltExecucao == null ? "" : entity.DatUltExecucao.Value.ToString("dd/MM/yyyy");
            Alteração = entity.DatUltAlteracao.ToString("dd/MM/yyyy");
            Duração = "00:00"; // Util.CalculaHoras(entity.DatFimExecucao, entity.DatInicioExecucao);
            Proxima_Execução = entity.DataProxExecucao.ToString("dd/MM/yyyy");
            Id_Agendamento = entity.TipAgendamento;
            TIpo_Agendamento = (Enum.GetName(typeof(TipoExecucaoRelatorio), Id_Agendamento) ?? " ").Replace("_", " ");
        }

       
    }
}
