using FluentValidation;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using Perlink.Oi.Juridico.Domain.Fechamento.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity
{
    public class AgendarApuracaoOutliers : EntityCrud<AgendarApuracaoOutliers, long>
    {
        public override AbstractValidator<AgendarApuracaoOutliers> Validator => new AgendarApuracaoOutliersValidator();

        public long CodigoEmpresaCentralizadora { get; set; }
        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal FatorDesvioPadrao { get; set; }
        public string Observacao { get; set; }
        public string NomeUsuario { get; set; }
        public string ArquivoBaseFechamento { get; set; }
        public string ArquivoResultado { get; set; }
        public AgendarApuracaoOutliersStatusEnum Status { get; set; }
        public string MgsStatusErro { get; set; }
        public FechamentosProcessosJEC FechamentosProcessosJEC { get; set; }
        public decimal ValorDesvioPadrao { get; set; }
        public decimal ValorMedia { get; set; }
        public decimal ValorCorteOutliers { get; set; }
        public decimal ValorTotalProcessos { get; set; }
        public long QtdProcessos { get; set; }

        public override void PreencherDados(AgendarApuracaoOutliers data)
        {
            Id = data.Id;
            CodigoEmpresaCentralizadora = data.CodigoEmpresaCentralizadora;
            MesAnoFechamento = data.MesAnoFechamento;
            DataFechamento = data.DataFechamento;
            DataFinalizacao = data.DataFinalizacao;
            DataSolicitacao = data.DataSolicitacao;
            FatorDesvioPadrao = data.FatorDesvioPadrao;
            Observacao = data.Observacao;
            NomeUsuario = data.NomeUsuario;
            ArquivoBaseFechamento = data.ArquivoBaseFechamento;
            ArquivoResultado = data.ArquivoResultado;
            Status = data.Status;
            MgsStatusErro = data.MgsStatusErro;
            ValorDesvioPadrao = data.ValorDesvioPadrao;
            ValorMedia = data.ValorMedia;
            ValorCorteOutliers = data.ValorCorteOutliers;
            ValorTotalProcessos = data.ValorTotalProcessos;
            QtdProcessos = data.QtdProcessos;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class AgendarApuracaoOutliersValidator : AbstractValidator<AgendarApuracaoOutliers>
    {
        public AgendarApuracaoOutliersValidator()
        {

        }
    }
}
