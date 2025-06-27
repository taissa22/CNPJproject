using Flunt.Notifications;
using Flunt.Validations;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias
{
    public class AtualizarTipoAudienciaCommand : Notifiable, ICommand
    {
        public AtualizarTipoAudienciaCommand() { }

        public AtualizarTipoAudienciaCommand(long? codigoTipoAudiencia, string descricao, string sigla, TipoProcessoEnum tipoProcesso, bool estaAtivo)
        {
            CodigoTipoAudiencia = codigoTipoAudiencia;
            Descricao = descricao;
            Sigla = sigla;
            EstaAtivo = estaAtivo;
            TipoProcesso = tipoProcesso;
        }

        public long? CodigoTipoAudiencia { get; set; }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public bool EstaAtivo { get; set; }

        public TipoProcessoEnum? TipoProcesso { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(CodigoTipoAudiencia, "AtualizarTipoAudienciaCommand.CodigoTipoAudiencia", "Código Tipo Audiência é obrigatório.")
            );
        }
    }
}
