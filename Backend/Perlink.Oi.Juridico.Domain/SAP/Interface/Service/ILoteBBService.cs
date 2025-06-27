using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Impl;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface ILoteBBService : IBaseCrudService<Lote, long>
    {
        byte[] Gerar(ArquivoBBDTO regerarArquivoBBDTO, ref string msgErro);
    }
}
