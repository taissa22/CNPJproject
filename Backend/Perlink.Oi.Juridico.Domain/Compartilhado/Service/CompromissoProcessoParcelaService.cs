using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using System.Linq;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CompromissoProcessoParcelaService : BaseCrudService<CompromissoProcessoParcela, long>, ICompromissoProcessoParcelaService
    {
        private readonly ICompromissoProcessoParcelaRepository repository;

        public CompromissoProcessoParcelaService(ICompromissoProcessoParcelaRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task AtualizarCompromissoProcessoParcela(CompromissoProcessoParcela parcelaCompromisso) {
            await repository.Atualizar(parcelaCompromisso);
        }

        public async Task<CompromissoProcessoParcela> ObterCompromissoParcela(long codigoProcesso, long codigoCompromisso, long codigoParcela) {
            return await repository.ObterCompromissoParcela(codigoProcesso, codigoCompromisso, codigoParcela);
        }

        public Task<DadosCompromissoEstornoDTO> ObterDadosCompromissoParaEstorno(long codigoProcesso, long codigoLancamento) {
            return repository.ObterDadosCompromissoParaEstorno(codigoProcesso, codigoLancamento);
        }

        public async Task<long> ObterNextCodigoParcelaCompromissoProcesso(long codigoProcesso, long codigoCompromisso) {
            ICollection<CompromissoProcessoParcela> parcelas = await repository.ObterParcelasCompromissoProcesso(codigoProcesso, codigoCompromisso);
            return parcelas.OrderBy(p => p.CodigoParcela).LastOrDefault().CodigoParcela + 1;

        }  
        public async Task<long> ObterNextNumeroParcelaCompromissoProcesso(long codigoProcesso, long codigoCompromisso) {
            ICollection<CompromissoProcessoParcela> parcelas = await repository.ObterParcelasCompromissoProcesso(codigoProcesso, codigoCompromisso);
            return parcelas.Max(p => p.NumeroParcela).GetValueOrDefault() + 1;


        }
    }
}
