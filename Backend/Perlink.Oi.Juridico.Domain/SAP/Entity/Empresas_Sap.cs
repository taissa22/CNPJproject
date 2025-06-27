using FluentValidation;
using Shared.Domain.Impl;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Empresas_Sap : EntityCrud<Empresas_Sap, long>
    {
        public override AbstractValidator<Empresas_Sap> Validator => new Empresas_SapValidator();

        public string Sigla { get; set; }
        public string Nome { get; set; }
        public bool IndicaEnvioArquivoSolicitacao { get; set; }
        public bool IndicaAtivo { get; set; }
        public long? CodigoRegiaoSAP { get; set; }
        public string CodigoOrganizacaoCompra { get; set; }
        public IList<FornecedorasContratos> fornecedorasContratos { get; set; }
        public IList<FornecedorasFaturas> fornecedorasFaturas { get; set; }
        public IList<EmpresasSapFornecedoras> empresasSapFornecedoras { get; set; }
        public IList<Parte> Partes { get; set; }

        public override void PreencherDados(Empresas_Sap data)
        {
            Sigla = data.Sigla;
            Nome = data.Nome;
            IndicaEnvioArquivoSolicitacao = data.IndicaEnvioArquivoSolicitacao;
            IndicaAtivo = data.IndicaAtivo;
            CodigoRegiaoSAP = data.CodigoRegiaoSAP;
            CodigoOrganizacaoCompra = data.CodigoOrganizacaoCompra;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class Empresas_SapValidator : AbstractValidator<Empresas_Sap>
        {
            public Empresas_SapValidator()
            {
                RuleFor(x => x.Sigla)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(4).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
                RuleFor(x => x.Nome)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
                RuleFor(x => x.CodigoOrganizacaoCompra)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(4).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
            }
        }

    }
}
