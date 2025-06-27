using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso
{
    public class VigenciaCivilFiltrosViewModel : OrdernacaoPaginacaoDTO
    {
        public Filtro Filtro { get; set; }
    }

    // TODO: Alan, verificar melhor abordagem;
    public class Filtro
    {
        public long? CodTipoProcesso { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
