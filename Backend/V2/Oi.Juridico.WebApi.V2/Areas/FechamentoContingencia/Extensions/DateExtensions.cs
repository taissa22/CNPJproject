using Nager.Date;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Validator;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Extensions
{
    public static class DateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime ToNextDayOfWeek(this DateTime value, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)value.DayOfWeek + 7) % 7;
            DateTime nextDayOfWeek = value.AddDays(daysToAdd);

            return nextDayOfWeek;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dayOfMonth"></param>
        /// <returns></returns>
        public static DateTime ToDayOfMonth(this DateTime dateTime, Int32 dayOfMonth)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dayOfMonth);
        }


        //public static IList<DatasNaoUteis> ListaDeDatasNaoUteis
        //{
        //    get
        //    {
        //        listaDeDatasNaoUteis = Domain.DatasNaoUteis.LoadAll();
        //        return listaDeDatasNaoUteis;
        //    }
        //}

        public static DateTime GetNextDiaUtil(DateTime dateTime)
        {
            try
            {
                while (true)
                {
                    if (dateTime.DayOfWeek == DayOfWeek.Saturday)
                        dateTime = dateTime.AddDays(2);
                    else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                        dateTime = dateTime.AddDays(1);

                    var publicHolidays = DateSystem.GetPublicHolidays(dateTime, dateTime, CountryCode.BR);

                    if (publicHolidays.Count() > 0)
                        dateTime = dateTime.AddDays(1);
                    else
                        return dateTime;
                }
            }
            catch (Exception E)
            {
                throw new Exception("Buscar próximo dia útil", E);
            }
        }

        public static bool VerificaSeDiaUtil(this DateTime data)
        {
            var isHoliday = HolidayValidator.IsHoliday(data);
            return (data.DayOfWeek != DayOfWeek.Sunday) &&
                    (data.DayOfWeek != DayOfWeek.Saturday)
                    && !isHoliday;
        }

        public static DateTime GetProximoDiaUtil(this DateTime data)
        {
            while (!VerificaSeDiaUtil(data))
            {
                data = data.AddDays(1);
            }
            return data;
        }

        public static Int64 ObterDiasUteis(this DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int diasTotal = 0;
            dataInicial = dataInicial.Date;
            dataFinal = dataFinal.Date;
            dias = dataInicial.Subtract(dataFinal).Days;

            if (dias == 0)
                return 0;

            if (dias < 0)
                dias = dias * -1;

            var publicHolidays = DateSystem.GetPublicHolidays(dataInicial, dataFinal, CountryCode.BR);
            for (int i = 0; i < dias; i++)
            {
                dataInicial = dataInicial.AddDays(1);
                var diaUtil = publicHolidays.Count(p => p.Date == dataInicial) <= 0;
                if (dataInicial.DayOfWeek != DayOfWeek.Sunday
                    && dataInicial.DayOfWeek != DayOfWeek.Saturday
                    && diaUtil
                    )
                    diasTotal++;
            }

            return diasTotal;
        }

        public static DateTime ObterProximaData(this DateTime data, DayOfWeek diaSemana)
        {
            while (data.DayOfWeek != diaSemana) data = data.AddDays(1);
            return data;
        }
    }
}
