using Perlink.Oi.Juridico.Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.ViewModel
{
    public class ObjetoViewModel
    {
        public int Id { get; set; }

        public string Descricao { get; set; }
        public bool EhTributarioAdministrativo { get; set; }
        public bool EhTributarioJudicial { get; set; }
        public bool AtivoTributarioAdministrativo { get; set; }
        public bool AtivoTributarioJudicial { get; set; }
        public bool EhTrabalhistaAdministrativo { get; set; }

        public int? GrupoPedidoId { get; set; }
        public string GrupoPedidoDescricao { get; set; } 
        public TipoProcesso TipoProcesso { get; set; }
    }
}
