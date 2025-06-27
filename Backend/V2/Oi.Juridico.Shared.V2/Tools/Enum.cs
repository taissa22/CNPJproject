using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Oi.Juridico.Shared.V2.Tools
{
    public class Enum<T> where T : Enum
    {
        public static IEnumerable<T> GetAllValuesAsIEnumerable()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }


    public enum TipoExecucaoRelatorio
    {
        [Description("Hoje")]
        Hoje = 1,
        [Description("Diariamente")]
        Diariamente = 2,
        [Description("Semanalmente")]
        Semanalmente = 3,
        [Description("Mensalmente")]
        Mensalmente = 4,
        [Description("Na Data")]
        Na_Data = 5
    }

    public enum DiasSemana
    {
        [Description("Domingo")]
        Domingo = 1,
        [Description("Segunda-feira")]
        Segunda_feira = 2,
        [Description("Terça-feira")]
        Terça_feira = 3,
        [Description("Quarta-feira")]
        Quarta_feira = 4,
        [Description("Quinta-feira")]
        Quinta_feira = 5,
        [Description("Sexta-feira")]
        Sexta_feira = 6,
        [Description("Sábado")]
        Sábado = 7
    }
}
