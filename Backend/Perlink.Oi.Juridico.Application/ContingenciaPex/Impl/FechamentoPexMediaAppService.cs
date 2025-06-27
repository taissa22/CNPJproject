using AutoMapper;
using Experimental.System.Messaging;
using Perlink.Oi.Juridico.Application.ContingenciaPex.Interface;
using Perlink.Oi.Juridico.Application.ContingenciaPex.ViewModel;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Application.Mensagens.Pex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ContingenciaPex.Impl
{
    public class FechamentoPexMediaAppService : BaseCrudAppService<FechamentoContingenciaPexMediaViewModel, FechamentoPexMedia, long>, IFechamentoPexMediaAppService
    {
        public readonly IFechamentoPexMediaService service;
        public readonly IParametroService parametroService;
        public readonly IPermissaoService permissao;
        public readonly IUsuarioService usuario;
        public readonly IMapper mapper;

        public FechamentoPexMediaAppService(IFechamentoPexMediaService service, IParametroService parametroService, IPermissaoService permissao, IUsuarioService usuario, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.parametroService = parametroService;
            this.permissao = permissao;
            this.usuario = usuario;
            this.mapper = mapper;
        }

        public async Task<IPagingResultadoApplication<IEnumerable<FechamentoContingenciaPexMediaViewModel>>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina)
        {
            var resultado = new PagingResultadoApplication<IEnumerable<FechamentoContingenciaPexMediaViewModel>>();

            try
            {
                var model = await service.ListarFechamentos(dataInicio, dataFim, quantidade, pagina);
                resultado.DefinirData(mapper.Map<IEnumerable<FechamentoContingenciaPexMediaViewModel>>(model));
                resultado.Total = await service.TotalFechamentos(dataInicio, dataFim);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        private async Task ColocaNaFilaDeMensagens(int fechamentoId, DateTime dataFechamento)
        {
            var nomeFilaDownload = parametroService.RecuperarPorNome("FECH_PEX_MEDIA_FILA").Conteudo;
            var usuarioLogado = await usuario.ObterUsuarioLogado();
            var fila = new MessageQueue(nomeFilaDownload);
            fila.Formatter = new XmlMessageFormatter(new Type[1] { typeof(MensagemDownloadFechamentoPex) });

            var mensagem = new MensagemDownloadFechamentoPex();
            mensagem.CodigoUsuario = usuarioLogado.Id;
            mensagem.Email = usuarioLogado.Email;
            mensagem.CodigoSolicitacaoFechamento = fechamentoId;
            mensagem.DataFechamento = dataFechamento;

            var mensagens = fila.GetAllMessages();
            bool naoExisteMensagem = true;

            foreach (var msg in mensagens)
            {
                var body = (MensagemDownloadFechamentoPex)msg.Body;

                if (body.CodigoSolicitacaoFechamento == mensagem.CodigoSolicitacaoFechamento)
                {
                    naoExisteMensagem = false;
                }
            }

            if (naoExisteMensagem)
            {
                fila.Send(mensagem);
            }
        }

        public async Task<(byte[] arquivo, string nomeArquivo)> ObterArquivo(int fechamentoId, DateTime dataFechamento, DateTime dataGeracao)
        {
            var zipFileName = $"{dataFechamento:yyyyMMdd}_PEX_Contingencia_Media_{fechamentoId}_{dataGeracao:yyyyMMddHHmm}.zip";

            var caminhoDownload = parametroService.RecuperarPorNome("FECH_PEX_MEDIA_DIR").Conteudo;

            var filePath = Path.Combine(caminhoDownload, zipFileName);

            if (!File.Exists(filePath))
            {
                await ColocaNaFilaDeMensagens(fechamentoId, dataFechamento);
                return (null, null);
            }

            return (await File.ReadAllBytesAsync(filePath), zipFileName);
        }
    }
}
