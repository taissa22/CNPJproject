using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Application.SAP
{
    public static class CustomExtension
    {
        public static Lote MapearParaEntidadeLote(this LoteCriacaoViewModel loteCriacaoViewModel)
        {
            var lote = new Lote()
            {
                Id = loteCriacaoViewModel.Id,
                DescricaoLote = loteCriacaoViewModel.IdentificacaoLote,
                Valor = loteCriacaoViewModel.ValorLote,
                DataCriacao = loteCriacaoViewModel.DataCriacaoLote,
                CodigoTipoProcesso = loteCriacaoViewModel.codigoTipoProcesso,
                CodigoParte = loteCriacaoViewModel.CodigoParteEmpresa,
                CodigoFornecedor = loteCriacaoViewModel.CodigoFornecedor,
                CodigoCentroCusto = loteCriacaoViewModel.CodigoCentroCusto,
                CodigoFormaPagamento = loteCriacaoViewModel.CodigoFormaPagamento,
                CodigoCentroSAP = loteCriacaoViewModel.CodigoCentroSAP.ToString(),
                CodigoUsuario = loteCriacaoViewModel.CodigoUsuario.ToString(),
                CodigoStatusPagamento = loteCriacaoViewModel.CodigoStatusPagamento,
            };


            return lote;
        }

        public static Fornecedor MapearParaEntidadeFornecedor(this FornecedorCriacaoViewModel fornecedorCriacaoViewModel)
        {

            var fornecedor = new Fornecedor()
            {

                Id = fornecedorCriacaoViewModel.Id,
                NomeFornecedor = fornecedorCriacaoViewModel.NomeFornecedor,
                CodigoTipoFornecedor = fornecedorCriacaoViewModel.CodigoTipoFornecedor,
                CodigoEscritorio = fornecedorCriacaoViewModel.CodigoEscritorio > 0 ? fornecedorCriacaoViewModel.CodigoEscritorio : null,
                CodigoProfissional = fornecedorCriacaoViewModel.CodigoProfissional > 0 ? fornecedorCriacaoViewModel.CodigoProfissional : null,
                CodigoBanco = fornecedorCriacaoViewModel.CodigoBanco > 0 ? fornecedorCriacaoViewModel.CodigoBanco : null,

                CodigoFornecedorSAP = fornecedorCriacaoViewModel.CodigoFornecedorSAP,

            };

            return fornecedor;
        }

        public static Fornecedor MapearParaEntidadeFornecedorEdicao(this FornecedorAtualizarViewModel FornecedorAtualizarViewModel)
        {

            var fornecedor = new Fornecedor()
            {
                Id = FornecedorAtualizarViewModel.Id,
                NomeFornecedor = FornecedorAtualizarViewModel.NomeFornecedor,
                CodigoTipoFornecedor = FornecedorAtualizarViewModel.CodigoTipoFornecedor,
                CodigoEscritorio = FornecedorAtualizarViewModel.CodigoEscritorio > 0 ? FornecedorAtualizarViewModel.CodigoEscritorio : null,
                CodigoProfissional = FornecedorAtualizarViewModel.CodigoProfissional > 0 ? FornecedorAtualizarViewModel.CodigoProfissional : null,
                CodigoBanco = FornecedorAtualizarViewModel.CodigoBanco > 0 ? FornecedorAtualizarViewModel.CodigoBanco : null,

                CodigoFornecedorSAP = FornecedorAtualizarViewModel.CodigoFornecedorSAP,


            };

            return fornecedor;
        }
    }
}

