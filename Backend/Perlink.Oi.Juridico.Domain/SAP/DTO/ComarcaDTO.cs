using Perlink.Oi.Juridico.Domain.SAP.Interface.IEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO {
    public class ComarcaDTO : IDualListItem<long> {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }
}
