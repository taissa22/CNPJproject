using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class VaraAppService : BaseCrudAppService<ComboboxViewModel<long>, Vara, long>, IVaraAppService
    {
        private readonly IVaraService service;
        private readonly IMapper mapper;

        public VaraAppService(IVaraService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarVaraPorComarca(long codigoComarca)
        {
            var result = new ResultadoApplication<IEnumerable<ComboboxViewModel<long>>>();

            try
            {                
                var model = await service.Pesquisar(new List<System.Linq.Expressions.Expression<Func<Vara, bool>>>
                {
                    e => e.Id == codigoComarca
                })
                .Select(e => new ComboboxViewModel<long>() { Id = e.CodigoVara, Descricao = e.CodigoVara.ToString() + "a" })
                .Distinct()
                .OrderBy(e => e.Id)
                .ToListAsync();

                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
    }
}
