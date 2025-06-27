using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.SqlServer.Server;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;
using System.Linq.Expressions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs
{
    public class EsF2500AcompanhamentoRequestDTO
    {
        public DateTime? PeriodoSentencaDe { get; set; }
        public DateTime? PeriodoSentencaAte { get; set; }
        public DateTime? PeriodoApuracaoDe { get; set; }
        public DateTime? PeriodoApuracaoAte { get; set; }
        public DateTime? PeriodoStatusDe { get; set; }
        public DateTime? PeriodoStatusAte { get; set; }
        public string? Processo { get; set; }
        public string? Reclamante { get; set; }
        public string? Cpf { get; set; }
        public List<byte?>? StatusExecucao { get; set; }
        public List<int?>? Empresa { get; set; }
        public List<string>? Uf { get; set; }

        public int? tipoFormulario { get; set; }
        public int? tipoFormularioTipo { get; set; }

        public string? Escritorio { get; set; }
        public string? Contador { get; set; }
        public bool statusNaoIniciadoContador { get; set; }
        public bool statusPendenteContador { get; set; }
        public bool statusFinalizadoContador { get; set; }
        public bool statusNaoIniciadoEscritorio { get; set; }
        public bool statusPendenteEscritorio { get; set; }
        public bool statusFinalizadoEscritorio { get; set; }
        public long? IdFormulario { get; set; }

        public List<int?>? CodProfissional { get; set; }
        //public int? CodEscritorioAcompanhante { get; set; }
        public List<int?>? CodContador { get; set; }
        public bool EhEscritorio { get; set; }
        public bool EhContador { get; set; }
        public string? criterioBuscaProcesso { get; set; }
        public string? campoProcesso { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            Cpf = Cpf.SoNumero();

            //if (this.Cpf is null)
            //{
            //    mensagensErro.Add("CPF não pode estar vazio.");
            //}

            //if (!this.Cpf.CPFValido())
            //{
            //    mensagensErro.Add("CPF inválido.");
            //}

            if (PeriodoStatusDe.HasValue && PeriodoStatusAte.HasValue &&
                
                    PeriodoStatusDe.Value > PeriodoStatusAte.Value
                
                )
            {
                mensagensErro.Add("O periodo inicial deve ser menor ou igual ao periodo final.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }

        public Expression<Func<VEsAcompanhamento, bool>> BuildFilter()
        {
            Expression<Func<VEsAcompanhamento, bool>> result = acompanhamento => true;

            if (!string.IsNullOrWhiteSpace(Reclamante))
            {
                Expression<Func<VEsAcompanhamento, bool>> reclamanteFilter = o => o.NomParte.Trim().Contains(Reclamante);

                result = result.And(reclamanteFilter);
            }

            if (!string.IsNullOrWhiteSpace(Cpf))
            {
                //var CpfFilter = Cpf;
                Expression<Func<VEsAcompanhamento, bool>> CpfFilter = o => o.CpfParte == Cpf;
                result = result.And(CpfFilter);
            }
            if (Uf != null && Uf!.Count > 0)
            {
                //var CpfFilter = Cpf;
                Expression<Func<VEsAcompanhamento, bool>> UfFilter = o => Uf!.Contains(o.CodEstado);
                result = result.And(UfFilter);
            }

            if (PeriodoStatusDe.HasValue && PeriodoStatusAte.HasValue)
            {
                Expression<Func<VEsAcompanhamento, bool>> periodoFilter = o => o.LogDataOperacao!.Value.Date >= PeriodoStatusDe.Value.Date && o.LogDataOperacao!.Value.Date <= PeriodoStatusAte.Value.Date;
                result = result.And(periodoFilter);

            }

            if (PeriodoApuracaoDe.HasValue && PeriodoApuracaoAte.HasValue)
            {
                // Converter as datas de período para o formato YYYYMM
                var periodoInicioFormatado = PeriodoApuracaoDe.Value.ToString("yyyyMM");
                var periodoFimFormatado = PeriodoApuracaoAte.Value.ToString("yyyyMM");

                // Criar o filtro usando apenas as comparações de ano e mês
                Expression<Func<VEsAcompanhamento, bool>> periodoApuracaoFilter = o =>
                    o.DataApuracao.CompareTo(periodoInicioFormatado) >= 0 && o.DataApuracao.CompareTo(periodoFimFormatado) <= 0;

                result = result.And(periodoApuracaoFilter);
            }



            if (PeriodoSentencaDe.HasValue && PeriodoSentencaAte.HasValue)
            {
                Expression<Func<VEsAcompanhamento, bool>> periodoSentencaFilter = o => o.InfoprocjudDtsent.HasValue && o.InfoprocjudDtsent.Value.Date >= PeriodoSentencaDe.Value.Date && o.InfoprocjudDtsent.Value.Date <= PeriodoSentencaAte.Value.Date;
                result = result.And(periodoSentencaFilter);

            }

            if (!string.IsNullOrEmpty(Processo) && !string.IsNullOrEmpty(criterioBuscaProcesso) && !string.IsNullOrEmpty(campoProcesso))
            {
                if (criterioBuscaProcesso == "I")
                {
                    if (campoProcesso == "CI")
                    {
                        Expression<Func<VEsAcompanhamento, bool>> partyFilter = o => o.CodProcesso == int.Parse(Processo);
                        result = result.And(partyFilter);
                    }
                    if (campoProcesso == "NP")
                    {
                        Expression<Func<VEsAcompanhamento, bool>> partyFilter = o => o.InfoprocessoNrproctrab == Processo;
                        result = result.And(partyFilter);
                    }
                }

                if (criterioBuscaProcesso == "C")
                {
                    Expression<Func<VEsAcompanhamento, bool>> partyFilter = o => o.InfoprocessoNrproctrab.Contains(Processo);
                    result = result.And(partyFilter);
                }
            }

            if (StatusExecucao != null && StatusExecucao!.Count > 0)
            {
                Expression<Func<VEsAcompanhamento, bool>> StatusExecucaoExpression = e => StatusExecucao.Contains(e.StatusFormulario);
                result = result.And(StatusExecucaoExpression);
            }

            if (Empresa != null && Empresa!.Count > 0)
            {
                Expression<Func<VEsAcompanhamento, bool>> StatusExecucaoExpression = e => Empresa.Contains(e.IdEsEmpresaAgrupadora);
                result = result.And(StatusExecucaoExpression);
            }

            if (tipoFormulario.HasValue && tipoFormulario == 1)
            {
                Expression<Func<VEsAcompanhamento, bool>> TipoFormularioExpression = e => e.TipoFormulario == "F_2500";
                result = result.And(TipoFormularioExpression);
            }

            if (tipoFormulario.HasValue && tipoFormulario == 2)
            {
                Expression<Func<VEsAcompanhamento, bool>> TipoFormularioExpression = e => e.TipoFormulario == "F_2501";
                result = result.And(TipoFormularioExpression);
            }

            if (tipoFormularioTipo.HasValue && tipoFormularioTipo == 1)
            {
                Expression<Func<VEsAcompanhamento, bool>> TipoFormularioTipoExpression = e => e.IdeeventoIndretif == 1;
                result = result.And(TipoFormularioTipoExpression);
            }

            if (tipoFormularioTipo.HasValue && tipoFormularioTipo == 2)
            {
                Expression<Func<VEsAcompanhamento, bool>> TipoFormularioTipoExpression = e => e.IdeeventoIndretif == 2;
                result = result.And(TipoFormularioTipoExpression);
            }

            if (EhEscritorio)
            {
                if (!string.IsNullOrWhiteSpace(Escritorio))
                {
                    Expression<Func<VEsAcompanhamento, bool>> escritorioFilter = o => o.NomEscritorio.Trim().Contains(Escritorio);

                    result = result.And(escritorioFilter);
                }
                else {
                    Expression<Func<VEsAcompanhamento, bool>> escritorioFilter = o => CodProfissional!.Contains(o.CodProfissional) || CodProfissional.Contains(o.CodEscritorioAcompanhante);

                    result = result.And(escritorioFilter);
                }
                
            }
            else {
                if (!string.IsNullOrWhiteSpace(Escritorio))
                {
                    Expression<Func<VEsAcompanhamento, bool>> escritorioFilter = o => o.NomEscritorio.Trim().Contains(Escritorio);

                    result = result.And(escritorioFilter);
                }
            }

            if (EhContador)
            {
                if (!string.IsNullOrWhiteSpace(Contador))
                {
                    Expression<Func<VEsAcompanhamento, bool>> contadorFilter = o => o.NomContador.Trim().Contains(Contador.Trim());

                    result = result.And(contadorFilter);
                }
                else
                {
                    Expression<Func<VEsAcompanhamento, bool>> contadorFilter = o => CodContador!.Contains(o.CodContador);

                    result = result.And(contadorFilter);
                }
               
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Contador))
                {
                    Expression<Func<VEsAcompanhamento, bool>> contadorFilter = o => o.NomContador.Trim().Contains(Contador.Trim());

                    result = result.And(contadorFilter);
                }
            }

            if (IdFormulario.HasValue)
            {
                Expression<Func<VEsAcompanhamento, bool>> idFormularioFilter = o => o.IdFormulario == IdFormulario;
                result = result.And(idFormularioFilter);
            }
            #region filtro escritorio
            if (!statusFinalizadoEscritorio || !statusPendenteEscritorio || !statusNaoIniciadoEscritorio)
            {
                if (statusFinalizadoEscritorio && statusPendenteEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoEscritorioFilter = o => o.FinalizadoEscritorio == "S" || o.FinalizadoEscritorio == "N";

                    result = result.And(statusFinalizadoEscritorioFilter);
                }
                else if (statusFinalizadoEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoEscritorioFilter = o => o.FinalizadoEscritorio == "S";

                    result = result.And(statusFinalizadoEscritorioFilter);
                }

                if (statusFinalizadoEscritorio && statusNaoIniciadoEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoEscritorioFilter = o => o.FinalizadoEscritorio == "S" || o.FinalizadoEscritorio == null;

                    result = result.And(statusFinalizadoEscritorioFilter);
                }

                if (statusPendenteEscritorio && statusNaoIniciadoEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusPendenteEscritorioFilter = o => o.FinalizadoEscritorio == "N" || o.FinalizadoEscritorio == null;

                    result = result.And(statusPendenteEscritorioFilter);
                }
                else if (statusPendenteEscritorio && !statusFinalizadoEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusPendenteEscritorioFilter = o => o.FinalizadoEscritorio == "N";

                    result = result.And(statusPendenteEscritorioFilter);
                }

                if (statusNaoIniciadoEscritorio && !statusFinalizadoEscritorio && !statusPendenteEscritorio)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusNaoIniciadoEscritorioFilter = o => o.FinalizadoEscritorio == null;

                    result = result.And(statusNaoIniciadoEscritorioFilter);
                }

            }
            #endregion
            #region filtro Contador
            if (!statusFinalizadoContador || !statusPendenteContador || !statusNaoIniciadoContador)
            {
                if (statusFinalizadoContador && statusPendenteContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoContadorFilter = o => o.FinalizadoContador == "S" || o.FinalizadoContador == "N";

                    result = result.And(statusFinalizadoContadorFilter);
                }
                else if (statusFinalizadoContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoContadorFilter = o => o.FinalizadoContador == "S";

                    result = result.And(statusFinalizadoContadorFilter);
                }

                if (statusFinalizadoContador && statusNaoIniciadoContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusFinalizadoContadorFilter = o => o.FinalizadoContador == "S" || o.FinalizadoContador == null;

                    result = result.And(statusFinalizadoContadorFilter);
                }

                if (statusPendenteContador && statusNaoIniciadoContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusPendenteContadorFilter = o => o.FinalizadoContador == "N" || o.FinalizadoContador == null;

                    result = result.And(statusPendenteContadorFilter);
                }
                else if (statusPendenteContador && !statusFinalizadoContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusPendenteContadorFilter = o => o.FinalizadoContador == "N";

                    result = result.And(statusPendenteContadorFilter);
                }

                if (statusNaoIniciadoContador && !statusFinalizadoContador && !statusPendenteContador)
                {
                    Expression<Func<VEsAcompanhamento, bool>> statusNaoIniciadoContadorFilter = o => o.FinalizadoContador == null;

                    result = result.And(statusNaoIniciadoContadorFilter);
                }
            }
            #endregion

            Expression<Func<VEsAcompanhamento, bool>> statusFilter = o => o.StatusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte();

            result = result.And(statusFilter);

            return result;
        }


    }

    #region Configuracao filtro
    public static class PredicateExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var parameter = a.Parameters[0];
            var visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = parameter;
            var body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);

        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var parameter = a.Parameters[0];
            var visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = parameter;
            var body = Expression.Or(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }

    internal class SubstExpressionVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (subst.TryGetValue(node, out var newValue))
            {
                return newValue;
            }

            return node;
        }

    }
    #endregion

}
