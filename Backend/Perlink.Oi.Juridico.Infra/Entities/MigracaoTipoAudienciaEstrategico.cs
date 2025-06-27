using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoTipoAudienciaEstrategico : Notifiable, IEntity, INotifiable
    {
        public MigracaoTipoAudienciaEstrategico()
        {
        }

        public MigracaoTipoAudienciaEstrategico(int codTipoAudienciaCivelEstrat, int codTipoAudienciaCivelCons)
        {
            CodTipoAudienciaCivelEstrat = codTipoAudienciaCivelEstrat;
            CodTipoAudienciaCivelCons = codTipoAudienciaCivelCons;
        }
        public static MigracaoTipoAudienciaEstrategico CriarMigracaoTipoAudienciaEstrategico(int codTipoAudienciaCivelEstrat, int? codTipoAudienciaCivelCons)
        {
            var MigracaoTipoAudienciaEstrategico = new MigracaoTipoAudienciaEstrategico()
            {
                CodTipoAudienciaCivelEstrat = codTipoAudienciaCivelEstrat,
                CodTipoAudienciaCivelCons = codTipoAudienciaCivelCons,
            };

            MigracaoTipoAudienciaEstrategico.Validate();
            return MigracaoTipoAudienciaEstrategico;
        }
        public void AtualizarMigracaoTipoAudienciaEstrategico(int codTipoAudienciaCivelEstrat, int codTipoAudienciaCivelCons)
        {
            CodTipoAudienciaCivelEstrat = codTipoAudienciaCivelEstrat;
            CodTipoAudienciaCivelCons = codTipoAudienciaCivelCons;
        }
        private void Validate()
        {
            if (CodTipoAudienciaCivelEstrat > 0)
            {
                AddNotification(nameof(CodTipoAudienciaCivelEstrat), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }

            if (CodTipoAudienciaCivelCons > 0)
            {
                AddNotification(nameof(CodTipoAudienciaCivelCons), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }
        }

        public int? CodTipoAudienciaCivelEstrat { get; set; }
        public int? CodTipoAudienciaCivelCons { get; set; }
    }
}
