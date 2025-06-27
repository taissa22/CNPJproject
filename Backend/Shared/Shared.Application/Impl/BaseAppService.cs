using AutoMapper;
using Shared.Application.Interface;
using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Application.Impl
{
    public class BaseAppService<TViewModel, TEntity, TType> : IBaseAppService<TViewModel, TEntity, TType> where TEntity : class, IEntity<TEntity, TType>
    {
        private readonly IBaseService<TEntity, TType> service;
        private readonly IMapper mapper;

        public BaseAppService(
            IBaseService<TEntity, TType> service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public virtual async Task<IResultadoApplication<ICollection<TViewModel>>> RecuperarTodos()
        {
            var result = new ResultadoApplication<ICollection<TViewModel>>();

            try
            {
                result.DefinirData(mapper.Map<ICollection<TViewModel>>(await service.RecuperarTodos()));
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public virtual async Task<IResultadoApplication<TViewModel>> RecuperarPorId(TType id)
        {
            var result = new ResultadoApplication<TViewModel>();

            try
            {
                var data = await service.RecuperarPorId(id);
                result.DefinirData(mapper.Map<TViewModel>(data));
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
    }
}
