using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.Interface;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.ViewModel;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.AlteracaoBloco.Impl
{
    public class AlteracaoEmBlocoAppService : BaseCrudAppService<AlteracaoEmBlocoViewModel, AlteracaoEmBloco, long>, IAlteracaoEmBlocoAppService
    {
        public readonly IAlteracaoEmBlocoService service;
        public readonly IParametroService parametroService;
        public readonly IPermissaoService permissao;
        public readonly IUsuarioService usuario;
        public readonly IMapper mapper;

        public AlteracaoEmBlocoAppService(IAlteracaoEmBlocoService service, IParametroService parametroService, IPermissaoService permissao, IUsuarioService usuario, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.parametroService = parametroService;
            this.permissao = permissao;
            this.usuario = usuario;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication> Upload(IFormFile arquivo, long codigoTipoProcesso)
        {

            var parametroDoTamanhoDoArquivo = parametroService.RecuperarPorNome("TAM_MAX_ALTERACAO_BLOCO");
            var parametroDaPastaDestino = parametroService.RecuperarPorNome("DIR_NAS_ALT_BLOCO_AGEND");
            var resultado = new ResultadoApplication();

            try
            {
                var arquivoDaPastaTemporaria = service.ValidarArquivoDaPastaTemporaria(arquivo);
                if (!arquivoDaPastaTemporaria)
                {
                    resultado.ExecutadoComErro(Textos.Geral_Mensagem_Erro_Extensao_CSV_Arquivo_Invalida);
                    return resultado;
                }

                if (arquivo.FileName.Length > 74)//nome + extensão
                {
                    resultado.ExecutadoComErro(Textos.Mensagem_Erro_Tamanho_Nome_Arquivo);
                    return resultado;
                }

                var caminhoDoArquivo = service.GravarArquivoNaPastaTemporaria(arquivo);

                var tamanhoDoArquivo = service.ValidarTamanhoDoArquivo(caminhoDoArquivo, parametroDoTamanhoDoArquivo);
                if (!tamanhoDoArquivo)
                {
                    resultado.ExecutadoComErro(Textos.Geral_Mensagem_Erro_Tamanho_Arquivo_10mb);
                    return resultado;
                }

                var validarColunas = service.ValidarColunasDoArquivo(caminhoDoArquivo, codigoTipoProcesso);
                if (!validarColunas)
                {
                    resultado.ExecutadoComErro(Textos.Mensagem_Erro_Quantidade_Colunas);
                    return resultado;
                }

                var validarDadosDoArquivo = service.ValidarDadosDoArquivo(caminhoDoArquivo, codigoTipoProcesso);
                if (!validarDadosDoArquivo)
                {
                    resultado.ExecutadoComErro(Textos.Mensagem_Erro_Arquivo_Vazio);
                    return resultado;
                }

                var nomeDoArquivo = service.GravarArquivoNoNas(caminhoDoArquivo.ToLower(), arquivo, parametroDaPastaDestino);

                var model = new AlteracaoEmBlocoViewModel();

                model.Arquivo = nomeDoArquivo;
                model.Status = AlteracaoEmBlocoEnum.Agendado;
                model.DataCadastro = DateTime.Now;
                model.CodigoDoUsuario = usuario.ObterUsuarioLogado().Result.Id.ToString();
                model.CodigoTipoProcesso = (TipoProcessoEnum?)codigoTipoProcesso;
                model.ProcessosAtualizados = 0;
                model.ProcessosComErro = 0;

                resultado.Resultado(await service.Inserir(mapper.Map<AlteracaoEmBloco>(model)));

                if (resultado.Sucesso)
                {
                    service.Commit();
                    resultado.ExibirMensagem(Textos.Mensagem_Arquivo_CSV_Enviado_Status_Agendado);
                }
            }
            catch (System.Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }


            return resultado;
        }

        public async Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> BaixarPlanilhaCarregada(long codigoAlteracaoEmBloco)
        {
            var parametroPastaDestinoArquivo = parametroService.RecuperarPorNome("DIR_NAS_ALT_BLOCO_AGEND");

            var resultado = new ResultadoApplication<AlteracaoEmBlocoDownloadViewModel>();
            try
            {
                var model = await service.RecuperarPorId(codigoAlteracaoEmBloco);
                var viewModel = await service.BaixarPlanilha(model, parametroPastaDestinoArquivo);
                resultado.DefinirData(mapper.Map<AlteracaoEmBlocoDownloadViewModel>(viewModel));
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }

        public async Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> BaixarPlanilhaResultado(long codigoAlteracaoEmBloco)
        {
            var parametroPastaDestinoArquivo = parametroService.RecuperarPorNome("DIR_NAS_ALT_BLOCO_RESULT");

            var resultado = new ResultadoApplication<AlteracaoEmBlocoDownloadViewModel>();
            try
            {
                var model = await service.RecuperarPorId(codigoAlteracaoEmBloco);
                model.Arquivo = "Resultado_" + model.Arquivo;
                var viewModel = await service.BaixarPlanilha(model, parametroPastaDestinoArquivo);
                resultado.DefinirData(mapper.Map<AlteracaoEmBlocoDownloadViewModel>(viewModel));
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }


        public async Task<IResultadoApplication> RemoverAgendamentoComStatusAgendado(long id)
        {
            var resultado = new ResultadoApplication<AlteracaoEmBlocoViewModel>();

            try
            {
                var parametroDaPastaDestino = parametroService.RecuperarPorNome("DIR_NAS_ALT_BLOCO_AGEND");
                var agendamento = await service.RecuperarPorId(id);

                if (agendamento == null)
                    throw new Exception(Textos.Mensagem_Erro_Agendamento_Nao_Encontrado);

                if (!(agendamento.Status.Equals(AlteracaoEmBlocoEnum.Agendado) || agendamento.Status.Equals(AlteracaoEmBlocoEnum.Erro)))
                    throw new Exception(Textos.Mensagem_Erro_Ao_Excluir_Agendamento_Status_Diferente_Agendado);

                service.RemoverAgendamento(id, parametroDaPastaDestino, agendamento.Arquivo);
                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception excecao)
            {
                resultado.ExecutadoComErro(excecao);
            }
            return resultado;
        }

        public async Task<IPagingResultadoApplication<ICollection<AlteracaoEmBlocoViewModel>>> ListarAgendamentos(int index = 1, int count = 5)
        {
            var resultado = new PagingResultadoApplication<ICollection<AlteracaoEmBlocoViewModel>>();

            try
            {
                var model = await service.ListarAgendamentos(index, count);
                var data = mapper.Map<ICollection<AlteracaoEmBlocoViewModel>>(model);

                resultado.DefinirData(data);
                resultado.Total = await service.ObterQuantidadeTotal();
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        public async Task<IResultadoApplication> ExpurgoAlteracaoEmBloco(ILogger logger)
        {
            var resultado = new ResultadoApplication();
            try
            {
                await service.ExpurgoAlteracaoEmBloco(logger);
                service.Commit();

            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        public async Task<IResultadoApplication> ProcessarAgendamentos(ILogger logger)
        {
            var resultado = new ResultadoApplication();
            try
            {
                await service.ProcessarAgendamentos(logger);
                service.Commit();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
                logger.LogInformation(ex.Message);
            }

            return resultado;
        }
    }
}
