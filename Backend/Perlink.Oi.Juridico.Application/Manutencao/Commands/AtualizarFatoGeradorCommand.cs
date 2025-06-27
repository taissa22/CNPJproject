using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarFatoGeradorCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
       

        public override void Validate()
        {
            if (Id == 0)
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome não pode estar vazia");
            }
           
        }
    }
}
