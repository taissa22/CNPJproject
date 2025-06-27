using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBResumoProcessamentoService : BaseCrudService<BBResumoProcessamento, long>, IBBResumoProcessamentoService
    {
        private readonly IBBResumoProcessamentoRepository repository;
        private readonly IBBStatusParcelasRepository statusParcelasRepository;
        private readonly IParametroRepository parametroRepository;
        private readonly ILancamentoProcessoRepository lancamentoRepository;
        private readonly ILoteRepository loteRepository;
        private readonly IBBStatusRemessaRepository remessaRepository;
        private readonly IBBOrgaosRepository bbOrgaosRepository;
        private readonly IBBNaturezasAcoesRepository bbNaturezasRepository;
        private readonly IBBComarcaRepository bbComarcaRepository;

        public BBResumoProcessamentoService(IBBResumoProcessamentoRepository repository,
                                            IBBStatusParcelasRepository statusParcelasRepository,
                                            IParametroRepository parametroRepository,
                                            ILancamentoProcessoRepository lancamentoRepository,
                                            ILoteRepository loteRepository,
                                            IBBOrgaosRepository bbOrgaosRepository,
                                            IBBNaturezasAcoesRepository bbNaturezasRepository,
                                            IBBComarcaRepository bbComarcaRepository,
                                            IBBStatusRemessaRepository remessaRepository
                                            ) : base(repository)
        {
            this.repository = repository;
            this.statusParcelasRepository = statusParcelasRepository;
            this.parametroRepository = parametroRepository;
            this.lancamentoRepository = lancamentoRepository;
            this.loteRepository = loteRepository;
            this.bbOrgaosRepository = bbOrgaosRepository;
            this.bbComarcaRepository = bbComarcaRepository;
            this.bbNaturezasRepository = bbNaturezasRepository;
            this.remessaRepository = remessaRepository;
        }

        public async Task<ICollection<BBResumoProcessamentoResultadoDTO>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            return await repository.ConsultarArquivoRetorno(filtroDTO);
        }

        public async Task<ICollection<BBResumoProcessamentoResultadoDTO>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            return await repository.ExportarArquivoRetorno(filtroDTO);
        }

        public async Task<ICollection<BBResumoProcessamentoGuiaDTO>> BuscarGuiasOK(long numeroLoteBB)
        {
            return await repository.BuscarGuiasOK(numeroLoteBB);
        }

        public async Task<int> TotaisArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            return await repository.TotaisArquivoRetorno(filtroDTO);
        }

        public async Task<BBResumoProcessamentoImportacaoDTO> RecuperarDadosImportacao(IFormFile file)
        {
            var retorno = new BBResumoProcessamentoImportacaoDTO();
            using (var sr = new StreamReader(file.OpenReadStream()))
            {
                string linha;
                if (file.Length <= 0)
                {
                    retorno.MsgErro = "O arquivo importado encontra-se vazio.";
                    return retorno;
                }
                // Lê linha por linha até o final do arquivo
                while ((linha = sr.ReadLine()) != null && string.IsNullOrEmpty(retorno.MsgErro))
                {
					if ((linha.Length != 0)) {  
					
						switch (linha.Substring(0, 3))
						{
							case "000":
								if (!DesmembrarHeader(linha, retorno))
									retorno.MsgErro = $"Problemas ao desmembrar registro header: {linha}";
								break;
							case "111":
								if (!DesmembrarDetalhes(linha, retorno))
									retorno.MsgErro = $"Problemas ao desmembrar registro detalhe: {retorno.MsgErro} \nna linha: {linha}";
								break;
							case "999":
								if (!DesmembrarTrailer(linha, retorno))
									retorno.MsgErro = $"Problemas ao desmembrar registro trailer: {linha}";
								break;
							default:
								retorno.MsgErro = $"Tipo de registro inválido: {linha}";
								break;
						}
					}
                }
            }
            if (!string.IsNullOrEmpty(retorno.MsgErro))
                retorno.MsgErro = "Arquivo {0} inválido. Por favor, verifique e tente novamente."; // 0 = nomeArquivo
            if (string.IsNullOrEmpty(retorno.MsgErro))
            {
                await DefinirDescricaoTipoProcesso(retorno);
                await RemoverRegistrosNaoProcessadosAsync(retorno);
            }
            return retorno;
        }

        private async Task RemoverRegistrosNaoProcessadosAsync(BBResumoProcessamentoImportacaoDTO importacaoDTO)
        {
            //executar query Retrieve
            var listaRemover = new List<BBResumoProcessamentoGuiaDTO>();
            var guiasBanco = await repository.BuscarGuiasOK(importacaoDTO.Resumo.NumeroLoteBB);
            foreach (var guiaArquivo in importacaoDTO.GuiasOk)
            {
                var guiaProcessada = guiasBanco.Any(gb => gb.CodigoLancamento == guiaArquivo.CodigoLancamento &&
                                                gb.CodigoProcesso == guiaArquivo.CodigoProcesso);
                if (!guiaProcessada)
                    listaRemover.Add(guiaArquivo);
            }
            listaRemover.ForEach(item =>
            {
                importacaoDTO.GuiasOk.Remove(item);
            });
        }

        private async Task DefinirDescricaoTipoProcesso(BBResumoProcessamentoImportacaoDTO retorno)
        {
            var lote = await loteRepository.RecuperarPorId(retorno.Resumo.CodLoteSisJur);
            if (lote != null)
                retorno.Resumo.TipoProcesso = EnumExtensions.GetDescricaoFromValue<TipoProcessoEnum>(lote.CodigoTipoProcesso);
        }

        private bool DesmembrarTrailer(string linha, BBResumoProcessamentoImportacaoDTO retorno)
        {
            try
            {
                retorno.Resumo.ValorTotalRemessa = decimal.Parse(linha.Substring(25, 17)) / 100;
                retorno.Resumo.ValorTotalGuiaProcessada = decimal.Parse(linha.Substring(52, 17)) / 100;
                retorno.Resumo.QuantidadeRegistrosProcessados = long.Parse(linha.Substring(43, 9));
                retorno.Resumo.QuantidadeRegistrosArquivo = long.Parse(linha.Substring(16, 9));
                return true;
            }
            catch (Exception e) { return false; }
        }

        private bool DesmembrarDetalhes(string linha, BBResumoProcessamentoImportacaoDTO retorno)
        {
            string idRegistro = "";
            string msgErro = "";
            bool isOk = true;
            long codEstadoParcela;
            long codLancamento;
            long codProcesso;
            long codLote;

            try
            {
                long.TryParse(linha.Substring(183, 9), out codProcesso);
                long.TryParse(linha.Substring(192, 4), out codLancamento);
                long.TryParse(linha.Substring(308, 4), out codEstadoParcela);
                var bbStatusParcela = statusParcelasRepository.RecuperarPorCodigoBBStatusParcela(codEstadoParcela).Result;
                if (bbStatusParcela == null) { retorno.MsgErro = $"Status parcela não localizado no banbco: {codEstadoParcela}"; return false; }
                var objAuxiliar = new
                {
                    CodigoTribunalBB = linha.Substring(3, 9),
                    CodigoComarca = linha.Substring(12, 9),
                    CodigoOrgaoBB = linha.Substring(21, 9),
                    CodigoNaturezaAcaoBB = linha.Substring(30, 4),
                    NomeReu = linha.Substring(42, 30),
                    ReuCPF_CNPJ = linha.Substring(73, 14),
                    NomeAutor = linha.Substring(87, 30),
                    AutorCPF_CNPJ = linha.Substring(118, 14),
                    NumeroProcessoJudicial = linha.Substring(133, 25),
                    DataGuia = $"{linha.Substring(158, 2)}/{linha.Substring(161, 2)}/{linha.Substring(164, 4)}",
                    Guia = linha.Substring(168, 15),
                    IdProcesso = codProcesso.ToString(),
                    IdLancamento = codLancamento.ToString(),
                    ValorParcela = decimal.Parse(linha.Substring(196, 17)) / 100,

                    DataEfetivacaoParcelaBB = $"{linha.Substring(298, 2)}/{linha.Substring(301, 2)}/{linha.Substring(304, 4)}",
                    NumeroContaJudicial = linha.Substring(312, 13),
                    NumeroParcelaJudicial = linha.Substring(325, 4),
                    AutenticacaoEletronica = linha.Substring(329, 16),
                    IdBBStatusParcela = bbStatusParcela.Id.ToString(),
                    StatusParcelaBB = bbStatusParcela.Descricao,
                };

                if (retorno.Resumo.CodLoteSisJur == 0)
                {
                    idRegistro = linha.Substring(217, 26);
                    retorno.Resumo.CodLoteSisJur = long.Parse(idRegistro.Substring(0, 6));
                    retorno.Resumo.NumeroLoteBB = long.Parse(idRegistro.Substring(6, 6));
                }
                codLote = retorno.Resumo.CodLoteSisJur;

                var parametroBBStatusParcela = parametroRepository.RecuperarPorId("BB_STATUS_PARCELA_OK").Result.Conteudo;
                parametroBBStatusParcela = "|" + parametroBBStatusParcela + "|";
                
                BBResumoProcessamentoGuiaDTO lancamentoProcesso = repository.RecuperarLancamentoProcessoDoArquivo(codProcesso, codLancamento, codLote).Result;
                if (!parametroBBStatusParcela.Contains("|" + codEstadoParcela + "|"))
                {
                    isOk = false;
                    msgErro = "Guia com erro de processamento pelo BB";
                }
                else
                {
                    ValidarLancamentoDoArquivo(ref msgErro, ref isOk, objAuxiliar.NumeroContaJudicial, objAuxiliar.ValorParcela, lancamentoProcesso);
                }

                // ------------------------------------------------------------------------------------------
                // Verifica se o registro está OK e define se deverá ser acrescentado na área de erro
                // ou na área de lançamentos ok
                // ------------------------------------------------------------------------------------------
                if (isOk)
                {
                    retorno.GuiasOk.Add(new BBResumoProcessamentoGuiaDTO
                    {
                        CodigoProcesso = codProcesso.ToString(),
                        CodigoLancamento = codLancamento.ToString(),
                        AutenticacaoEletronica = objAuxiliar.AutenticacaoEletronica,
                        IdBBStatusParcela = objAuxiliar.IdBBStatusParcela,
                        StatusParcelaBB = objAuxiliar.StatusParcelaBB,
                        DataEfetivacaoParcelaBB = objAuxiliar.DataEfetivacaoParcelaBB,
                        NumeroContaJudicial = objAuxiliar.NumeroContaJudicial,
                        NumeroParcelaJudicial = objAuxiliar.NumeroParcelaJudicial,
                        NumeroProcesso = objAuxiliar.NumeroProcessoJudicial,
                        Comarca = lancamentoProcesso.Comarca,
                        Juizado = lancamentoProcesso.Juizado,
                        DescricaoEmpresaGrupo = lancamentoProcesso.DescricaoEmpresaGrupo,
                        DataLancamento = lancamentoProcesso.DataLancamento,
                        DescricaoTipoLancamento = lancamentoProcesso.DescricaoTipoLancamento,
                        DescricaoCategoriaPagamento = lancamentoProcesso.DescricaoCategoriaPagamento,
                        StatusPagamento = lancamentoProcesso.StatusPagamento,
                        DataEnvioEscritorio = lancamentoProcesso.DataEnvioEscritorio,
                        DescricaoEscritorio = lancamentoProcesso.DescricaoEscritorio,
                        NumeroPedidoSAP = lancamentoProcesso.NumeroPedidoSAP,
                        NumeroGuia = lancamentoProcesso.NumeroGuia,
                        DataRecebimentoFiscal = lancamentoProcesso.DataRecebimentoFiscal,
                        DataPagamentoPedido = lancamentoProcesso.DataPagamentoPedido,
                        ValorLiquido = lancamentoProcesso.ValorLiquido,
                        Autor = lancamentoProcesso.Autor
                    });
                }
                else
                {
                    var descricaoNatureza = bbNaturezasRepository.RecuperarPorCodigoBB(long.Parse(objAuxiliar.CodigoNaturezaAcaoBB)).Result.Descricao;
                    var descricaoOrgao = bbOrgaosRepository.RecuperarPorCodigoBB(long.Parse(objAuxiliar.CodigoOrgaoBB)).Result.Nome;
                    var descricaoComarca = bbComarcaRepository.RecuperarPorCodigoBB(long.Parse(objAuxiliar.CodigoComarca)).Result.Descricao;

                    retorno.GuiasComProblema.Add(new BBResumoProcessamentoGuiasComProblemaDTO
                    {
                        NumeroProcesso = objAuxiliar.NumeroProcessoJudicial,
                        CodigoComarca = objAuxiliar.CodigoComarca,
                        CodigoOrgaoBB = objAuxiliar.CodigoOrgaoBB,
                        CodigoNaturezaAcaoBB = objAuxiliar.CodigoNaturezaAcaoBB,
                        NomeNaturezaBB = descricaoNatureza,
                        NomeOrgaoBB = descricaoOrgao,
                        NomeComarca = descricaoComarca,
                        NomeAutor = objAuxiliar.NomeAutor,
                        AutorCPF_CNPJ = objAuxiliar.AutorCPF_CNPJ,
                        NomeReu = objAuxiliar.NomeReu,
                        ReuCPF_CNPJ = objAuxiliar.ReuCPF_CNPJ,
                        ValorParcela = objAuxiliar.ValorParcela,
                        DataGuia = objAuxiliar.DataGuia,
                        Guia = objAuxiliar.Guia,
                        NumeroConta = objAuxiliar.NumeroContaJudicial,
                        NumeroParcela = objAuxiliar.NumeroParcelaJudicial,
                        DataEfetivacaoParcelaBB = objAuxiliar.DataEfetivacaoParcelaBB,
                        IdBBStatusParcela = objAuxiliar.IdBBStatusParcela,
                        StatusParcelaBB = objAuxiliar.StatusParcelaBB,
                        IdProcesso = objAuxiliar.IdProcesso,
                        IdLancamento = objAuxiliar.IdLancamento,
                        DescricaoErroGuia = msgErro,

                    });
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static void ValidarLancamentoDoArquivo(ref string msgErro, ref bool isOk, string NumeroContaJudicial, decimal ValorParcela,
                        BBResumoProcessamentoGuiaDTO guiaBanco)
        {
            if (guiaBanco == null)
            {
                isOk = false;
                msgErro = "Guia não localizada";
            }
            else if (guiaBanco.AutenticacaoEletronica != null)
            {
                isOk = false;
                msgErro = "Guia já processada";
            }
            else if ((guiaBanco.NumeroContaJudicial != null) && (guiaBanco.NumeroContaJudicial != NumeroContaJudicial))
            {
                isOk = false;
                msgErro = "Divergência de conta judicial";
            }
            else if (guiaBanco.ValorLiquido != ValorParcela)
            {
                isOk = false;
                msgErro = "Divergência de valor";
            }
        }

        private bool DesmembrarHeader(string linha, BBResumoProcessamentoImportacaoDTO retorno)
        {
            try
            {
                retorno.Resumo.NumeroLoteBB = 0;
                retorno.Resumo.Status = linha.Substring(260, 50);
                var CodigoBBEstado = long.Parse(linha.Substring(256, 4));
                retorno.Resumo.IdBBStatusRemessa = remessaRepository.RecuperarIdBBStatusRemessa(CodigoBBEstado);
                var auxremessa = linha.Substring(10, 10);
                retorno.Resumo.DataRemessa = auxremessa.Substring(0, 2) + "/" + auxremessa.Substring(3, 2) + "/" + auxremessa.Substring(6, 4);
                var auxprocessamento = linha.Substring(246, 10);
                retorno.Resumo.DataProcessamentoRemessa = auxprocessamento.Substring(0, 2) + "/" + auxprocessamento.Substring(3, 2) + "/" + auxprocessamento.Substring(6, 4);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<BBResumoProcessamentoGuiaExibidaDTO> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento)
        {
            return await repository.BuscarGuiaExibicao(codigoProcesso, codigoLancamento);
        }

        public async Task SalvarImportacao(BBResumoProcessamentoImportacaoDTO dto)
        {
            await AtualizarLoteImportado(dto);
            await AtualizarLancamentosImportados(dto);
            await CriarArquivoImportado(dto);
            Commit();
        }

        private async Task CriarArquivoImportado(BBResumoProcessamentoImportacaoDTO dto)
        {
            var bbResumoArq = new BBResumoProcessamento
            {
                NumeroLoteBB = dto.Resumo.NumeroLoteBB,
                DataRemessa = DateTime.Parse(dto.Resumo.DataRemessa),
                DataProcessamentoRemessa = DateTime.Parse(dto.Resumo.DataProcessamentoRemessa),
                QuantidadeRegistrosArquivo = dto.Resumo.QuantidadeRegistrosArquivo,
                ValorTotalRemessa = dto.Resumo.ValorTotalRemessa,
                QuantidadeRegistrosProcessados = dto.Resumo.QuantidadeRegistrosProcessados,
                ValorTotalGuiaProcessada = dto.Resumo.ValorTotalGuiaProcessada,
                CodigoLote = dto.Resumo.CodLoteSisJur,
                CodigoBBStatusRemessa = dto.Resumo.IdBBStatusRemessa,
            };
            await repository.Inserir(bbResumoArq);
        }

        private async Task AtualizarLoteImportado(BBResumoProcessamentoImportacaoDTO dto)
        {
            var lote = await loteRepository.RecuperarPorId(dto.Resumo.CodLoteSisJur);
            lote.DataRetornoBB = DateTime.Now;
            await loteRepository.Atualizar(lote);
        }

        private async Task AtualizarLancamentosImportados(BBResumoProcessamentoImportacaoDTO dto)
        {
            foreach (var guia in dto.GuiasOk)
            {
                var lancamento = await lancamentoRepository.ObterLancamentoProcesso(long.Parse(guia.CodigoProcesso), long.Parse(guia.CodigoLancamento));
                lancamento.NumeroContaJudicial = long.Parse(guia.NumeroContaJudicial);
                lancamento.NumeroParcelaContaJudicial = long.Parse(guia.NumeroParcelaJudicial);
                lancamento.CodigoAutenticacaoEletronica = guia.AutenticacaoEletronica;
                lancamento.DataEfetivacaoParcelaBancoDoBrasil = DateTime.Parse(guia.DataEfetivacaoParcelaBB);
                lancamento.IdBbStatusParcela = long.Parse(guia.IdBBStatusParcela);
                await lancamentoRepository.Atualizar(lancamento);
            }
        }

        /**
         * Busca o limite máximo de arquivos permitidos para Upload
         */
        public async Task<int> ConsultarParametroMaxArquivosUpload()
        {
            return await Task.FromResult(Convert.ToInt32(parametroRepository.RecuperarPorNome("SAP_QTD_MAX_ARQUIV_UPLOAD").Conteudo));
        }

        /**
         * Busca o tamanho máximo de arquivo permitido para Upload
         */
        public async Task<long> ConsultarParametroTamanhoArquivosUpload()
        {
            return await Task.FromResult(Convert.ToInt64(parametroRepository.RecuperarPorNome("SAP_QTD_MAX_SIZE_UPLOAD").Conteudo));
        }
    }
}