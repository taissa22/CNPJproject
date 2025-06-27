using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO
{
    public class GrupoDeEstadosDTO
    {
        public string Nome { get; set; }
        public string NomeAnterior { get; set; }
        public EstadoDTO[] EstadosGrupo { get; set; }
        public int EstadosIniciais { get; set; }
        public bool Persistido { get; set; }
        public bool Excluido { get; set; }

    }
}
