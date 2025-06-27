using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AudienciaDoProcesso : Notifiable, IEntity, INotifiable
    {
        private AudienciaDoProcesso()
        {
        }

        public static AudienciaDoProcesso Criar(Processo processo, DateTime dataAudiencia,
            string observacaoAudiencia, TipoAudiencia tipoAudiencia, int ProxSequencial, int advogadoEscritorioId, int escritorioId)
        {
            var audienciaDoProcesso = new AudienciaDoProcesso()
            {
                ProcessoId = processo.Id,
                Processo = processo,
                Sequencial = ProxSequencial,
                DataAudiencia = dataAudiencia,
                HoraAudiencia = dataAudiencia,
                Comentario = observacaoAudiencia,
                TipoAudienciaId = tipoAudiencia.Id,
                TipoAudiencia = tipoAudiencia,
                AdvogadoEscritorioId = advogadoEscritorioId,
                EscritorioId = escritorioId
                // Ver como entra o Status de um novo registro no sistem
            };

            audienciaDoProcesso.Validate(dataAudiencia);
            return audienciaDoProcesso;
        }

        public void Atualizar(AdvogadoDoEscritorio advogadoEscritorio, TipoAudiencia tipoAudiencia,
            DateTime? dataAudiencia, DateTime? horaAudiencia, string comentario, Escritorio escritorio,
            Preposto preposto)
        {
            this.AtualizarAdvogadoEscritorio(advogadoEscritorio);
            this.AtualizarAdvogadoEscritorioId(advogadoEscritorio.Id);
            this.AtualizarComentario(comentario);
            this.AtualizarEscritorio(escritorio);
            this.AtualizarEscritorioId(escritorio.Id);
            this.AtualizarDadosDaAudiencia((DateTime)dataAudiencia, (DateTime)horaAudiencia, tipoAudiencia, preposto);

            Validate(dataAudiencia);
        }

        public void AtualizarAgenda(AdvogadoDoEscritorio advogadoEscritorio, Escritorio escritorio, Preposto preposto) {
            AdvogadoEscritorioId = advogadoEscritorio?.Id;
            AdvogadoEscritorio = advogadoEscritorio;
            EscritorioId = escritorio?.Id;
            Escritorio = escritorio;
            Preposto = preposto;
            PrepostoId = preposto?.Id;            
        }

        public int Sequencial { get; private set; }

        public int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        internal int? PrepostoId { get; private set; }
        public Preposto Preposto { get; private set; }

        //public int? ProfissionalId { get; private set; } = null;
        //public Profissional Profissional { get; private set; }
        
        public int? AdvogadoEscritorioId { get; private set; }
        public AdvogadoDoEscritorio AdvogadoEscritorio { get; private set; }
        
        public DateTime DataAudiencia { get; private set; }
        
        public DateTime HoraAudiencia { get; private set; }
       
        internal int? SituacaoId { get; private set; }
       
        public StatusAudiencia Situacao { get; private set; }
       
        public string Comentario { get; private set; }
    
        internal int TipoAudienciaId { get; private set; }
      
        public TipoAudiencia TipoAudiencia { get; private set; }
       
        public int? EscritorioId { get; private set; }
      
        public Escritorio Escritorio { get; private set; }

        private void Validate(DateTime? dataAudiencia)
        {
            if (dataAudiencia.HasValue && dataAudiencia < Processo.DataDistribuicao)
            {
                AddNotification(nameof(dataAudiencia), "A data da audiência não pode ser menor que a data de distribuição do processo");
            }
        }

        public void AtualizarDadosDaAudiencia(DateTime dataAudiencia, DateTime horaAudiencia, TipoAudiencia tipoAudiencia, Preposto preposto)
        {
            if (AtualizouAlgumaPropriedade())
                AtualizarPreposto(null);
            else
                AtualizarPreposto(preposto);

            if (dataAudiencia != DateTime.MinValue)
                DataAudiencia = dataAudiencia;
            if (horaAudiencia != DateTime.MinValue)
                HoraAudiencia = horaAudiencia;
            if (tipoAudiencia != null)
                TipoAudiencia = tipoAudiencia;

            bool AtualizouAlgumaPropriedade()
            {
                bool alterouData = DataAudiencia.Date != dataAudiencia.Date;
                bool alterouHora = HoraAudiencia.Hour != horaAudiencia.Hour || (HoraAudiencia.Hour == horaAudiencia.Hour && HoraAudiencia.Minute != horaAudiencia.Minute);
                bool alterouTipo = TipoAudiencia.Id != tipoAudiencia.Id;
                return alterouData || alterouHora || alterouTipo;
            }
        }

        private void AtualizarComentario(string comentario)
        {
            if (!string.IsNullOrEmpty(Comentario) && Comentario.Length > 4000)
                new Exception("O comentário não pode ter mais de 4000 caracteres");

            Comentario = comentario;
        }

        private void AtualizarEscritorioId(int? escritorioId)
        {
            if (!escritorioId.HasValue || (escritorioId.HasValue && escritorioId.Value == 0))
                EscritorioId = null;
            else
                EscritorioId = escritorioId;
        }

        private void AtualizarAdvogadoEscritorioId(int? advogadoId)
        {
            if (!advogadoId.HasValue || (advogadoId.HasValue && advogadoId.Value == 0))
                AdvogadoEscritorio = null;
            else
                AdvogadoEscritorioId = advogadoId;
        }

        private void AtualizarEscritorio(Escritorio escritorio)
        {
            EscritorioId = escritorio.Id;
            Escritorio = escritorio;
        }

        private void AtualizarPreposto(Preposto preposto)
        {
            if (preposto != null)
            {
                PrepostoId = preposto.Id;
                Preposto = preposto;
            }
        }

        private void AtualizarAdvogadoEscritorio(AdvogadoDoEscritorio advogadoDoEscritorio)
        {
            AdvogadoEscritorio = advogadoDoEscritorio;
            EscritorioId = advogadoDoEscritorio.EscritorioId;
            Escritorio = advogadoDoEscritorio.Escritorio;
        }
    }
}