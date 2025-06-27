using Oi.Juridico.Shared.V2.Tools;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Shared.V2.Tools
{
    public class Util
    {
        public static DateTime GetNextWeekday(DateTime? dtIni, DateTime? dtFim, int? day, string indExecutarDiaUtil)
        {
            var dIni = dtIni ?? DateTime.Now.Date;
            var dFim = dtFim ?? DateTime.Now.Date;
            var diaI = day != null ? (DiasSemana)day : DiasSemana.Terça_feira;

            var dtProxExec = DateTime.Now.Date;


            if (dIni != dFim)
            {
                // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                int daysToAdd = ((int)diaI - (int)dIni.DayOfWeek + 6) % 7;

                dtProxExec = dIni.AddDays(daysToAdd);

                if (dtProxExec > dFim) dtProxExec = DateTime.Now.Date.AddDays(-1);

            }

            if (indExecutarDiaUtil != "N")
                foreach (DateTime dia in Util.EachDay(dtProxExec, dFim))
                {
                    if (!Util.EhSabadoOuDomingo(dia))
                    {
                        dtProxExec = dia;
                        break;
                    }
                }

            return dtProxExec;

        }

        public static DateTime GetNextMonthday(DateTime? dtIni, DateTime? dtFim, int? diaIni, string indExecutarDiaUtil)
        {
            var dIni = dtIni ?? DateTime.Now.Date;
            var dFim = dtFim ?? DateTime.Now.Date;
            var diaI = diaIni ?? 1;

            var dtProxExec = DateTime.Now.Date;

            if (dIni != dFim)
            {
                dIni = new DateTime(dIni.Year, dIni.Month, diaI);
                dtProxExec = dIni.AddDays(30);

                if (dtProxExec > dFim) dtProxExec = DateTime.Now.Date.AddDays(1);

            }

            if (indExecutarDiaUtil != "N")
                foreach (DateTime day in Util.EachDay(dtProxExec, dFim))
                {
                    if (!Util.EhSabadoOuDomingo(day))
                    {
                        dtProxExec = day;
                        break;
                    }
                }
            

            return dtProxExec;
        }

        public static bool EhSabadoOuDomingo(DateTime data)
        {
            return data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;
        }

        public static IEnumerable<DateTime> EachDay(DateTime? from, DateTime? thru)
        {
            var dIni = from ?? DateTime.Now.Date;
            var dFim = thru ?? DateTime.Now.Date;
            var lstDias = new List<DateTime>();

            for (var day = dIni.Date; day.Date <= dFim.Date; day = day.AddDays(1))
            {
                lstDias.Add(day);
            }

            return lstDias;


        }

        public static string FormataDataNullable(DateTime? dat, string format)
        {
            try
            {
                var data = dat ?? DateTime.Now.Date;
                if (data != DateTime.Now.Date)
                    return data.ToString("yyyy-MM-dd");
                else
                    return "";

            }
            catch (Exception)
            {

                return "";
            }
        }

      




}
}
