using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Application.Impl
{
    public class Criptografador
    {
        private String chaveEmbaralhada;
        private String chave;

        public Criptografador(String chave)
        {
            this.chave = chave.ToUpper();
        }

        public String GerarToken(String codigoDoUsuario)
        {
            codigoDoUsuario = RetiraAcentos(codigoDoUsuario).ToUpper();
            Int32 quantidadeComplementar = (chave.Length % 5);
            if (!quantidadeComplementar.Equals(0))
            {
                chave += CompletarChave(codigoDoUsuario, 5 - quantidadeComplementar);
            }
            Int32 quantidadeDeCadaBloco = chave.Length / 5;
            String[] blocos = { ObterBloco(1, quantidadeDeCadaBloco), ObterBloco(2, quantidadeDeCadaBloco), ObterBloco(3, quantidadeDeCadaBloco), ObterBloco(4, quantidadeDeCadaBloco), ObterBloco(5, quantidadeDeCadaBloco) };
            if (String.IsNullOrEmpty(chaveEmbaralhada))
            {
                chaveEmbaralhada = String.Format("{0}{1}{2}{3}{4}", blocos[3], blocos[1], blocos[4], blocos[0], blocos[2]);
            }

            return ObterChaveCriptografada(codigoDoUsuario);
        }

        private String ObterChaveCriptografada(String codigoDoUsuario)
        {
            StringBuilder retorno = new StringBuilder();
            for (Int32 i = 0; i < chaveEmbaralhada.Length; i++)
            {
                Int32 indiceParaCodigoDoUsuario = (codigoDoUsuario.Length - 1) - (i % codigoDoUsuario.Length);
                retorno.Append(ObterLetraParaRetorno(chaveEmbaralhada[i], ObterValorDaLetra(codigoDoUsuario[indiceParaCodigoDoUsuario])));
            }

            return retorno.ToString();
        }

        private Int32 ObterValorDaLetra(Char letra)
        {
            return (Convert.ToInt32(letra) - 64);
        }

        private char ObterLetraParaRetorno(Char letra, Int32 valorASomarNoAscii)
        {
            Int32 codigoAscii = Convert.ToInt32(letra) + valorASomarNoAscii;
            if (codigoAscii > 90)
            {
                codigoAscii = (codigoAscii - 90) + 64;
            }
            return Convert.ToChar(codigoAscii);
        }

        private String ObterBloco(Int32 numeroDoBloco, Int32 tamanho)
        {
            String bloco = chave.Substring((numeroDoBloco - 1) * tamanho, tamanho);
            StringBuilder retorno = new StringBuilder();
            for (Int32 i = tamanho - 1; i >= 0; i--)
            {
                retorno.Append(bloco[i]);
            }
            return retorno.ToString();
        }

        private String CompletarChave(String codigoDoUsuario, Int32 quantidadeComplementar)
        {
            if (codigoDoUsuario.Length < 5)
            {
                return String.Format("{0}{0}{0}{0}{0}", codigoDoUsuario).Substring(0, quantidadeComplementar);
            }
            return codigoDoUsuario.Substring(0, quantidadeComplementar);
        }

        private String RetiraAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
    }
}
