using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Dto
{
    public class EscritorioEstadoDTO
    {
        public EscritorioEstadoDTO(string estado,bool selecionado, int tipoProcessoId)
        {
            Id = estado;
            Selecionado = selecionado;
            TipoProcessoId = tipoProcessoId;
        }

        public string Id { get; set; }
        public bool Selecionado { get; set; }
        public int TipoProcessoId { get; set; }
    }
}
