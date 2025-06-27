using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel
{
    public class ApuracaoOutliersDownloadResultadoViewModel
    {   
        //public List<ListaProcessosBaseFechamentoJec> ListaProcessos { get; set; }
        
        [Name("Código Processo")]
        public string CodigoProcesso { get; set; }

        [Name("Total Pagamentos")]
        public string TotalPagamentos { get; set; }
        
        [Name("")]
        public string ColunaVazia { get; set; }

        [Name("Média Pagamentos (G/H)")]
        public string MediaPagamentos { get; set; }     
        
        [Name("Fator Desvio Padrão")]
        public string FatorDesvioPadrao { get; set; }

        [Name("Desvio Padrão (DESVPAD(B))")]
        public string DesvioPadrao { get; set; }

        [Name("Valor Total de Pagamentos  (SOMA(B))")]
        public string ValorTotalPagamentos { get; set; }

        [Name("Quantidade de Processos com Pagamentos (QTD TOTAL(A))")]
        public string QtdProcessosPagamentos { get; set; }

        [Name("Valor de Corte de Outlier (D+(E*F))")]
        public string ValorCorteOutliers { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<ApuracaoOutliersDownloadResultadoDTO, ApuracaoOutliersDownloadResultadoViewModel>();
        }
    }

    public class ListaProcessosBaseFechamentoJec
    {
        [Name("Código Processo")]
        public string CodigoProcesso { get; set; }

        [Name("Total Pagamentos")]
        public string TotalPagamentos { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<ListaProcessosBaseFechamentoJecDTO, ListaProcessosBaseFechamentoJec>();
        }
    }

    public class ApuracaoOutliersDownloadBaseFechamentoViewModel
    {
        [Name("Código do Processo")]
        public string CodigoInterno { get; set; }

        [Name("Número do Processo")]
        public string NumeroProcesso { get; set; }

        [Name("Estado")]
        public string Estado { get; set; }

        [Name("Código da Comarca")]
        public string CodigoComarca { get; set; }

        [Name("Comarca")]
        public string NomeComarca { get; set; }

        [Name("Vara")]
        public string CodigoVara { get; set; }

        [Name("Código Tipo Vara")]
        public string CodigoTipoVara { get; set; }

        [Name("Tipo Vara")]
        public string NomeTipoVara { get; set; }

        [Name("Código Empresa")]
        public string CodigoEmpresaGrupo { get; set; }

        [Name("Empresa do Grupo")]
        public string NomeEmpresaGrupo { get; set; }
        
        [Name("Data de Cadastro do Processo")]
        public string DataCadastro { get; set; }

        [Name("Data de Finalização Contábil")]
        public string DataFinalizacaoContabil { get; set; }

        [Name("Influencia a Contingência")]
        public string ProcInfluenciaContingencia { get; set; }

        [Name("Pré ou Pós RJ")]
        public string PrePos { get; set; }

        [Name("Código Lançamento do Processo")]
        public string CodigoLancamento { get; set; }

        [Name("Valor Lançamento")]
        public string ValorLancamento { get; set; }

        [Name("Código Categoria de Pagamento")]
        public string CodigoCategoriaPagamento { get; set; }

        [Name("Categoria de Pagamento")]
        public string DescricaoCategoriaPagamento { get; set; }

        [Name("Categoria de Pagamento Influencia Contingência")]
        public string CatPagInfluenciaContingencia { get; set; }

        [Name("Data de Recebimento Fiscal")]
        public string DataRecebimentoFiscal { get; set; }

        [Name("Data de Pagamento")]
        public string DataPagamento { get; set; }        

        [Name("Data Média Móvel Utilizada")]
        public string ParametroMediaMovel { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<ApuracaoOutliersDownloadBaseFechamentoDTO, ApuracaoOutliersDownloadBaseFechamentoViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => string.Format("[{0}",orig.NumeroProcesso)))
                .ForMember(dest => dest.ValorLancamento, opt => opt.MapFrom(orig => orig.ValorLancamento.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ProcInfluenciaContingencia, opt => opt.MapFrom(orig => orig.ProcInfluenciaContingencia.RetornaSimNao()))
                .ForMember(dest => dest.CatPagInfluenciaContingencia, opt => opt.MapFrom(orig => orig.CatPagInfluenciaContingencia.RetornaSimNao()))
                .ForMember(dest => dest.DataPagamento, opt => opt.MapFrom(orig => orig.DataPagamento.HasValue ? orig.DataPagamento.Value.ToString("dd/MM/yyyy") : string.Empty))
                .ForMember(dest => dest.DataRecebimentoFiscal, opt => opt.MapFrom(orig => orig.DataRecebimentoFiscal.HasValue ? orig.DataRecebimentoFiscal.Value.ToString("dd/MM/yyyy") : string.Empty))
                .ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.ToString("dd/MM/yyyy")));
        }
    }


    public class ApuracaoOutlierDownloadArquivoViewModel
    {
        public string NomeArquivo { get; set; }
        public byte[] Arquivo { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ApuracaoOutliersDownloadArquivoDTO, ApuracaoOutlierDownloadArquivoViewModel>()
                .ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(orig => orig.NomeArquivo))
                .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.Arquivo));
        }
    }
}
