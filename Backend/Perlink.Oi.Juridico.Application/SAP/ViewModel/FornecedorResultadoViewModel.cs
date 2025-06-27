using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class FornecedorResultadoViewModel
    {
        public long Id { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedorSap { get; set; }
        public long CodigoTipoFornecedor { get; set; }
        public long? CodigoEscritorio { get; set; }
        public long? CodigoProfissional { get; set; }
        public long? CodigoBanco { get; set; }
        public string NomeTipoFornecedor { get; set; }
        public string NomeEscritorio { get; set; }
        public string NomeProfissional { get; set; }
        public string NomeBanco { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<FornecedorResultadoDTO, FornecedorResultadoViewModel>();

            mapper.CreateMap<FornecedorResultadoViewModel, FornecedorResultadoDTO>();
        }
    }
}