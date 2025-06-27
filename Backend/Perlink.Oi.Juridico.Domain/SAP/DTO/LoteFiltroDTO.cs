using AutoMapper;
using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LoteFiltroDTO
    {
        #region Filtros Gerais
        public DateTime? DataCriacaoPedidoMaior { get; set; }
        public DateTime? DataCriacaoPedidoMenor { get; set; }
        public DateTime? DataCriacaoMaior { get; set; }
        public DateTime? DataCriacaoMenor { get; set; }
        public long tipoProcesso { get; set; }
        public int StatusContabil { get; set; }
        public int StatusProcesso { get; set; }
        public DateTime? DataCancelamentoLoteInicio { get; set; }
        public DateTime? DataCancelamentoLoteFim { get; set; }
        public DateTime? DataErroProcessamentoInicio { get; set; }
        public DateTime? DataErroProcessamentoFim { get; set; }
        public DateTime? DataRecebimentoFiscalInicio { get; set; }
        public DateTime? DataRecebimentoFiscalFim { get; set; }
        public DateTime? DataPagamentoPedidoInicio { get; set; }
        public DateTime? DataPagamentoPedidoFim { get; set; }
        public DateTime? DataEnvioEscritorioInicio { get; set; }
        public DateTime? DataEnvioEscritorioFim { get; set; }
        public decimal? ValorTotalLoteInicio { get; set; }
        public decimal? ValorTotalLoteFim { get; set; }

        #endregion

        #region filtros listas dos critérios
        public IEnumerable<long?> IdsProcessos { get; set; }
        public IEnumerable<long?> NumeroContaJudicial { get; set; }
        public IEnumerable<long?> IdsEmpresasGrupo { get; set; }
        public IEnumerable<long?> IdsEscritorios { get; set; }
        public IEnumerable<long?> IdsFornecedores { get; set; }
        public IEnumerable<long?> IdsCentroCustos { get; set; }
        public IEnumerable<long?> IdsTipoLancamentos { get; set; }
        public IEnumerable<long?> IdsCategoriasPagamentos { get; set; }
        public IEnumerable<long?> IdsStatusPagamentos { get; set; }
        public IEnumerable<long?> IdsNumerosGuia { get; set; }
        public IEnumerable<long?> IdsPedidosSAP { get; set; }
        public IEnumerable<long?> IdsNumerosLote { get; set; }
        #endregion 

        public long Id { get; set; }
        #region Paginação
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public int Total { get; set; }
        #endregion
    }
}