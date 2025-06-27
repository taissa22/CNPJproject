using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class DecisaoEvento : Notifiable, IEntity, INotifiable
    {
        private DecisaoEvento()
        {
        }

        public static DecisaoEvento Criar(int? decisaoId,
                                          int eventoId, 
                                          string descricao,
                                          bool? riscoPerda,
                                          string perdaPotencial,
                                          bool decisaoDefault,
                                          bool? reverCalculo)
        {
            DecisaoEvento decisao = new DecisaoEvento();
            decisao.Id = decisaoId;
            decisao.EventoId = eventoId;
            decisao.Descricao = descricao;
            decisao.RiscoPerda = riscoPerda;
            decisao.PerdaPotencial = perdaPotencial;
            decisao.DecisaoDefault = decisaoDefault;
            decisao.ReverCalculo = reverCalculo;

            decisao.Validate();
            return decisao;
        }

        public void Atualizar(int decisaoEventoId,
                              int eventoId,
                              string descricao,
                              bool? riscoPerda,
                              string perdaPotencial,
                              bool decisaoDefault,
                              bool? reverCalculo)
        {
            this.EventoId = eventoId;
            this.Id = decisaoEventoId;
            this.Descricao = descricao;
            this.RiscoPerda = riscoPerda;
            this.PerdaPotencial = perdaPotencial;
            this.DecisaoDefault = decisaoDefault;
            this.ReverCalculo = reverCalculo;
            Validate();
        }

        public void AtualizarDecisaoDefault(int decisaoEventoId,
                             int eventoId,
                             bool decisaoDefault)
        {
            this.EventoId = eventoId;
            this.Id = decisaoEventoId;
            this.DecisaoDefault = decisaoDefault;
            Validate();
        }


        public int? Id { get; private set; }
        public int EventoId { get; private set; }
        public string Descricao { get; private set; }
        public bool? RiscoPerda { get; private set; }      
        public string? PerdaPotencial { get; private set; }
        public bool DecisaoDefault { get; private set; }
        public bool? ReverCalculo { get; private set; }



        public void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo requerido");
            }
        }
    }
}