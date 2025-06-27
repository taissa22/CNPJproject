using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using Shared.Domain.Impl.Validator;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class BorderoService : BaseCrudService<Bordero, long>, IBorderoService
    {
        private readonly IBorderoRepository borderoRepository;


        public BorderoService(IBorderoRepository borderoRepo) : base(borderoRepo)
        {
            this.borderoRepository = borderoRepo;
        }


        public async Task CriacaoBordero(IList<BorderoDTO> borderos, Lote lote)
        {
            await borderoRepository.CriacaoBordero(borderos, lote);
        }

        public async Task<IEnumerable<Bordero>> GetBordero(long codigoLote)
        {
            return await borderoRepository.GetBordero(codigoLote);
        }

    }
}
