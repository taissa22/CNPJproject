using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia
{
    public class FornecedorContigenciaAtualizaViewModel
    {       
        public long id { get; set; }
        public double valorCartaFianca { get; set; }
        public DateTime? dataVencimentoCartaFianca { get; set; }
        public long statusFornecedor { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<Fornecedor, FornecedorContigenciaAtualizaViewModel>();

            mapper.CreateMap<FornecedorContigenciaAtualizaViewModel, Fornecedor>();
        }
    }
}
