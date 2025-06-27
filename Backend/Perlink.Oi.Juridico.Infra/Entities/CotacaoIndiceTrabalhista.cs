using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{    
    public sealed class CotacaoIndiceTrabalhista : Notifiable, IEntity, INotifiable
    {
        public CotacaoIndiceTrabalhista()
        {

        }
        public static CotacaoIndiceTrabalhista Criar(DateTime dataCorrecao, DateTime dataBase, Decimal valorCotacao )
        {
            CotacaoIndiceTrabalhista cotacaoIndiceTrabalhista = new CotacaoIndiceTrabalhista();

            cotacaoIndiceTrabalhista.DataCorrecao = dataCorrecao;
            cotacaoIndiceTrabalhista.DataBase = dataBase;
            cotacaoIndiceTrabalhista.ValorCotacao = valorCotacao;
            cotacaoIndiceTrabalhista.Validate();
            return cotacaoIndiceTrabalhista;
        }
        public void Atualizar(DateTime dataCorrecao, DateTime dataBase, Decimal valorCotacao)
        {
            DataCorrecao = dataCorrecao;
            DataBase = dataBase;
            ValorCotacao = valorCotacao;
            Validate();          
        }

        public DateTime DataCorrecao { get; private set; }
        public DateTime DataBase { get; private set; }
        public Decimal ValorCotacao { get; private set; }

        public void Validate()
        {
            if (DataCorrecao == null)
            {
                AddNotification(nameof(DataCorrecao), "Campo requerido");
            }
            if (DataBase == null)
            {
                AddNotification(nameof(DataCorrecao), "Campo requerido");
            }            
        }

    }
}
