using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FechamentoAtmPex : Notifiable, IEntity, INotifiable
    {
        public int Id { get;  set; }

        public int CodSolicFechamento { get;  set; }

        public decimal ValDesvioPadrao { get;  set; }

        public DateTime DataFechamento { get;  set; }

        public int NumeroDeMeses { get;  set; }

        public FechamentoAtmPex()
        {
        }
    }

    public sealed class AgendamentoDeFechamentoAtmPex : Notifiable, IEntity, INotifiable
    {
        public int AgendamentoId { get; private set; }

        public int CodSolicFechamento { get; private set; }

        public decimal? ValDesvioPadrao { get; private set; }

        public int? NumeroDeMeses { get; private set; }

        public DateTime DataFechamento { get; private set; }

        internal string UsuarioId { get; private set; } = null!;

        public Usuario Usuario { get; private set; } = null!;

        public DateTime DataAgendamento { get; private set; }

        public DateTime InicioDaExecucao { get; private set; } = DateTime.MinValue;

        public DateTime FimDaExecucao { get; private set; } = DateTime.MinValue;

        public short Status { get; private set; } = 0; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)

        public string? Erro { get; private set; }

        private HashSet<AgendamentoDeFechamentoAtmUfPex> indices = new HashSet<AgendamentoDeFechamentoAtmUfPex>();
        public IReadOnlyCollection<AgendamentoDeFechamentoAtmUfPex> Indices => indices;

        public AgendamentoDeFechamentoAtmPex()
        {
        }

        public static AgendamentoDeFechamentoAtmPex Criar(FechamentoAtmPex fechamento, DateTime dataAgendamento, Usuario usuario)
        {
            return new AgendamentoDeFechamentoAtmPex()
            {
                CodSolicFechamento = fechamento.CodSolicFechamento,
                ValDesvioPadrao = fechamento.ValDesvioPadrao,
                NumeroDeMeses = fechamento.NumeroDeMeses,
                DataFechamento = fechamento.DataFechamento,
                DataAgendamento = dataAgendamento,
                UsuarioId = usuario.Id,
                Usuario = usuario,
                Status = 0
            };
        }

        public void AddIndice(AtmIndiceUfPadrao indice, Estado estado)
        {
            if (indices.Any(x => x.CodEstado == estado.Id))
            {
                throw new InvalidOperationException($"Índice já presente para o estado '{ estado.Id }'");
            }

            this.indices.Add(AgendamentoDeFechamentoAtmUfPex.Criar(this, indice, estado));
        }
    }

    public sealed class AgendamentoDeFechamentoAtmUfPex : Notifiable, IEntity, INotifiable
    {

        public int Id { get; private set; }

        public int AgendamentoId { get; private set; }

        //internal AgendamentoDeFechamentoAtmPex Agendamento { get; private set; }

        public string CodEstado { get; private set; }

        public int IndiceId { get; private set; }

        public int TipoProcessoId { get; set; }

        public AgendamentoDeFechamentoAtmUfPex()
        {
        }

        internal static AgendamentoDeFechamentoAtmUfPex Criar(AgendamentoDeFechamentoAtmPex agendamento, AtmIndiceUfPadrao indice, Estado estado)
        {
            return new AgendamentoDeFechamentoAtmUfPex()
            {
                AgendamentoId = agendamento.AgendamentoId,
                //Agendamento = agendamento,
                TipoProcessoId = TipoProcesso.PEX.Id,
                CodEstado = estado.Id,
                IndiceId = indice.CodIndice
            };
        }
    }
}