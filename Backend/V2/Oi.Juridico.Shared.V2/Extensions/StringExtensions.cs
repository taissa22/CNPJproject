using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Oi.Juridico.Shared.V2.Extensions
{
    public static class StringExtensions
    {
        public static string Compress(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new();
            using (GZipStream zip = new(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(this string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using MemoryStream ms = new();
            int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

            byte[] buffer = new byte[msgLength];

            ms.Position = 0;
            using (GZipStream zip = new(ms, CompressionMode.Decompress))
            {
                zip.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }


        public static string SoNumero(this string texto)
        {
            return texto is null ? texto : Regex.Replace(texto, "[^0-9]+", "");
        }

        public static bool CPFValido(this string textoCPF)
        {
            if (textoCPF is null)
            {
                return false;
            }

            var textoCPFNormalizado = Regex.Replace(textoCPF, "[^0-9]+", "");
            if (textoCPFNormalizado.Length != 11)
                return false;

            if (textoCPFNormalizado.Equals("00000000000") ||
                textoCPFNormalizado.Equals("11111111111") ||
                textoCPFNormalizado.Equals("22222222222") ||
                textoCPFNormalizado.Equals("33333333333") ||
                textoCPFNormalizado.Equals("44444444444") ||
                textoCPFNormalizado.Equals("55555555555") ||
                textoCPFNormalizado.Equals("66666666666") ||
                textoCPFNormalizado.Equals("77777777777") ||
                textoCPFNormalizado.Equals("88888888888") ||
                textoCPFNormalizado.Equals("99999999999"))
            {
                return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;

            if (textoCPFNormalizado.Length != 11)
                return false;

            tempCpf = textoCPFNormalizado.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();
            if (!textoCPFNormalizado.EndsWith(digito))
                return false;

            return true;            
        }

        public static bool CPFValidoSisjur(this string textoCPF)
        {
            var textoCPFNormalizado = Regex.Replace(textoCPF, "[^0-9]+", "");
            if (textoCPFNormalizado.Equals("11111111111") || textoCPFNormalizado.Equals("99999999999"))
            {
                return true;
            }

            return textoCPFNormalizado.CPFValido();
        }

        public static bool CNPJValido(this string textoCNPJ)
        {
            if (textoCNPJ is null)
            {
                return false;
            }

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            string textoCNPJNormalizado = Regex.Replace(textoCNPJ, "[^0-9]+", "");

            if (textoCNPJNormalizado.Length != 14)
                return false;

            if (textoCNPJNormalizado.Equals("00000000000000") ||
                textoCNPJNormalizado.Equals("11111111111111") ||
                textoCNPJNormalizado.Equals("22222222222222") ||
                textoCNPJNormalizado.Equals("33333333333333") ||
                textoCNPJNormalizado.Equals("44444444444444") ||
                textoCNPJNormalizado.Equals("55555555555555") ||
                textoCNPJNormalizado.Equals("66666666666666") ||
                textoCNPJNormalizado.Equals("77777777777777") ||
                textoCNPJNormalizado.Equals("88888888888888") ||
                textoCNPJNormalizado.Equals("99999999999999"))
            {
                return false;
            }

            tempCnpj = textoCNPJNormalizado.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return textoCNPJNormalizado.EndsWith(digito);
        }

        public static bool CNPJValidoSisjur(this string textoCNPJ)
        {
            string textoCNPJNormalizado = Regex.Replace(textoCNPJ, "[^0-9]+", "");
            if (textoCNPJNormalizado.Equals("11111111111111") || textoCNPJNormalizado.Equals("99999999999999"))
            {
                return true;
            }
            return textoCNPJNormalizado.CNPJValido();
        }

        public static string FormatCNPJ(this string textoCNPJ)
        {
            if (String.IsNullOrEmpty(textoCNPJ) || textoCNPJ.Length < 14) return "";
            var textoCNPJTemp = Regex.Replace(textoCNPJ, "[^0-9]+", "");
            return Convert.ToUInt64(textoCNPJTemp).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormatCPF(this string textoCPF)
        {
            if (String.IsNullOrEmpty(textoCPF) || textoCPF.Length < 11) return "";
            textoCPF = Regex.Replace(textoCPF, "[^0-9]+", "");
            return Convert.ToUInt64(textoCPF).ToString(@"000\.000\.000\-00");
        }

        public static string FormatData(this string textodata)
        {
            if (String.IsNullOrEmpty(textodata)) return "";
            textodata = $"{textodata}-01";
            return Convert.ToDateTime(textodata).ToString(@"dd/MM/yyyy");
        }
    }
}
