using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Indice : Notifiable, IEntity, INotifiable
    {
        private Indice()
        {
        }

        public static Indice Criar(string descricao, bool codigoTipoIndice, string? codigoValorIndice, bool acumulado, bool acumuladoAutomatico)
        {
            var indice = new Indice();

            indice.Descricao = descricao;
            indice.CodigoTipoIndice = codigoTipoIndice ? "M" : "D";
            indice.CodigoValorIndice = codigoValorIndice;
            indice.Acumulado = acumulado;
            indice.AcumuladoAutomatico = acumuladoAutomatico;

            indice.Validate();
            return indice;
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; } = null!;
        public string CodigoTipoIndice { get; private set; } = null!;
        public string? CodigoValorIndice { get; private set; }
        public bool? Acumulado { get; private set; }
        public bool? AcumuladoAutomatico { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(CodigoValorIndice))
            {
                AddNotification(nameof(CodigoValorIndice), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(CodigoValorIndice) && (!new[] { "F", "P", "V" }.Contains(CodigoValorIndice)))
            {
                AddNotification(nameof(CodigoValorIndice), "Valor diferente do esperado");
            }
        }

        public void Atualizar(string descricao, bool codigoTipoIndice, string? codigoValorIndice, bool acumulado, bool acumuladoAutomatico)
        {
            Descricao = descricao;
            CodigoTipoIndice = codigoTipoIndice ? "M" : "D";
            CodigoValorIndice = codigoValorIndice;
            Acumulado = acumulado;
            AcumuladoAutomatico = acumulado.Equals(false) ? false : acumuladoAutomatico;

            Validate();
        }
    }
}
