using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class Empresas_SapExportarViewModel
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Sigla")]
        public string Sigla { get; set; }
        [Name("Nome")]
        public string Nome { get; set; }
        [Name("Envio SAP")]
        public string IndicaEnvioArquivoSolicitacao { get; set; }
        [Name("Ativa")]
        public string IndicaAtivo { get; set; }
        [Name("Organização de Compras")]
        public string CodigoOrganizacaocompra { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EmpresaSapDTO, Empresas_SapExportarViewModel>()
                .ForMember(dest => dest.IndicaAtivo, opt => opt.MapFrom(orig => orig.IndicaAtivo.RetornaSimNao()))
                .ForMember(dest => dest.IndicaEnvioArquivoSolicitacao, opt => opt.MapFrom(orig => orig.IndicaEnvioArquivoSolicitacao.RetornaSimNao()));

            mapper.CreateMap<Empresas_SapExportarViewModel, EmpresaSapDTO>();
        }
    }
}
