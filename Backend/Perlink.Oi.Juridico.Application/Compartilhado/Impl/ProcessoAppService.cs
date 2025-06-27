using AutoMapper;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class ProcessoAppService : BaseCrudAppService<ProcessoViewModel, Processo, long>, IProcessoAppService
    {
        private readonly IProcessoService service;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;
        private readonly IUsuarioService usuarioService;
        private readonly IEscritorioService escritorioService;

        public ProcessoAppService(IProcessoService service, IPermissaoService permissaoService, IUsuarioService usuarioService, IEscritorioService escritorioService, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
            this.usuarioService = usuarioService;
            this.escritorioService = escritorioService;
        }

        public async Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, string rota = "defaultValue")
        {
            var result = new ResultadoApplication<ICollection<ProcessoFiltroViewModel>>();
            ICollection<EscritorioDTO> escritorios = new List<EscritorioDTO>();

            try
            {
                var usuarioLogado = await usuarioService.ObterUsuarioLogado();

                if (usuarioLogado == null)
                {
                    throw new Exception("Erro ao obter o usuário logado.");
                }

                if (usuarioLogado.EhEscritorio)
                {
                    escritorios = await escritorioService.RecuperarEscritorioTrabalhistaFiltro(usuarioLogado.EhEscritorio, usuarioLogado.Id);

                    if (escritorios == null || escritorios.Count <= 0)
                    {
                        throw new Exception("Não foi encontrado nenhum escritório.");
                    }
                }

                var model = await service.RecuperarProcessoPeloCodigoTipoProcesso(numeroProcesso, codigoTipoProcesso, usuarioLogado.EhEscritorio, escritorios?.Select(esc => esc.Id).ToList());

                switch (rota.ToEnum(ControleRotaEnum.defaultValue))
                {
                    case ControleRotaEnum.consultaLote:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                    case ControleRotaEnum.consultaSaldoGarantia:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                    default:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                }
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> RecuperarProcessoPeloCodigoInterno(long codigoInterno, long codigoTipoProcesso, string rota = "defaultValue")
        {
            var result = new ResultadoApplication<ICollection<ProcessoFiltroViewModel>>();
            ICollection<EscritorioDTO> escritorios = new List<EscritorioDTO>();

            try
            {
                var usuarioLogado = await usuarioService.ObterUsuarioLogado();

                if (usuarioLogado == null)
                {
                    throw new Exception("Erro ao obter o usuário logado.");
                }

                if (usuarioLogado.EhEscritorio)
                {
                    escritorios = await escritorioService.RecuperarEscritorioTrabalhistaFiltro(usuarioLogado.EhEscritorio, usuarioLogado.Id);

                    if (escritorios == null || escritorios.Count <= 0)
                    {
                        throw new Exception("Não foi encontrado nenhum escritório.");
                    }
                }

                var model = await service.RecuperarProcessoPeloCodigoInterno(codigoInterno, codigoTipoProcesso, usuarioLogado.EhEscritorio, escritorios?.Select(esc => esc.Id).ToList());

                switch (rota.ToEnum(ControleRotaEnum.defaultValue))
                {
                    case ControleRotaEnum.consultaLote:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                    case ControleRotaEnum.consultaSaldoGarantia:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                    default:
                        result.DefinirData(mapper.Map<ICollection<ProcessoFiltroViewModel>>(model));
                        break;
                }
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
        public async Task<IResultadoApplication<string>> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, ILogger logger)
        {
            var resultado = new ResultadoApplication<string>();

            try
            {
                resultado.DefinirData(await this.service.ExportacaoPrePosRj(tipoProcesso, logger));
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception problema)
            {
                resultado.ExecutadoComErro(problema);
            }
            return resultado;
        }

        public async Task<IResultadoApplication> ExpurgoPrePosRj(ILogger logger)
        {
            var resultado = new ResultadoApplication();

            try
            {
                await service.ExpurgoPrePosRj(logger);
                service.Commit();
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
            }
            catch (Exception problema)
            {
                resultado.ExecutadoComErro(problema);
            }
            return resultado;
        }
    }
}