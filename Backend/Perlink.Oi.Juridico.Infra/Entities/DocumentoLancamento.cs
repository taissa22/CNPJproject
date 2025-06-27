using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using TipoDocumentoLancamentoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoDocumentoLancamento;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities {
    public sealed class DocumentoLancamento : Notifiable, IEntity, INotifiable {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private DocumentoLancamento() {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        // TODO: TipoDeDocumentoLancamento do JEC
        //public static DocumentoLancamento Criar(Processo processo, TipoDocumento tipoDocumento, DataString nome,
        public static DocumentoLancamento Criar(Processo processo, TipoDocumentoLancamentoEnum tipoDocumentoLancamento, string nomeDocumento,
            Lancamento lancamento, Usuario usuarioVinculado) {
            var documentoLancamento = new DocumentoLancamento() {               
                ProcessoId = processo.Id,
                Processo = processo,

                Nome = nomeDocumento,
                DataVinculo = DateTime.Now,

                TipoDocumentoId = tipoDocumentoLancamento.Id,

                SequencialLancamento = lancamento.Sequencial,
                Lancamento = lancamento,

                UsuarioVinculadoId = usuarioVinculado.Id,
                UsuarioVinculado = usuarioVinculado
            };
            return documentoLancamento;
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }
        public DateTime? DataVinculo { get; private set; }

        internal int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public int TipoDocumentoId { get; private set; }
        public TipoDocumentoLancamentoEnum TipoDocumentoLancamento() => TipoDocumentoLancamentoEnum.PorId(TipoDocumentoId);

        internal int SequencialLancamento { get; private set; }
        public Lancamento Lancamento { get; private set; }

        internal string UsuarioVinculadoId { get; private set; }
        public Usuario UsuarioVinculado { get; private set; }

        public int? AutoDocumentoGedId { get; private set; }

        public void AtualizarAutoDocumentoGed(int autoDocumentoGedId) {
            AutoDocumentoGedId = autoDocumentoGedId;
        }

        public void AtulizarDocumentoAnexado(string nomeDocumento, Usuario usuarioVinculado) {
            UsuarioVinculado = usuarioVinculado;
            Nome = nomeDocumento;
            DataVinculo = DateTime.Now;
        }
    }
}
