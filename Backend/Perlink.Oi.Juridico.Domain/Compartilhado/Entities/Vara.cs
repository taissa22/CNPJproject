using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Vara : EntityCrud<Vara, long> {
        public override AbstractValidator<Vara> Validator => throw new NotImplementedException();

        public long CodigoVara { get; set; }
        public long CodigoTipoVara { get; set; }
        public string EnderecoVara { get; set; }
        public long? CodigoEscritorioJuizado { get; set; }
        public string CodigoVaraBancoDoBrasil { get; set; }
        public long? IdBancoDoBrasilOrgao { get; set; }
        public IList<Processo> ProcessosVara { get; set; }
        public BBOrgaos BBOrgaos { get; set; }

        public override void PreencherDados(Vara data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }
    }
}
