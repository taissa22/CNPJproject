using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity {
    public class Parametro : EntityCrud<Parametro, string> {
        public override AbstractValidator<Parametro> Validator => new ParametroValidator();

        //id herdado pelo entityCrud 
        //vai referenciar cod_parametro na tabela de parametros

        public string TipoParametro { get; set; }

        public string Descricao { get; set; }

        public string Conteudo { get; set; }

        public override void PreencherDados(Parametro data) {
            TipoParametro = data.TipoParametro;
            Descricao = data.Descricao;
            Conteudo = data.Conteudo;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ParametroValidator : AbstractValidator<Parametro> {
        public ParametroValidator() {
            RuleFor(x => x.TipoParametro);
            RuleFor(x => x.Descricao);
            RuleFor(x => x.Conteudo);
        }
    }
}