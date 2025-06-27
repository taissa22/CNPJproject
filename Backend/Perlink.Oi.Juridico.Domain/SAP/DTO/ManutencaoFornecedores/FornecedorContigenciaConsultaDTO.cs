using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores
{
    public class FornecedorContigenciaConsultaDTO : OrdernacaoPaginacaoDTO
    {
        public string codigo { get; set; }
        public string nome { get; set; }
        public string cnpj { get; set; }

        public long statusFornecedor { get; set; }
    }
}