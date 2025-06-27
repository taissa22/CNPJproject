using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Services;
using Perlink.Oi.Juridico.Infra.External;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501Service
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialF2501Service(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        public async Task<bool> ExisteFormularioPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501.AsNoTracking().AnyAsync(x => x.IdF2501 == codigoFormulario, ct);
        }

        public async Task<bool> ExisteFormularioComStatusPorIdAsync(int codigoFormulario, EsocialStatusFormulario status, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario == status.ToByte(), ct);
        }

        public async Task<bool> FormularioPodeSerSalvoPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario && x.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte(), ct);
        }

        public async Task<EsF2501?> RetornaFormularioPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);
        }

        public async Task<(bool alteradoComSucesso, string MensagemErro)> AlteraNumeroReciboAsync(int codigoFormulario, string? numeroRecibo, ClaimsPrincipal? user, CancellationToken ct)
        {
            var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

            if (formulario2501!.StatusFormulario != EsocialStatusFormulario.RetornoOkSemRecibo.ToByte() && (formulario2501.OkSemRecibo == "N" || string.IsNullOrEmpty(formulario2501.OkSemRecibo)))
            {
                return (false, "O Número do recibo não pode ser alterado pois o formulário não se encontra no status correto.");
            }

            if (formulario2501 is not null)
            {
                if (!string.IsNullOrEmpty(numeroRecibo))
                {
                    formulario2501.StatusFormulario = EsocialStatusFormulario.RetornoESocialOk.ToByte();
                    formulario2501.IdeeventoNrrecibo = numeroRecibo;
                    formulario2501.OkSemRecibo = "S";
                    formulario2501.LogCodUsuario = user!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                }
                else
                {
                    formulario2501.StatusFormulario = EsocialStatusFormulario.RetornoOkSemRecibo.ToByte();
                    formulario2501.IdeeventoNrrecibo = null;
                    formulario2501.LogCodUsuario = user!.Identity!.Name;
                    formulario2501.LogDataOperacao = DateTime.Now;
                }

            }

            if (formulario2501!.StatusFormulario != EsocialStatusFormulario.RetornoOkSemRecibo.ToByte() && (formulario2501.OkSemRecibo == "N" || string.IsNullOrEmpty(formulario2501.OkSemRecibo)))
            {
                return (false, "O Número do recibo não pode ser alterado pois o formulário não se encontra no status correto.");
            }

            await _eSocialDbContext.SaveChangesAsync(ct);

            return (true, string.Empty);
        }

        public async Task<IEnumerable<string>> FilhosInvalido(ESocialDbContext eSocialDbContext, int codigoFormulario, CancellationToken ct)
        {
            var mensagensErro = new List<string>();

            var formulario2501 = await _eSocialDbContext.EsF2501.FirstOrDefaultAsync(x => x.IdF2501 == codigoFormulario, ct);

            //Tento pegar a retificadore se tiver:
            var query = _eSocialDbContext.EsF2500
                .AsNoTracking()
                .Include(x => x.LogCodUsuarioNavigation)
                .Where(x => x.CodParte == formulario2501!.CodParte
                    && x.CodProcesso == formulario2501.CodProcesso
                    && (!new[] { 0, 12, 13, 14, 15, 16, 17 }.Contains(x.StatusFormulario)));

            var listaFormularios2500 = await query.ToListAsync(ct);
            var formulario2500 = listaFormularios2500.MaxBy(x => x.IdF2500)!;

            if (formulario2500 is null)
            {
                mensagensErro.Add($"Para finalizar este formulário é necessário que o formulário 2500 tenha sido iniciado.");
                return mensagensErro;
            }

            var listaFormulario2501Infocrirrf = await eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == codigoFormulario).ToListAsync(ct);
            var listaFormulario2501Calctrib = await _eSocialDbContext.EsF2501Calctrib.AsNoTracking().Where(x => x.IdEsF2501 == codigoFormulario).ToListAsync(ct);
            if (listaFormulario2501Infocrirrf.Any())
            {
                foreach (var infoCrIrrf in listaFormulario2501Infocrirrf)
                {
                    if (infoCrIrrf.InfocrcontribTpcr == 188951 && (string.IsNullOrEmpty(infoCrIrrf.InforraDescrra) || !infoCrIrrf.InforraQtdmesesrra.HasValue))
                    {
                        mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está com o código de receita 188951 - IRRF - RRA. Assim, os campos Descrição dos Rendimentos Recebidos Acumuladamente RRA e Nº de Meses RRA são de preenchimento obrigatório. Favor verificar clicando no botão de edição da linha Informações Complementares - Rendimentos Recebidos Acumuladamente RRA.");
                    }

                    if (infoCrIrrf.InfocrcontribTpcr != 188951 && (!string.IsNullOrEmpty(infoCrIrrf.InforraDescrra) || infoCrIrrf.InforraQtdmesesrra.HasValue))
                    {
                        mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está com o código de receita diferente de 188951 - IRRF - RRA. Assim, os campos Descrição dos Rendimentos Recebidos Acumuladamente RRA e Nº de Meses RRA não devem ser preenchidos. Favor verificar clicando no botão de edição da linha Informações Complementares - Rendimentos Recebidos Acumuladamente RRA.");
                    }

                    if (infoCrIrrf.DespprocjudVlrdespadvogados.HasValue)
                    {
                        if (!eSocialDbContext.EsF2501Ideadv.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf))
                        {
                            mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está com valor de despesas com advogados informado. Assim, pelo menos um advogado deve ser identificado com CPF ou CNPJ. Favor verificar clicando no botão de edição da linha Informações Complementares - Rendimentos Recebidos Acumuladamente RRA.");
                        }
                    }
                    if (!infoCrIrrf.DespprocjudVlrdespadvogados.HasValue || infoCrIrrf.DespprocjudVlrdespadvogados == 0)
                    {
                        if (eSocialDbContext.EsF2501Ideadv.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf))
                        {

                            mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está sem valor de despesa com advogados informado. Assim, os advogados não podem ser registrados. Favor verificar clicando no botão de edição da linha Informações Complementares - Rendimentos Recebidos Acumuladamente RRA.");
                        }
                    }


                    if (infoCrIrrf.InfocrcontribTpcr == 188951 && eSocialDbContext.EsF2501Deddepen.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf))
                    {
                        mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está com o código de receita 188951 - IRRF - RRA. Assim, não podem ser registrados dependentes. Favor verificar clicando no botão de edição da linha Dedução do Rendimento Tributável - Dependentes.");
                    }

                    if (infoCrIrrf.InfocrcontribTpcr == 188951 && eSocialDbContext.EsF2501Infoprocret.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf))
                    {
                        mensagensErro.Add($"O ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF está com o código de receita 188951 - IRRF - RRA. Assim, não podem ser registrados processos relacionados não retenção de tributos. Favor verificar clicando no botão de edição da linha Processos Relacionados a não Retenção de Tributos ou a Depósitos Judiciais.");
                    }
                    else if (eSocialDbContext.EsF2501Infoprocret.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf))
                    {
                        var infoProcRet = _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf);

                        if (infoProcRet.Any())
                        {
                            var contador = 0;
                            foreach (var item in infoProcRet)
                            {
                                var infoValores = _eSocialDbContext.EsF2501Infovalores.AsNoTracking().Where(x => x.IdEsF2501Infoprocret == item.IdEsF2501Infoprocret);
                                if (infoValores.Any())
                                {
                                    foreach (var itemValor in infoValores)
                                    {
                                        if (item.InfoprocretTpprocret != 2 && itemValor.InfovaloresVlrcmpanoant.HasValue && itemValor.InfovaloresVlrcmpanocal.HasValue && contador == 0)
                                        {
                                            mensagensErro.Add("Os campos Valor Compensação Ano Calendário e Valor Compensação Anos Anteriores só podem ser preenchidos para processos judiciais. Favor verificar.");
                                            contador++;
                                        }

                                        if (itemValor.InfovaloresIndapuracao == 1 && infoCrIrrf.InfoirVrrendtrib < itemValor.InfovaloresVlrrendsusp)
                                        {
                                            mensagensErro.Add("O valor do rendimento com exigibilidade suspensa não pode ser maior do que o valor do rendimento tributável mensal do IR. Favor verificar.");
                                        }

                                        if (itemValor.InfovaloresIndapuracao == 2 && infoCrIrrf.InfoirVrrendtrib13 < itemValor.InfovaloresVlrrendsusp)
                                        {
                                            mensagensErro.Add("O valor do rendimento com exigibilidade suspensa não pode ser maior do que o valor do rendimento tributável do IR (13º).  Favor verificar.");
                                        }

                                        var dedSusp = _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == itemValor.IdEsF2501Infovalores);
                                        if (dedSusp.Any())
                                        {
                                            var qtdMensRendsusp = 0;
                                            var qtdMensVlrDed = 0;

                                            foreach (var itemDedSusp in dedSusp)
                                            {
                                                decimal? somaVlrDed = 0;

                                                if ((!itemValor.InfovaloresVlrrendsusp.HasValue || itemValor.InfovaloresVlrrendsusp == 0) && qtdMensRendsusp == 0)
                                                {
                                                    mensagensErro.Add("O valor da dedução só pode ser preenchido se o valor do rendimento com exigibilidade suspensa for maior do que zero. Favor verificar.");
                                                    qtdMensRendsusp += 1;
                                                }

                                                var benefPen = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == itemDedSusp.IdEsF2501Dedsusp);

                                                if (benefPen.Any())
                                                {
                                                    foreach (var itemBebefPen in benefPen)
                                                    {
                                                        somaVlrDed += itemBebefPen.BenefpenVlrdepensusp;
                                                    }
                                                }

                                                if (benefPen.Any() && itemDedSusp.DedsuspVlrdedsusp != somaVlrDed && (itemDedSusp.DedsuspIndtpdeducao == 5 || itemDedSusp.DedsuspIndtpdeducao == 7) && qtdMensVlrDed == 0)
                                                {
                                                    mensagensErro.Add("O valor da dedução do quadro Detalhamento das Deduções tem que ser o somatório dos valores da dedução dos dependentes/beneficiários. Favor verificar.");
                                                    qtdMensVlrDed += 1;
                                                }
                                            }
                                        }


                                    }

                                }
                            }
                        }
                    }

                    
                    if (infoCrIrrf.InfocrcontribTpcr == 188951 && eSocialDbContext.EsF2501Penalim.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf && x.PenalimTprend != ESocialTipoRendimento.RRA.ToByte()))
                    {
                        mensagensErro.Add($"ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF. Se o Código de Receita – CR relativo a Imposto de Renda Retido na Fonte for igual a \"188951 - IRRF - RRA\" (Bloco E), obrigatoriamente e exclusivamente o campo Tipo de Rendimento (Pensão Alimentícia - Beneficiários) deverá ser de preenchimento com o valor 18 - RRA.");
                    }

                    if (infoCrIrrf.InfocrcontribTpcr != 188951 && eSocialDbContext.EsF2501Penalim.Any(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf && x.PenalimTprend == ESocialTipoRendimento.RRA.ToByte()))
                    {
                        mensagensErro.Add($"ID {infoCrIrrf.IdEsF2501Infocrirrf} do IRRF. Somente informe o Tipo de Rendimento igual a \"18 - RRA\" (Pensão Alimentícia - Beneficiários) quando o campo Código Receita (CR) IRRF (Bloco E) estiver preenchido com o valor \"188951 - IRRF - RRA\".");
                    }
                }

            }

            if (listaFormulario2501Calctrib.Any())
            {
                foreach (var Calctrib in listaFormulario2501Calctrib)
                {
                    var contrato = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == formulario2500!.IdF2500, ct);

                    if (contrato is null)
                    {
                        mensagensErro.Add("Para finalizar este formulário é necessário pelo menos um \"Contrato\" (Bloco D) cadastrado no formulário 2500.");
                        return mensagensErro;
                    }

                    if (!_eSocialDbContext.EsF2500Ideperiodo.Any(x => x.IdEsF2500Infocontrato == contrato!.IdEsF2500Infocontrato && x.IdeperiodoPerref == Calctrib.CalctribPerref))
                    {
                        var periodoRef = Calctrib.CalctribPerref.Split("-");
                        mensagensErro.Add($"O período de referência {periodoRef[1]}/{periodoRef[0]} precisa constar no bloco K do formulário 2500 desse reclamante. Favor verificar essa integridade antes de enviar o formulário 2501 para o eSocial.");
                    }
                }
            }


            if (formulario2500!.InfoprocjudDtsent.HasValue)
            {
                var ano = formulario2501!.IdeprocPerapurpgto.Substring(0, 4);
                var mes = formulario2501!.IdeprocPerapurpgto.Substring(4, 2);
                var dataPeriodoApuracao = new DateTime(int.Parse(ano), int.Parse(mes), 1);
                var dataSentenca = new DateTime(formulario2500!.InfoprocjudDtsent!.Value.Year, formulario2500!.InfoprocjudDtsent!.Value.Month, 1);

                if (dataPeriodoApuracao < dataSentenca)
                {
                    mensagensErro.Add("O período de apuração precisa ser maior ou igual ao mês da sentença ou homologação do acordo informado no formulário 2500 desse reclamante. Favor verificar essa informação antes de enviar o formulário 2501 para o eSocial.");
                }
            } else
            {
                mensagensErro.Add("Para finalizar este formulário 2501 é necessário informar a \"Data de Sentença\" no formulário 2500.");
            }

            return mensagensErro;
        }

    }
}
