using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICargaCompromissoParcelaRepository : IBaseCrudRepository<CargaCompromissoParcela, string> {
        Task<bool> GravarLogSolicCancelamento(long codLote, string usrCodSolic, string dataSolic);
        Task<bool> GravarCancelamento(long codLote, string comentario, string motivo, string status);
    }
}
