using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class FornecedorExportarViewModel 
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Nome do Fornecedor")]
        public string NomeFornecedor { get; set; }
        [Name("Código do Fornecedor SAP")]
        public string CodigoFornecedorSap { get; set; }
        [Name("Tipo")]
        public string TipoFornecedor { get; set; }
        [Name("Escritório")]
        public string Escritorio { get; set; }
        [Name("Profissional")]
        public string profissional { get; set; }
        [Name("Banco")]
        public string Banco { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FornecedorExportarDTO, FornecedorExportarViewModel>();

            mapper.CreateMap<FornecedorExportarViewModel, FornecedorExportarDTO>();
        }
    }
}
