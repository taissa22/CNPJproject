using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class GruposLotesJuizadosService : BaseCrudService<GruposLotesJuizados, long>, IGruposLotesJuizadosService
    {
        private readonly IGruposLotesJuizadosRepository repository;

        public GruposLotesJuizadosService(IGruposLotesJuizadosRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros)
        {
            return await repository.ObterQuantidadeTotalPorFiltro(filtros);
        }

        public async Task<long> ObterUltimoId()
        {
            return await repository.ObterUltimoId();
        }

        public async Task<IEnumerable<GruposLotesJuizados>> RecuperarGrupoLoteJuizadoPorFiltro(FiltrosDTO filtros)
        {
            return await repository.RecuperarGrupoLoteJuizadoPorFiltro(filtros);
        }

    }
}
