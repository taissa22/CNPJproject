using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.Processos;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.Processos
{
    public class PrepostoService : BaseCrudService<Preposto, long>, IPrepostoService
    {

        private readonly IPrepostoRepository repository;
        public PrepostoService(IPrepostoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<PrepostoDTO>> ConsultarPreposto()
        {
            var lista = await repository.RecuperarPrepostoTrabalhista();

            var listaCompleta = lista.ToList();

            listaCompleta.Insert(0, new PrepostoDTO { Id = 0, Descricao = "Sem Preposto" });            

            return listaCompleta;
        }

        public async Task<IEnumerable<PrepostoDTO>> ListarPreposto(long? tipoProcesso)
        {
            return await repository.ListarPreposto(tipoProcesso);
        }
    }
}
