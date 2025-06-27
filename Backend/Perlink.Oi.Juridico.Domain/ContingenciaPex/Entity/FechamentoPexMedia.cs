using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity
{
    public class FechamentoPexMedia : EntityCrud<FechamentoPexMedia, long>
    {
        public override AbstractValidator<FechamentoPexMedia> Validator => new FechamentoPexMediaValidator();

        public long CodEmpresaCentralizadora { get; set; }
        public long? CodSolicFechamentoCont { get; set; }
        public string CodUsuario { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime? DataIndMensal { get; set; }
        public bool? IndAplicarHaircutProcGar { get; set; }
        public bool IndMensal { get; set; }
        public long NroMesesMediaHistorica { get; set; }
        public decimal? PerHaircut { get; set; }
        public decimal? ValMultDesvioPadrao { get; set; }

        public override void PreencherDados(FechamentoPexMedia data)
        {
            CodEmpresaCentralizadora = data.CodEmpresaCentralizadora;
            CodSolicFechamentoCont = data.CodSolicFechamentoCont;
            CodUsuario = data.CodUsuario;
            DataFechamento = data.DataFechamento;
            DataGeracao = data.DataGeracao;
            DataIndMensal = data.DataIndMensal;
            IndAplicarHaircutProcGar = data.IndAplicarHaircutProcGar;
            IndMensal = data.IndMensal;
            NroMesesMediaHistorica = data.NroMesesMediaHistorica;
            PerHaircut = data.PerHaircut;
            ValMultDesvioPadrao = data.ValMultDesvioPadrao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class FechamentoPexMediaValidator : AbstractValidator<FechamentoPexMedia>
    {
        public FechamentoPexMediaValidator()
        {
        }
    }
}
