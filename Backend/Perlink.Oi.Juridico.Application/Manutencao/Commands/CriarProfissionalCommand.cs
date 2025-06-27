using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;
using EnumEstado = Perlink.Oi.Juridico.Infra.Enums.EstadoEnum;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarProfissionalCommand : Validatable, IValidatable
    {
        public string Nome { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public bool PessoaJuridica { get; set; } = false;

        public string Email { get; set; }

        public string Estado { get; set; }

        public string Endereco { get; set; }

        public int? CEP { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string EnderecosAdicionais { get; set; }

        public string Telefone { get; set; }

        public string Fax { get; set; }

        public string Celular { get; set; }

        public string TelefonesAdicionais { get; set; }

        public bool? Advogado { get; set; }

        public string NumeroOAB { get; set; }

        public string EstadoOAB { get; set; }

        public bool? Contador { get; set; }

        public bool? ContadorPex { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome é obrigatório");
            }
            if (!string.IsNullOrEmpty(Nome) && Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres");
            }

            if (string.IsNullOrEmpty(Documento))
            {
                AddNotification(nameof(Documento), "O CPF/CNPJ é obrigatório");
            }

            if (PessoaJuridica)
            {
                if (!CNPJ.IsValidForSisjur(Documento))
                {
                    AddNotification(nameof(Documento), "CNPJ inválido");
                }
            }
            else
            {
                if (!CPF.IsValidForSisjur(Documento))
                {
                    AddNotification(nameof(Documento), "CPF inválido.");
                }
            }

            if (!string.IsNullOrEmpty(Email))
            {
                EmailAddressAttribute emailAttr = new EmailAddressAttribute();

                if (!emailAttr.IsValid(Email))
                {
                    AddNotification("", "O campo Email não está em formato válido");
                }

                if (Email?.Length > 60)
                {
                    AddNotification("", "O campo Email permite no máximo 60 caracteres");
                }
            }

            if (!string.IsNullOrEmpty(Estado))
            {
                var estado = EnumEstado.PorId(Estado);
              
                if (estado == null || (estado != null && estado.Nome == estado.Id))
                {
                    AddNotification(nameof(Estado), "O Estado informado não existe");
                }
            }

            if (!string.IsNullOrEmpty(Endereco) && Endereco?.Length > 60)
            {
                AddNotification(nameof(Endereco), "O campo endereço permite no máximo 60 caracteres");
            }

            if (CEP != null && CEP.ToString().Length > 8)
            {
                AddNotification(nameof(CEP), "O campo CEP permite no máximo 8 caracteres");
            }

            if (!string.IsNullOrEmpty(Bairro) && Bairro?.Length > 30)
            {
                AddNotification(nameof(Bairro), "O campo bairro permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(Cidade) && Cidade?.Length > 30)
            {
                AddNotification(nameof(Cidade), "O campo cidade permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(EnderecosAdicionais) && EnderecosAdicionais?.Length > 4000)
            {
                AddNotification(nameof(EnderecosAdicionais), "O campo endereços adicionais permite no máximo 4000 caracteres");
            }

            if (!string.IsNullOrEmpty(Telefone))
            {
                if (Telefone?.Length > 13)
                {
                    AddNotification(nameof(Telefone), "O campo telefone permite no máximo 13 caracteres");
                }

                if (!Telefone.IsNumeric())
                {
                    AddNotification(nameof(Telefone), "O campo telefone precisa ser numérico");
                }
            }

            if (!string.IsNullOrEmpty(Fax))
            {
                if (Fax?.Length > 13)
                {
                    AddNotification(nameof(Fax), "O campo Fax permite no máximo 13 caracteres");
                }

                if (!Fax.IsNumeric())
                {
                    AddNotification(nameof(Fax), "O campo Fax precisa ser numérico");
                }
            }

            if (!string.IsNullOrEmpty(Celular))
            {
                if (Celular?.Length > 13)
                {
                    AddNotification(nameof(Celular), "O campo celular permite no máximo 13 caracteres");
                }

                if (!Celular.IsNumeric())
                {
                    AddNotification(nameof(Celular), "O campo Celular precisa ser numérico");
                }
            }

            if (!string.IsNullOrEmpty(TelefonesAdicionais) && TelefonesAdicionais?.Length > 4000)
            {
                AddNotification(nameof(TelefonesAdicionais), "O campo telefones adicionais permite no máximo 4000 caracteres");
            }

            if (!string.IsNullOrEmpty(NumeroOAB) && NumeroOAB?.Length > 7)
            {
                AddNotification(nameof(NumeroOAB), "O campo Registro OAB permite no máximo 7 caracteres");
            }

            if (!string.IsNullOrEmpty(EstadoOAB))
            {
                var estado = EnumEstado.PorId(EstadoOAB);

                if (estado == null || (estado != null && estado.Nome == estado.Id))
                {
                    AddNotification(nameof(EstadoOAB), "O Estado informado não existe");
                }
            }
        }
    }
}