using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.Shared.V2.Seguranca
{
    public class Criptografador
    {
        private string _chaveEmbaralhada = "";
        private string _chave;

        public Criptografador(string chave)
        {
            _chave = chave.ToUpper();
        }

        public String GerarToken(string codigoDoUsuario)
        {
            codigoDoUsuario = RetiraAcentos(codigoDoUsuario).ToUpper();
            int quantidadeComplementar = (_chave.Length % 5);
            if (!quantidadeComplementar.Equals(0))
            {
                _chave += CompletarChave(codigoDoUsuario, 5 - quantidadeComplementar);
            }
            int quantidadeDeCadaBloco = _chave.Length / 5;
            string[] blocos = { ObterBloco(1, quantidadeDeCadaBloco), ObterBloco(2, quantidadeDeCadaBloco), ObterBloco(3, quantidadeDeCadaBloco), ObterBloco(4, quantidadeDeCadaBloco), ObterBloco(5, quantidadeDeCadaBloco) };
            if (string.IsNullOrEmpty(_chaveEmbaralhada))
            {
                _chaveEmbaralhada = string.Format("{0}{1}{2}{3}{4}", blocos[3], blocos[1], blocos[4], blocos[0], blocos[2]);
            }

            return ObterChaveCriptografada(codigoDoUsuario);
        }

        private string ObterChaveCriptografada(string codigoDoUsuario)
        {
            StringBuilder retorno = new();
            for (int i = 0; i < _chaveEmbaralhada.Length; i++)
            {
                int indiceParaCodigoDoUsuario = (codigoDoUsuario.Length - 1) - (i % codigoDoUsuario.Length);
                retorno.Append(ObterLetraParaRetorno(_chaveEmbaralhada[i], ObterValorDaLetra(codigoDoUsuario[indiceParaCodigoDoUsuario])));
            }

            return retorno.ToString();
        }

        private static int ObterValorDaLetra(char letra) => Convert.ToInt32(letra) - 64;

        private static char ObterLetraParaRetorno(char letra, int valorASomarNoAscii)
        {
            int codigoAscii = Convert.ToInt32(letra) + valorASomarNoAscii;
            if (codigoAscii > 90)
            {
                codigoAscii = (codigoAscii - 90) + 64;
            }
            return Convert.ToChar(codigoAscii);
        }

        private string ObterBloco(int numeroDoBloco, int tamanho)
        {
            string bloco = _chave.Substring((numeroDoBloco - 1) * tamanho, tamanho);
            StringBuilder retorno = new();
            for (int i = tamanho - 1; i >= 0; i--)
            {
                retorno.Append(bloco[i]);
            }
            return retorno.ToString();
        }

        private static string CompletarChave(string codigoDoUsuario, int quantidadeComplementar)
        {
            if (codigoDoUsuario.Length < 5)
            {
                return string.Format("{0}{0}{0}{0}{0}", codigoDoUsuario)[..quantidadeComplementar];
            }
            return codigoDoUsuario[..quantidadeComplementar];
        }

        private static string RetiraAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string CriptografarSenha(string senha)
        {
            int multiplicador;
            int c;
            if (String.IsNullOrEmpty(senha))
                return String.Empty;
            var decryptString = " !\"" + "#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLM" + "N" + "OPQRSTUVWXYZ" +
                                "[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            var encryptString = "~{[}u;Ce83KX%:VIm!|gs]_aL-QEOpx<UlzZjBq6#1($\\\"FS5H0'cM&>Po.NGA*Jr)Y Dv/t9kd?^fni,hR2Wy=`+4T@7wb";
            var tamanho = senha.Length;
            if ((1 <= tamanho) && (tamanho <= 3))
                multiplicador = 1;
            else if ((4 <= tamanho) && (tamanho <= 6))
                multiplicador = 2;
            else if ((7 <= tamanho) && (tamanho <= 9))
                multiplicador = 3;
            else
                multiplicador = 4;
            var senhaEncriptada = "";
            for (c = 1; c <= tamanho; c++)
            {
                var deslocamento = c * multiplicador;
                var texto = senha.Substring((c - 1), 1);
                var posicao = decryptString.IndexOf(texto);
                posicao += deslocamento;
                posicao = (posicao + 1) % 95;
                texto = encryptString.Substring(posicao, 1);
                senhaEncriptada += texto;
                if ((1 <= multiplicador) && (multiplicador <= 3))
                    multiplicador += 1;
                else
                    multiplicador = 1;
            }
            return senhaEncriptada;
        }

        public static string DecriptografarSenha(string senha)
        {
            int multiplicador;
            int c;
            if (String.IsNullOrEmpty(senha))
                return String.Empty;
            var decryptString = " !\"" + "#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLM" + "N" + "OPQRSTUVWXYZ" +
                                "[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            var encryptString = "~{[}u;Ce83KX%:VIm!|gs]_aL-QEOpx<UlzZjBq6#1($\\\"FS5H0'cM&>Po.NGA*Jr)Y Dv/t9kd?^fni,hR2Wy=`+4T@7wb";
            var tamanho = (senha).Length;
            if (1 <= tamanho && tamanho <= 3)
                multiplicador = 1;
            else if (4 <= tamanho && tamanho <= 6)
                multiplicador = 2;
            else if (7 <= tamanho && tamanho <= 9)
                multiplicador = 3;
            else
                multiplicador = 4;
            var senhaDesencriptada = "";
            for (c = 1; c <= tamanho; c++)
            {
                var deslocamento = c * multiplicador;
                var aux = senha.Substring((c - 1), 1);
                var posicao = encryptString.IndexOf(aux);
                posicao = posicao - deslocamento;
                posicao--;
                while (posicao < 0)
                    posicao = 95 + posicao;
                aux = decryptString.Substring(posicao, 1);
                senhaDesencriptada = senhaDesencriptada + aux;
                if (1 <= multiplicador && multiplicador <= 3)
                    multiplicador++;
                else
                    multiplicador = 1;
            }
            return senhaDesencriptada;
        }

    }
}
