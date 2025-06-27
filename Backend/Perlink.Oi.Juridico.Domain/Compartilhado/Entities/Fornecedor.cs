using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Fornecedor : EntityCrud<Fornecedor, long>
    {
        public override AbstractValidator<Fornecedor> Validator => new FornecedorValidator();

        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSAP { get; set; }
        public long CodigoTipoFornecedor { get; set; }
        public long? CodigoEscritorio { get; set; }
        public long? CodigoProfissional { get; set; }
        public long? CodigoBanco { get; set; }
        public string NumeroCNPJ { get; set; }
        public double ValorCartaFianca { get; set; }
        public DateTime? DataCartaFianca { get; set; }
        public bool IndicaAtivoSAP { get; set; }
        public DateTime? DataAtualizaIndiceAtivo { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public  IList<Lote> Lotes { get; set; }
        public  IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public IList<FornecedorasContratos> fornecedorasContratos { get; set; }
        public IList<FornecedorasFaturas> fornecedorasFaturas { get; set; }
        public IList<EmpresasSapFornecedoras> EmpresasSapFornecedoras { get; set; }
        public  Banco Banco { get; set; }

        public Profissional Profissional { get; set; }

        public  Profissional Escritorio { get; set; }
        public IList<CompromissoProcesso> CompromissoProcessos { get; set; }

        public override void PreencherDados(Fornecedor data)
        {
            NomeFornecedor = data.NomeFornecedor;
            CodigoTipoFornecedor = data.CodigoTipoFornecedor;
            CodigoFornecedorSAP = data.CodigoFornecedorSAP;
            CodigoEscritorio = data.CodigoEscritorio;
            CodigoProfissional = data.CodigoProfissional;
            CodigoBanco = data.CodigoBanco;
            NumeroCNPJ = data.NumeroCNPJ;
            ValorCartaFianca = data.ValorCartaFianca;
            DataCartaFianca = data.DataCartaFianca;
            IndicaAtivoSAP = data.IndicaAtivoSAP;
            DataAtualizaIndiceAtivo = data.DataAtualizaIndiceAtivo;
            UsuarioUltimaAlteracao = data.UsuarioUltimaAlteracao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class FornecedorValidator : AbstractValidator<Fornecedor>
        {
            public FornecedorValidator()
            {
                RuleFor(x => x.NomeFornecedor)
                .MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);

               
            }
        }
    }
}