using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class StatusPagamentoAppService : BaseCrudAppService<StatusPagamentoViewModel, StatusPagamento, long>, IStatusPagamentoAppService
    {
        private readonly IStatusPagamentoService service;

        public StatusPagamentoAppService(IStatusPagamentoService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
        }
    }
}