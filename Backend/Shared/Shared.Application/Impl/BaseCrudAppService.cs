using AutoMapper;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Application.Impl
{
    public abstract class BaseCrudAppService<TViewModel, TEntity, TType> : BaseAppService<TViewModel, TEntity, TType>,
        IBaseCrudAppService<TViewModel, TEntity, TType> where TEntity : class, IEntity<TEntity, TType>
    {
        private readonly IBaseCrudService<TEntity, TType> service;
        private readonly IMapper mapper;

        public BaseCrudAppService(
            IBaseCrudService<TEntity, TType> service,
            IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public virtual async Task<IResultadoApplication> Inserir(TViewModel viewModel)
        {
            var application = new ResultadoApplication();

            try
            {
                application.Resultado(await service.Inserir(mapper.Map<TEntity>(viewModel)));

                if (application.Sucesso)
                {
                    service.Commit();
                    application.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                }
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public virtual async Task<IResultadoApplication> Atualizar(TViewModel viewModel)
        {
            var application = new ResultadoApplication();

            try
            {
                application.Resultado(await service.Atualizar(mapper.Map<TEntity>(viewModel)));

                if (application.Sucesso)
                {
                    service.Commit();
                    application.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                }
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public virtual async Task<IResultadoApplication> RemoverPorId(TType id)
        {
            var application = new ResultadoApplication();

            try
            {
                await service.RemoverPorId(id);
                service.Commit();
                application.ExecutadoComSuccesso().ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public virtual async Task<IResultadoApplication<IDictionary<string, string>>> RecuperarDropdown()
        {
            var application = new ResultadoApplication<IDictionary<string, string>>();

            try
            {
                application.DefinirData(await service.RecuperarDropDown());
                application.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }
    }
}
