using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Data.Interface.FactoryMethod
{
    public interface IQueryFM
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
