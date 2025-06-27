using System.Text;

namespace Perlink.Oi.Juridico.Domain.Patterns.Interface.FactoryMethod
{
    public interface IQuerySaldoGarantiaFM
    {
        string RetornaSelect();
        string RetornaFrom();
        string RetornaJoin();
        string RetornaWhere();
        string RetornaGroupBy();
        string RetornaOrderBy();
        StringBuilder CalcularSaldoQuery(StringBuilder query);
    }
}
