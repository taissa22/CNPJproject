using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
#nullable enable
namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class MotivoProvavelZero : Notifiable, IEntity, INotifiable
    {
        private MotivoProvavelZero()
        {
        }

        public static MotivoProvavelZero Criar(string descricao)
        {
            MotivoProvavelZero motivo = new MotivoProvavelZero();
            motivo.Descricao = descricao;

            motivo.Validate();
            return motivo;
        }

        public void Atualizar(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;

            Validate();
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }

        public void Validate()
        {
           
            if (String.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Descricao não pode ser vazio");
            }
        }
    }
}