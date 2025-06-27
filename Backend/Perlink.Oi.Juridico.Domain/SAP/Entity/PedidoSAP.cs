using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity {
    public class PedidoSAP : EntityCrud<PedidoSAP, long>  {
        public override AbstractValidator<PedidoSAP> Validator => new PedidoSAPValidator();

        public long NumeroItemPedidoSAP { get; set; }
        public long? CodigoMaterial { get; set; }
        public string DescricaoMaterial { get; set; }
        public string NomeFornecedor { get; set; }
        public DateTime? DataEmissao { get; set; }
        public double? ValorPedido { get; set; }
        public long? CentroCusto { get; set; }
        public DateTime? DataCompesacao { get; set; }
        public string CodigoFornecedor { get; set; }
        public string numeroDocumento { get; set; }

        public override void PreencherDados(PedidoSAP data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }

        internal class PedidoSAPValidator : AbstractValidator<PedidoSAP> {
            public PedidoSAPValidator() {
            }
        }
    }
}
