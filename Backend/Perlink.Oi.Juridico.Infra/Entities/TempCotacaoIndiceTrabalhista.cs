using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class TempCotacaoIndiceTrab : Notifiable, IEntity, INotifiable
    {
        public DateTime DataCorrecao { get; private set; }
        public DateTime DataBase { get; private set; }
        public Decimal ValorCotacao { get; private set; }

        public TempCotacaoIndiceTrab()
        {

        }

        public static TempCotacaoIndiceTrab Criar(DateTime dataCorrecao, DateTime dataBase, Decimal valorCotacao)
        {
            TempCotacaoIndiceTrab cotacaoIndiceTrabalhista = new TempCotacaoIndiceTrab();

            cotacaoIndiceTrabalhista.DataCorrecao = dataCorrecao;
            cotacaoIndiceTrabalhista.DataBase = dataBase;
            cotacaoIndiceTrabalhista.ValorCotacao = valorCotacao;
            return cotacaoIndiceTrabalhista;
        }
    }
}
