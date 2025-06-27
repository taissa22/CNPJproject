using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.External
{
    internal class ReplacerExpressionVisitor : ExpressionVisitor
    {
        public ReplaceType ReplaceType { get; }

        public ReplacerExpressionVisitor(ReplaceType replaceType)
        {
            ReplaceType = replaceType;
        }

        public override Expression Visit(Expression node)
        {
            if (node is MethodCallExpression call)
            {
                switch (call.Method.Name)
                {
                    case "OrderBy":
                    case "OrderByDescending":
                    case "ThenBy":
                    case "ThenByDescending":                    
                        try
                        {
                            return base.Visit(ProcessSortExpression(call));
                        }
                        catch (Exception)
                        {
                            break;
                        }

                    case "Include":
                    case "ThenInclude":
                    case "Equals":
                    case "ToString":
                    case "ToUpper":
                    case "Contains":
                    case "Where":
                    case "Skip":
                    case "Take":
                    case "Replace":
                    case "FirstOrDefault":
                        return base.Visit(node);

                    default:
                        try
                        {
                            return base.Visit(Expression.Call(null, call.Method, ProcessExpressions(call.Arguments)));
                        }
                        catch (Exception)
                        {
                            break;
                        }
                }
            }

            return base.Visit(node);
        }

        private Expression ProcessSortExpression(MethodCallExpression call)
        {
            UnaryExpression unary = (UnaryExpression)call.Arguments[1];

            LambdaExpression lambda = (LambdaExpression)unary.Operand;

            MemberExpression member = (MemberExpression)lambda.Body;

            if (member.Type != ReplaceType.TypeFrom)
            {
                return call;
            }

            ParameterExpression parameter = (ParameterExpression)member.Expression;

            MemberExpression nMember = Expression.Property(parameter, ReplaceType.PropertyName);

            LambdaExpression nLambda = Expression.Lambda(nMember, parameter);

            Expression queryExpression = Expression.Call(typeof(Queryable), call.Method.Name, new Type[] { ReplaceType.EntityType, ReplaceType.TypeTo }, call.Arguments[0], nLambda);

            return queryExpression;
        }

        private IEnumerable<Expression> ProcessExpressions(IEnumerable<Expression> expressions)
        {
            var exps = new List<Expression>(expressions.Count());
            foreach (var exp in expressions)
            {
                exps.Add(ProcessExpression(exp));
            }

            return exps;
        }

        private Expression ProcessExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Quote:
                    if (expression is UnaryExpression unary)
                    {
                        return ProcessUnary(unary);
                    }
                    break;

                default:
                    break;
            }
            return expression;
        }

        private UnaryExpression ProcessUnary(UnaryExpression unary)
        {
            if (!(unary.Operand is LambdaExpression operand))
            {
                return unary;
            }

            switch (operand.Body.NodeType)
            {
                case ExpressionType.Equal:
                    break;

                case ExpressionType.MemberAccess:
                    var newBody = ProcessMember((MemberExpression)operand.Body);
                    return Expression.Quote(Expression.Lambda(newBody, (ParameterExpression)newBody.Expression));

                default:
                    return unary;
            }

            if (!(operand.Body is BinaryExpression binary))
            {
                return unary;
            }

            ParameterExpression? parameter = GetParameter(binary);

            if (parameter is null)
            {
                return unary;
            }

            Expression leftExpression;
            if (binary.Left is MemberExpression left)
            {
                leftExpression = ProcessMember(left);
            }
            else
            {
                leftExpression = binary.Left;
            }

            Expression rightExpression;
            if (binary.Right is MemberExpression right)
            {
                rightExpression = ProcessMember(right);
            }
            else
            {
                rightExpression = binary.Right;
            }

            var newOperand = Expression.Equal(leftExpression, rightExpression);

            var lambda = Expression.Lambda(newOperand, parameter);

            return Expression.Quote(lambda);
        }

        private MemberExpression ProcessMember(MemberExpression member)
        {
            if (member.Expression is ParameterExpression parameter)
            {
                return Expression.Property(parameter, ReplaceType.PropertyName);
            }
            else
            {
                return Expression.Property(member, ReplaceType.TypeMemberName);
            }
        }

        private ParameterExpression? GetParameter(BinaryExpression binary)
        {
            if (binary.Left is MemberExpression left && left.Expression is ParameterExpression leftParameter)
            {
                return leftParameter;
            }

            if (binary.Right is MemberExpression right && right.Expression is ParameterExpression rightParameter)
            {
                return rightParameter;
            }

            return null;
        }
    }
}