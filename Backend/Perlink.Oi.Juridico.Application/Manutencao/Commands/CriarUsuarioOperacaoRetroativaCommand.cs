using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarUsuarioOperacaoRetroativaCommand : Validatable, IValidatable
    {
        public string CodUsuario { get; set; }
        public int LimiteAlteracao { get; set; }
        public int TipoProcesso { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(CodUsuario))
            {
                AddNotification(nameof(CodUsuario), "O campo deve ser preenchido");
            }

            if (LimiteAlteracao <= 0)
            {
                AddNotification(nameof(LimiteAlteracao), "O campo deve ser preenchido");
            }                        
        }
    }
}
