using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class CategoriaPagamento : EntityCrud<CategoriaPagamento, long>
    {
        public override AbstractValidator<CategoriaPagamento> Validator => new CategoriaPagamentoValidator();

        public long CodigoTipoLancamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public long? CodigoMaterialSap { get; set; }
        public bool IndicadorCivel { get; set; }
        public bool IndicadorTrabalhista { get; set; }
        public bool IndicadorTributarioAdministrativo { get; set; }
        public bool IndicadorEnvioSap { get; set; }
        public bool IndicadorBaixaGarantia { get; set; }
        public bool IndicadorBaixaPagamento { get; set; }
        public bool IndicadorAtivo { get; set; }
        public string IndicadorBloqueioDeposito { get; set; }
        public bool IndicadorJuizado { get; set; }
        public bool IndicadorBaixaMulta { get; set; }
        public bool IndicadorCivelEstrategico { get; set; }
        public bool IndicadorTributarioJudicial { get; set; }
        public bool IndicadorInfluenciaContingencia { get; set; }
        public bool IndicadorAdministrativo { get; set; }
        public bool IndicadorRequerimentoNumeroGuia { get; set; }
        public long? ClgarCodigoClasseGarantia { get; set; }
        public bool IndicadorHistorico { get; set; }
        public long? TmgarCodigoTipoMovicadorGarantia { get; set; }
        public long? GrpcgIdGrupoCorrecaoGar { get; set; }
        public bool IndicadorFinalizacaoContabil { get; set; }
        public bool IndicadorCriminalAdministrativo { get; set; }
        public bool IndicadorCriminalJudicial { get; set; }
        public bool IndicadorCivelAdministrativo { get; set; }
        public long? CodigoMaterilSapR2 { get; set; }
        public bool IndicadorProcon { get; set; }
        public bool IndicadorPex { get; set; }
        public bool? IndicadorEncerraProcessoContabilmente { get; set; }
        public bool? IndicadorEscritorioSolicitaLancamento { get; set; }
        public bool? IndicadorRequerDataVencimento { get; set; }
        public bool? IndicadorRequerComprovanteSolicitacao { get; set; }
        public long? TipoFornecedorPermitido { get; set; }
        public string DescricaoJustificativaNaoInfluenciaContigencia { get; set; }
        public long? CodPagamentoA { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public IList<CategoriaFinalizacao> CategoriaFinalizacoes { get; set; }
        public IList<CompromissoProcesso> CompromissoProcessos { get; set; }

        public ClassesGarantias ClasseGarantia { get; set; }
        public GrupoCorrecaoGarantia GrupoCorrecao { get; set; }
        public decimal? ResponsabilidadeOi { get; set; }

        public override void PreencherDados(CategoriaPagamento data)
        {
            CodigoTipoLancamento = data.CodigoTipoLancamento;
            DescricaoCategoriaPagamento = data.DescricaoCategoriaPagamento;
            CodigoMaterialSap = data.CodigoMaterialSap;
            IndicadorCivel = data.IndicadorCivel;
            IndicadorTrabalhista = data.IndicadorTrabalhista;
            IndicadorTributarioAdministrativo = data.IndicadorTributarioAdministrativo;
            IndicadorEnvioSap = data.IndicadorEnvioSap;
            IndicadorBaixaGarantia = data.IndicadorBaixaGarantia;
            IndicadorBaixaPagamento = data.IndicadorBaixaPagamento;
            IndicadorAtivo = data.IndicadorAtivo;
            IndicadorBloqueioDeposito = data.IndicadorBloqueioDeposito;
            IndicadorJuizado = data.IndicadorJuizado;
            IndicadorBaixaMulta = data.IndicadorBaixaMulta;
            IndicadorCivelEstrategico = data.IndicadorCivelEstrategico;
            IndicadorTributarioJudicial = data.IndicadorTributarioJudicial;
            IndicadorInfluenciaContingencia = data.IndicadorInfluenciaContingencia;
            IndicadorAdministrativo = data.IndicadorAdministrativo;
            IndicadorRequerimentoNumeroGuia = data.IndicadorRequerimentoNumeroGuia;
            ClgarCodigoClasseGarantia = data.ClgarCodigoClasseGarantia;
            IndicadorHistorico = data.IndicadorHistorico;
            TmgarCodigoTipoMovicadorGarantia = data.TmgarCodigoTipoMovicadorGarantia;
            GrpcgIdGrupoCorrecaoGar = data.GrpcgIdGrupoCorrecaoGar;
            IndicadorFinalizacaoContabil = data.IndicadorFinalizacaoContabil;
            IndicadorCriminalAdministrativo = data.IndicadorCriminalAdministrativo;
            IndicadorCriminalJudicial = data.IndicadorCriminalJudicial;
            IndicadorCivelAdministrativo = data.IndicadorCivelAdministrativo;
            CodigoMaterilSapR2 = data.CodigoMaterilSapR2;
            IndicadorProcon = data.IndicadorProcon;
            IndicadorPex = data.IndicadorPex;
            IndicadorEncerraProcessoContabilmente = data.IndicadorEncerraProcessoContabilmente;
            IndicadorEscritorioSolicitaLancamento = data.IndicadorEscritorioSolicitaLancamento;
            IndicadorRequerDataVencimento = data.IndicadorRequerDataVencimento;
            IndicadorRequerComprovanteSolicitacao = data.IndicadorRequerComprovanteSolicitacao;
            TipoFornecedorPermitido = data.TipoFornecedorPermitido;
            DescricaoJustificativaNaoInfluenciaContigencia = data.DescricaoJustificativaNaoInfluenciaContigencia;
            CodPagamentoA = data.CodPagamentoA;
            ResponsabilidadeOi = data.ResponsabilidadeOi;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class CategoriaPagamentoValidator : AbstractValidator<CategoriaPagamento>
    {
        public CategoriaPagamentoValidator()
        {
            RuleFor(x => x.DescricaoCategoriaPagamento)
                .NotEmpty()
                .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                .MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                .WithName("Descrição");

            RuleFor(x => x.IndicadorAtivo)
               .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => new { x.CodigoMaterialSap, x.IndicadorEnvioSap })
                .Custom((obj, context) =>
                {
                    if ((!obj.CodigoMaterialSap.HasValue || obj.CodigoMaterialSap == 0) && obj.IndicadorEnvioSap)
                    {
                        context.AddFailure("O preenchimento do Código Material Sap é obrigatório quando marcar Envia Sap.");
                    }
                });

            RuleFor(x => new { x.ResponsabilidadeOi, x.DescricaoCategoriaPagamento})
            .Custom((obj, context) =>
            {
                if (!string.IsNullOrEmpty(obj.DescricaoCategoriaPagamento) && obj.ResponsabilidadeOi == 0)
                {
                    context.AddFailure("O % de responsabilidade Oi deve ser maior que 0 e menor que 100.");
                }
            });
        }
    }
}