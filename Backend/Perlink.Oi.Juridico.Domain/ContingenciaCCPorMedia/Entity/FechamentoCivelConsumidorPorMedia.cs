using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Entity
{
    public class FechamentoCivelConsumidorPorMedia : EntityCrud<FechamentoCivelConsumidorPorMedia, long>
    {
        public override AbstractValidator<FechamentoCivelConsumidorPorMedia> Validator => new FechamentoCCPorMediaValidator();

        public long? CodSolicFechamentoCont { get; set; }
        //public long? CodEmpresaCentralizadora { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime? DataIndMensal { get; set; }
        public long CodEmpresaCentralizadora { get; set; }
        //public EmpresasCentralizadoras EmpresasCentralizadoras { get; set; }
        public string IndBaseGerada { get; set; }
        public string IndMensal { get; set; }
        public DateTime? MesAnoFechamento { get; set; }
        public long NroMesesMediaHistorica { get; set; }
        public decimal? PerHaircut { get; set; }
        public string CodUsuario { get; set; }
        public decimal ValorCorte { get; set; }
        
        public override void PreencherDados(FechamentoCivelConsumidorPorMedia data)
        {
            CodSolicFechamentoCont = data.CodSolicFechamentoCont;
            //CodTipoProcessoAssociaFechamento = data.CodTipoProcessoAssociaFechamento;
            DataFechamento = data.DataFechamento;
            DataGeracao = data.DataGeracao;
            DataIndMensal = data.DataIndMensal;
            CodEmpresaCentralizadora = data.CodEmpresaCentralizadora;
            IndBaseGerada = data.IndBaseGerada;
            IndMensal = data.IndMensal;
            MesAnoFechamento = data.MesAnoFechamento;
            NroMesesMediaHistorica = data.NroMesesMediaHistorica;
            PerHaircut = data.PerHaircut;
            CodUsuario = data.CodUsuario;
            ValorCorte = data.ValorCorte;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class FechamentoCCPorMediaValidator : AbstractValidator<FechamentoCivelConsumidorPorMedia>
        {
            public FechamentoCCPorMediaValidator()
            {
            }

        }

        
    }
}
