using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBResumoProcessamentoViewModel
    {
        public long Id { get; set; }
        public long NumeroLoteBB { get; set; }
        public DateTime DataRemessa { get; set; }
        public DateTime? DataProcessamentoRemessa { get; set; }
        public decimal ValorTotalRemessa { get; set; }
        public long QuantidadeRegistrosProcessados { get; set; }
        public long QuantidadeRegistrosArquivo { get; set; }
        public decimal ValorTotalGuiaProcessada { get; set; }
        public long CodigoLote { get; set; }
        public long CodigoBBStatusRemessa { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoViewModel, BBResumoProcessamento>();

            mapper.CreateMap<BBResumoProcessamento, BBResumoProcessamentoViewModel>();

        }
    }

    public class BBResumoProcessamentoResultadoViewModel
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

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoResultadoDTO, BBResumoProcessamentoResultadoViewModel>();
        }
    }
    public class BBResumoProcessamentoResultadoExportarViewModel
    {
        [Name("Tipo de Processo")]
        public string TipoProcesso { get; set; }
        [Name("Lote BB")]
        public long NumeroLoteBB { get; set; }
        [Name("Data Remessa")]
        public string DataRemessa { get; set; }
        [Name("Data Processamento da Remessa")]
        public string DataProcessamentoRemessa { get; set; }
        [Name("Status de Processamento da Remessa")]
        public string Status { get; set; }
        [Name("Qtd de Registros do Arquivo")]
        public long QuantidadeRegistrosArquivo { get; set; }
        [Name("Valor Total da Remessa")]
        public string ValorTotalRemessa { get; set; }
        [Name("Qtd Registros Processados")]
        public long QuantidadeRegistrosProcessados { get; set; }
        [Name("Valor Total Processado")]
        public string ValorTotalGuiaProcessada { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoResultadoDTO, BBResumoProcessamentoResultadoExportarViewModel>()
                .ForMember(dest => dest.ValorTotalRemessa, opt => opt.MapFrom(orig => orig.ValorTotalRemessa.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorTotalGuiaProcessada, opt => opt.MapFrom(orig => orig.ValorTotalGuiaProcessada.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));

        }
    }



    public class BBResumoProcessamentoGuiaViewModel
    {
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

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoGuiaDTO, BBResumoProcessamentoGuiaViewModel>();

        }
    }

    public class BBResumoProcessamentoGuiaExportarViewModel
    {
        [Name("N° Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Comarca")]
        public string Comarca { get; set; }
        [Name("Juizado")]
        public string Juizado { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Data Lançamento")]
        public string DataLancamento { get; set; }
        [Name("Tipo Lançamento")]
        public string DescricaoTipoLancamento { get; set; }
        [Name("Categoria de Pagamento")]
        public string DescricaoCategoriaPagamento { get; set; }
        [Name("Status Pagamento")]
        public string StatusPagamento { get; set; }
        [Name("Data Envio Escritório")]
        public string DataEnvioEscritorio { get; set; }
        [Name("Escritório")]
        public string DescricaoEscritorio { get; set; }
        [Name("N° Pedido SAP")]
        public string NumeroPedidoSAP { get; set; }
        [Name("N° da Guia")]
        public string NumeroGuia { get; set; }
        [Name("Data Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get; set; }
        [Name("Data Pagamento Pedido")]
        public string DataPagamentoPedido { get; set; }
        [Name("Valor Líquido")]
        public string ValorLiquido { get; set; }
        [Name("Autor")]
        public string Autor { get; set; }
        [Name("Nº Conta Judicial")]
        public string NumeroContaJudicial { get; set; }
        [Name("Nº Parcela Judicial")]
        public string NumeroParcelaJudicial { get; set; }
        [Name("Autenticação Eletrônica")]
        public string AutenticacaoEletronica { get; set; }
        [Name("Data Efetivação Parcela BB")]
        public string DataEfetivacaoParcelaBB { get; set; }
        [Name("Status Parcela BB")]
        public string StatusParcelaBB { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoGuiaDTO, BBResumoProcessamentoGuiaExportarViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => "'" + orig.NumeroProcesso))
                .ForMember(dest => dest.NumeroContaJudicial, opt => opt.MapFrom(orig => "'" + orig.NumeroContaJudicial))
                .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }

    public class BBResumoProcessamentoGuiaExibidaViewModel
    {

        public string Comarca { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string NumeroProcesso { get; set; }
        public string NumeroGuia { get; set; }
        public string ValorLiquido { get; set; }
        public string Autor { get; set; }
        public string NumeroContaJudicial { get; set; }
        public string NumeroParcelaJudicial { get; set; }
        public string AutenticacaoEletronica { get; set; }
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

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<BBResumoProcessamentoGuiaExibidaDTO, BBResumoProcessamentoGuiaExibidaViewModel>()
                .ForMember(dest => dest.ValorLiquido, opt => opt.MapFrom(orig => orig.ValorLiquido.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }
    public class BBResumoProcessamentoGuiasComProblemaViewModel
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
        public string ValorParcela { get; set; }
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

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoGuiasComProblemaDTO, BBResumoProcessamentoGuiasComProblemaViewModel>()
                .ForMember(dest => dest.ReuCPF_CNPJ, opt => opt.MapFrom(orig => orig.ReuCPF_CNPJ.FormataCPF_CNPJ()))
                .ForMember(dest => dest.AutorCPF_CNPJ, opt => opt.MapFrom(orig => orig.AutorCPF_CNPJ.FormataCPF_CNPJ()))
                .ForMember(dest => dest.ValorParcela, opt => opt.MapFrom(orig => orig.ValorParcela.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }

    public class BBResumoProcessamentoGuiasComProblemaExportarViewModel
    {
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Cód Comarca BB")]
        public string CodigoComarca { get; set; }
        [Name("Nome Comarca BB")]
        public string NomeComarca { get; set; }
        [Name("Cód Órgão BB")]
        public string CodigoOrgaoBB { get; set; }
        [Name("Nome Orgão BB")]
        public string NomeOrgaoBB { get; set; }
        [Name("Cód Natureza Ação BB")]
        public string CodigoNaturezaAcaoBB { get; set; }
        [Name("Nome Natureza BB")]
        public string NomeNaturezaBB { get; set; }
        [Name("Nome do Autor")]
        public string NomeAutor { get; set; }
        [Name("CPF/CNPJ Autor")]
        public string AutorCPF_CNPJ { get; set; }
        [Name("Nome do Réu")]
        public string NomeReu { get; set; }
        [Name("CPF/CNPJ Réu")]
        public string ReuCPF_CNPJ { get; set; }
        [Name("Valor da Parcela")]
        public string ValorParcela { get; set; }
        [Name("Data da Guia")]
        public string DataGuia { get; set; }
        [Name("Guia")]
        public string Guia { get; set; }
        [Name("Número Conta")]
        public string NumeroConta { get; set; }
        [Name("Número Parcela")]
        public string NumeroParcela { get; set; }
        [Name("Efetivação Parcela")]
        public string DataEfetivacaoParcelaBB { get; set; }
        [Name("Cód Status Parcela")]
        public string IdBBStatusParcela { get; set; }
        [Name("Descrição Status Parcela")]
        public string StatusParcelaBB { get; set; }

        [Name("Situação Processamento SISJUR x BB")]
        public string DescricaoErroGuia { get; set; }
        [Name("ID Processo")]
        public string IdProcesso { get; set; }
        [Name("ID Lançamento")]
        public string IdLancamento { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoGuiasComProblemaDTO, BBResumoProcessamentoGuiasComProblemaExportarViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => "'" + orig.NumeroProcesso))
                .ForMember(dest => dest.NumeroConta, opt => opt.MapFrom(orig => "'" + orig.NumeroConta))
                .ForMember(dest => dest.ReuCPF_CNPJ, opt => opt.MapFrom(orig => orig.ReuCPF_CNPJ.FormataCPF_CNPJ()))
                .ForMember(dest => dest.AutorCPF_CNPJ, opt => opt.MapFrom(orig => orig.AutorCPF_CNPJ.FormataCPF_CNPJ()))
                .ForMember(dest => dest.ValorParcela, opt => opt.MapFrom(orig => orig.ValorParcela.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }

    }

    public class BBResumoProcessamentoImportacaoViewModel
    {
        public ICollection<BBResumoProcessamentoGuiaViewModel> GuiasOk { get; set; }
        public ICollection<BBResumoProcessamentoGuiasComProblemaViewModel> GuiasComProblema { get; set; }
        public BBResumoProcessamentoResultadoViewModel Resumo { get; set; }
        public byte[] ArquivoGuiasOk { get; set; }
        public byte[] ArquivoGuiasNaoOk { get; set; }
        public string nomeArquivo { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBResumoProcessamentoImportacaoDTO, BBResumoProcessamentoImportacaoViewModel>();
        }
    }
}
