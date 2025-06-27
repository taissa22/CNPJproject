using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Models
{
    public class ModeloModel
    {
        public decimal Id { get; set; } = 0;
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string DataIniSolicitacao { get; set; } = "";
        public string DataFimSolicitacao { get; set; } = "";
        public string DataIniVencimento { get; set; } = "";
        public string DataFimVencimento { get; set; } = "";
        public List<int>? ColunasRelatorioSelecionadas { get; set; } = new List<int>();
        public List<int>? EscritoriosSelecionados { get; set; } = new List<int>();
        public List<int>? TiposDeLancamentoSelecionados { get; set; } = new List<int>();
        public List<string>? UfsSelecionadas { get; set; } = new List<string>();
        public List<int>? StatusSolicitacoesSelecionados { get; set; } = new List<int>();
        public string DataDeCadastro { get; set; } = "";
        public string DataDeAlteracao { get; set; } = "";
        public ModeloModel() { }
        public ModeloModel(AgPexRelSolicModelo entity) {
            Id = entity.Id;
            Nome = entity.Nome;
            Descricao = entity.Descricao;
            DataIniSolicitacao = entity.DatIniSolcitacao != null ? entity.DatIniSolcitacao.Value.ToString("dd/MM/yyyy") : "";
            DataFimSolicitacao = entity.DatFimSolcitacao != null ?  entity.DatFimSolcitacao.Value.ToString("dd/MM/yyyy") : "";
            DataIniVencimento = entity.DatIniVencimento != null ? entity.DatIniVencimento.Value.ToString("dd/MM/yyyy") : "";
            DataFimVencimento = entity.DatFimVencimento != null ? entity.DatFimVencimento.Value.ToString("dd/MM/yyyy") : "";
            ColunasRelatorioSelecionadas = String.IsNullOrEmpty(entity.ColunasRelSelecionadas) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(entity.ColunasRelSelecionadas);
            EscritoriosSelecionados = String.IsNullOrEmpty(entity.EscritoriosRelSelecionados) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(entity.EscritoriosRelSelecionados);
            TiposDeLancamentoSelecionados = String.IsNullOrEmpty(entity.TiposLancRelSelecionados) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(entity.TiposLancRelSelecionados);
            StatusSolicitacoesSelecionados = String.IsNullOrEmpty(entity.StatusSolRelSelecionados) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(entity.StatusSolRelSelecionados);
            UfsSelecionadas = String.IsNullOrEmpty(entity.UfsSelecionadas) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(entity.UfsSelecionadas);
            DataDeCadastro = entity.DatCriacao != null ? entity.DatCriacao.Value.ToString("dd/MM/yyyy") : "";
            DataDeAlteracao = entity.DatAlteracao != null ? entity.DatAlteracao.Value.ToString("dd/MM/yyyy") : "";
        }
        public AgPexRelSolicModelo retornaEntity()
        {
            if (String.IsNullOrEmpty(Nome))
            {
                throw new Exception("Preencha o nome do modelo");
            }

            return new AgPexRelSolicModelo()
            {
                Nome = Nome,
                Descricao = Descricao,
                DatIniSolcitacao = !String.IsNullOrEmpty(DataIniSolicitacao) ? DateTime.Parse(DataIniSolicitacao) : null,
                DatFimSolcitacao = !String.IsNullOrEmpty(DataFimSolicitacao) ? DateTime.Parse(DataFimSolicitacao) : null,
                DatIniVencimento = !String.IsNullOrEmpty(DataIniVencimento) ? DateTime.Parse(DataIniVencimento) : null,
                DatFimVencimento = !String.IsNullOrEmpty(DataFimVencimento) ? DateTime.Parse(DataFimVencimento) : null,
                ColunasRelSelecionadas = JsonConvert.SerializeObject(ColunasRelatorioSelecionadas),
                EscritoriosRelSelecionados = JsonConvert.SerializeObject(EscritoriosSelecionados),
                TiposLancRelSelecionados = JsonConvert.SerializeObject(TiposDeLancamentoSelecionados),
                UfsSelecionadas = JsonConvert.SerializeObject(UfsSelecionadas),
                UsrCodUsuario = "Perlink",
                StatusSolRelSelecionados = JsonConvert.SerializeObject(StatusSolicitacoesSelecionados),
                DatCriacao = DateTime.Now,
                DatAlteracao = DateTime.Now
            };
        }
    }

} 
