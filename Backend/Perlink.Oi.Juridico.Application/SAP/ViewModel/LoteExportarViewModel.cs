using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel {
    public class LoteExportarToCEorTrabViewModel {
        [Name("Lote")]
        public string Id { get; set; }
        [Name("Data Criação Lote")]
        public string DataCriacaoLote { get; set; }
        [Name("Identificação do Lote")]
        public string DescricaoLote { get; set; }
        [Name("Status Pagamento Atual")]
        public string DescricaoStatusPagamento { get; set; }
        [Name("Data Criação Pedido")]
        public string DataCriacaoPedido { get; set; }
        [Name("N° Pedido SAP")]
        public string NumeroPedido { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Fornecedor")]
        public string DescricaoFornecedor { get; set; }
        [Name("Centro Custo")]
        public string DescricaoCentroCusto { get; set; }
        [Name("Valor Lote")]
        public string ValorLote { get; set; }
        [Name("Qtd Lançamentos")]
        public string QuantidadeLancamento { get; set; }
        [Name("Data Erro Processamento")]
        public string DataErroProcessamento { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteExportarDTO, LoteExportarToCEorTrabViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
            .ForMember(dest => dest.DataCriacaoLote, opt => opt.MapFrom(orig => orig.DataCriacaoLote))
            .ForMember(dest => dest.DescricaoLote, opt => opt.MapFrom(orig => orig.DescricaoLote))
            .ForMember(dest => dest.DescricaoStatusPagamento, opt => opt.MapFrom(orig => orig.DescricaoStatusPagamento))
            .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido))
            .ForMember(dest => dest.NumeroPedido, opt => opt.MapFrom(orig => orig.NumeroPedido))
            .ForMember(dest => dest.DescricaoEmpresaGrupo, opt => opt.MapFrom(orig => orig.DescricaoEmpresaGrupo))
            .ForMember(dest => dest.DescricaoFornecedor, opt => opt.MapFrom(orig => orig.DescricaoFornecedor))
            .ForMember(dest => dest.DescricaoCentroCusto, opt => opt.MapFrom(orig => orig.DescricaoCentroCusto))
            .ForMember(dest => dest.ValorLote, opt => opt.MapFrom(orig => orig.ValorLote))
            .ForMember(dest => dest.QuantidadeLancamento, opt => opt.MapFrom(orig => orig.QuantidadeLancamento))
            .ForMember(dest => dest.DataErroProcessamento, opt => opt.MapFrom(orig => orig.DataErroProcessamento));
        }
    }

    public class LoteExportarViewModel {
        [Name("Lote")]
        public string Id { get; set; }
        [Name("Data Criação Lote")]
        public string DataCriacaoLote { get; set; }
        [Name("Identificação do Lote")]
        public string DescricaoLote { get; set; }
        [Name("Status Pagamento Atual")]
        public string DescricaoStatusPagamento { get; set; }
        [Name("Data Criação Pedido")]
        public string DataCriacaoPedido { get; set; }
        [Name("N° Pedido SAP")]
        public string NumeroPedido { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Fornecedor")]
        public string DescricaoFornecedor { get; set; }
        [Name("Centro Custo")]
        public string DescricaoCentroCusto { get; set; }
        [Name("Valor Lote")]
        public string ValorLote { get; set; }
        [Name("Qtd Lançamentos")]
        public string QuantidadeLancamento { get; set; }
        [Name("Geração Arq BB")]
        public string DataGeracaoArquivoBB { get; set; }
        [Name("Retorno BB")]
        public string DataRetornoBB { get; set; }
        [Name("N° Lote BB")]
        public string NumeroLoteBB { get; set; }
        [Name("Data Erro Processamento")]
        public string DataErroProcessamento { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteExportarDTO, LoteExportarViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
            .ForMember(dest => dest.DataCriacaoLote, opt => opt.MapFrom(orig => orig.DataCriacaoLote))
            .ForMember(dest => dest.DescricaoLote, opt => opt.MapFrom(orig => orig.DescricaoLote))
            .ForMember(dest => dest.DescricaoStatusPagamento, opt => opt.MapFrom(orig => orig.DescricaoStatusPagamento))
            .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido))
            .ForMember(dest => dest.NumeroPedido, opt => opt.MapFrom(orig => orig.NumeroPedido))
            .ForMember(dest => dest.DescricaoEmpresaGrupo, opt => opt.MapFrom(orig => orig.DescricaoEmpresaGrupo))
            .ForMember(dest => dest.DescricaoFornecedor, opt => opt.MapFrom(orig => orig.DescricaoFornecedor))
            .ForMember(dest => dest.DescricaoCentroCusto, opt => opt.MapFrom(orig => orig.DescricaoCentroCusto))
            .ForMember(dest => dest.ValorLote, opt => opt.MapFrom(orig => orig.ValorLote))
            .ForMember(dest => dest.DataGeracaoArquivoBB, opt => opt.MapFrom(orig => orig.DataGeracaoArquivoBB))
            .ForMember(dest => dest.DataRetornoBB, opt => opt.MapFrom(orig => orig.DataRetornoBB))
            .ForMember(dest => dest.NumeroLoteBB, opt => opt.MapFrom(orig => orig.NumeroLoteBB))
            .ForMember(dest => dest.QuantidadeLancamento, opt => opt.MapFrom(orig => orig.QuantidadeLancamento))
            .ForMember(dest => dest.DataErroProcessamento, opt => opt.MapFrom(orig => orig.DataErroProcessamento));

        }
    }
}
