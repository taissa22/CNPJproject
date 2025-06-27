using FluentValidation;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class TipoProcesso : EntityCrud<TipoProcesso, long>
    {
        public override AbstractValidator<TipoProcesso> Validator => new TipoProcessoValidator();

        public string Descricao { get; set; }

        public IList<GrupoCorrecaoGarantia> GruposCorrecoesGarantias { get; set; }

        public IList<Lote> Lotes { get; set; }

        public ICollection<JuroCorrecaoProcesso> ListaDeJuroCorrecaoProcesso { get; set; } 

        public override void PreencherDados(TipoProcesso data)
        {
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class TipoProcessoValidator : AbstractValidator<TipoProcesso>
        {
            public TipoProcessoValidator()
            {
            }
        }
    }
}