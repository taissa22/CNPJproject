using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
{
    public class ESocialF2500InfoContratoService
    {
        public async Task<IEnumerable<string>> ValidaInclusaoInfoContrato(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct)
        {
            var listaErros = new List<string>();

            listaErros.AddRange((await ValidaErrosGlobais(context, requestDTO, contrato, formulario, ct)).ToList());

            return listaErros;
           
        }

        public async Task<IEnumerable<string>> ValidaAlteracaoInfoContrato(ESocialDbContext context, EsF2500InfocontratoRequestDTO requestDTO, EsF2500Infocontrato contrato, EsF2500 formulario, CancellationToken ct)
        {
            var listaErros = new List<string>();            

            listaErros.AddRange((await ValidaErrosGlobais(context, requestDTO, contrato, formulario, ct)).ToList());

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

            if (requestDTO.InfovlrCompini.HasValue && requestDTO.InfovincDtadm.HasValue && infoContrato.InfocontrTpcontr.HasValue && (infoContrato.InfocontrTpcontr == 1 || infoContrato.InfocontrTpcontr == 3))
            {
                if (requestDTO.InfovlrCompini!.Value.Date < new DateTime(requestDTO.InfovincDtadm!.Value.Date.Year, requestDTO.InfovincDtadm.Value.Date.Month, 1).Date)
                {
                    listaErrosTemp.Add("Se o \"Tipo do Contrato\" (Bloco D) for igual a \"1\" ou \"3\", o campo \"Competência Inicial\" (Bloco J) deve ser maior ou igual ao mês/ano informado na \"Data de Admissão\" (Bloco E).");
                }
            }

            if (requestDTO.InfovlrCompini.HasValue && requestDTO.InfovincDtadm.HasValue && infoContrato.InfocontrTpcontr.HasValue && (infoContrato.InfocontrTpcontr == 2 || infoContrato.InfocontrTpcontr == 4 || infoContrato.InfocontrTpcontr == 5))
            {
                if (requestDTO.InfovlrCompini.Value.Date != new DateTime(requestDTO.InfovincDtadm.Value.Date.Year, requestDTO.InfovincDtadm.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("Se o \"Tipo do Contrato\" (Bloco D) for igual a igual a \"2\" ou \"4\" ou \"5\", o campo \"Competência Inicial\" (Bloco J) deve ser igual ao mês/ano da \"Data de Admissão\" (Bloco E).");
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
                if ((formulario.InfoprocjudDtsent.HasValue && requestDTO.InfovlrCompfim.Value.Date > (new DateTime(formulario.InfoprocjudDtsent!.Value.Date.Year, formulario.InfoprocjudDtsent.Value.Date.Month, 1)))
                    || (formulario.InfoccpDtccp.HasValue && requestDTO.InfovlrCompfim.Value.Date > new DateTime(formulario.InfoccpDtccp!.Value.Date.Year, formulario.InfoccpDtccp.Value.Date.Month, 1)))
                {
                    listaErrosTemp.Add("O período da \"Competência Final\" no (Bloco J) deve ser menor ou igual ao mês/ano da \"Data da Sentença ou Homologação do Acordo do Processo Judicial\" do (Bloco B).");
                }
            }

            if (requestDTO.InfovlrCompfim.HasValue && requestDTO.InfodesligDtdeslig.HasValue && infoContrato.InfocontrTpcontr.HasValue && (infoContrato.InfocontrTpcontr == 1 || infoContrato.InfocontrTpcontr == 2))
            {
                if (requestDTO.InfovlrCompfim!.Value.Date > new DateTime(requestDTO.InfodesligDtdeslig!.Value.Date.Year, requestDTO.InfodesligDtdeslig.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("Se o \"Tipo de Contrato\" (Bloco D) for igual a \"1\" ou \"2\", o campo \"Competência Final\" (Bloco J) deve ser preenchido com o valor menor ou igual ao mês/ano da \"Data de Desligamento\", caso tenha sido informada.");
                }
            }

            if (requestDTO.InfovlrCompfim.HasValue && requestDTO.InfodesligDtdeslig.HasValue && infoContrato.InfocontrTpcontr.HasValue
                && (infoContrato.InfocontrTpcontr == 3 || infoContrato.InfocontrTpcontr == 4 || infoContrato.InfocontrTpcontr == 5)
                && requestDTO.InfovlrCompfim!.Value.Date != new DateTime(requestDTO.InfodesligDtdeslig!.Value.Date.Year, requestDTO.InfodesligDtdeslig.Value.Date.Month, 1))
            {
                listaErrosTemp.Add("Se o \"Tipo de Contrato\" no (Bloco D) for igual a \"3\", \"4\" ou \"5\", o campo \"Competência Final\" (Bloco J) deve ser preenchido com o valor igual ao mês/ano da \"Data de Desligamento\" (Bloco E), caso tenha sido informada.");
            }

            if (requestDTO.InfovlrCompfim.HasValue && infoContrato.InfocontrTpcontr.HasValue && infoContrato.InfotermDtterm.HasValue && infoContrato.InfocontrTpcontr == 6)
            {
                if (requestDTO.InfovlrCompfim.Value.Date > new DateTime(infoContrato.InfotermDtterm!.Value.Date.Year, infoContrato.InfotermDtterm!.Value.Date.Month, 1))
                {
                    listaErrosTemp.Add("Se o valor informado no campo \"Tipo de contrato\" no (Bloco D) for igual a \"6\", o campo \"Competência Final\" (Bloco J) deve ser preenchido com o período menor ou igual ao mês/ano da \"Data de Término do TSVE\" (Bloco D), caso tenha sido informada.");
                }
            }

            #region Valida Remuneracao                 

            //valida remunerações
            var listaRemuneracoes = eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var remuneracao in listaRemuneracoes)
            {
                listaErrosTemp.AddRange(ESocialF2500RemuneracaoService.ValidaErrosGlobais(remuneracao, infoContrato).ToList());
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

            #region Bases de Cálculo de Contribuição Previdenciária e FGTS 

            //valida periodo base de calculo

            var listaPeriodos = eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var periodo in listaPeriodos)
            {
                listaErrosTemp.AddRange(ESocialF2500IdePeriodoService.ValidaErrosGlobais(periodo, infoContrato).ToList());
            }

            if (infoContrato.InfovlrRepercproc == ESocialRepercussaoProcesso.DecisaoComPagamento.ToByte() && !eSocialDbContext.EsF2500Ideperiodo.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"É obrigatório informar o grupo \"Bases de Cálculo de Contribuição Previdenciária e FGTS\" (Bloco K) quando o campo \"Repercussão do Processo\" (Bloco J) estiver preenchido com \"1- Decisão com Pagamento de Verbas de Natureza Remuneratória\"");
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

            if ((requestDTO.InfovincDtadm.HasValue && formulario!.IdetrabDtnascto.HasValue) && (requestDTO.InfovincDtadm.Value <= formulario.IdetrabDtnascto.Value))
            {
                listaErrosTemp.Add("A \"Data de Admissão\" deve ser maior que a \"Data de Nascimento\" do trabalhador.");
            }

            if (String.IsNullOrEmpty(requestDTO.InfodesligMtvdeslig) && infoContrato.InfocontrCodcateg.HasValue && infoContrato.InfocontrCodcateg.Value == 721)
            {//infodesligMtvdeslig
                listaErrosTemp.Add("O campo \"Motivo do Desligamento\" é  obrigatório quando a \"Categoria\" (Bloco D) estiver preenchido com o código 721");
            }

            if (requestDTO.SucessaovincTpinsc.HasValue && requestDTO.SucessaovincTpinsc == 5 && (requestDTO.SucessaovincDttransf!.Value.Date > new DateTime(1999, 6, 30).Date))
            {
                listaErrosTemp.Add("Só informe \"5 - CGC\" para o campo Tipo de Inscrição da Sucessão do Vínculo, se a Data da Transferência for igual ou menor a 30/06/1999.");
            }

            #region Valida unicidade

            //valida unicidade contratual

            var listaUnicidades = eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();
            foreach (var unicidade in listaUnicidades)
            {
                listaErrosTemp.AddRange(ESocialF2500UnicContrService.ValidaErrosGlobais(unicidade, infoContrato).ToList());
            }

            var ExisteUnicidade = eSocialDbContext.EsF2500Uniccontr.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
            if (!string.IsNullOrEmpty(infoContrato!.InfocontrIndunic) && infoContrato!.InfocontrIndunic == "S" && !ExisteUnicidade)
            {
                listaErrosTemp.Add("Os campos do \"Bloco I\" são de preenchimento obrigatório, quando o valor informado no campo \"Unicidade contratual\" (Bloco D) for igual a \"Sim\".");
            }
            #endregion

            #region Valida Mudança Categoria

            //valida mudanças de categoria
            var listaMudancasCategoria = eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato).ToList();

            foreach (var mudancaCategoria in listaMudancasCategoria)
            {
                listaErrosTemp.AddRange(ESocialF2500MudancaCategoriaService.ValidaErrosGlobais(mudancaCategoria, infoContrato).ToList());
            }

            var ExisteMudancaCategoria = eSocialDbContext.EsF2500Mudcategativ.Any(x => x.IdEsF2500Infocontrato == infoContrato.IdEsF2500Infocontrato);
            if (((!string.IsNullOrEmpty(infoContrato!.InfocontrIndcateg) && infoContrato!.InfocontrIndcateg == "S") || (!string.IsNullOrEmpty(infoContrato!.InfocontrIndnatativ) && infoContrato!.InfocontrIndnatativ == "S")) && !ExisteMudancaCategoria)
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
            #endregion

            #region valida bloco F
            if (infoContrato.InfocontrIndcontr == "S" && eSocialDbContext.EsF2500Observacoes.Any(x => x.IdEsF2500Infocontrato == infoContrato!.IdEsF2500Infocontrato))
            {
                listaErrosTemp.Add($"Não deve ser informado o grupo \"Observações\" (Bloco F) caso o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) esteja preenchido com \"Sim\".");
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

            var sufixo = validacaoExterna ? $"Contrato ID {contrato.IdEsF2500Infocontrato} - " : string.Empty;
                        

            if (requestDTO.InfocontrDtadmorig.HasValue && formulario.IdetrabDtnascto.HasValue && (requestDTO.InfocontrDtadmorig.Value.Date <= formulario.IdetrabDtnascto.Value.Date))
            {
                listaErrosGlobais.Add($"{sufixo}\"Data de Admissão Original\" (Bloco D) deve ser maior que a \"Data Nascimento do Trabalhador\" (Bloco A).");
            }

            if (formulario.IderespNrinsc is not null && (requestDTO.InfocontrIndcontr is null || requestDTO.InfocontrIndcontr != "N"))
            {
                listaErrosGlobais.Add($"{sufixo}Preencha o campo \"Possui Inf. Evento Admissao/Inicio\" (Bloco D) com \"Não\" quando o \"Número de Inscrição do Responsável\" (Bloco A) estiver preenchido.");
            }

            if (formulario.IderespNrinsc is not null && (requestDTO.InfocontrIndunic is null || requestDTO.InfocontrIndunic != "N"))
            {
                listaErrosGlobais.Add($"{sufixo}Preencha o campo \"Unicidade Contratual\" (Bloco D) com \"Não\" quando o \"Número de Inscrição do Responsável\" (Bloco A) estiver preenchido.");
            }

            if (requestDTO.InfocontrDtinicio.HasValue && formulario.IdetrabDtnascto.HasValue && (requestDTO.InfocontrDtinicio.Value.Date <= formulario.IdetrabDtnascto.Value.Date))
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

            if (!string.IsNullOrEmpty(formulario.IderespNrinsc) && requestDTO.InfocontrTpcontr != 8)
            {
                listaErrosGlobais.Add($"{sufixo}\"Quando os dados do Real Empregador (Bloco A) estiverem preenchidos, o \"Tipo de Contrato\" (Bloco D) deve ser \"8 - RESPONSABILIDADE INDIRETA\".");
            }

            return listaErrosGlobais;
        }

       
    }
}
