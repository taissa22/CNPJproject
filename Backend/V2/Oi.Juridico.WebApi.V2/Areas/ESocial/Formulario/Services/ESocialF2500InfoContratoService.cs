using Microsoft.IdentityModel.Tokens;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Services;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500InfoContratoService
    {
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;

        public ESocialF2500InfoContratoService(ESocialDbContext eSocialDbContext, ParametroJuridicoContext parametroJuridicoDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
        }

        public async Task<IEnumerable<string>> ValidaInclusaoInfoContrato(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct)
        {
            var listaErros = new List<string>();

            listaErros.AddRange((await ValidaErrosGlobais(context, requestDTO, contrato, formulario, ct)).ToList());
            listaErros.AddRange((await ValidaErrosInclusao(context, requestDTO, contrato, formulario, ct)).ToList());

            return listaErros;

        }

        public async Task<IEnumerable<string>> ValidaAlteracaoInfoContrato(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, int codigoInfoContrato, CancellationToken ct)
        {
            var listaErros = new List<string>();

            listaErros.AddRange((await ValidaErrosGlobais(context, requestDTO, contrato, formulario, ct)).ToList());
            listaErros.AddRange((await ValidaErrosAlteracao(context, requestDTO, contrato, formulario, codigoInfoContrato, ct)).ToList());

            return listaErros;

        }

        public List<string> ValidaCamposContador(EsF2500InfocontratoRequestDTO requestDTO, IEnumerable<string> listaErros, EsF2500? formulario, EsF2500Infocontrato? infoContrato, ESocialDbContext eSocialDbContext)
        {
            var listaErrosTemp = listaErros.ToList();

            #region validações Contador
            if (string.IsNullOrEmpty(infoContrato!.InfovlrCompini))
            {
                listaErrosTemp.Add("O campo \"Competência Inicial\" do (Bloco J) é obrigatório!");
            }

            if (string.IsNullOrEmpty(infoContrato!.InfovlrCompfim))
            {
                listaErrosTemp.Add("O campo \"Competência Final\" do (Bloco J) é obrigatório!");
            }

            if (requestDTO.InfovlrCompini.HasValue && requestDTO.InfovlrCompfim.HasValue)
            {
                if (requestDTO.InfovlrCompfim.Value.Date < requestDTO.InfovlrCompini.Value.Date)
                {
                    listaErrosTemp.Add("O período da \"Competência Final\" deve ser maior ou igual ao da \"Competência Inicial\".");
                }
            }

            if (requestDTO.InfovlrCompini.HasValue && requestDTO.InfovincDtadm.HasValue && infoContrato.InfocontrTpcontr.HasValue
                && (infoContrato.InfocontrTpcontr == 1 || infoContrato.InfocontrTpcontr == 3 || infoContrato.InfocontrTpcontr == 7
                || infoContrato.InfocontrTpcontr == 8 || infoContrato.InfocontrTpcontr == 9))
            {
                if (requestDTO.InfovlrCompini!.Value.Date < new DateTime(requestDTO.InfovincDtadm!.Value.Date.Year, requestDTO.InfovincDtadm.Value.Date.Month, 1).Date)
                {
                    listaErrosTemp.Add("Se o \"Tipo do Contrato\" (Bloco D) for igual a \"1\", \"3\", \"7\", \"8\" ou \"9\", o campo \"Competência Inicial\" (Bloco J) deve ser maior ou igual ao mês/ano informado na \"Data de Admissão\" (Bloco E).");
                }
            }

            if (requestDTO.InfovlrCompini.HasValue && requestDTO.InfovincDtadm.HasValue && infoContrato.InfocontrTpcontr.HasValue && (infoContrato.InfocontrTpcontr == 2 || infoContrato.InfocontrTpcontr == 4 || infoContrato.InfocontrTpcontr == 5))
            {
                if (requestDTO.InfovlrCompini.Value.Date != new DateTime(requestDTO.InfovincDtadm.Value.Date.Year, requestDTO.InfovincDtadm.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("Se o \"Tipo do Contrato\" (Bloco D) for igual a igual a \"2\", \"4\" ou \"5\", o campo \"Competência Inicial\" (Bloco J) deve ser igual ao mês/ano da \"Data de Admissão\" (Bloco E).");
                }
            }

            if (requestDTO.InfovlrCompini.HasValue && infoContrato.InfocontrTpcontr.HasValue && infoContrato.InfocontrDtinicio.HasValue && infoContrato.InfocontrTpcontr == 6)
            {
                if (requestDTO.InfovlrCompini.Value.Date < new DateTime(infoContrato.InfocontrDtinicio!.Value.Date.Year, infoContrato.InfocontrDtinicio!.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("Se o \"Tipo do Contrato\" no (Bloco D) for igual a 6, o campo \"Competência Inicial\" (Bloco J) deve ser maior ou igual ao mês/ano da \"Data de Início do TSVE\" (Bloco D).");
                }
            }

            if (requestDTO.InfovlrCompfim.HasValue && (formulario!.InfoprocjudDtsent.HasValue || formulario.InfoccpDtccp.HasValue))
            {
                if (formulario.InfoprocjudDtsent.HasValue && requestDTO.InfovlrCompfim.Value.Date > new DateTime(formulario.InfoprocjudDtsent!.Value.Date.Year, formulario.InfoprocjudDtsent.Value.Date.Month, 1)
                    || formulario.InfoccpDtccp.HasValue && requestDTO.InfovlrCompfim.Value.Date > new DateTime(formulario.InfoccpDtccp!.Value.Date.Year, formulario.InfoccpDtccp.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("O período da \"Competência Final\" no (Bloco J) deve ser menor ou igual ao mês/ano da \"Data da Sentença ou Homologação do Acordo do Processo Judicial\" do (Bloco B).");
                }
            }

            #region Valida Remuneracao                 

            //valida remunerações
            var listaRemuneracoes = eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var remuneracao in listaRemuneracoes)
            {
                listaErrosTemp.AddRange(ESocialF2500RemuneracaoService.ValidaErrosGlobais(_eSocialDbContext, remuneracao, infoContrato).ToList());
            }

            var ExisteRemuneracao = eSocialDbContext.EsF2500Remuneracao.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
            if (infoContrato.InfocontrIndcontr == "S" && ExisteRemuneracao)
            {
                listaErrosTemp.Add($"Não deve ser informado o grupo \"Remuneração\" (Bloco G) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato.InfocontrIndcontr != "S")
            {
                var naoExisteRemuneracao = !eSocialDbContext.EsF2500Remuneracao.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
                if (infoContrato.InfocontrTpcontr.HasValue
                    && infoContrato.InfocontrTpcontr != ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()
                    && requestDTO.InfovincTpregtrab.HasValue
                    && requestDTO.InfovincTpregtrab == ESocialRegimeTrabalhista.CLT.ToByte()
                    && naoExisteRemuneracao)

                {
                    listaErrosTemp.Add($"É obrigatório informar o grupo \"Remuneração\" (Bloco G), quando o valor informado no campo 'Tipo de Contrato' (Bloco D) for diferente de \"6 - Trabalhador sem vínculo de emprego/estatutário (TSVE), sem reconhecimento de vínculo empregatício\" e o Tipo de regime trabalhista (Bloco E)  for igual a \"CLT\" .");
                }

                if ((infoContrato.InfocontrCodcateg == 721 || infoContrato.InfocontrCodcateg == 722 || infoContrato.InfocontrCodcateg == 771) && naoExisteRemuneracao)
                {
                    listaErrosTemp.Add($"É obrigatório informar o grupo \"Remuneração\" (Bloco G), quando o valor informado no campo  “Categoria” (Bloco D) for igual a '721', '722' OU '771'.");
                }
            }

            #endregion

            #region Abono

            if (!string.IsNullOrEmpty(infoContrato.InfovlrIdenabono) && infoContrato.InfovlrIdenabono == "S" && !eSocialDbContext.EsF2500Abono.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"É obrigatório informar o grupo \"Abono\" (Bloco J), quando o campo \"Indenização Abono Salarial\" estiver marcado.");
            }

            if ((string.IsNullOrEmpty(infoContrato.InfovlrIdenabono) || !string.IsNullOrEmpty(infoContrato.InfovlrIdenabono) && infoContrato.InfovlrIdenabono == "N") && eSocialDbContext.EsF2500Abono.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"Não deve ser informado o grupo \"Abono\" (Bloco J), quando o campo \"Indenização Abono Salarial\" estiver desmarcado.");
            }

            #endregion

            #region Bases de Cálculo de Contribuição Previdenciária e FGTS 

            //valida periodo base de calculo

            var listaPeriodos = eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var periodo in listaPeriodos)
            {
                listaErrosTemp.AddRange(ESocialF2500IdePeriodoService.ValidaErrosGlobais(periodo, infoContrato, _parametroJuridicoDbContext, _eSocialDbContext).ToList());
            }

            if ((infoContrato.InfovlrIndreperc == ESocialRepercussaoProcesso_v1_2.DecisaoComPagamento.ToByte() || infoContrato.InfovlrIndreperc == ESocialRepercussaoProcesso_v1_2.DecisaoTributaria.ToByte()) && !eSocialDbContext.EsF2500Ideperiodo.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"Se o campo Repercussão do Processo (Bloco J)  estiver preenchido com \"1 - Decisão com repercussão tributária e/ou FGTS com rendimentos informados em S-2501\" ou \"5 - Decisão com repercussão tributária e/ou FGTS com pagamento através de depósito judicial\" será necessário informar os campos do Bloco K.");
            }

            if (infoContrato.InfocontrIndcontr == "N"
                    && infoContrato.InfocontrCodcateg == 111)
            {                
                    foreach (var item in listaPeriodos)
                {
                    var infoInterm = _eSocialDbContext.EsF2500Infointerm.Any(x => x.IdEsF2500Ideperiodo == item.IdEsF2500Ideperiodo);
                    if (!infoInterm)
                    {
                    var dtPeriodo = new DateTime(int.Parse(item.IdeperiodoPerref.Substring(0, 4)), int.Parse(item.IdeperiodoPerref.Substring(5, 2)), 1);
                    listaErrosTemp.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for igual a \"Não\" e a Categoria informada (Bloco D) for igual a \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" é de preenchimento obrigatório para o periodo {dtPeriodo.ToString("MM/yyyy")}. Caso não tenha havido trabalho no mês, informe 0 (zero).");
                    }
                }
            }

            if (infoContrato.InfocontrIndcontr == "N"
                && infoContrato.InfocontrCodcateg == 111
                && !listaPeriodos.Any())
            {
                listaErrosTemp.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for igual a \"Não\" e a Categoria informada (Bloco D) for igual a \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" é de preenchimento obrigatório. Caso não tenha havido trabalho no mês, informe 0 (zero).");
            }

            if (infoContrato.InfocontrIndcontr != "N"
               || infoContrato.InfocontrCodcateg != 111)
            {
                foreach (var item in listaPeriodos)
                {
                    var infoInterm = _eSocialDbContext.EsF2500Infointerm.Any(x => x.IdEsF2500Ideperiodo == item.IdEsF2500Ideperiodo);
                    if (infoInterm)
                    {
                    var dtPeriodo = new DateTime(int.Parse(item.IdeperiodoPerref.Substring(0, 4)), int.Parse(item.IdeperiodoPerref.Substring(5, 2)), 1);
                    listaErrosTemp.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for diferente de \"Não\" e a Categoria informada (Bloco D) for diferente de \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" não deve ser preenchido para o periodo {dtPeriodo.ToString("MM/yyyy")}.");
                    }
                }
            }

            #endregion

            #endregion // validações contador
            return listaErrosTemp;
        }

        public List<string> ValidaCamposEscritorio(int codigoFormulario, EsF2500InfocontratoRequestDTO requestDTO, IEnumerable<string> listaErros, EsF2500? formulario, EsF2500Infocontrato? infoContrato, ESocialDbContext eSocialDbContext)
        {
            var listaErrosTemp = listaErros.ToList();

            #region validações escritório

            if (infoContrato!.InfovincTpregprev == 2 && (infoContrato.InfocontrCodcateg == 101 || infoContrato.InfocontrCodcateg == 102 ||
                                                        infoContrato.InfocontrCodcateg == 103 || infoContrato.InfocontrCodcateg == 105 ||
                                                        infoContrato.InfocontrCodcateg == 106 || infoContrato.InfocontrCodcateg == 107 ||
                                                        infoContrato.InfocontrCodcateg == 108 || infoContrato.InfocontrCodcateg == 111))
            {
                listaErrosTemp.Add("Se o valor informado no campo \"Categoria\" (Bloco D) for igual a 101, 102, 103, 105, 106, 107, 108 ou 111, o \"Tipo de Regime Previdenciário\" (Bloco E) não pode ser preenchido com \"Regime Próprio de Previdência Social - RPPS ou Sistema de Proteção Social dos Militares\".");
            }

            if (requestDTO.InfovincTpregtrab != 1 && infoContrato.InfocontrCodcateg == 104)
            {
                listaErrosTemp.Add("Quando a \"Categoria\"  (Bloco D) for igual \"Empregado - Doméstico\", o \"Tipo de Regime Trabalhista\" (Bloco E) deve ser \"CLT\".");
            }

            if (requestDTO.InfovincTpregprev != 1 && infoContrato.InfocontrCodcateg == 104)
            {
                listaErrosTemp.Add("Quando a \"Categoria\"  (Bloco D) for igual \"Empregado - Doméstico\", o Tipo de Regime Previdenciário (Bloco E) deve ser \"1 - Regime Geral de Previdência Social - RGPS.\"");
            }

            if (requestDTO.SucessaovincTpinsc.HasValue && requestDTO.SucessaovincTpinsc == 1 &&
                formulario!.IdeempregadorTpinsc.HasValue && formulario.IdeempregadorTpinsc != 1 &&
                !string.IsNullOrEmpty(requestDTO.SucessaovincNrinsc) && !string.IsNullOrEmpty(formulario.IdeempregadorNrinsc) &&
                requestDTO.SucessaovincNrinsc == formulario.IdeempregadorNrinsc &&
                formulario.IdeempregadorNrinsc.Length < 14)
            {
                listaErrosTemp.Add("Se o valor informado no campo \"Tipo de Inscrição da Sucessão do Vínculo\" for igual a CNPJ, o campo \"Número de Inscrição da Sucessão do Vínculo\" deve ser diferente do \"Número de Inscrição do empregador\" (exceto se este possuir 14 algarismos).");
            }

            if (requestDTO.DuracaoDtterm.HasValue && requestDTO.InfovincDtadm.HasValue && requestDTO.SucessaovincDttransf.HasValue)
            {
                if (requestDTO.DuracaoDtterm.Value.Date < requestDTO.InfovincDtadm.Value.Date ||
                    requestDTO.DuracaoDtterm.Value.Date < requestDTO.SucessaovincDttransf.Value.Date)
                {
                    listaErrosTemp.Add("A \"Data de Término\" (Bloco E) deve ser maior ou igual que a \"Data de Admissão/Transferência\" (Bloco E)");
                }
            }
            else if (requestDTO.DuracaoDtterm.HasValue && (requestDTO.InfovincDtadm.HasValue || requestDTO.SucessaovincDttransf.HasValue))
            {
                if (requestDTO.InfovincDtadm.HasValue && requestDTO.DuracaoDtterm.Value.Date < requestDTO.InfovincDtadm!.Value.Date &&
                    !requestDTO.SucessaovincDttransf.HasValue)
                {
                    listaErrosTemp.Add("A \"Data de Término\" (Bloco E) deve ser maior ou igual que a \"Data de Admissão/Transferência\" (Bloco E)");
                }
                else if (requestDTO.SucessaovincDttransf.HasValue && requestDTO.DuracaoDtterm.Value.Date < requestDTO.SucessaovincDttransf!.Value.Date &&
                    !requestDTO.InfovincDtadm.HasValue)
                {
                    listaErrosTemp.Add("A \"Data de Término\" (Bloco E) deve ser maior ou igual que a \"Data de Admissão/Transferência\" (Bloco E)");
                }
            }

            if (infoContrato.DuracaoTpcontr == 2 && !requestDTO.DuracaoDtterm.HasValue)
            {
                listaErrosTemp.Add("\"Data de Término\" (Bloco E) obrigatório se o \"Tipo de Contrato\" (Bloco E) for \"2-Prazo determinado definido em dias\".");
            }

            if (requestDTO.InfovincTmpparc.HasValue && requestDTO.InfovincTmpparc == 1 && infoContrato.InfocontrCodcateg != 104)
            {
                listaErrosTemp.Add("Somente informe \"Limitado a 25 horas semanais\", se o campo \"Categoria\" (Bloco D), for igual a 104.");
            }

            if ((requestDTO.InfovincTmpparc.HasValue && requestDTO.InfovincTmpparc == 2 || requestDTO.InfovincTmpparc == 3) && infoContrato.InfocontrCodcateg != 104)
            {
                listaErrosTemp.Add("Não informe \"Limitado a 30 horas semanais ou Limitado a 26 horas semanais\" se o valor do campo \"Categoria\" (Bloco D), for igual a 104.");
            }

            if (requestDTO.InfovincDtadm.HasValue && formulario!.IdetrabDtnascto.HasValue && requestDTO.InfovincDtadm.Value <= formulario.IdetrabDtnascto.Value)
            {
                listaErrosTemp.Add("A \"Data de Admissão\" deve ser maior que a \"Data de Nascimento\" do trabalhador.");
            }

            if (string.IsNullOrEmpty(requestDTO.InfodesligMtvdeslig) && infoContrato.InfocontrCodcateg.HasValue && infoContrato.InfocontrCodcateg.Value == 721)
            {//infodesligMtvdeslig
                listaErrosTemp.Add("O campo \"Motivo do Desligamento\" é  obrigatório quando a \"Categoria\" (Bloco D) estiver preenchido com o código 721");
            }

            if (requestDTO.SucessaovincTpinsc.HasValue && requestDTO.SucessaovincTpinsc == 5 && requestDTO.SucessaovincDttransf!.Value.Date > new DateTime(1999, 6, 30).Date)
            {
                listaErrosTemp.Add("Só informe \"5 - CGC\" para o campo Tipo de Inscrição da Sucessão do Vínculo, se a Data da Transferência for igual ou menor a 30/06/1999.");
            }

            #region valida bloco D
            var qtdContratosRespInd = eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == formulario!.IdF2500).Count();
            if (!string.IsNullOrEmpty(formulario!.IderespNrinsc)
                && (!eSocialDbContext.EsF2500Infocontrato.Any(x => x.IdF2500 == formulario!.IdF2500 && x.InfocontrTpcontr == ESocialTipoContratoTSVE.ResponsabilidadeIndireta.ToByte() || x.InfocontrTpcontr == ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()) || qtdContratosRespInd == 0 || qtdContratosRespInd > 1))
            {
                listaErrosTemp.Add("Quando o campo Real Empregador (bloco A) é preenchido, é obrigatório o registro de um e somente um contrato (bloco D) e esse deve ter os campos abaixo preenchidos com os seguintes conteúdos: <br/> Tipo de Contrato: 8 - RESPONSABILIDADE INDIRETA ou 6 - TRABALHADOR SEM VÍNCULO DE EMPREGO/ESTATUTÁRIO (TSVE), SEM RECONHECIMENTO DE VÍNCULO EMPREGATÍCIO <br/> Possui Inf. Evento Admissão/Início: Não");
            }

            if (string.IsNullOrEmpty(formulario!.IderespNrinsc) && eSocialDbContext.EsF2500Infocontrato.Any(x => x.IdF2500 == formulario!.IdF2500
            && (x.InfocontrTpcontr == ESocialTipoContratoTSVE.ResponsabilidadeIndireta.ToByte() || x.InfocontrTpcontr == ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()) && x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add("Para contratos do tipo 8 - RESPONSABILIDADE INDIRETA ou 6 - TRABALHADOR SEM VÍNCULO DE EMPREGO/ESTATUTÁRIO (TSVE), SEM RECONHECIMENTO DE VÍNCULO EMPREGATÍCIO, é necessário o preenchimento dos campos Tipo e Número de Inscrição do Real Empregador (bloco A).");
            }

            #endregion

            #region Valida unicidade

            //valida unicidade contratual

            var listaUnicidades = eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var unicidade in listaUnicidades)
            {
                listaErrosTemp.AddRange(ESocialF2500UnicContrService.ValidaErrosGlobais(_eSocialDbContext, unicidade, infoContrato).ToList());
            }

            var ExisteUnicidade = eSocialDbContext.EsF2500Uniccontr.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
            if (infoContrato!.InfocontrTpcontr.HasValue && infoContrato!.InfocontrTpcontr == ESocialTipoContratoTSVE.TrabComUnicidadeContratual.ToByte() && !ExisteUnicidade)
            {
                listaErrosTemp.Add("Os campos do \"Bloco I\" são de preenchimento obrigatório, quando o valor informado no campo \"Tipo de Contrato\" (Bloco D) for igual a \"9 - TRABALHADOR CUJOS CONTRATOS FORAM UNIFICADOS (UNICIDADE CONTRATUAL)\".");
            }
            #endregion

            #region Valida Mudança Categoria

            //valida mudanças de categoria
            var listaMudancasCategoria = eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();

            foreach (var mudancaCategoria in listaMudancasCategoria)
            {
                listaErrosTemp.AddRange(ESocialF2500MudancaCategoriaService.ValidaErrosGlobais(eSocialDbContext, mudancaCategoria, infoContrato).ToList());
            }

            var ExisteMudancaCategoria = eSocialDbContext.EsF2500Mudcategativ.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
            if ((!string.IsNullOrEmpty(infoContrato!.InfocontrIndcateg) && infoContrato!.InfocontrIndcateg == "S" || !string.IsNullOrEmpty(infoContrato!.InfocontrIndnatativ) && infoContrato!.InfocontrIndnatativ == "S") && !ExisteMudancaCategoria)
            {
                listaErrosTemp.Add("Deve ser informado pelo menos uma \"Mudança de categoria e/ou natureza da atividade\" (Bloco H) caso o campo \"Categoria Diferente Contrato\" (Bloco D) ou o campo \"Natureza Diferente Contrato\" (Bloco D) estejam preenchidos com \"Sim\".");
            }

            #endregion

            #region valida bloco E
            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.InfocomplCodcbo))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Código CBO\" (Bloco D) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfocomplNatatividade.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Natureza da Atividade\" (Bloco D) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfovincDtadm.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Data de Admissão\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfovincTpregtrab.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Tipo de Regime Trabalhista\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfovincTpregprev.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Tipo de Regime Previdenciário\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfovincTmpparc.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Tipo de Contrato em Tempo Parcial\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.SucessaovincTpinsc.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Tipo de Inscrição da Sucessão do Vínculo\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.SucessaovincNrinsc))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Número de Inscrição da Sucessão do Vínculo\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.SucessaovincMatricant))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Matrícula Anterior\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.SucessaovincDttransf.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Data de Transferência\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.DuracaoTpcontr.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Tipo de Contrato\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.DuracaoDtterm.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Data de Término\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.DuracaoClauassec))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Contrato por Prazo Determinado Contém Cláusula Assecuratória\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.DuracaoObjdet))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Objeto Determinante da Contratação por Prazo Determinado\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfodesligDtdeslig.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Data do Desligamento\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && !string.IsNullOrEmpty(infoContrato!.InfodesligMtvdeslig))
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Motivo do Desligamento\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            if (infoContrato!.InfocontrIndcontr == "S" && infoContrato!.InfodesligDtprojfimapi.HasValue)
            {
                listaErrosTemp.Add($"Não deve ser informado o campo \"Data Projetada para o Término do Aviso Prévio Indenizado\" (Bloco E) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }


            //Se tipo contrato for igual a 8 os campos obrigatórios do InfoDeslig devem ser preenchidos

            if (infoContrato!.InfocontrIndcontr == "N" && infoContrato!.InfocontrTpcontr != ESocialTipoContratoTSVE.ResponsabilidadeIndireta.ToByte() && !infoContrato!.InfodesligDtdeslig.HasValue)
            {
                listaErrosTemp.Add($"Caso o campo \"Tipo de Contrato\" (Bloco D) esteja preenchido com valor diferente de \"8 - Responsabilidade Indireta.\" o campo \"Data do Desligamento\" (Bloco E) deve ser informado.");
            }

            if (infoContrato!.InfocontrIndcontr == "N" && infoContrato!.InfocontrTpcontr != ESocialTipoContratoTSVE.ResponsabilidadeIndireta.ToByte() && string.IsNullOrEmpty(infoContrato!.InfodesligMtvdeslig))
            {
                listaErrosTemp.Add($"Caso o campo \"Tipo de Contrato\" (Bloco D) esteja preenchido com valor diferente de \"8 - Responsabilidade Indireta.\" o campo \"Motivo do Desligamento\" (Bloco E) deve ser informado.");
            }

         
            #endregion

            #region valida bloco F
            if (infoContrato.InfocontrIndcontr == "S" && eSocialDbContext.EsF2500Observacoes.Any(x => x.IdEsF2500Infocontrato == infoContrato!.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"Não deve ser informado o grupo \"Observações\" (Bloco F) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
            }
            #endregion

            #region VALIDA RETIFICAÇÃO

            if (formulario.IdeeventoIndretif == ESocialIndRetif.Retificacao.ToByte())
            {

                if (infoContrato.InfocontrTpcontr == 8 //Responsabilidade indireta
                    && _eSocialDbContext.EsF2500Infocontrato.Include(x => x.IdF2500Navigation).Any(x => x.IdF2500Navigation.StatusFormulario != EsocialStatusFormulario.Excluido3500.ToByte() && x.IdF2500Navigation.CodParte == formulario.CodParte && x.IdF2500Navigation.CodProcesso == formulario.CodProcesso && x.IdF2500Navigation.IdeeventoIndretif == ESocialIndRetif.Original.ToByte() && x.InfocontrTpcontr != 8))
                {
                    listaErrosTemp.Add($"O formulário que está sendo retificado não foi cadastrado com um contrato do tipo \"8 - RESPONSABILIDADE INDIRETA\". Assim, a retificação também não pode ter um contrato do tipo 8");
                }

                if (infoContrato.InfocontrTpcontr != 8 //Responsabilidade indireta
                    && eSocialDbContext.EsF2500Infocontrato.Include(x => x.IdF2500Navigation).Any(x => x.IdF2500Navigation.StatusFormulario != EsocialStatusFormulario.Excluido3500.ToByte() && x.IdF2500Navigation.CodParte == formulario.CodParte && x.IdF2500Navigation.CodProcesso == formulario.CodProcesso && x.IdF2500Navigation.IdeeventoIndretif == ESocialIndRetif.Original.ToByte() && x.InfocontrTpcontr == 8))
                {
                    listaErrosTemp.Add($"O formulário que está sendo retificado foi cadastrado com um contrato do tipo \"8 - RESPONSABILIDADE INDIRETA\". Assim, a retificação também deve ter um contrato do tipo 8.");
                }

            }

            #endregion

            #endregion

            return listaErrosTemp;
        }

        public static async Task<IEnumerable<string>> ValidaErrosGlobais(ESocialDbContext context, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct)
        {
            if (contrato == null || contrato == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação do contrato.");
            }

            var requestDTO = new EsF2500InfocontratoRequestDTO()
            {
                InfocontrCodcateg = contrato.InfocontrCodcateg,
                InfocontrDtadmorig = contrato.InfocontrDtadmorig,
                InfocontrDtinicio = contrato.InfocontrDtinicio,
                InfocontrIndcateg = contrato.InfocontrIndcateg,
                InfocontrIndcontr = contrato.InfocontrIndcontr,
                InfocontrIndmotdeslig = contrato.InfocontrIndmotdeslig,
                InfocontrIndnatativ = contrato.InfocontrIndnatativ,
                InfocontrIndreint = contrato.InfocontrIndreint,
                InfocontrIndunic = contrato.InfocontrIndunic,
                InfocontrMatricula = contrato.InfocontrMatricula,
                InfocontrTpcontr = contrato.InfocontrTpcontr,
                InfocomplCodcbo = contrato.InfocomplCodcbo,
                InfocomplNatatividade = contrato.InfocomplNatatividade,
                InfotermDtterm = contrato.InfotermDtterm,
                InfotermMtvdesligtsv = contrato.InfotermMtvdesligtsv
            };

            return await ValidaErrosGlobais(context, requestDTO, contrato, formulario, ct, true);
        }

        private static async Task<IEnumerable<string>> ValidaErrosGlobais(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct, bool validacaoExterna = false)
        {
            var listaErrosGlobais = new List<string>();
            var existeContratos = false;

            var sufixo = validacaoExterna ? $"Contrato ID {contrato.IdEsF2500Infocontrato} - " : string.Empty;

            if (requestDTO.InfocontrIndcontr == "N")
            {
                if (contrato == null)
                {
                    existeContratos = await context.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.InfocontrIndcontr == "N", ct);
                }
                else
                {
                    existeContratos = await context.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.IdEsF2500Infocontrato != contrato.IdEsF2500Infocontrato && x.InfocontrIndcontr == "N", ct);
                }

                if (existeContratos)
                {
                    listaErrosGlobais.Add($"{sufixo}O formulário 2500 pode conter apenas um contrato com o campo \"Possui Inf. Evento Admissão/Início\" marcado como \"Não\".");
                }
            }

            if (requestDTO.InfocontrDtadmorig.HasValue && formulario.IdetrabDtnascto.HasValue && requestDTO.InfocontrDtadmorig.Value.Date <= formulario.IdetrabDtnascto.Value.Date)
            {
                listaErrosGlobais.Add($"{sufixo}\"Data de Admissão Original\" (Bloco D) deve ser maior que a \"Data Nascimento do Trabalhador\" (Bloco A).");
            }

            if (formulario.IderespNrinsc is not null && (requestDTO.InfocontrIndcontr is null || requestDTO.InfocontrIndcontr != "N"))
            {
                listaErrosGlobais.Add($"{sufixo}Preencha o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) com \"Não\" quando o \"Número de Inscrição do Responsável\" (Bloco A) estiver preenchido.");
            }

            //if (formulario.IderespNrinsc is not null && (requestDTO.InfocontrIndunic is null || requestDTO.InfocontrIndunic != "N"))
            //{
            //    listaErrosGlobais.Add($"{sufixo}Preencha o campo \"Unicidade Contratual\" (Bloco D) com \"Não\" quando o \"Número de Inscrição do Responsável\" (Bloco A) estiver preenchido.");
            //}

            if (requestDTO.InfocontrDtinicio.HasValue && formulario.IdetrabDtnascto.HasValue && requestDTO.InfocontrDtinicio.Value.Date <= formulario.IdetrabDtnascto.Value.Date)
            {
                listaErrosGlobais.Add($"{sufixo}\"Data de Início de TSVE\" (Bloco D) deve ser maior que a \"Data Nascimento do Trabalhador\" (Bloco A).");
            }

            if (contrato.InfovincTpregtrab != 1 && requestDTO.InfocontrCodcateg == 104)
            {
                listaErrosGlobais.Add($"{sufixo}Quando a \"Categoria\"  (Bloco D) for igual \"104 - Empregado - Doméstico\", o \"Tipo de Regime Trabalhista\" (Bloco E) deve ser \"CLT\".");
            }

            if (contrato.InfovincTpregprev != 1 && requestDTO.InfocontrCodcateg == 104)
            {
                listaErrosGlobais.Add($"{sufixo}Quando a \"Categoria\"  (Bloco D) for igual \"104 - Empregado - Doméstico\", o Tipo de Regime Previdenciário (Bloco E) deve ser \"1 - Regime Geral de Previdência Social - RGPS.\".");
            }

            if (!string.IsNullOrEmpty(formulario.IderespNrinsc) && requestDTO.InfocontrTpcontr != 8 && requestDTO.InfocontrTpcontr != 6)
            {
                listaErrosGlobais.Add($"{sufixo}\"Quando os dados do Real Empregador (Bloco A) estiverem preenchidos, o \"Tipo de Contrato\" (Bloco D) deve ser \"8 - RESPONSABILIDADE INDIRETA\" ou \"6 - TRABALHADOR SEM VÍNCULO DE EMPREGO/ESTATUTÁRIO (TSVE), SEM RECONHECIMENTO DE VÍNCULO EMPREGATÍCIO\".");
            }

            return listaErrosGlobais;
        }

        private static async Task<IEnumerable<string>> ValidaErrosInclusao(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct, bool validacaoExterna = false)
        {
            var listaErrosInclusao = new List<string>();

            if (!string.IsNullOrEmpty(requestDTO.InfocontrMatricula))
            {
                if (requestDTO.InfocontrCodcateg.HasValue && requestDTO.InfocontrDtinicio.HasValue)
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg && x.InfocontrDtinicio!.Value.Date == requestDTO.InfocontrDtinicio.Value.Date, ct))
                    {
                        listaErrosInclusao.Add("Matrícula, Categoria e Data de Início TSVE já informados.");
                    }
                }
                else if (requestDTO.InfocontrCodcateg.HasValue)
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg, ct))
                    {
                        listaErrosInclusao.Add("Matrícula e Categoria já informados.");
                    }
                }
                else
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula, ct))
                    {
                        listaErrosInclusao.Add("Matrícula já informada.");
                    }
                }
            }
            else if (requestDTO.InfocontrCodcateg.HasValue && requestDTO.InfocontrDtinicio.HasValue)
            {
                if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdF2500 == formulario.IdF2500 && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg && x.InfocontrDtinicio!.Value.Date == requestDTO.InfocontrDtinicio.Value.Date, ct))
                {
                    listaErrosInclusao.Add("Categoria e Data de Início de TSVE já informados");
                }
            }

            return listaErrosInclusao;
        }

        private static async Task<IEnumerable<string>> ValidaErrosAlteracao(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, int codigoInfoContrato, CancellationToken ct, bool validacaoExterna = false)
        {
            var listaErrosAlteracao = new List<string>();

            if (!string.IsNullOrEmpty(requestDTO.InfocontrMatricula))
            {
                if (requestDTO.InfocontrCodcateg.HasValue && requestDTO.InfocontrDtinicio.HasValue)
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato != codigoInfoContrato && x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg && x.InfocontrDtinicio!.Value.Date == requestDTO.InfocontrDtinicio.Value.Date, ct))
                    {
                        listaErrosAlteracao.Add("Matrícula, Categoria e Data de Início TSVE já informados.");
                    }
                }
                else if (requestDTO.InfocontrCodcateg.HasValue)
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato != codigoInfoContrato && x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg, ct))
                    {
                        listaErrosAlteracao.Add("Matrícula e Categoria já informados.");
                    }
                }
                else
                {
                    if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato != codigoInfoContrato && x.IdF2500 == formulario.IdF2500 && x.InfocontrMatricula == requestDTO.InfocontrMatricula, ct))
                    {
                        listaErrosAlteracao.Add("Matrícula já informada.");
                    }
                }
            }
            else if (requestDTO.InfocontrCodcateg.HasValue && requestDTO.InfocontrDtinicio.HasValue)
            {
                if (await context.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato != codigoInfoContrato && x.IdF2500 == formulario.IdF2500 && x.InfocontrCodcateg == requestDTO.InfocontrCodcateg && x.InfocontrDtinicio!.Value.Date == requestDTO.InfocontrDtinicio.Value.Date, ct))
                {
                    listaErrosAlteracao.Add("Categoria e Data de Início de TSVE já informados");
                }
            }

            return listaErrosAlteracao;
        }

        public async Task<string> CalculaDiferencaMesesCompetencia(int codigoFormulario, ESocialDbContext eSocialDbContext, CancellationToken ct)
        {
            var listaContratosRetornos = new List<string>();
            var mensagemRetorno = string.Empty;

            var listaContratos = await eSocialDbContext.EsF2500Infocontrato.AsNoTracking().Where(x => x.IdF2500 == codigoFormulario).ToListAsync(ct);

            foreach (var contrato in listaContratos)
            {

                var anoFinal = int.Parse(contrato.InfovlrCompfim.Substring(0, 4));
                var mesFinal = int.Parse(contrato.InfovlrCompfim.Substring(4));

                var anoInicial = int.Parse(contrato.InfovlrCompini.Substring(0, 4));
                var mesInicial = int.Parse(contrato.InfovlrCompini.Substring(4));


                // Total de meses = diferença de anos * 12 + diferença de meses
                int totalMeses = (anoFinal - anoInicial) * 12 + mesFinal - mesInicial + 1;

                var qtdPerBaseCalc = await eSocialDbContext.EsF2500Ideperiodo.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato).CountAsync();

                if (totalMeses > 0 && totalMeses != qtdPerBaseCalc && (contrato.InfovlrIndreperc == 1 || contrato.InfovlrIndreperc == 5))
                    listaContratosRetornos.Add($"ID {contrato.IdEsF2500Infocontrato}");
            }

            if (listaContratosRetornos.Count > 0)
            {
                var plural = listaContratosRetornos.Count > 1 ? "s" : string.Empty;
                mensagemRetorno = $"Número de bases de cálculo informadas (bloco K) menor do que a quantidade de meses informados entre as datas de competência inicial e final (bloco J) no{plural} contrato{plural} a seguir: {String.Join(",", listaContratosRetornos)}. Deseja continuar?";
            }

            return mensagemRetorno;
        }


        #region Funções Auxiliares

        public async Task<bool> ExisteContratoPorIdAsync(long codigoContrato, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);
        }

        public async Task<EsF2500Infocontrato?> RetornaContratoPorIdAsync(int codigoContrato, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato, ct);
        }

        #endregion

    }
}
