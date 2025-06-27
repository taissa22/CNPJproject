using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros
{
    public class CategoriaDePagamentofiltroDTO
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        //Lista com o nome Dados a pedido do front
        public ICollection<CategoriaDePagamentoDTO> Dados { get; set; }
    }
    public class CategoriaDePagamentoDTO
    {
        public CategoriaDePagamentoDTO()
        {

        }
        public CategoriaDePagamentoDTO(long id, string descricao, bool ativo, long tipoLancamento)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            TipoLancamento = tipoLancamento;
        }

        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public long TipoLancamento { get; set; }

    }
}
