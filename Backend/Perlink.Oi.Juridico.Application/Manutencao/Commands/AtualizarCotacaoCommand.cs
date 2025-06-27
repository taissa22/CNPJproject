using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarCotacaoCommand : Validatable, IValidatable
    {
        public int CodigoIndice { get; set; }

        public DateTime DataCotacao { get; set; }

        public decimal Valor { get; set; }

        public decimal ValorAcumulado { get; set; }

        public override void Validate()
        {
            if (CodigoIndice <= 0)
            {
                AddNotification(nameof(CodigoIndice), "Campo Requerido");
            }

            // TODO: ROBSON - valor pode ser negativo igual módulo interno.
            //if (Valor <= 0)
            //{
            //    AddNotification(nameof(Valor), "Campo Requerido");
            //}

            // TODO: Weriton - validar data
        }
    }
}