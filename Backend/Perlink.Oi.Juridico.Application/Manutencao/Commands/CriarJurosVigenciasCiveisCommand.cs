using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarJurosVigenciasCiveisCommand : Validatable, IValidatable
    {
        public int CodTipoProcesso { get; set; }

        public DateTime DataVigencia { get; set; }

        public decimal ValorJuros { get; set; }

        public override void Validate()
        {
            if (CodTipoProcesso != 1 && CodTipoProcesso != 9)
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Processo Inválido.");
            }

            if (DataVigencia <= DateTime.MinValue.AddYears(1900))
            {
                AddNotification(nameof(CodTipoProcesso), "Data inválida.");
            }

            if (ValorJuros <= 0)
            {
                AddNotification(nameof(CodTipoProcesso), "Valor do Juros deve ser maior que 0");
            }
        }
    }
}