using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class JurosCorrecaoProcesso : Notifiable, IEntity, INotifiable
    {
        private JurosCorrecaoProcesso()
        {          
        }

        public static JurosCorrecaoProcesso Criar(TipoProcesso tipoProcesso, DateTime dataVigencia, decimal valorJuros)
        {
            JurosCorrecaoProcesso jurosCorrecaoProcesso = new JurosCorrecaoProcesso();

            jurosCorrecaoProcesso.CodTipoProcesso = tipoProcesso.Id;
            jurosCorrecaoProcesso.DataVigencia = dataVigencia.Date;
            jurosCorrecaoProcesso.ValorJuros = valorJuros;         

            jurosCorrecaoProcesso.Validate();
            return jurosCorrecaoProcesso;
        }

        public void Atualizar(decimal valorJuros)
        {
            ValorJuros = valorJuros;

            Validate();
        }

        public int CodTipoProcesso { get; private set; }
        public TipoProcesso TipoProcesso => TipoProcesso.PorId(CodTipoProcesso);

        public DateTime DataVigencia { get; private set; }

        public decimal ValorJuros { get; private set; }

        private void Validate()
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
