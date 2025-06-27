using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoTipoPrazoConsumidor : Notifiable, IEntity, INotifiable
    {
        public MigracaoTipoPrazoConsumidor()
        {

        }

        public MigracaoTipoPrazoConsumidor(int codTipoPrazoCivel, int codTipoPrazoCivelEstrat)
        {
            CodTipoPrazoCivel = codTipoPrazoCivel;
            CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat;
        }

        public static MigracaoTipoPrazoConsumidor CriarMigracaoTipoPrazoConsumidor(int codTipoPrazoCivel, int? codTipoPrazoCivelEstrat)
        {
            var MigracaoTipoPrazoConsumidor = new MigracaoTipoPrazoConsumidor()
            {
                CodTipoPrazoCivel = codTipoPrazoCivel,
                CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat,
            };

            MigracaoTipoPrazoConsumidor.Validate();
            return MigracaoTipoPrazoConsumidor;
        }

        public void AtualizarMigracaoTipoPrazoConsumidor(int codTipoPrazoCivel, int codTipoPrazoCivelEstrat)
        {
            CodTipoPrazoCivel = codTipoPrazoCivel;
            CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat;
        }

        private void Validate()
        {
            if (CodTipoPrazoCivel > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivel), "O campo CodTipoPrazoCivel deve ser informado.");
            }

            if (CodTipoPrazoCivelEstrat > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivelEstrat), "O campo CodTipoPrazoCivelEstrat deve ser informado.");
            }
        }

        public int? CodTipoPrazoCivel { get; set; }
        public int? CodTipoPrazoCivelEstrat { get; set; }
    }
}
