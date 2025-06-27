using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Export.CSV
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        private readonly CsvFormatterOptions _options;

        private readonly bool useJsonAttributes = true;

        public string ContentType { get; private set; }

        public CsvOutputFormatter(CsvFormatterOptions csvFormatterOptions)
        {
            ContentType = "text/csv";
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            _options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
        }

        protected override bool CanWriteType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return IsTypeOfIEnumerable(type);
        }

        private bool IsTypeOfIEnumerable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Returns the JsonProperty data annotation name
        /// </summary>
        /// <param name="pi">Property Info</param>
        /// <returns></returns>
        private string GetDisplayNameFromNewtonsoftJsonAnnotations(PropertyInfo pi)
        {
            if (pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.PropertyName is string value)
            {
                return value;
            }

            return pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding == null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            var response = context.HttpContext.Response;

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            var streamWriter = new StreamWriter(response.Body, selectedEncoding);

            if (_options.IncludeExcelDelimiterHeader)
            {
                await streamWriter.WriteLineAsync($"sep ={_options.CsvDelimiter}");
            }

            if (_options.UseSingleLineHeaderInCsv)
            {
                var values = useJsonAttributes
                    ? itemType.GetProperties().Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>(false).Any())    // Only get the properties that do not define JsonIgnore
                        .Select(pi => new
                        {
                            Order = pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                            Prop = pi
                        }).OrderBy(d => d.Order).Select(d => GetDisplayNameFromNewtonsoftJsonAnnotations(d.Prop))
                    : itemType.GetProperties().Select(pi => pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name);

                await streamWriter.WriteLineAsync(string.Join(_options.CsvDelimiter, values));
            }


            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                var vals = useJsonAttributes
                    ? obj.GetType().GetProperties()
                        .Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                        .Select(pi => new
                        {
                            Order = pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                            Value = pi.GetValue(obj, null)
                        }).OrderBy(d => d.Order).Select(d => new { d.Value })
                    : obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Value = pi.GetValue(obj, null)
                        });

                string valueLine = string.Empty;

                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {

                        var _val = val.Value.ToString();

                        //Escape quotas
                        _val = _val.Replace("\"", "\"\"");

                        //Check if the value contans a delimiter and place it in quotes if so
                        if (_val.Contains(_options.CsvDelimiter))
                            _val = string.Concat("\"", _val, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        valueLine = string.Concat(valueLine, _val, _options.CsvDelimiter);

                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, _options.CsvDelimiter);
                    }
                }

                await streamWriter.WriteLineAsync(valueLine.Remove(valueLine.Length - _options.CsvDelimiter.Length));
            }

            await streamWriter.FlushAsync();
        }
    }
}
