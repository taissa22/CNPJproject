using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FechamentoAtmJec : Notifiable, IEntity, INotifiable
    {
        public int Id { get; private set; }

        public DateTime MesAnoFechamento { get; private set; }

        public DateTime DataFechamento { get; private set; }

        public short NumeroDeMeses { get; private set; }

        public bool Mensal { get; private set; }

        private FechamentoAtmJec()
        {
        }
    }

    public sealed class AgendamentoDeFechamentoAtmJec : Notifiable, IEntity, INotifiable
    {
        //AFAJ_ID      NUMBER(8,0)         No
        public int Id { get; private set; }

        //FAJ_ID        NUMBER(8,0)         No
        internal int FechamentoId { get; private set; }
        public FechamentoAtmJec Fechamento { get; private set; } = null!;

        //USR_COD_USUARIO	VARCHAR2(30 BYTE)	No
        internal string UsuarioId { get; private set; } = null!;
        public Usuario Usuario { get; private set; } = null!;

        //DAT_AGENDAMENTO	DATE	No
        public DateTime DataAgendamento { get; private set; }

        //DAT_INICIO_EXECUCAO	DATE	No
        public DateTime InicioDaExecucao { get; private set; } = DateTime.MinValue;

        //DAT_FIM_EXECUCAO	DATE	No
        public DateTime FimDaExecucao { get; private set; } = DateTime.MinValue;

        //STATUS	NUMBER(2,0)	No
        public short Status { get; private set; } = 0; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)

        //MSG_ERRO	CLOB	Yes
        public string? Erro { get; private set; }

        private HashSet<AgendamentoDeFechamentoAtmUfJec> indices = new HashSet<AgendamentoDeFechamentoAtmUfJec>();
        public IReadOnlyCollection<AgendamentoDeFechamentoAtmUfJec> Indices => indices;

        private AgendamentoDeFechamentoAtmJec()
        {
        }

        public static AgendamentoDeFechamentoAtmJec Criar(FechamentoAtmJec fechamento, DateTime dataAgendamento, Usuario usuario)
        {
            return new AgendamentoDeFechamentoAtmJec()
            {
                FechamentoId = fechamento.Id,
                Fechamento = fechamento,
                DataAgendamento = dataAgendamento,
                UsuarioId = usuario.Id,
                Usuario = usuario,
                Status = 0
            };
        }

        public void AddIndice(Indice indice, Estado estado)
        {
            if (indices.Any(x => x.Estado == estado.Id))
            {
                throw new InvalidOperationException($"Índice já presente para o estado '{ estado.Id }'");
            }

            this.indices.Add(AgendamentoDeFechamentoAtmUfJec.Criar(this, indice, estado));
        }
    }

    public sealed class AgendamentoDeFechamentoAtmUfJec : Notifiable, IEntity, INotifiable
    {
        //AFAJ_ID       NUMBER(8,0)         No
        public int Id { get; private set; }
        internal AgendamentoDeFechamentoAtmJec Agendamento { get; private set; } = null!;

        //COD_ESTADO    VARCHAR2(3 BYTE)    No
        public string Estado { get; private set; } = null!;

        //COD_INDICE    NUMBER(4,0)         No
        public int IndiceId { get; private set; }

        //IND_ACUMULADO CHAR(1 BYTE)        No
        public bool Acumulado { get; private set; }

        private AgendamentoDeFechamentoAtmUfJec()
        {
        }

        internal static AgendamentoDeFechamentoAtmUfJec Criar(AgendamentoDeFechamentoAtmJec agendamento, Indice indice, Estado estado)
        {
            return new AgendamentoDeFechamentoAtmUfJec()
            {
                Id = agendamento.Id,
                Agendamento = agendamento,
                Estado = estado.Id,
                IndiceId = indice.Id,
                Acumulado = indice.Acumulado.GetValueOrDefault()
            };
        }
    }
}