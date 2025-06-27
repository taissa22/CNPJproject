using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class PedidoProcesso : EntityCrud<PedidoProcesso, long>
    {
        public override AbstractValidator<PedidoProcesso> Validator => new PedidoProcessoValidator();

        public long CodigoProcesso { get; set; }
        public string Comentario { get; set; }
        public bool IndicaAcessoRestrito { get; set; }
        public Pedido Pedido { get; set; }
        public Processo Processo { get; set; }

        public override void PreencherDados(PedidoProcesso data)
        {
            CodigoProcesso = data.CodigoProcesso;
            Comentario = data.Comentario;
            IndicaAcessoRestrito = data.IndicaAcessoRestrito;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class PedidoProcessoValidator : AbstractValidator<PedidoProcesso>
    {
        public PedidoProcessoValidator()
        {

        }
    }
}
