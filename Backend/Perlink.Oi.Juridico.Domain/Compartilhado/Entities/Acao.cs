using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;


namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Acao : EntityCrud<Acao, long>
    {
        public override AbstractValidator<Acao> Validator => new AcaoValidator();
        public long? IdBBNaturezasAcoes { get; set; }
        public string Descricao { get; set; }
        public bool IndicadorAcaoCivel { get; set; }
        public bool IndicadorAcaoTrabalhista { get; set; }
        public bool IndicadorPrincipalParalela { get; set; }
        public bool? IndicadorAcaoTributaria { get; set; }
        public bool? IndicadorAcaoJuizado { get; set; }
        public bool IndicadorCivelEstrategico { get; set; }
        public bool IndicadorAtivo { get; set; }
        public bool IndicadorCriminalJudicial { get; set; }
        public bool IndicadorProcon { get; set; }
        public bool IndicadorPex { get; set; }
        public bool? IndicadorRequerEscritorio { get; set; }
        public BBNaturezasAcoes BBNaturezasAcoes { get; set; }

        public ClassesGarantias ClasseGarantia { get; set; }
        public GrupoCorrecaoGarantia GrupoCorrecao { get; set; }

        public override void PreencherDados(Acao data)
        {
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class AcaoValidator : AbstractValidator<Acao>
    {
        public AcaoValidator()
        {
            
        }
    }
}