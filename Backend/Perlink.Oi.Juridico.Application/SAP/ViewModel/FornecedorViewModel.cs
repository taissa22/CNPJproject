using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class FornecedorViewModel : BaseViewModel<long>
    {
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSAP { get; set; }
        public long CodigoTipoFornecedor { get; set; }
        public long? CodigoEscritorio { get; set; }
        public long? CodigoProfissional { get; set; }
        public long? CodigoBanco { get; set; }
        public string NumeroCNPJ { get; set; }
        public double ValorCartaFianca { get; set; }
        public DateTime? DataCartaFianca { get; set; }
        public bool IndicaAtivoSAP { get; set; }
        public DateTime? DataAtualizaIndiceAtivo { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FornecedorViewModel, Fornecedor>();

            mapper.CreateMap<Fornecedor, FornecedorViewModel>();
            
        }
    }
}
