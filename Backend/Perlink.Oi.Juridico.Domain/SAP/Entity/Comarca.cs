using System.Collections.Generic;
using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity {
    public class Comarca : EntityCrud<Comarca, long>
    {
        public string CodigoEstado { get; set; }
        public string Nome { get; set; }
        public long? CodigoEscritorioCivel { get; set; }
        public long? CodigoEscritorioTrabalhista { get; set; }
        public string CodigoComarcaBancoDoBrasil { get; set; }
        public long? CodigoProfissionalCivelEstrategico { get; set; }
        public long? CodigoBBComarca { get; set; }
        public BBComarca BBComarca { get; set; }
        public IList<Processo> ProcessosComarca { get; set; }

        public override AbstractValidator<Comarca> Validator => new ComarcaValidator();

        public override void PreencherDados(Comarca data)
        {
            CodigoEstado = data.CodigoEstado;
            Nome = data.Nome;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class ComarcaValidator : AbstractValidator<Comarca>
    {
        public ComarcaValidator()
        {
           
        }
    }
}
