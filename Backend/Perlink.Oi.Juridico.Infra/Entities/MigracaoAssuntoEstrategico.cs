using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoAssuntoEstrategico : Notifiable, IEntity, INotifiable
    {
        public MigracaoAssuntoEstrategico()
        {

        }

        public MigracaoAssuntoEstrategico(int codAssuntoCivelCons, Assunto Assunto)
        {
            CodAssuntoCivelCons = codAssuntoCivelCons;

        }

        public static MigracaoAssuntoEstrategico CriarMigracaoAssuntoEstrategico(int codAssuntoCivelEstrat, int codAssuntoCivelCons)
        {
            var migracaoAssuntoEstrategico = new MigracaoAssuntoEstrategico()
            {
                CodAssuntoCivelEstrat = codAssuntoCivelEstrat,
                CodAssuntoCivelCons = codAssuntoCivelCons,
            };

            return migracaoAssuntoEstrategico;
        }

        public void AtualizarMigracaoAssuntoEstrategico(int codAssuntoCivelEstrat, int codAssuntoCivelCons)
        {
            CodAssuntoCivelEstrat = codAssuntoCivelEstrat;
            CodAssuntoCivelCons = codAssuntoCivelCons;

        }

        public int CodAssuntoCivelEstrat { get; set; }
        public int CodAssuntoCivelCons { get; set; }
    }
}