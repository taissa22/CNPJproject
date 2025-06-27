using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.Models
{
    public class AgendamentoLogProcessoModel
    { 
        public decimal Id { get; set; }
        public DateTime DataIni { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataDeAgendamento { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public string UsuarioId { get; set; } = "";
        public string Operacao { get; set; } = "";
        public int TipoLog { get; set; }
        public int Status { get; set; }
        public string NomeDoArquivo { get; set; } = "";
        public string MensagemErro { get; set; } = "";
        public DateTime? DataIniExecucao { get;  set; }
        public DateTime? DataFimExecucao { get;  set; } 
        public string ArquivoBase64 { get; set; }

        public AgendamentoLogProcessoModel() {
            DataDeAgendamento = DateTime.Now;
            DataDeCadastro = DateTime.Now;
            Status = 1;
        } 
        public AgendLogProcesso retornaEntity(string userName)
        {
            return new AgendLogProcesso()
            {
                Id = Id,
                DatLogIni =  DataIni,
                DatLogFim =  DataFim,
                DatAgendamento = DataDeAgendamento,
                DatCadastro = DataDeCadastro,
                UsrCodUsuario = userName,
                CodOperacao = Operacao,
                CodTipoLog =  TipoLog,
                Status =  Status,
                NomArquivo =  NomeDoArquivo,
                MsgErro =  MensagemErro,
                DatExecucaoIni = DataIniExecucao,
                DatExecucaoFim = DataFimExecucao
        };
            
        }

    }
}
