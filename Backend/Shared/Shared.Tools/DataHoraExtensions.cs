using System;
using System.Globalization;

namespace Shared.Tools
{
    public static class DataHoraExtensions
    {
        public const string FORMATO_DATA_PADRAO = "dd/MM/yyyy";
        public const string FORMATO_DATA_MES_ANO_PADRAO = "MM/yyyy";

        public const string FORMATO_DATAHORA_PADRAO = "dd/MM/yyyy HH:mm";
        public const string FORMATO_DATAHORA_COM_SEG_PADRAO = "dd/MM/yyyy HH:mm:ss";
        public const string FORMATO_HORA_PADRAO = "HH:mm";
        public const string FORMATO_HORA_COM_SEG_PADRAO = "HH:mm:ss";

        #region Validacao

        public static bool AniversarioValido(this string date)
        {
            return Valida(string.Format("{0}/2000", date), FORMATO_DATA_PADRAO);
        }

        private static bool Valida(string date, string format)
        {
            var defaultDate = DateTime.Today;

            return DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out defaultDate);
        }

        public static bool DataValida(this string date, string format = null)
        {
            if (string.IsNullOrEmpty(format))
                return Valida(date, FORMATO_DATA_PADRAO);

            return Valida(date, format);
        }

        #endregion

        #region From Date

        public static string DataFormatada(this DateTime date)
        {
            return DataFormatada(date, null);
        }

        public static string DataFormatada(this DateTime? date)
        {
            return DataFormatada(date, null);
        }
        public static string DataFormatadaToSQLFim(this DateTime? date)
        {
            return $"TO_DATE('{date.Value.ToString("dd/MM/yyyy")} 23:59:59', 'dd/MM/yyyy HH24:MI:SS')";
        }
        public static string DataFormatadaToSQLInicio(this DateTime? date)
        {
            return $"TO_DATE('{date.Value.ToString("dd/MM/yyyy")} 00:00:00', 'dd/MM/yyyy HH24:MI:SS')";
        }
        public static string DataFormatada(this DateTime? date, string format)
        {
            if (date.HasValue)
            {
                return date.Value.DataFormatada(format);
            }

            return null;
        }

        public static string DataFormatada(this DateTime date, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_DATA_PADRAO;

            return date.ToString(format);
        }

        #endregion

        #region From Datetime

        public static string DataHoraFormatada(this DateTime dateTime)
        {
            return DataHoraFormatada(dateTime, null);
        }

        public static string DataHoraFormatada(this DateTime? dateTime)
        {
            return DataHoraFormatada(dateTime, null);
        }

        public static string DataHoraFormatada(this DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.DataHoraFormatada(format);
            }

            return null;
        }

        public static string DataHoraFormatada(this DateTime dateTime, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_DATAHORA_PADRAO;

            return dateTime.ToString(format);
        }

        public static string DataHoraAsFormatada(this DateTime dateTime)
        {
            return $"{dateTime:dd/MM/yyyy} às {dateTime:HH:mm}";
        }

        public static string DataHoraSegundoFormatada(this DateTime? dateTime)
        {
            return DataHoraSegundoFormatada(dateTime, null);
        }

        public static string DataHoraSegundoFormatada(this DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.DataHoraSegundoFormatada(format);
            }

            return null;
        }

        public static string DataHoraSegundoFormatada(this DateTime dateTime)
        {
            return DataHoraSegundoFormatada(dateTime, null);
        }

        public static string DataHoraSegundoFormatada(this DateTime dateTime, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_DATAHORA_COM_SEG_PADRAO;

            return dateTime.ToString(format);
        }

        public static string DataMesAnoFormatada(this DateTime dateTime)
        {
            return DataMesAnoFormatada(dateTime, null);
        }

        public static string DataMesAnoFormatada(this DateTime? dateTime)
        {
            return DataMesAnoFormatada(dateTime, null);
        }

        public static string DataMesAnoFormatada(this DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.DataMesAnoFormatada(format);
            }

            return null;
        }

        public static string DataMesAnoFormatada(this DateTime dateTime, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_DATA_MES_ANO_PADRAO;

            return dateTime.ToString(format);
        }

        public static string HoraFormatada(this DateTime dateTime)
        {
            return HoraFormatada(dateTime, null);
        }

        public static string HoraFormatada(this DateTime? dateTime)
        {
            return HoraFormatada(dateTime, null);
        }

        public static string HoraFormatada(this DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.HoraFormatada(format);
            }

            return null;
        }

        public static string HoraFormatada(this DateTime dateTime, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_HORA_PADRAO;

            return dateTime.ToString(format);
        }

        public static string HoraComSegFormatada(this DateTime? dateTime)
        {
            return HoraComSegFormatada(dateTime, null);
        }

        public static string HoraComSegFormatada(this DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.HoraComSegFormatada(format);
            }

            return null;
        }
        public static string HoraComSegFormatada(this DateTime dateTime, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = FORMATO_HORA_COM_SEG_PADRAO;

            return dateTime.ToString(format);
        }

        #endregion

        #region To Date

        public static DateTime? Data(this string date)
        {
            return date.Data(null);

        }

        public static DateTime? Data(this string date, string format)
        {
            if (string.IsNullOrEmpty(date))
                return new DateTime?();

            if (format == null)
                format = FORMATO_DATA_PADRAO;

            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }

        #endregion

        #region To Datetime

        public static DateTime? DataHora(this string date)
        {
            return date.Data(FORMATO_DATAHORA_PADRAO);
        }

        public static DateTime? DataHora(this string date, string time)
        {
            return date.DataHora(time, null);
        }

        public static DateTime? DataHora(this string date, string time, string format)
        {
            if (format == null)
                format = FORMATO_DATAHORA_PADRAO;

            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                return null;
            else
                return DateTime.ParseExact(string.Format("{0} {1}", date, time), format, CultureInfo.InvariantCulture);
        }

        #endregion

        public static string InsereBarraMesAno(this string valor)
        {
            if (!String.IsNullOrEmpty(valor))
            {
                var mes = valor.Substring(0, 2);
                var ano = valor.Substring(2, 4);
                valor = mes + "/" + ano;
            }

            return valor;
        }
    }
}
