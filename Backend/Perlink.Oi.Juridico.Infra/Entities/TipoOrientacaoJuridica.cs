using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoOrientacaoJuridica : Notifiable, IEntity, INotifiable
    {
        private TipoOrientacaoJuridica()
        {
        }

        public static TipoOrientacaoJuridica Criar(string descricao)
        {
            var tipoOrientacaoJuridica = new TipoOrientacaoJuridica();


            tipoOrientacaoJuridica.Descricao = descricao;
            

            tipoOrientacaoJuridica.Validate();
            return tipoOrientacaoJuridica;
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }


        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }
        }

        public void Atualizar(string descricao)
        {
            Descricao = descricao;
            Validate();
        }

    }
}
