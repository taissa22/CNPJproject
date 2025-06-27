using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500InfocontratoRequestDTO
    {
        public byte? InfocontrTpcontr { get; set; }
        public string? InfocontrIndcontr { get; set; }
        public DateTime? InfocontrDtadmorig { get; set; }
        public string? InfocontrIndreint { get; set; }
        public string? InfocontrIndcateg { get; set; }
        public string? InfocontrIndnatativ { get; set; }
        public string? InfocontrIndmotdeslig { get; set; }
        public string? InfocontrIndunic { get; set; }
        public string? InfocontrMatricula { get; set; }
        public short? InfocontrCodcateg { get; set; }
        public DateTime? InfocontrDtinicio { get; set; }
        public string? InfocomplCodcbo { get; set; }
        public byte? InfocomplNatatividade { get; set; }
        public byte? InfovincTpregtrab { get; set; }
        public byte? InfovincTpregprev { get; set; }
        public DateTime? InfovincDtadm { get; set; }
        public decimal? InfovincTmpparc { get; set; }
        public byte? DuracaoTpcontr { get; set; }
        public DateTime? DuracaoDtterm { get; set; }
        public string? DuracaoClauassec { get; set; }
        public string? DuracaoObjdet { get; set; }
        public byte? SucessaovincTpinsc { get; set; }
        public string? SucessaovincNrinsc { get; set; }
        public string? SucessaovincMatricant { get; set; }
        public DateTime? SucessaovincDttransf { get; set; }
        public DateTime? InfodesligDtdeslig { get; set; }
        public string? InfodesligMtvdeslig { get; set; }
        public DateTime? InfodesligDtprojfimapi { get; set; }
        public DateTime? InfotermDtterm { get; set; }
        public string? InfotermMtvdesligtsv { get; set; }
        public byte? IdeestabTpinsc { get; set; }
        public string? IdeestabNrinsc { get; set; }
        public DateTime? InfovlrCompini { get; set; }
        public DateTime? InfovlrCompfim { get; set; }
        public decimal? InfovlrVrremun { get; set; }
        public decimal? InfovlrVrapi { get; set; }
        public decimal? InfovlrVr13api { get; set; }
        public decimal? InfovlrVrinden { get; set; }
        public decimal? InfovlrVrbaseindenfgts { get; set; }
        public string? InfovlrPagdiretoresc { get; set; }
        public decimal? InfovincPensalim { get; set; }
        public decimal? InfovincPercaliment { get; set; }
        public decimal? InfovincVralim { get; set; }
        public bool? InfovlrIdensd { get; set; }
        public bool? InfovlrIdenabono { get; set; }
        public byte? InfovlrIndreperc { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (!InfocontrTpcontr.HasValue || InfocontrTpcontr.HasValue && InfocontrTpcontr.Value < 0)
            {
                mensagensErro.Add("O campo tipo de contrato é obrigatório.");
            }

            if (InfocontrTpcontr.HasValue && (InfocontrTpcontr.Value <= 0 || InfocontrTpcontr.Value > 9))
            {
                mensagensErro.Add("O campo tipo de contrato inválido.");
            }

            if (InfocontrTpcontr.HasValue && InfocontrTpcontr.Value != 2 && InfocontrTpcontr.Value != 4 && InfocontrDtadmorig.HasValue)
            {
                mensagensErro.Add("A Data de admissão original não deve ser informada se o Tipo de Contrato for diferente de \"2-Trabalhador com vínculo formalizado, com alteração na data de admissão\" ou \"4-Trabalhador com vínculo formalizado, com alteração nas datas de admissão e de desligamento\".");
            }

            if (InfocontrDtinicio.HasValue && !string.IsNullOrEmpty(InfocontrMatricula) && !string.IsNullOrEmpty(InfocontrIndcontr) && InfocontrIndcontr == "S")
            {
                mensagensErro.Add("A Data de início de TSVE não deve ser informada se o campo \"Possui inf Evento de admissão/Início\" estiver preenchido com \"Sim\" e se o campo \"Matrícula\" tiver sido informado.");
            }

            if (InfocontrDtinicio.HasValue && InfocontrTpcontr.HasValue && InfocontrTpcontr.Value != 6)
            {
                mensagensErro.Add("A Data de Início de TSVE não deve ser informada se o Tipo de Contrato for diferente de \"6-Trabalhador sem vínculo de emprego/estatutário (TSVE), sem reconhecimento de vínculo empregatício\".");
            }

            if (!string.IsNullOrEmpty(InfocontrIndreint) && InfocontrTpcontr.HasValue && InfocontrTpcontr.Value == 6)
            {
                mensagensErro.Add("O campo \"Reintegração\" não deve ser informado se o Tipo de Contrato for igual a \"6-Trabalhador sem vínculo de emprego/estatutário (TSVE), sem reconhecimento de vínculo empregatício\".");
            }

            if (!string.IsNullOrEmpty(InfocontrIndreint) && !string.IsNullOrEmpty(InfocontrIndcontr) && InfocontrIndcontr == "N")
            {
                mensagensErro.Add("O campo Reintegração não deve ser informado se o campo \"Possui inf Evento de admissão/Início\" estiver preenchido com \"Não\".");
            }

            if (InfocontrIndcontr is null)
            {
                mensagensErro.Add("O campo indicativo se o contrato possui informação no evento S-2190, S-2200 ou S-2300 no declarante é obrigatório.");
            }

            if (!InfocontrDtadmorig.HasValue &&
                InfocontrTpcontr.HasValue &&
                new[] { ESocialTipoContratoTSVE.TrabVinculoComAlteracaoDataAdmissao.ToInt(),
                        ESocialTipoContratoTSVE.TrabVinculoComAlteracaoDataAdmissaoDesligamento.ToInt()
                       }.Contains(InfocontrTpcontr.Value))
            {
                mensagensErro.Add("O campo data de admissão original do vínculo é obrigatório\n"
                                  + "caso o campo tipo de contrato esteja preenchido com 2 ou 4.");
            }

            if (InfocontrIndreint is null &&
                InfocontrTpcontr.HasValue && InfocontrTpcontr.Value != ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()
                       && InfocontrIndcontr is not null && InfocontrIndcontr == "S")
            {
                mensagensErro.Add("O campo indicativo de reintegração do empregado é obrigatório"
                                  + "caso o campo tipo de contrato seja diferente de 6\n"
                                  + "e o campo campo indicativo se o contrato possui informação no evento S-2190,\n"
                                  + "S-2200 ou S-2300 no declarante esteja preeenchido com 'Sim'.");
            }

            if (InfocontrIndcateg is null)
            {
                mensagensErro.Add("O campo indicativo se houve reconhecimento de categoria do trabalhador diferente da cadastrada(no eSocial ou na GFIP) pelo declarante é obrigatório");
            }

            //if (this.InfocontrIndnatativ is null)
            //{
            //    mensagensErro.Add("O campo indicativo se houve reconhecimento de natureza da atividade diferente da cadastrada pelo declarante é obrigatório");
            //}

            if (InfocontrIndmotdeslig is null)
            {
                mensagensErro.Add("O campo indicativo se houve reconhecimento de motivo de desligamento diferente do informado pelo declarante é obrigatório");
            }

            if (InfocontrMatricula is not null && InfocontrMatricula.Length > 30)
            {
                mensagensErro.Add("A matrícula deve conter no máximo 30 caracteres");
            }

            //if ((this.InfocontrIndcontr is not null && this.InfocontrIndcontr == "S") && (this.InfocontrIndcateg is not null && this.InfocontrIndcateg == "N"))
            //{

            //}
            if (InfocontrCodcateg.HasValue && InfocontrCodcateg.Value < 0
                && (InfocontrIndcontr != "S" || InfocontrIndcateg != "N"))
            {
                mensagensErro.Add("O campo código da categoria é obrigatório.");
            }

            if (!InfocontrDtinicio.HasValue &&
                InfocontrTpcontr.HasValue && InfocontrTpcontr.Value == ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()
                 && InfocontrIndcontr is not null && InfocontrIndcontr == "N")
            {
                mensagensErro.Add("O campo data de início de TSVE é obrigatório"
                                  + "caso o campo tipo de contrato esteja preenchido com valor 6\n"
                                  + "e o campo campo indicativo se o contrato possui informação no evento S-2190,\n"
                                  + "S-2200 ou S-2300 no declarante esteja preeenchido com 'Não'.");
            }

            if (SucessaovincTpinsc == 1)
            {
                if (SucessaovincNrinsc is not null && !SucessaovincNrinsc.CNPJValido())
                {
                    mensagensErro.Add("CNPJ da sucessão do inválido.");
                }
            }
            else
            {
                //TODO: validar demais tipos
            }

            if (IdeestabTpinsc == 1)
            {
                if (!IdeestabNrinsc.CNPJValido())
                {
                    mensagensErro.Add("CNPJ do estabelecimento responsável pelo pagamento do inválido.");
                }
            }
            else
            {
                //TODO: validar demais tipos
            }

            if (DuracaoTpcontr == ESocialTipoContrato.PrazoDeterminadoFato.ToByte()
                && (DuracaoObjdet is null || DuracaoObjdet is not null && DuracaoObjdet.Length <= 0))
            {
                mensagensErro.Add("O campo indicação do objeto determinante da contratação por prazo determinado é obrigatório.");
            }

            if (DuracaoObjdet is not null && DuracaoObjdet.Length > 255)
            {
                mensagensErro.Add("O campo indicação do objeto determinante da contratação por prazo determinado não pode conter mais do que 255 caracteres.");
            }

            if (InfocontrIndcontr is not null && InfocontrIndcontr == "N" && string.IsNullOrEmpty(InfocontrMatricula))
            {
                mensagensErro.Add("Deve ser criado uma matrícula para o trabalhador, quando o campo “Indicativo se o contrato possui informação no evento de admissão ou início” for igual a 'Não'.");
            }

            //if (this.InfovlrVrbaseindenfgts >= 0 && string.IsNullOrEmpty(this.InfovlrPagdiretoresc))
            //{
            //    mensagensErro.Add("Campo 'Multa Rescisória do FGTS Paga Diretamente para o Trabalhador Mediante a Decisão/Autorização' de preenchimento obrigatório se o campo 'Multa Rescisória do FGTS para Geração de Guia' for informado.");
            //}

            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarSubgrupo()
        {
            var mensagensErro = new List<string>();

            var (_, ListaErrosEscritorio) = ValidarSubgrupoEscritorio();
            // var (_, ListaErrosContador) = ValidarSubgrupoContador();

            mensagensErro.AddRange(ListaErrosEscritorio.ToList());
            // mensagensErro.AddRange(ListaErrosContador.ToList());

            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarSubgrupoEscritorio()
        {
            var mensagensErro = new List<string>();

            if (!InfocontrDtadmorig.HasValue &&
                InfocontrTpcontr.HasValue &&
                new[] { ESocialTipoContratoTSVE.TrabVinculoComAlteracaoDataAdmissao.ToInt(),
                        ESocialTipoContratoTSVE.TrabVinculoComAlteracaoDataAdmissaoDesligamento.ToInt()
                       }.Contains(InfocontrTpcontr.Value))
            {
                mensagensErro.Add("O campo data de admissão original do vínculo é obrigatório\n"
                                  + "caso o campo tipo de contrato esteja preenchido com 2 ou 4.");
            }

            if (InfocontrIndreint is null &&
                InfocontrTpcontr.HasValue && InfocontrTpcontr.Value != ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()
                       && InfocontrIndcontr is not null && InfocontrIndcontr == "S")
            {
                mensagensErro.Add("O campo indicativo de reintegração do empregado é obrigatório"
                                  + "caso o campo tipo de contrato seja diferente de 6\n"
                                  + "e o campo campo indicativo se o contrato possui informação no evento S-2190,\n"
                                  + "S-2200 ou S-2300 no declarante esteja preeenchido com 'Sim'.");
            }

            //if (this.InfocontrIndnatativ is null)
            //{
            //    mensagensErro.Add("O campo indicativo se houve reconhecimento de natureza da atividade diferente da cadastrada pelo declarante é obrigatório");
            //}

            if (SucessaovincTpinsc == 1)
            {
                if (SucessaovincNrinsc is not null && !SucessaovincNrinsc.CNPJValido())
                {
                    mensagensErro.Add("CNPJ da sucessão do inválido.");
                }
            }
            else
            {
                //TODO: validar demais tipos se necessário
            }

            if (IdeestabTpinsc == 1)
            {
                if (!IdeestabNrinsc.CNPJValido())
                {
                    mensagensErro.Add("CNPJ do estabelecimento responsável pelo pagamento do inválido.");
                }
            }
            else
            {
                //TODO: validar demais tipos se necessário
            }

            if (DuracaoTpcontr == ESocialTipoContrato.PrazoDeterminadoFato.ToByte()
                && (DuracaoObjdet is null || DuracaoObjdet is not null && DuracaoObjdet.Length <= 0))
            {
                mensagensErro.Add("O campo indicação do objeto determinante da contratação por prazo determinado é obrigatório.");
            }

            if (DuracaoObjdet is not null && DuracaoObjdet.Length > 255)
            {
                mensagensErro.Add("O campo indicação do objeto determinante da contratação por prazo determinado não pode conter mais do que 255 caracteres.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarSubgrupoContador()
        {
            var mensagensErro = new List<string>();

            //Implementar validações de back do contador se necessário.
            if (!InfovlrCompini.HasValue)
            {
                mensagensErro.Add("O campo \"Competência Inicial\" é obrigatório!");
            }

            if (!InfovlrCompfim.HasValue)
            {
                mensagensErro.Add("O campo \"Competência Final\" é obrigatório!");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarRascunho()
        {
            var mensagensErro = new List<string>();

            if (DuracaoObjdet is not null && DuracaoObjdet.Length > 255)
            {
                mensagensErro.Add("O campo indicação do objeto determinante da contratação por prazo determinado não pode conter mais do que 255 caracteres.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
