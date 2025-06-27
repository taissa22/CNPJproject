using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IFeriadosService : IBaseCrudService<Feriados, long> {
        Task<DateTime> ValidarDataNovaParcela(int quantidadeDias);
    }
}
