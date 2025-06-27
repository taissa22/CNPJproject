using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class FornecedorAtualizarViewModel
    {
        public long Id { get; set; }
        public long CodigoTipoFornecedor { get; set; }
        public long? CodigoEscritorio { get; set; }
        public long? CodigoProfissional { get; set; }
        public long? CodigoBanco { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSAP { get; set; }
        public bool ConfirmacaoEnvio = false;

        public bool CriarCodigoFornecedorSAP = false;
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<Fornecedor, FornecedorAtualizarViewModel>();

            mapper.CreateMap<FornecedorAtualizarViewModel, Fornecedor>();
        }
    }
}