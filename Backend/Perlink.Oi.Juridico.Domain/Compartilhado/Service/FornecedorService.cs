using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class FornecedorService : BaseCrudService<Fornecedor, long>, IFornecedorService
    {
        private readonly IFornecedorRepository repository;

        public FornecedorService(IFornecedorRepository repository, IPermissaoService permissaoService) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<Fornecedor> AtualizarFornecedor(Fornecedor fornecedor)
        {
            return await repository.AtualizarFornecedor(fornecedor);
        }

        public async Task<Fornecedor> CadastrarFornecedor(Fornecedor fornecedor)
        {
             return await repository.CadastrarFornecedor(fornecedor);
        }

        public async Task ExcluirFornecedor(long codigoFornecedor) {

            await repository.RemoverPorId(codigoFornecedor);
        }

        public async Task<ICollection<FornecedorExportarDTO>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            return await repository.ExportarFornecedores(fornecedorFiltroDTO);
        }

        public async Task<Fornecedor> FornecedorComCodigoSAPJaCadastrado(string codigoSAP)
        {
            return await repository.FornecedorComCodigoSAPJaCadastrado(codigoSAP);
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO) {
            return await repository.ObterQuantidadeTotalPorFiltro(fornecedorFiltroDTO);
        }

        public async Task<IEnumerable<FornecedorDTOFiltro>> RecuperarFornecedorParaFiltroLote()
        {
            return await repository.RecuperarFornecedorParaFiltroLote();
        }

        public async Task<IEnumerable<FornecedorResultadoDTO>> RecuperarFornecedorPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            return await repository.FiltroConsultaFornecedor(fornecedorFiltroDTO);
        }

        public async Task<Fornecedor> VerificarCodigoSap(string codigoSAP, long CodigoFornecedor)
        {
            return await repository.VerificarCodigoSap(codigoSAP, CodigoFornecedor);
        }
    }
}