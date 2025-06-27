using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarObjetoCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool EhTributarioAdminstrativo { get; set; }
        public bool EhTributarioJudicial { get; set; }
        public int? GrupoId { get; set; }
        public bool AtivoTributarioAdminstrativo { get; set; }
        public bool AtivoTributarioJudicial { get; set; }
        public bool EhTrabalhistaAdministrativo { get; set; }

        public override void Validate()
        {
            if (Id == 0)
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }
            if (Descricao.Length > 50)
            {
                AddNotification(nameof(Descricao), "Limite de caracteres 50");
            }
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "A descrição não pode estar vazia");
            }
            if (GrupoId == 0)
            {
                AddNotification(nameof(GrupoId), "Campo Requerido");
            }
        }
    }
}
