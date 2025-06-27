using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class FechamentoCCMedia : Notifiable, IEntity, INotifiable
    {
        private FechamentoCCMedia()
        {
        }

        //public FechamentoCCMedia(int id, DateTime dataFechamento, int numeroMeses, bool indMensal, DateTime? dataIndMensal,
        //    bool indBaseGerada, DateTime dataGeracao, string usuarioId, Usuario usuario, int empresaCentralizadoraId, EmpresaCentralizadora empresaCentralizadora,
        //    decimal? valorCorte, decimal percentualHaitCut, long solicitacaoFechamentoContingencia)
        //{
        //    Id = id;
        //    DataFechamento = dataFechamento;
        //    NumeroMesesMediaHistorica = numeroMeses;
        //    IndMensal = indMensal;
        //    DataIndMensal = dataIndMensal;
        //    IndBaseGerada = indBaseGerada;
        //    DataGeracao = dataGeracao;
        //    UsuarioId = usuarioId;
        //    Usuario = usuario;
        //    EmpresaCentralizadoraId = empresaCentralizadoraId;
        //    EmpresaCentralizadora = empresaCentralizadora;
        //    ValorCorte = valorCorte;
        //    PercentualHairCut = percentualHaitCut;
        //    SolicitacaoFechamentoContingencia = solicitacaoFechamentoContingencia;
        //}

        public int Id { get; private set; }
        public DateTime DataFechamento { get; private set; }
        public int NumeroMesesMediaHistorica { get; private set; }
        public bool IndMensal { get; private set; }
        public DateTime? DataIndMensal { get; private set; }
        public bool IndBaseGerada { get; private set; }
        public DateTime DataGeracao { get; private set; }
        internal string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        internal int EmpresaCentralizadoraId { get; private set; }
        public EmpresaCentralizadora EmpresaCentralizadora { get; private set; }
        internal long EmpresaCentralizadoraAgenFechAutoId { get; private set; }
        public EmpresaCentralizadoraAgendamentoFechAuto EmpresaCentralizadoraAgendamentoFechAuto { get; private set; }
        public decimal? ValorCorte { get; private set; }
        public decimal PercentualHairCut { get; private set; }
        public long SolicitacaoFechamentoContingencia { get; set; }
        public string EmpresasParticipantes { get; set; }
        public bool FechamentoGerado { get; set; }
    }
}