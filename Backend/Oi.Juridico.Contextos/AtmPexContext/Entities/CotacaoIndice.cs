using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.AtmPexContext.Entities
{
    public class CotacaoIndice
    {
        public int IndiceId { get; private set; }
        public DateTime DataCotacao { get; private set; }
        public decimal ValorCotacao { get; private set; }
        public decimal? ValorCotacaoAcumulado { get; private set; } 
    }
}