using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class Empresas_SapResultadoViewModel
    {
        public long Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public bool IndicaEnvioArquivoSolicitacao { get; set; }
        public bool IndicaAtivo { get; set; }
        public string CodigoOrganizacaocompra { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EmpresaSapDTO, Empresas_SapResultadoViewModel>();

            mapper.CreateMap<Empresas_SapResultadoViewModel, EmpresaSapDTO>();
        }
    }
}
