using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.Entities.Internal;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Estabelecimento : Notifiable, IEntity, INotifiable
    {

        private Estabelecimento()
        {
        }


        public static Estabelecimento Criar(DataString nome, CNPJ cnpj, DataString endereco, DataString bairro, DataString cidade, DataString cep,
            EstadoEnum estado, Telefone? telefone, Telefone? celular)
        {
            var estabelecimento = new Estabelecimento()
            {
                Nome = nome,
                CNPJ = cnpj.ToString(),
                Endereco = endereco,
                Bairro = bairro,
                Cidade = cidade,
                CEP = cep,
                EstadoId = estado.Id,
                Telefone = telefone.HasValue ? telefone.Value.Numero : null ,
                TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null,
                CelularDDD = celular.HasValue ? celular.Value.Area : null,
                Celular = celular.HasValue ? celular.Value.Numero : null,

                TipoParteValor = TipoParte.ESTABELECIMENTO.Valor


            };
            estabelecimento.Validate();
            return estabelecimento;
        }

        public void Atualizar(DataString nome, CNPJ cnpj, DataString endereco, DataString bairro, DataString cidade, DataString cep,
            EstadoEnum estado, Telefone? telefone, Telefone? celular)
        {
            Nome = nome;
            CNPJ = cnpj.ToString();
            Endereco = endereco;
            Bairro = bairro;
            Cidade = cidade;
            CEP = cep;
            EstadoId = estado.Id;
            Telefone = telefone.HasValue ? telefone.Value.Numero : null;
            TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null;
            CelularDDD = celular.HasValue ? celular.Value.Area : null;
            Celular = celular.HasValue ? celular.Value.Numero : null;

            Validate();
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public string CNPJ { get; private set; }

        public string Endereco { get; private set; }

        public string Bairro { get; private set; }

        public string Cidade { get; private set; }

        public string CEP { get; private set; }

        public string EstadoId { get; private set; }

        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);

        public string CelularDDD { get; private set; }
        public string Celular { get; private set; }

        public string TelefoneDDD { get; private set; }
        public string Telefone { get; private set; }

        public string FaxDDD { get; private set; }
        public string Fax { get; private set; }

        internal string TipoParteValor { get; private set; }

        public TipoParte TipoParte => TipoParte.PorValor(TipoParteValor);

        internal ParteBase ParteBase { get; private set; }

        public int? EmpresaCentralizadoraId { get; private set; }

        public void Validate()
        {
            if (!Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }

            if (!string.IsNullOrEmpty(Endereco) && !Endereco.HasMaxLength(400))
            {
                AddNotification(nameof(Endereco), "O Endereço permite no máximo 60 caracteres.");
            }

            if (!string.IsNullOrEmpty(Bairro) && !Bairro.HasMaxLength(30))
            {
                AddNotification(nameof(Bairro), "O Bairro permite no máximo 30 caracteres.");
            }

            if (!string.IsNullOrEmpty(Cidade) && !Cidade.HasMaxLength(30))
            {
                AddNotification(nameof(Cidade), "A Cidade permite no máximo 30 caracteres.");
            }

            if (!CEP.HasMaxLength(8) && !CEP.IsNumeric())
            {
                AddNotification(nameof(CEP), "A Cep permite no máximo 8 caracteres.");
            }
        }
    }
}