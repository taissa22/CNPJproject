using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel {
    public class LoteLancamentoViewModel {
        public long Id { get; set; }
        [Name("Nº Processo")]
        public string NumeroProcesso { get;  set; }
        [Name("Comarca")]
        public string Comarca { get;  set; }
        [Name("Vara")]
        public string Vara { get;  set; }
        [Name("Data Envio Escritório")]
        public DateTime? DataEnvioEscritorio { get;  set; }
        [Name("Escritório")]
        public string Escritorio { get;  set; }
        [Name("Tipo de Lançamento")]
        public string TipoLancamento { get;  set; }
        [Name("Categoria de Pagamento")]
        public string CategoriaPagamento { get;  set; }
        [Name("Status Pagamento")]
        public string StatusPagamento { get;  set; }
        [Name("Data Lançamento")]
        public string DataLancamento { get;  set; }
        [Name("Nº Pedido SAP")]
        public string NumeroPedidoSAP { get;  set; }
        [Name("Data Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get;  set; }
        [Name("Data Pagto Pedido")]
        public string DataPagamentoPedido { get;  set; }
        [Name("Valor Líquido")]
        public string ValorLiquido { get;  set; }
        [Name("Texto SAP")]
        public string TextoSAP { get;  set; }
        [Name("Comentário")]
        public string Comentario { get;  set; }
        [Name("Nº Guia")]
        public string NumeroGuia { get;  set; }
        [Name("Autor")]
        public string Autor { get;  set; }
        [Name("Nº Conta Judicial")]
        public string NumeroContaJudicial { get;  set; }
        [Name("Nº Parcela Judicial")]
        public string NumeroParcelaJudicial { get;  set; }
        [Name("Autenticação Eletrônica")]
        public string AutenticacaoEletronica { get;  set; }
        [Name("Status Parcela BB")]
        public string StatusParcelaBancoDoBrasil { get;  set; }
        [Name("Data Efetivação Parcela BB")]
        public string DataEfetivacaoParcelaBancoDoBrasil { get;  set; }
        public bool LancamentoEstornado { get; set; }

        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteLancamentoDTO, LoteLancamentoViewModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => orig.NumeroProcesso))
              .ForMember(dest => dest.Comarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
              .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}",orig.CodigoVara, orig.NomeTipoVara)))
              .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio))
              .ForMember(dest => dest.Escritorio, opt => opt.MapFrom(orig => orig.Escritorio))
              .ForMember(dest => dest.TipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
              .ForMember(dest => dest.CategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
              .ForMember(dest => dest.StatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
              .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
              .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.NumeroGuia))
              .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
              .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal))
              .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
              .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido))
              .ForMember(dest => dest.TextoSAP, opt => opt.MapFrom(orig => orig.TextoSAP))
              .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
              .ForMember(dest => dest.Autor, opt => opt.MapFrom(orig => orig.Autor ?? ""))
              .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => orig.NumeroContaJudicial))
              .ForMember(dest => dest.NumeroParcelaJudicial, opt => opt.MapFrom(orig => orig.NumeroParcelaJudicial))
              .ForMember(dest => dest.AutenticacaoEletronica, opt => opt.MapFrom(orig => orig.AutenticacaoEletronica ?? ""))
              .ForMember(dest => dest.StatusParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.StatusParcelaBancoDoBrasil ?? ""))
              .ForMember(dest => dest.DataEfetivacaoParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.DataEfetivacaoParcelaBancoDoBrasil))
              .ForMember(dest => dest.CodigoProcesso, opt => opt.MapFrom(orig => orig.CodigoProcesso))
              .ForMember(dest => dest.CodigoLancamento, opt => opt.MapFrom(orig => orig.CodigoLancamento));

            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoViewModel>()
             .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => orig.NumeroProcesso))
             .ForMember(dest => dest.Comarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
             .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}", orig.CodigoVara, orig.NomeTipoVara)))
             .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio))
             .ForMember(dest => dest.Escritorio, opt => opt.MapFrom(orig => orig.Escritorio))
             .ForMember(dest => dest.TipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
             .ForMember(dest => dest.CategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
             .ForMember(dest => dest.StatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
             .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
             .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.NumeroGuia))
             .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
             .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal))
             .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
             .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido))
             .ForMember(dest => dest.TextoSAP, opt => opt.MapFrom(orig => orig.TextoSAP))
             .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
             .ForMember(dest => dest.Autor, opt => opt.MapFrom(orig => orig.Autor ?? ""))
             .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => orig.NumeroContaJudicial))
             .ForMember(dest => dest.NumeroParcelaJudicial, opt => opt.MapFrom(orig => orig.NumeroParcelaJudicial))
             .ForMember(dest => dest.AutenticacaoEletronica, opt => opt.MapFrom(orig => orig.AutenticacaoEletronica ?? ""))
             .ForMember(dest => dest.StatusParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.StatusParcelaBancoDoBrasil ?? ""))
             .ForMember(dest => dest.DataEfetivacaoParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.DataEfetivacaoParcelaBancoDoBrasil));
        }
    }   
    public class LoteLancamentoCeViewModel{
        [Name("Lançamento Estornado")]
        public string LancamentoEstornado { get; set; }  
        [Name("cod_lote")]
        public string CodigoLote { get; set; }
        [Name("cod_processo")]
        public string CodigoProcesso { get; set; }
        [Name("cod_lancamento")]
        public string CodigoLancamento { get; set; }
        [Name("nro_processo_cartorio")]
        public string NumeroProcessoCartorio { get; set; }
        [Name("cod_estado")]
        public string CodigoEstado { get; set; }
        [Name("cod_comarca")]
        public string CodigoComarca { get; set; }
        [Name("nom_comarca")]
        public string NomeComarca { get; set; }
        [Name("cod_vara")]
        public string CodigoVara { get; set; }
        [Name("cod_tipo_vara")]
        public string CodigoTipoVara { get; set; }
        [Name("nom_tipo_vara")]
        public string NomeTipoVara { get; set; }
        [Name("cod_tipo_lancamento")]
        public string CodigoTipoLancamento { get; set; }
        [Name("dsc_tipo_lancamento")]
        public string DescricaoTipoLancamento { get; set; }
        [Name("cod_cat_pagamento")]
        public string CodigoCategoriaPagamento { get; set; }
        [Name("dsc_cat_pagamento")]
        public string DescricaoCategoriaPagamento { get; set; }
        [Name("dat_lancamento")]
        public string DataLancamento { get; set; }
        [Name("qtd_lancamento")]
        public string QuantidadeLancamento { get; set; }
        [Name("val_lancamento")]
        public string ValorLancamento { get; set; }
        [Name("comentario_sap")]
        public string ComentarioSap { get; set; }
        [Name("nro_pedido_sap")]
        public string NumeroPedidoSAP { get; set; }
        [Name("dat_criacao_pedido")]
        public string DataCriacaoPedido { get; set; }
        [Name("dat_recebimento_fisico")]
        public string DataRecebimentoFisico { get; set; }
        [Name("dat_pagamento_pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("cod_usuario_recebedor")]
        public string CodigoUsuarioRecebedor { get; set; }
        [Name("dat_envio_escritorio")]
        public string DataEnvioEscritorio { get; set; }
        [Name("comentario")]
        public string Comentario { get; set; }
        [Name("ind_excluido")]
        public string IndicaExcluido { get; set; }
        [Name("nom_profissional")]
        public string NomeProfissional { get; set; }
        [Name("cod_status_pagamento")]
        public string CodigoStatusPagamento { get; set; }
        [Name("dsc_status_pagamento")]
        public string DescricaoStatusPagamento { get; set; }
        [Name("cod_profissional")]
        public string CodigoProfissional { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoCeViewModel>()
                .ForMember(dest => dest.CodigoLote, opt => opt.MapFrom(orig => orig.Id.ToString()))
                .ForMember(dest => dest.CodigoProcesso, opt => opt.MapFrom(orig => orig.CodigoProcesso))
                .ForMember(dest => dest.CodigoLancamento, opt => opt.MapFrom(orig => orig.CodigoLancamento))
                .ForMember(dest => dest.NumeroProcessoCartorio, opt => opt.MapFrom(orig => orig.NumeroProcesso))
                .ForMember(dest => dest.CodigoEstado, opt => opt.MapFrom(orig => orig.Uf))
                .ForMember(dest => dest.CodigoComarca, opt => opt.MapFrom(orig => orig.CodigoComarca))
                .ForMember(dest => dest.NomeComarca, opt => opt.MapFrom(orig => orig.NomeComarca))
                .ForMember(dest => dest.CodigoVara, opt => opt.MapFrom(orig => orig.CodigoVara))
                .ForMember(dest => dest.CodigoTipoVara, opt => opt.MapFrom(orig => orig.CodigoTipoVara))
                .ForMember(dest => dest.NomeTipoVara, opt => opt.MapFrom(orig => orig.NomeTipoVara))
                .ForMember(dest => dest.CodigoTipoLancamento, opt => opt.MapFrom(orig => orig.CodigoTipoLancamento))
                .ForMember(dest => dest.DescricaoTipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
                .ForMember(dest => dest.CodigoCategoriaPagamento, opt => opt.MapFrom(orig => orig.CodigoCategoriaPagamento))
                .ForMember(dest => dest.DescricaoCategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
                .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
                .ForMember(dest => dest.QuantidadeLancamento, opt => opt.MapFrom(orig => orig.QuantidadeLancamento))
                .ForMember(dest => dest.ValorLancamento, opt => opt.MapFrom(orig => orig.ValorLiquido))
                .ForMember(dest => dest.ComentarioSap, opt => opt.MapFrom(orig => orig.TextoSAP))
                .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
                .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido))
                .ForMember(dest => dest.DataRecebimentoFisico, opt => opt.MapFrom(orig => orig.DataRecebimentoFisico))
                .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
                .ForMember(dest => dest.CodigoUsuarioRecebedor, opt => opt.MapFrom(orig => orig.CodigoUsuarioRecebedor))
                .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio.HasValue ? orig.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : ""))
                .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
                .ForMember(dest => dest.IndicaExcluido, opt => opt.MapFrom(orig => orig.IndicaExcluido))
                .ForMember(dest => dest.LancamentoEstornado, opt => opt.MapFrom(orig => orig.IndicaExcluido == "S" ? "Sim" : "Não"))
                .ForMember(dest => dest.NomeProfissional, opt => opt.MapFrom(orig => orig.Escritorio))
                .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento))
                .ForMember(dest => dest.DescricaoStatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
                .ForMember(dest => dest.CodigoProfissional, opt => opt.MapFrom(orig => orig.CodigoProfissional));
        }
    }
    public class LoteLancamentoTrabViewModel {
        [Name("Lançamento Estornado")]
        public string LancamentoEstornado { get; set; }  
        [Name("cod_lote")]
        public string CodigoLote { get; set; }
        [Name("cod_processo")]
        public string CodigoProcesso { get; set; }
        [Name("cod_lancamento")]
        public string CodigoLancamento { get; set; }
        [Name("nro_processo_cartorio")]
        public string NumeroProcessoCartorio { get; set; }
        [Name("cod_estado")]
        public string CodigoEstado { get; set; }
        [Name("cod_comarca")]
        public string CodigoComarca { get; set; }
        [Name("nom_comarca")]
        public string NomeComarca { get; set; }
        [Name("cod_vara")]
        public string CodigoVara { get; set; }
        [Name("cod_tipo_vara")]
        public string CodigoTipoVara { get; set; }
        [Name("nom_tipo_vara")]
        public string NomeTipoVara { get; set; }
        [Name("cod_tipo_lancamento")]
        public string CodigoTipoLancamento { get; set; }
        [Name("dsc_tipo_lancamento")]
        public string DescricaoTipoLancamento { get; set; }
        [Name("cod_cat_pagamento")]
        public string CodigoCategoriaPagamento { get; set; }
        [Name("dsc_cat_pagamento")]
        public string DescricaoCategoriaPagamento { get; set; }
        [Name("dat_lancamento")]
        public string DataLancamento { get; set; }
        [Name("qtd_lancamento")]
        public string QuantidadeLancamento { get; set; }
        [Name("val_lancamento")]
        public string ValorLancamento { get; set; }
        [Name("comentario_sap")]
        public string ComentarioSap { get; set; }
        [Name("nro_pedido_sap")]
        public string NumeroPedidoSAP { get; set; }
        [Name("dat_criacao_pedido")]
        public string DataCriacaoPedido { get; set; }
        [Name("dat_recebimento_fisico")]
        public string DataRecebimentoFisico { get; set; }
        [Name("dat_pagamento_pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("cod_usuario_recebedor")]
        public string CodigoUsuarioRecebedor { get; set; }
        [Name("dat_envio_escritorio")]
        public string DataEnvioEscritorio { get; set; }
        [Name("comentario")]
        public string Comentario { get; set; }
        [Name("ind_excluido")]
        public string IndicaExcluido { get; set; }
        [Name("nom_profissional")]
        public string NomeProfissional { get; set; }
        [Name("cod_status_pagamento")]
        public string CodigoStatusPagamento { get; set; }
        [Name("dsc_status_pagamento")]
        public string DescricaoStatusPagamento { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoTrabViewModel>()
                .ForMember(dest => dest.CodigoLote, opt => opt.MapFrom(orig => orig.Id.ToString()))
                .ForMember(dest => dest.CodigoProcesso, opt => opt.MapFrom(orig => orig.CodigoProcesso))
                .ForMember(dest => dest.CodigoLancamento, opt => opt.MapFrom(orig => orig.CodigoLancamento))
                .ForMember(dest => dest.NumeroProcessoCartorio, opt => opt.MapFrom(orig => orig.NumeroProcesso))
                .ForMember(dest => dest.CodigoEstado, opt => opt.MapFrom(orig => orig.Uf))
                .ForMember(dest => dest.CodigoComarca, opt => opt.MapFrom(orig => orig.CodigoComarca))
                .ForMember(dest => dest.NomeComarca, opt => opt.MapFrom(orig => orig.NomeComarca))
                .ForMember(dest => dest.CodigoVara, opt => opt.MapFrom(orig => orig.CodigoVara))
                .ForMember(dest => dest.CodigoTipoVara, opt => opt.MapFrom(orig => orig.CodigoTipoVara))
                .ForMember(dest => dest.NomeTipoVara, opt => opt.MapFrom(orig => orig.NomeTipoVara))
                .ForMember(dest => dest.CodigoTipoLancamento, opt => opt.MapFrom(orig => orig.CodigoTipoLancamento))
                .ForMember(dest => dest.DescricaoTipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
                .ForMember(dest => dest.CodigoCategoriaPagamento, opt => opt.MapFrom(orig => orig.CodigoCategoriaPagamento))
                .ForMember(dest => dest.DescricaoCategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
                .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
                .ForMember(dest => dest.QuantidadeLancamento, opt => opt.MapFrom(orig => orig.QuantidadeLancamento))
                .ForMember(dest => dest.ValorLancamento, opt => opt.MapFrom(orig => orig.ValorLiquido))
                .ForMember(dest => dest.ComentarioSap, opt => opt.MapFrom(orig => orig.TextoSAP))
                .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
                .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido))
                .ForMember(dest => dest.DataRecebimentoFisico, opt => opt.MapFrom(orig => orig.DataRecebimentoFisico))
                .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
                .ForMember(dest => dest.CodigoUsuarioRecebedor, opt => opt.MapFrom(orig => orig.CodigoUsuarioRecebedor))
                .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio.HasValue ? orig.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : ""))
                .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
                .ForMember(dest => dest.IndicaExcluido, opt => opt.MapFrom(orig => orig.IndicaExcluido))
                .ForMember(dest => dest.LancamentoEstornado, opt => opt.MapFrom(orig => orig.IndicaExcluido == "S" ?  "Sim" : "Não"))
                .ForMember(dest => dest.NomeProfissional, opt => opt.MapFrom(orig => orig.Escritorio))
                .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento))
                .ForMember(dest => dest.DescricaoStatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento));
        }
    }
    public class LoteLancamentoCcViewModel {
        [Name("Lançamento Estornado")]
        public string LancamentoEstornado { get; set; }
        [Name("Nº Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Comarca")]
        public string Comarca { get; set; }
        [Name("Vara")]
        public string Vara { get; set; }
        [Name("Data Envio Escritório")]
        public string DataEnvioEscritorio { get; set; }
        [Name("Escritório")]
        public string Escritorio { get; set; }
        [Name("Tipo de Lançamento")]
        public string TipoLancamento { get; set; }
        [Name("Categoria de Pagamento")]
        public string CategoriaPagamento { get; set; }
        [Name("Status Pagamento")]
        public string StatusPagamento { get; set; }
        [Name("Data Lançamento")]
        public string DataLancamento { get; set; }
        [Name("Nº Pedido SAP")]
        public string NumeroPedidoSAP { get; set; }
        [Name("Data Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get; set; }
        [Name("Data Pagto Pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("Valor Líquido")]
        public string ValorLiquido { get; set; }
        [Name("Texto SAP")]
        public string TextoSAP { get; set; }
        [Name("Comentário")]
        public string Comentario { get; set; }
        [Name("Autor")]
        public string Autor { get; set; }
        [Name("Nº Conta Judicial")]
        public string NumeroContaJudicial { get; set; }
        [Name("Nº Parcela Judicial")]
        public string NumeroParcelaJudicial { get; set; }
        [Name("Autenticação Eletrônica")]
        public string AutenticacaoEletronica { get; set; }
        [Name("Status Parcela BB")]
        public string StatusParcelaBancoDoBrasil { get; set; }
        [Name("Data Efetivação Parcela BB")]
        public string DataEfetivacaoParcelaBancoDoBrasil { get; set; }

        public static void Mapping(Profile mapper) {
            
            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoCcViewModel>()
             .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => orig.NumeroProcesso))
             .ForMember(dest => dest.Comarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
             .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}", orig.CodigoVara, orig.NomeTipoVara)))
             .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio.HasValue ? orig.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : ""))
             .ForMember(dest => dest.Escritorio, opt => opt.MapFrom(orig => orig.Escritorio))
             .ForMember(dest => dest.TipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
             .ForMember(dest => dest.CategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
             .ForMember(dest => dest.StatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
             .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
             .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
             .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal))
             .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
             .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido))
             .ForMember(dest => dest.TextoSAP, opt => opt.MapFrom(orig => orig.TextoSAP))
             .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
             .ForMember(dest => dest.Autor, opt => opt.MapFrom(orig => orig.Autor ?? ""))
             .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => orig.NumeroContaJudicial))
             .ForMember(dest => dest.NumeroParcelaJudicial, opt => opt.MapFrom(orig => orig.NumeroParcelaJudicial))
             .ForMember(dest => dest.AutenticacaoEletronica, opt => opt.MapFrom(orig => orig.AutenticacaoEletronica ?? ""))
             .ForMember(dest => dest.StatusParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.StatusParcelaBancoDoBrasil ?? ""))
             .ForMember(dest => dest.LancamentoEstornado, opt => opt.MapFrom(orig => orig.IndicaExcluido == "S" ? "Sim" : "Não"))
             .ForMember(dest => dest.DataEfetivacaoParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.DataEfetivacaoParcelaBancoDoBrasil));
        }
    }
    public class LoteLancamentoJecViewModel {
        [Name("Lançamento Estornado")]
        public string LancamentoEstornado { get; set; } 
        [Name("Nº Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Comarca")]
        public string Comarca { get; set; }
        [Name("Juizado")]
        public string Vara { get; set; }
        [Name("Data Envio Escritório")]
        public string DataEnvioEscritorio { get; set; }
        [Name("Escritório")]
        public string Escritorio { get; set; }
        [Name("Tipo de Lançamento")]
        public string TipoLancamento { get; set; }
        [Name("Categoria de Pagamento")]
        public string CategoriaPagamento { get; set; }
        [Name("Status Pagamento")]
        public string StatusPagamento { get; set; }
        [Name("Data Lançamento")]
        public string DataLancamento { get; set; }
        [Name("Nº da Guia")]
        public string NumeroGuia { get; set; }
        [Name("Nº Pedido SAP")]
        public string NumeroPedidoSAP { get; set; }
        [Name("Data Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get; set; }
        [Name("Data Pagto Pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("Valor Líquido")]
        public string ValorLiquido { get; set; }
        [Name("Texto SAP")]
        public string TextoSAP { get; set; }
        [Name("Comentário")]
        public string Comentario { get; set; }
        [Name("Autor")]
        public string Autor { get; set; }
        [Name("Nº Conta Judicial")]
        public string NumeroContaJudicial { get; set; }
        [Name("Nº Parcela Judicial")]
        public string NumeroParcelaJudicial { get; set; }
        [Name("Autenticação Eletrônica")]
        public string AutenticacaoEletronica { get; set; }
        [Name("Status Parcela BB")]
        public string StatusParcelaBancoDoBrasil { get; set; }
        [Name("Data Efetivação Parcela BB")]
        public string DataEfetivacaoParcelaBancoDoBrasil { get; set; }

        public static void Mapping(Profile mapper) {

            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoJecViewModel>()
             .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => orig.NumeroProcesso))
             .ForMember(dest => dest.Comarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
             .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}", orig.CodigoVara, orig.NomeTipoVara)))
             .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio.HasValue ? orig.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : ""))
             .ForMember(dest => dest.Escritorio, opt => opt.MapFrom(orig => orig.Escritorio))
             .ForMember(dest => dest.TipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
             .ForMember(dest => dest.CategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
             .ForMember(dest => dest.StatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
             .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
             .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.NumeroGuia))
             .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
             .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal))
             .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
             .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido))
             .ForMember(dest => dest.TextoSAP, opt => opt.MapFrom(orig => orig.TextoSAP))
             .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
             .ForMember(dest => dest.Autor, opt => opt.MapFrom(orig => orig.Autor ?? ""))
             .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => orig.NumeroContaJudicial))
             .ForMember(dest => dest.NumeroParcelaJudicial, opt => opt.MapFrom(orig => orig.NumeroParcelaJudicial))
             .ForMember(dest => dest.AutenticacaoEletronica, opt => opt.MapFrom(orig => orig.AutenticacaoEletronica ?? ""))
             .ForMember(dest => dest.StatusParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.StatusParcelaBancoDoBrasil ?? ""))
             .ForMember(dest => dest.LancamentoEstornado, opt => opt.MapFrom(orig => orig.IndicaExcluido == "S" ? "Sim" :"Não"))
             .ForMember(dest => dest.DataEfetivacaoParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.DataEfetivacaoParcelaBancoDoBrasil));
        }
    }
    public class LoteLancamentoPexViewModel {
        [Name("Lançamento Estornado")]
        public string LancamentoEstornado { get; set; }
        [Name("Nº Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Comarca")]
        public string Comarca { get; set; }
        [Name("Vara/Juizado")]
        public string Vara { get; set; }
        [Name("Data Envio Escritório")]
        public string DataEnvioEscritorio { get; set; }
        [Name("Escritório")]
        public string Escritorio { get; set; }
        [Name("Tipo de Lançamento")]
        public string TipoLancamento { get; set; }
        [Name("Categoria de Pagamento")]
        public string CategoriaPagamento { get; set; }
        [Name("Status Pagamento")]
        public string StatusPagamento { get; set; }
        [Name("Data Lançamento")]
        public string DataLancamento { get; set; }
        [Name("Nº da Guia")]
        public string NumeroGuia { get; set; }
        [Name("Nº Pedido SAP")]
        public string NumeroPedidoSAP { get; set; }
        [Name("Data Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get; set; }
        [Name("Data Pagto Pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("Valor Líquido")]
        public string ValorLiquido { get; set; }
        [Name("Texto SAP")]
        public string TextoSAP { get; set; }
        [Name("Comentário")]
        public string Comentario { get; set; }
        [Name("Autor")]
        public string Autor { get; set; }
        [Name("Nº Conta Judicial")]
        public string NumeroContaJudicial { get; set; }
        [Name("Nº Parcela Judicial")]
        public string NumeroParcelaJudicial { get; set; }
        [Name("Autenticação Eletrônica")]
        public string AutenticacaoEletronica { get; set; }
        [Name("Status Parcela BB")]
        public string StatusParcelaBancoDoBrasil { get; set; }
        [Name("Data Efetivação Parcela BB")]
        public string DataEfetivacaoParcelaBancoDoBrasil { get; set; }

        public static void Mapping(Profile mapper) {

            mapper.CreateMap<LoteLancamentoExportacaoDTO, LoteLancamentoPexViewModel>()
             .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => orig.NumeroProcesso))
             .ForMember(dest => dest.Comarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
             .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}", orig.CodigoVara, orig.NomeTipoVara)))
             .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio.HasValue ? orig.DataEnvioEscritorio.Value.ToString("dd/MM/yyy") : ""))
             .ForMember(dest => dest.Escritorio, opt => opt.MapFrom(orig => orig.Escritorio))
             .ForMember(dest => dest.TipoLancamento, opt => opt.MapFrom(orig => orig.TipoLancamento))
             .ForMember(dest => dest.CategoriaPagamento, opt => opt.MapFrom(orig => orig.CategoriaPagamento))
             .ForMember(dest => dest.StatusPagamento, opt => opt.MapFrom(orig => orig.StatusPagamento))
             .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(orig => orig.DataLancamento))
             .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.NumeroGuia))
             .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
             .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal))
             .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido))
             .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido))
             .ForMember(dest => dest.TextoSAP, opt => opt.MapFrom(orig => orig.TextoSAP))
             .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
             .ForMember(dest => dest.Autor, opt => opt.MapFrom(orig => orig.Autor ?? ""))
             .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => orig.NumeroContaJudicial))
             .ForMember(dest => dest.NumeroParcelaJudicial, opt => opt.MapFrom(orig => orig.NumeroParcelaJudicial))
             .ForMember(dest => dest.AutenticacaoEletronica, opt => opt.MapFrom(orig => orig.AutenticacaoEletronica ?? ""))
             .ForMember(dest => dest.LancamentoEstornado, opt => opt.MapFrom(orig => orig.IndicaExcluido == "S" ?"Sim" :"Não"))
             .ForMember(dest => dest.StatusParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.StatusParcelaBancoDoBrasil ?? ""))
             .ForMember(dest => dest.DataEfetivacaoParcelaBancoDoBrasil, opt => opt.MapFrom(orig => orig.DataEfetivacaoParcelaBancoDoBrasil));
        }
    }
}
