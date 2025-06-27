using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class UsuarioOperacaoRetroativa : Notifiable, IEntity, INotifiable
    {
        private UsuarioOperacaoRetroativa()
        {
        }


        public static UsuarioOperacaoRetroativa Criar(string codUsuario, int limiteAlteracao, int tipoProcesso)
        {
            UsuarioOperacaoRetroativa operacao = new UsuarioOperacaoRetroativa();
            operacao.CodUsuario = codUsuario;
            operacao.LimiteAlteracao = limiteAlteracao;
            operacao.TipoProcesso = tipoProcesso;

            operacao.Validate();
            return operacao;
        }

        public void Atualizar(string codUsuario, int limiteAlteracao , int tipoProcesso)
        {
            CodUsuario = codUsuario;
            LimiteAlteracao = limiteAlteracao;
            TipoProcesso = tipoProcesso;

            Validate();
        }
        private void Validate()
        {
            if (string.IsNullOrEmpty(CodUsuario))
            {
                AddNotification(nameof(CodUsuario), "Campo Requerido.");
            }

            if (LimiteAlteracao <= 0)
            {
                AddNotification(nameof(CodUsuario), "Campo Requerido.");
            }

            if (TipoProcesso <= 0 )
            {
                AddNotification(nameof(CodUsuario), "Campo Requerido.");
            }
        }

        public string CodUsuario { get; private set; } 
        public int LimiteAlteracao { get; private set; }
        public int TipoProcesso { get; private set; }
        public Usuario Usuario { get; private set; }
    }
}