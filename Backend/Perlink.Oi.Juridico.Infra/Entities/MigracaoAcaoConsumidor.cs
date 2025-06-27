using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;


namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class MigracaoAcaoConsumidor : Notifiable, IEntity, INotifiable
    {
        public MigracaoAcaoConsumidor()
        {

        }

        public static MigracaoAcaoConsumidor CriarMigracaoAcaoConsumidor(int codAcaoCivel, int codAcaoCivelEstrategico)
        {

            var migracaoAcao = new MigracaoAcaoConsumidor()
            {
                CodAcaoCivel = codAcaoCivel,
                CodAcaoCivelEstrategico = codAcaoCivelEstrategico
            };

            migracaoAcao.Validate();
            return migracaoAcao;
        }

        public void AtualizarAcaoConsumidor(int codAcaoCivel, int codAcaoCivelEstrategico)
        {
            CodAcaoCivel = codAcaoCivel;
            CodAcaoCivelEstrategico = codAcaoCivelEstrategico;
        }


        private void Validate()
        {
            if (CodAcaoCivel > 0)
            {
                AddNotification(nameof(CodAcaoCivel), "O campo CodAcaoEstrategico deve ser informado.");
            }

            if (CodAcaoCivelEstrategico > 0)
            {
                AddNotification(nameof(CodAcaoCivelEstrategico), "O campo CodAcaoConsumidor deve ser informado.");
            }
        }

        public int CodAcaoCivel { get; set; }
        public int CodAcaoCivelEstrategico { get; set; }
    }
}
