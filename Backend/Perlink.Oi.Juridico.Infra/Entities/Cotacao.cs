using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Cotacao : Notifiable, IEntity, INotifiable
    {
        private Cotacao()
        {
        }

        /// <summary>
        /// Instancia uma Ação - <c>Tributária</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
       
        public static Cotacao Criar(Indice indice, DateTime dataCotacao, decimal valor, decimal? valorAcumulado)
        {
            var cotacao = new Cotacao();

            var month = dataCotacao.Month;
            var year = dataCotacao.Year;
            DateTime dataFormatada = new DateTime(year, month, 1);

            cotacao.Id = indice.Id;
            cotacao.DataCotacao = dataFormatada;
            cotacao.Valor = valor;
            cotacao.ValorAcumulado = valorAcumulado;           

            cotacao.Validate();
            return cotacao;
        }

        public int Id { get; private set; }

        public Indice Indice { get; private set; }

        public DateTime DataCotacao { get; private set; }

        public decimal Valor { get; private set; }
        public decimal? ValorAcumulado { get; private set; }

        private void Validate()
        {
            //if (Descricao.Length > 50)
            //{
            //    AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            //}
        }
       
        public void Atualizar(decimal valor, decimal? valorAcumulado)
        {
            Valor = valor;
            ValorAcumulado = valorAcumulado;
            Validate();
        }
    }
}