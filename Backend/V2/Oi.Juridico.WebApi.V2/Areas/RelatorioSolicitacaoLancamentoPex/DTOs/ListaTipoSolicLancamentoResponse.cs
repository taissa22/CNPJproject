namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs
{
    public class ListaTipoSolicLancamentoResponse
    {
        public int Codigo { get { return Convert.ToInt32(_enum); } }
        public string Nome { get { return _enum.ToDescription(); } }

        private Enum _enum;

        public ListaTipoSolicLancamentoResponse(Enum inputEnum)
        {
            _enum = inputEnum;
        }
    }
}
