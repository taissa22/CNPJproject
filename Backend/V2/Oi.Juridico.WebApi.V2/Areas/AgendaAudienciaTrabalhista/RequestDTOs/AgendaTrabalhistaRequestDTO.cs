using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Entities;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs;
using System.Linq.Expressions;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.RequestDTOs
{
    public class AgendaTrabalhistaRequestDTO
    {
        public DateTime? DataAudienciaDe { get; set; }
        public DateTime? DataAudienciaAte { get; set; }
        public bool ClassificHierarUnico { get; set; }
        public bool ClassificHierarPrimario { get; set; }
        public bool ClassificHierarSecundario { get; set; }
        public bool ClassificProcessoPrimario { get; set; }
        public bool ClassificProcessoTerceiro { get; set; }
        public List<string>? Estado { get; set; }
        public List<int>? TipoAudiencia { get; set; }
        public int? ProcessoEstrategico { get; set; }
        public List<long>? Empresa { get; set; }
        public string? EstadoSelecionado { get; set; }
        public List<int?>? ModalidadeAudiencia { get; set; }
        public List<int?>? LocalidadeAudiencia { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();
            return (mensagensErro.Count > 0, mensagensErro);
        }

        public Expression<Func<VAgendaTrabalhista, bool>> BuildFilter()
        {
            Expression<Func<VAgendaTrabalhista, bool>> result = agendaTrabalhista => true;

            //if (!string.IsNullOrWhiteSpace(Reclamante))
            //{
            //    Expression<Func<VEsAcompanhamento, bool>> reclamanteFilter = o => o.NomParte.Trim().Contains(Reclamante);

            //    result = result.And(reclamanteFilter);
            //}

            if (DataAudienciaDe.HasValue && DataAudienciaAte.HasValue)
            {
                Expression<Func<VAgendaTrabalhista, bool>> dataAudienciaFilter = o => o.DateAudiencia.HasValue && o.DateAudiencia.Value >= DataAudienciaDe.Value.Date && o.DateAudiencia.Value <= DataAudienciaAte.Value.Date.AddDays(1).AddSeconds(-1);
                result = result.And(dataAudienciaFilter);

            }

            if (ClassificHierarPrimario && ClassificHierarUnico && ClassificHierarSecundario)
            {
                Expression<Func<VAgendaTrabalhista, bool>> classificHierarPrimarioFilter = o => o.ClassificacaoHierarquica == "P" || o.ClassificacaoHierarquica == "U" || o.ClassificacaoHierarquica == "S";
                result = result.And(classificHierarPrimarioFilter);
            }
            else
            {
                if (ClassificHierarPrimario && ClassificHierarUnico)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarPrimarioFilter = o => o.ClassificacaoHierarquica == "P" || o.ClassificacaoHierarquica == "U";
                    result = result.And(classificHierarPrimarioFilter);
                }else if (ClassificHierarPrimario &&  ClassificHierarSecundario)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarPrimarioFilter = o => o.ClassificacaoHierarquica == "P" || o.ClassificacaoHierarquica == "S";
                    result = result.And(classificHierarPrimarioFilter);
                }else if (ClassificHierarPrimario)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarPrimarioFilter = o => o.ClassificacaoHierarquica == "P";
                    result = result.And(classificHierarPrimarioFilter);
                }else if (ClassificHierarUnico && ClassificHierarSecundario)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarUnicoFilter = o => o.ClassificacaoHierarquica == "U" || o.ClassificacaoHierarquica == "S";
                    result = result.And(classificHierarUnicoFilter);
                }
                else if (ClassificHierarUnico)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarUnicoFilter = o => o.ClassificacaoHierarquica == "U";
                    result = result.And(classificHierarUnicoFilter);
                }else if (ClassificHierarSecundario)
                {
                    Expression<Func<VAgendaTrabalhista, bool>> classificHierarSecundarioFilter = o => o.ClassificacaoHierarquica == "S";
                    result = result.And(classificHierarSecundarioFilter);
                }

            }




            if (Estado != null && Estado!.Count > 0)
            {
                Expression<Func<VAgendaTrabalhista, bool>> estadoFilter = o => Estado!.Contains(o.Estado);
                result = result.And(estadoFilter);
            }

            if (ClassificProcessoPrimario && ClassificProcessoTerceiro)
            {
                Expression<Func<VAgendaTrabalhista, bool>> classificProcessoPrimarioFilter = o => o.ClassificacaoProcesso == "P" || o.ClassificacaoProcesso == "T";
                result = result.And(classificProcessoPrimarioFilter);
            }
            else if (ClassificProcessoPrimario)
            {
                Expression<Func<VAgendaTrabalhista, bool>> classificProcessoPrimarioFilter = o => o.ClassificacaoProcesso == "P";
                result = result.And(classificProcessoPrimarioFilter);
            }
            else if (ClassificProcessoTerceiro)
            {
                Expression<Func<VAgendaTrabalhista, bool>> classificProcessoTerceiroFilter = o => o.ClassificacaoProcesso == "T";
                result = result.And(classificProcessoTerceiroFilter);
            }

            if (TipoAudiencia != null && TipoAudiencia.Count > 0)
            {
                Expression<Func<VAgendaTrabalhista, bool>> tipoAudienciaFilter = o => TipoAudiencia.Contains(o.CodTipoAudiencia);
                result = result.And(tipoAudienciaFilter);
            }

            if (ProcessoEstrategico.HasValue && ProcessoEstrategico == 1)
            {
                Expression<Func<VAgendaTrabalhista, bool>> processoEstrategicoFilter = o => o.Estrategico == "S";
                result = result.And(processoEstrategicoFilter);
            }

            if (ProcessoEstrategico.HasValue && ProcessoEstrategico == 2)
            {
                Expression<Func<VAgendaTrabalhista, bool>> processoEstrategicoFilter = o => o.Estrategico == "N";
                result = result.And(processoEstrategicoFilter);
            }

            if (Empresa != null && Empresa!.Count > 0)
            {
                Expression<Func<VAgendaTrabalhista, bool>> EmpresaAgrupadoraFilter = e => Empresa.Contains(e.EmpresaAgrupadora);
                result = result.And(EmpresaAgrupadoraFilter);
            }

            if (ModalidadeAudiencia != null && ModalidadeAudiencia.Count > 0)
            {
                Expression<Func<VAgendaTrabalhista, bool>> modalidadeAudienciaFilter = o => ModalidadeAudiencia.Contains(o.CodModalidadeAudiencia);
                result = result.And(modalidadeAudienciaFilter);
            }

            if (LocalidadeAudiencia != null && LocalidadeAudiencia.Count > 0)
            {
                Expression<Func<VAgendaTrabalhista, bool>> localidadeAudienciaFilter = o => LocalidadeAudiencia.Contains(o.CodLocalidadeAudiencia);
                result = result.And(localidadeAudienciaFilter);
            }
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
