using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB {
    /// <summary>
    /// Representa o filtro da tela de busca
    /// </summary>
    public class BBResumoProcessamentoFiltroDTO : OrdernacaoPaginacaoDTO
    {
        #region Filtros Gerais
        public DateTime? DataRemessaMenor { get; set; }
        public DateTime? DataRemessaMaior { get; set; }
        public long? NumeroRemessaMenor { get; set; }
        public long? NumeroRemessaMaior { get; set; }
        public decimal? ValorGuiaInicio { get; set; }
        public decimal? ValorGuiaFim { get; set; }
        #endregion

        #region filtros listas dos critérios
        public IEnumerable<long?> IdsProcessosJEC { get; set; }
        public IEnumerable<long?> IdsProcessosCC { get; set; }
        public IEnumerable<long?> IdsProcessosPEX { get; set; }
        public IEnumerable<long?> NumerosContasJudiciais { get; set; }
        public IEnumerable<long?> IdsNumerosGuia { get; set; }
        #endregion 
    }
    /// <summary>
    /// Representa o Resumo do arquivo (Dados do resultado da Busca e Resumo do arquivo importado)
    /// </summary>
    public class BBResumoProcessamentoResultadoDTO
    {
        public string TipoProcesso { get; set; }
        public long NumeroLoteBB { get; set; }
        public string DataRemessa { get; set; }
        public string DataProcessamentoRemessa { get; set; }
        public string Status { get; set; }
        public long QuantidadeRegistrosArquivo { get; set; }
        public decimal ValorTotalRemessa { get; set; }
        public long QuantidadeRegistrosProcessados { get; set; }
        public decimal ValorTotalGuiaProcessada { get; set; }
        public long IdBBStatusRemessa { get; set; }
        public long CodLoteSisJur { get; set; }
    }
    /// <summary>
    /// Representa as Guias OK
    /// </summary>
    public class BBResumoProcessamentoGuiaDTO {
        public string CodigoProcesso { get; set; }
        public string CodigoLancamento { get; set; }
        public string NumeroProcesso { get; set; }
        public string Comarca { get; set; }
        public string Juizado { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string DataLancamento { get; set; }
        public string DescricaoTipoLancamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public string DataEnvioEscritorio { get; set; }
        public string DescricaoEscritorio { get; set; }
        public string NumeroPedidoSAP { get; set; }
        public string NumeroGuia { get; set; }
        public string DataRecebimentoFiscal { get; set; }
        public string DataPagamentoPedido { get; set; }
        public decimal ValorLiquido { get; set; }
        public string Autor { get; set; }
        public string NumeroContaJudicial { get; set; }
        public string NumeroParcelaJudicial { get; set; }
        public string AutenticacaoEletronica { get; set; }
        public string DataEfetivacaoParcelaBB { get; set; }
        public string StatusParcelaBB { get; set; }
        public string IdBBStatusParcela { get; set; }
    }
    /// <summary>
    /// DTO para armazenar dados recuperados do arquivo importado
    /// </summary>
    public class BBResumoProcessamentoImportacaoDTO
    {
        public BBResumoProcessamentoImportacaoDTO()
        {
            GuiasOk = new List<BBResumoProcessamentoGuiaDTO>();
            GuiasComProblema = new List<BBResumoProcessamentoGuiasComProblemaDTO>();
            Resumo = new BBResumoProcessamentoResultadoDTO();
        }
        public ICollection<BBResumoProcessamentoGuiaDTO> GuiasOk { get; set; }
        public ICollection<BBResumoProcessamentoGuiasComProblemaDTO> GuiasComProblema { get; set; }
        public BBResumoProcessamentoResultadoDTO Resumo { get; set; }
        public string NomeArquivo { get; set; }
        public string MsgErro { get; set; }
    }
    /// <summary>
    /// Representa Guias com Erro
    /// </summary>
    public class BBResumoProcessamentoGuiasComProblemaDTO
    {
        public string NumeroProcesso { get; set; }
        public string CodigoComarca { get; set; }
        public string NomeComarca { get; set; }
        public string CodigoOrgaoBB { get; set; }
        public string NomeOrgaoBB { get; set; }
        public string CodigoNaturezaAcaoBB { get; set; }
        public string NomeNaturezaBB { get; set; }
        public string NomeAutor { get; set; }
        public string AutorCPF_CNPJ { get; set; }
        public string NomeReu { get; set; }
        public string ReuCPF_CNPJ { get; set; }
        public decimal ValorParcela { get; set; }
        public string DataGuia { get; set; }
        public string Guia { get; set; }
        public string NumeroConta { get; set; }
        public string NumeroParcela { get; set; }
        public string IdBBStatusParcela { get; set; }
        public string DescricaoErroGuia { get; set; }
        public string DataEfetivacaoParcelaBB { get; set; }
        public string StatusParcelaBB { get; set; }
        public string IdProcesso { get; set; }
        public string IdLancamento { get; set; }
    }

   
    public class BBResumoProcessamentoGuiaExibidaDTO : BBResumoProcessamentoGuiaDTO {

        public string DataRecebimentoFisico { get; set; }
        public string Agencia { get; set; }
        public string DescricaoModalidadeBB { get; set; }
        public string DataGuia { get; set; }
        public string DescricaoTribunalBB { get; set; }
        public string DescricaoOrgaoBB { get; set; }
        public string Depositante { get; set; }
        public string TipoReu { get; set; } 
        public string CpfCnpjReu { get; set; }
        public string TipoAutor { get; set; } 
        public string CpfCnpjAutor { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }
        public string CodigoEstado { get; set; }
    }
   
}
