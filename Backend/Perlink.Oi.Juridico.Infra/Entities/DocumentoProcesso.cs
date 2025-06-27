using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities {
    public sealed class DocumentoProcesso : Notifiable, IEntity, INotifiable {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private DocumentoProcesso() {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        public static DocumentoProcesso Criar(Processo processo, TipoDocumento tipoDocumento, DataString? comentario, Usuario usuarioCadastro) {
            var documentoProcesso = new DocumentoProcesso() {
                ProcessoId = processo.Id,
                Processo = processo,

                TipoDocumentoId = tipoDocumento.Id,
                TipoDocumento = tipoDocumento,             
               
                DataCadastro = DateTime.Now,
                Comentario = comentario,                

                UsuarioCadastroId = usuarioCadastro.Id,
                UsuarioCadastro = usuarioCadastro
            };
            return documentoProcesso;
        }

        public int Id { get; private set; }

        internal int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        internal int TipoDocumentoId { get; private set; }
        public TipoDocumento TipoDocumento { get; private set; }

        public string Nome { get; private set; }

        public DateTime? DataRecebimento { get; private set; }
        public DateTime? DataPrevistaRetorno { get; private set; }
        public DateTime? DataVinculo { get; private set; }
        public DateTime? DataCadastro { get; private set; }
        public string? Comentario { get; private set; }

        internal string UsuarioVinculoId { get; private set; }
        public Usuario UsuarioVinculo { get; private set; }

        internal string UsuarioCadastroId { get; private set; }
        public Usuario UsuarioCadastro { get; private set; }

        public int? AutoDocumentoGedId { get; private set; }

        public void AtualizarAutoDocumentoGed(int autoDocumentoGedId) {
            AutoDocumentoGedId = autoDocumentoGedId;
        }

        public void AtulizarDocumentoAnexado(string nomeDocumento, Usuario usuarioVinculado) {
            UsuarioVinculoId = usuarioVinculado.Id;
            UsuarioVinculo = usuarioVinculado;
            Nome = nomeDocumento;
            DataVinculo = DateTime.Now;            
        }
    }
}
