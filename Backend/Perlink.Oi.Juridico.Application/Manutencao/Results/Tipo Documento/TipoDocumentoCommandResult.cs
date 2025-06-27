using Perlink.Oi.Juridico.Infra.Enums;
using TipoDePrazo = Perlink.Oi.Juridico.Infra.Entities.TipoPrazo;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento
{
    public class TipoDocumentoCommandResult
    {
        public TipoDocumentoCommandResult(int id, string descricao, bool marcadoCriacaoProcesso, bool ativo, int codTipoProcesso, int? codTipoPrazo, TipoDePrazo tipoPrazo, bool indRequerDatAudiencia, bool indPrioritarioFila, bool indDocumentoProtocolo, bool indDocumentoApuracao, bool indEnviarAppPreposto, bool ativoDePara, int? idMigracao, string descricaoMigracao)
        {
            Id = id;
            Descricao = descricao;
            MarcadoCriacaoProcesso = marcadoCriacaoProcesso;
            Ativo = ativo;
            CodTipoProcesso = codTipoProcesso;
            CodTipoPrazo = codTipoPrazo;
            TipoPrazo = tipoPrazo;
            IndRequerDatAudiencia = indRequerDatAudiencia;
            IndPrioritarioFila = indPrioritarioFila;
            IndDocumentoProtocolo = indDocumentoProtocolo;
            IndDocumentoApuracao = indDocumentoApuracao;
            IndEnviarAppPreposto = indEnviarAppPreposto;
            AtivoDePara = ativoDePara;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public bool MarcadoCriacaoProcesso { get; private set; }
        public bool Ativo { get; private set; }
        public int CodTipoProcesso { get; private set; }
        public TipoProcesso TipoProcesso { get { return TipoProcesso.PorId(CodTipoProcesso); } }
        public int? CodTipoPrazo { get; private set; }
        public TipoDePrazo TipoPrazo { get; private set; }
        public bool IndRequerDatAudiencia { get; private set; }
        public bool IndPrioritarioFila { get; private set; }
        public bool IndDocumentoProtocolo { get; private set; }
        public bool IndDocumentoApuracao { get; private set; }
        public bool IndEnviarAppPreposto { get; private set; }
        public bool AtivoDePara { get; private set; }
        public int? IdMigracao { get; private set; }
        public string DescricaoMigracao { get; private set; }
    }
}
