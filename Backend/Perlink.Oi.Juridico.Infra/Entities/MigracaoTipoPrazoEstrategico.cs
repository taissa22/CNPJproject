using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoTipoPrazoEstrategico : Notifiable, IEntity, INotifiable
    {
        public MigracaoTipoPrazoEstrategico()
        {

        }

        public MigracaoTipoPrazoEstrategico(int codTipoPrazoCivelEstrat, int codTipoPrazoCivelCons)
        {
            CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat;
            CodTipoPrazoCivelCons = codTipoPrazoCivelCons;
        }

        public static MigracaoTipoPrazoEstrategico CriarMigracaoTipoPrazoEstrategico(int codTipoPrazoCivelEstrat, int? codTipoPrazoCivelCons)
        {
            var MigracaoTipoPrazoEstrategico = new MigracaoTipoPrazoEstrategico()
            {
                CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat,
                CodTipoPrazoCivelCons = codTipoPrazoCivelCons,
            };

            MigracaoTipoPrazoEstrategico.Validate();
            return MigracaoTipoPrazoEstrategico;
        }

        public void AtualizarMigracaoTipoPrazoEstrategico(int codTipoPrazoCivelEstrat, int codTipoPrazoCivelCons)
        {
            CodTipoPrazoCivelEstrat = codTipoPrazoCivelEstrat;
            CodTipoPrazoCivelCons = codTipoPrazoCivelCons;
        }

        private void Validate()
        {
            if (CodTipoPrazoCivelEstrat > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivelEstrat), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }

            if (CodTipoPrazoCivelCons > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivelCons), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }
        }

        public int? CodTipoPrazoCivelEstrat { get; set; }
        public int? CodTipoPrazoCivelCons { get; set; }
    }
}
