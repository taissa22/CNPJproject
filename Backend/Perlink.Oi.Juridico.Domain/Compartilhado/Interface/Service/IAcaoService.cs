﻿using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IAcaoService : IBaseCrudService<Acao, long> {
        Task<bool> ExisteBBNaturezaAcaoAssociadoAcao(long id);
    }
}
