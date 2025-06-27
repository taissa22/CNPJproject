using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarEstabelecimentoCommand : Validatable, IValidatable
    {        

        public string Nome { get; set; }

        public string Cnpj { get; set; }

        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Cep { get; set; }

        public string Estado { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public override void Validate()
        {
            if (!Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres");
            }

            if (!string.IsNullOrEmpty(Endereco) && !Endereco.HasMaxLength(400))
            {
                AddNotification(nameof(Endereco), "O Endereço permite no máximo 60 caracteres");
            }

            if (!string.IsNullOrEmpty(Bairro) && !Bairro.HasMaxLength(30))
            {
                AddNotification(nameof(Bairro), "O Bairro permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(Cidade) && !Cidade.HasMaxLength(30))
            {
                AddNotification(nameof(Cidade), "A Cidade permite no máximo 30 caracteres");
            }

            if (!Cep.HasMaxLength(8) && !Cep.IsNumeric())
            {
                AddNotification(nameof(Cep), "O Cep permite no máximo 8 caracteres");
            }
        }
    }
}
