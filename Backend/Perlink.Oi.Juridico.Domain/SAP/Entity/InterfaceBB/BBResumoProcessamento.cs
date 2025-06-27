using FluentValidation;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB
{
    public class BBResumoProcessamento : EntityCrud<BBResumoProcessamento, long>
    {
        public long NumeroLoteBB { get; set; }
        public DateTime DataRemessa { get; set; }
        public DateTime? DataProcessamentoRemessa { get; set; }
        public decimal ValorTotalRemessa { get; set; }
        public long QuantidadeRegistrosProcessados { get; set; }
        public long QuantidadeRegistrosArquivo { get; set; }
        public decimal ValorTotalGuiaProcessada { get; set; }
        public long CodigoLote { get; set; }
        public long CodigoBBStatusRemessa { get; set; }
        public Lote Lote { get; set; }
        public BBStatusRemessa BBStatusRemessa { get; set; }

        public override AbstractValidator<BBResumoProcessamento> Validator => new BBResumoProcessamentoValidator();


        public override void PreencherDados(BBResumoProcessamento data)
        {
            NumeroLoteBB = data.NumeroLoteBB;
            DataRemessa = data.DataRemessa;
            DataProcessamentoRemessa = data.DataProcessamentoRemessa;
            ValorTotalRemessa = data.ValorTotalRemessa;
            QuantidadeRegistrosProcessados = data.QuantidadeRegistrosProcessados;
            ValorTotalGuiaProcessada = data.ValorTotalGuiaProcessada;
            CodigoLote = data.CodigoLote;
            CodigoBBStatusRemessa = data.CodigoBBStatusRemessa;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class BBResumoProcessamentoValidator : AbstractValidator<BBResumoProcessamento>
    {
        public BBResumoProcessamentoValidator()
        {
       
        }
    }
}
