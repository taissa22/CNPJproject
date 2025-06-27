using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using EnumEstado = Perlink.Oi.Juridico.Infra.Enums.EstadoEnum;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarParteCommand : Validatable, IValidatable
    {
        public int Id { get; set; } = 0;

        public string Nome { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public bool PessoaJuridica { get; set; } = false;
        public string EstadoId { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public string Endereco { get; set; } = string.Empty;

        public int? CEP { get; set; } = 0;

        public int? CarteiraTrabalho { get; set; } = 0;

        public string Cidade { get; set; } = string.Empty;

        public string Bairro { get; set; } = string.Empty;

        public string EnderecosAdicionais { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Celular { get; set; } = string.Empty;

        public string TelefonesAdicionais { get; set; } = string.Empty;

        public decimal ValorCartaFianca { get; set; }

        public DateTime? DataCartaFianca { get; set; }

        public override void Validate()
        {
            if (Id is 0)
            {
                AddNotification(nameof(Id), "O Id é obrigatório");
            }

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
                    AddNotification(nameof(Documento), "CPF inválido");
                }
            }

            if (!string.IsNullOrEmpty(Endereco) && Endereco?.Length > 400) {
                AddNotification(nameof(Endereco), "O campo endereço permite no máximo 400 caracteres");
            }

            if (CEP != null && CEP.ToString().Length > 8) {
                AddNotification(nameof(CEP), "O campo CEP permite no máximo 8 caracteres");
            }

            if (!string.IsNullOrEmpty(Bairro) && Bairro?.Length > 30) {
                AddNotification(nameof(Bairro), "O campo bairro permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(Cidade) && Cidade?.Length > 30) {
                AddNotification(nameof(Cidade), "O campo cidade permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(EnderecosAdicionais) && EnderecosAdicionais?.Length > 4000) {
                AddNotification(nameof(EnderecosAdicionais), "O campo endereços adicionais permite no máximo 4000 caracteres");
            }

            if (!string.IsNullOrEmpty(Telefone)) {
                if (Telefone?.Length > 13) {
                    AddNotification(nameof(Telefone), "O campo telefone permite no máximo 13 caracteres");
                }

                if (!Telefone.IsNumeric()) {
                    AddNotification(nameof(Telefone), "O campo telefone precisa ser numérico");
                }
            }


            if (!string.IsNullOrEmpty(Celular)) {
                if (Celular?.Length > 13) {
                    AddNotification(nameof(Celular), "O campo celular permite no máximo 13 caracteres");
                }

                if (!Celular.IsNumeric()) {
                    AddNotification(nameof(Celular), "O campo Celular precisa ser numérico");
                }
            }

            if (!string.IsNullOrEmpty(TelefonesAdicionais) && TelefonesAdicionais?.Length > 4000) {
                AddNotification(nameof(TelefonesAdicionais), "O campo telefones adicionais permite no máximo 4000 caracteres");
            }

            if (!string.IsNullOrEmpty(Estado)) {
                var estado = EnumEstado.PorId(Estado);

                if (estado == null || (estado != null && estado.Nome == estado.Id)) {
                    AddNotification(nameof(Estado), "O Estado informado não existe");
                }
            }
        }
    }
}