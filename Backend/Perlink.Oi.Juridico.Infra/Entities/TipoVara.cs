using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Collections;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoVara : Notifiable, IEntity, INotifiable
    {
        private TipoVara()
        {
        }

        public static TipoVara Criar(string nome, bool eh_CivelConsumidor, bool eh_CivelEstrategico, bool eh_Trabalhista, bool eh_Tributaria, bool eh_Juizado, bool eh_CriminalJudicial, bool eh_Procon)
        {
            var tipoVara = new TipoVara();

            tipoVara.Nome = nome;
            tipoVara.Eh_CivelConsumidor = eh_CivelConsumidor;
            tipoVara.Eh_Trabalhista = eh_Trabalhista;
            tipoVara.Eh_Tributaria = eh_Tributaria;
            tipoVara.Eh_Juizado = eh_Juizado;
            tipoVara.Eh_CivelEstrategico = eh_CivelEstrategico;
            tipoVara.Eh_CriminalJudicial = eh_CriminalJudicial;
            tipoVara.Eh_Procon = eh_Procon;

            tipoVara.Validate();
            return tipoVara;
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public bool Eh_CivelConsumidor { get; private set; }
        public bool Eh_Trabalhista { get; private set; }
        public bool Eh_Tributaria { get; private set; }
        public bool Eh_Juizado { get; private set; }
        public bool Eh_CivelEstrategico { get; private set; }
        public bool Eh_CriminalJudicial { get; private set; }
        public bool Eh_Procon { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }
        }

        public void Atualizar(string nome, bool eh_CivelConsumidor, bool eh_CivelEstrategico, bool eh_Trabalhista, bool eh_Tributaria, bool eh_Juizado, bool eh_CriminalJudicial, bool eh_Procon)
        {
            Nome = nome;
            Eh_CivelConsumidor = eh_CivelConsumidor;
            Eh_Trabalhista = eh_Trabalhista;
            Eh_Tributaria = eh_Tributaria;
            Eh_Juizado = eh_Juizado;
            Eh_CivelEstrategico = eh_CivelEstrategico;
            Eh_CriminalJudicial = eh_CriminalJudicial;
            Eh_Procon = eh_Procon;
            Validate();
        }
    }
}