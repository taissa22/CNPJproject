using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class BaseDeCalculo : Notifiable, IEntity, INotifiable
    {

        private BaseDeCalculo()
        {
        }

        public static BaseDeCalculo Criar(string descricao)
        {
            BaseDeCalculo baseDeCalculo = new BaseDeCalculo();
            baseDeCalculo.Descricao = descricao;           

            baseDeCalculo.Validate();
            return baseDeCalculo;
        }

        public void Atualizar(string descricao, bool indBaseInicial)
        {
            Descricao = descricao;
            IndBaseInicial = indBaseInicial;

            Validate();
        }    

        public void DesmarcarBaseInicial()
        {
            IndBaseInicial = false;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }
        }

        public int Codigo { get; private set; }

        public string Descricao { get; private set; }

        public bool IndBaseInicial { get; set; }
    }
}
