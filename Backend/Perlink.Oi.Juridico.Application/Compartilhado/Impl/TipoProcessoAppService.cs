using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Tools;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class TipoProcessoAppService : BaseCrudAppService<TipoProcessoViewModel, TipoProcesso, long>, ITipoProcessoAppService
    {
        private readonly ITipoProcessoService service;
        private readonly IMapper mapper;

        public TipoProcessoAppService(ITipoProcessoService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<TipoProcessoViewModel>>> RecuperarTodosSAP(string tela) {
            var result = new ResultadoApplication<ICollection<TipoProcessoViewModel>>();
            try {
                IEnumerable<TipoProcesso> model;
                
                switch (tela.ToEnum(ControleRotaEnum.defaultValue)) {
                    case ControleRotaEnum.consultaLote:
                        model = await service.RecuperarTodosConsultaLote();
                        break;
                    case ControleRotaEnum.criaLote:
                        model = await service.RecuperarTodosCriaLote();
                        break;
                    case ControleRotaEnum.estornaLancamento:
                        model = await service.RecuperarTodosEstornaLancamento();
                        break;
                    case ControleRotaEnum.consultaSaldoGarantia:
                        model = await service.RecuperarParaConsultaSaldoDeGarantia();
                        break;
                    case ControleRotaEnum.manutencaoCategoriaPagamento:
                        model = await service.RecuperarTodosManutencaoCategoriaPagamento();
                        break;
                    case ControleRotaEnum.manutencaoVigienciaCivil:
                        model = await service.RecuperarTodosManutencaoVigenciaCivil();
                        break;
                    case ControleRotaEnum.manutencaoTipoAudiencia:
                        model = await service.RecuperarTodosManutencaoTipoAudiencia();
                        break;
                    default:
                        model = await service.RecuperarTodos(); 
                        break;
                }
                
                result.DefinirData(mapper.Map<ICollection<TipoProcessoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

            } catch (Exception ex) {
                result.ExecutadoComErro(ex);            

            }
            return result;
        }
    }
}