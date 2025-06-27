using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using System;
using System.Globalization;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia
{
    public class FornecedorContigenciaResultadoViewModel
    {
        [Name("Código")]
        public long Id { get; set; }

        [Name("Código do Fornecedor")]
        public string Codigo { get; set; }

        [Name("Nome do Fornecedor")]
        public string Nome { get; set; }

        [Name("Valor da Carta de Fiança")]
        public double ValorCartaFianca { get; set; }

        [Name("Data de Vencimento da Carta de Fiança")]
        public string DataVencimentoCartaFianca { get; set; }

        [Name("Status do Fornecedor")]
        public long StatusFornecedor { get; set; }

        [Name("CNPJ")]
        public string CNPJ { get; set; }


        public static void Mapping(Profile mapper)
        {

            mapper.CreateMap<FornecedorContigenciaResultadoDTO, FornecedorContigenciaResultadoViewModel>()
                  .ForMember(dest => dest.DataVencimentoCartaFianca, opt => opt.MapFrom(orig => (orig.DataVencimentoCartaFianca == null) ? "" : orig.DataVencimentoCartaFianca.Value.ToString("dd/MM/yyyy")));
           
        }
    }
    public class FornecedorContigenciaExportarViewModel
    {
        [Name("Código")]
        public long Id { get; set; }

        [Name("Nome do Fornecedor")]
        public string Nome { get; set; }

        [Name("Código do Fornecedor SAP")]
        public string Codigo { get; set; }

        [Name("CNPJ")]
        public string CNPJ { get; set; }

        [Name("Valor da Carta de Fiança")]
        public string ValorCartaFianca { get; set; }


        [Name("Data de Vencimento da Carta de Fiança")]
        public string DataVencimentoCartaFianca { get; set; }

        [Name("Status do Fornecedor")]
        public string StatusFornecedor { get; set; }

       

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FornecedorContigenciaExportarDTO, FornecedorContigenciaExportarViewModel>()

                  .ForMember(dest => dest.DataVencimentoCartaFianca, opt => opt.MapFrom(orig => (orig.DataVencimentoCartaFianca == null) ? "" : orig.DataVencimentoCartaFianca.Value.ToString("dd/MM/yyyy")))
                  .ForMember(dest => dest.Codigo, opt => opt.MapFrom(orig => $"'{orig.Codigo}"))
                  .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(orig => $"{orig.CNPJ.Substring(0, 2)}.{orig.CNPJ.Substring(2, 4)}/{orig.CNPJ.Substring(6, 4)}-{orig.CNPJ.Substring(10, 2)}"));
            
        }
    }
}
