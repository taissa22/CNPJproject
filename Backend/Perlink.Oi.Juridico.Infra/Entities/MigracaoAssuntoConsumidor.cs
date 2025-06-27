using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
  public class MigracaoAssunto : Notifiable, IEntity, INotifiable
    
    {
        public MigracaoAssunto()
        {

        }     

        public static MigracaoAssunto CriarMigracaoAssuntoConsumidor(int codAssuntoCivel, int codAssuntoCivelEstrategico)
        {
            var migracaoAssunto = new MigracaoAssunto()
            {
                CodAssuntoCivel = codAssuntoCivel,
                CodAssuntoCivelEstrategico = codAssuntoCivelEstrategico,               
            };
     
            return migracaoAssunto;
        }

        public void AtualizarMigracaoAssuntoConsumidor(int codAssuntoCivel, int codAssuntoCivelEstrategico)
        {
            CodAssuntoCivel = codAssuntoCivel;
            CodAssuntoCivelEstrategico = codAssuntoCivelEstrategico;          

        }


        public int CodAssuntoCivel { get; set; }
        public int CodAssuntoCivelEstrategico { get; set; }
        
    }
}
