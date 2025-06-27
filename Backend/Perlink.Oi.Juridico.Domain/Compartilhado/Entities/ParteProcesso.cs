using FluentValidation;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities 
{
    public class ParteProcesso : EntityCrud<ParteProcesso, long> 
    {
        public override AbstractValidator<ParteProcesso> Validator => throw new NotImplementedException();

        ////COD_PROCESSO == id
        
        //COD_PARTE
        public long CodigoParte { get; set; }

        //COD_TIPO_PARTICIPACAO
        public long? CodigoTipoParticipacao { get; set; }

        public TipoParticipacao TipoParticipacao { get; set; }

        //DAT_DESLIGAMENTO
        public DateTime? DataDesligamento { get; set; }

        //STNEG_COD_STATUS_ANDAMENTO_NEG
        public long? CodigoStatusAndamentoNegociacao { get; set; }

        //DATA_ULT_ATU_STATUS_AND_NEG
        public DateTime? DataUltimaAtualizacaoStatusAndamentoNegociacao { get; set; }

        //VALOR_DEPOSITO_RECURSAL
        public double ValorDepositoRecursal { get; set; }

        //VALOR_RECUPERADO
        public double ValorRecuperado { get; set; }

        //VALOR_RECUPERADO_CUSTAS
        public double ValorrecuperadoCustas { get; set; }

        //DESC_NEGOCIACAO_FORNECEDOR
        public string DescricaoNegociacaoFornecedor { get; set; }

        //TPRE_COD_TIPO_RELAC_EMPRESA
        public long? CodigoTipoRelacionamentoEmpresa { get; set; }

        //CPF_REU
        public string CpfReu { get; set; }

        //IND_ACESSO_RESTRITO
        public bool IndicaAcessoRestrito { get; set; }

        //COD_TIPO_IDENTIFICACAO_PARTE
        public string CodigoTipoIdentificacaoParte { get; set; }

        //ID_CLASSIFICACAO_AUTOR
        public long? CodigoClassificacaoAutor { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public Parte Parte { get; set; }
        public Processo Processo { get; set; }

        public override void PreencherDados(ParteProcesso data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }
    }
}
