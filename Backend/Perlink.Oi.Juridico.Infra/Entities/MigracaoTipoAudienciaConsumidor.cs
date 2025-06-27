using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoTipoAudienciaConsumidor : Notifiable, IEntity, INotifiable
    {
        public MigracaoTipoAudienciaConsumidor()
        {
        }

        public MigracaoTipoAudienciaConsumidor(int codTipoAudCivel, int codTipoAudCivelEstrat)
        {
            CodTipoAudCivel = codTipoAudCivel;
            CodTipoAudCivelEstrat = codTipoAudCivelEstrat;
        }

        public static MigracaoTipoAudienciaConsumidor CriarMigracaoTipoAudienciaConsumidor(int codTipoAudCivel, int? codTipoAudCivelEstrat)
        {
            var MigracaoTipoAudienciaConsumidor = new MigracaoTipoAudienciaConsumidor()
            {
                CodTipoAudCivel = codTipoAudCivel,
                CodTipoAudCivelEstrat = codTipoAudCivelEstrat,
            };

            MigracaoTipoAudienciaConsumidor.Validate();
            return MigracaoTipoAudienciaConsumidor;
        }

        public void AtualizarMigracaoTipoAudienciaConsumidor(int codTipoAudCivel, int codTipoAudCivelEstrat)
        {
            CodTipoAudCivel = codTipoAudCivel;
            CodTipoAudCivelEstrat = codTipoAudCivelEstrat;
        }
        private void Validate()
        {
            if (CodTipoAudCivelEstrat > 0)
            {
                AddNotification(nameof(CodTipoAudCivelEstrat), "O campo CodTipoAudCivelEstrat deve ser informado.");
            }

            if (CodTipoAudCivel > 0)
            {
                AddNotification(nameof(CodTipoAudCivel), "O campo CodTipoAudCivel deve ser informado.");
            }
        }

        public int? CodTipoAudCivel { get; set; }
        public int? CodTipoAudCivelEstrat { get; set; }
    }
}
