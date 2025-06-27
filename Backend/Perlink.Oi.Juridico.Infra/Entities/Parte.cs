using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Formatters;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Parte : Notifiable, IEntity, INotifiable
    {
        private Parte()
        {
        }

        public Parte(int id, string nome, string cpf, string cnpj) {
            Id = id;
            Nome = nome;
            CPF = cpf;
            CNPJ = cnpj;
        }

        private Parte(string nome, TipoParte tipoParte, EstadoEnum estado, string endereco, string cep, string cidade, string bairro,
            string enderecosAdicionais, string telefone, string celular, string telefonesAdicionais, int? empresaCentralizadoraId)
        {
            Nome = nome.Padronizar();
            TipoParteValor = tipoParte.Valor;
            EstadoId = estado.Id;
            Endereco = endereco.Padronizar();
            CEP = cep;
            Cidade = cidade.Padronizar();
            Bairro = bairro.Padronizar();
            TelefonesAdicionais = telefonesAdicionais;
            EnderecosAdicionais = enderecosAdicionais;

            TelefoneDDD = telefone is null ? telefone : telefone.Substring(0, 2);
            CelularDDD = celular is null ? celular : celular.Substring(0, 2);

            Telefone = telefone is null ? telefone : telefone.Remove(0, 2);
            Celular = celular is null ? celular : celular.Remove(0, 2);
            EmpresaCentralizadoraId = empresaCentralizadoraId;
        }

        public Parte(string nome, string cpf, int? carteiraDeTrabalho, EstadoEnum estado, string endereco, string cep, string cidade, string bairro,
            string enderecosAdicionais, string telefone, string celular, string telefonesAdicionais, int? empresaCentralizadoraId) : this(nome, Enums.TipoParte.PESSOA_FISICA, estado, endereco, cep, cidade, bairro, enderecosAdicionais, telefone, celular, telefonesAdicionais, empresaCentralizadoraId)
        {
            CPF = cpf;
            CarteiraDeTrabalho = carteiraDeTrabalho;
        }

        public Parte(string nome, string cnpj, EstadoEnum estado, string endereco, string cep, string cidade, string bairro,
            string enderecosAdicionais, string telefone, string celular, string telefonesAdicionais, int? empresaCentralizadoraId) : this(nome, Enums.TipoParte.PESSOA_JURIDICA, estado, endereco, cep, cidade, bairro, enderecosAdicionais, telefone, celular, telefonesAdicionais, empresaCentralizadoraId)
        {
            CNPJ = cnpj;
        }

        /// <summary>
        /// Instancia uma <see cref="Parte"/> definido como <see cref="TipoParte.PESSOA_JURIDICA"/>
        /// </summary>
        public static Parte CriarPessoaJuridica(
            string nome, CNPJ cnpj, EstadoEnum estado, string endereco, int? cep, string cidade, string bairro,
            string enderecosAdicionais, Telefone? telefone, Telefone? celular, string telefonesAdicionais, DateTime? dataCartaFianca, decimal? valorCartaFianca)
        {
            var parte = new Parte()
            {
                Nome = nome.Padronizar(),
                TipoParteValor = TipoParte.PESSOA_JURIDICA.Valor,
                CNPJ = cnpj,
                CPF = null,
                EstadoId = estado.Id,
                Endereco = endereco.Padronizar(),
                CEP = cep.ToString(),
                Cidade = cidade.Padronizar(),
                Bairro = bairro.Padronizar(),
                EnderecosAdicionais = enderecosAdicionais.Padronizar(),
                Telefone = telefone.HasValue ? telefone.Value.Numero : null,
                TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null,
                CelularDDD = celular.HasValue ? celular.Value.Area : null,
                Celular = celular.HasValue ? celular.Value.Numero : null,
                TelefonesAdicionais = telefonesAdicionais.Padronizar(),
                DataCartaFianca = dataCartaFianca,
                ValorCartaFianca = valorCartaFianca ?? 0
        };
            parte.Validate();
            return parte;
        }       

        /// <summary>
        /// Instancia uma <see cref="Parte"/> definido como <see cref="TipoParte.PESSOA_FISICA"/>
        /// </summary>
        public static Parte CriarPessoaFisica(
            string nome, CPF cpf, EstadoEnum estado, string endereco, int? cep, string cidade, string bairro,
            string enderecosAdicionais, Telefone? telefone, Telefone? celular, string telefonesAdicionais, int? carteiraTrabalho, DateTime? dataCartaFianca, decimal? valorCartaFianca)
        {
            var parte = new Parte()
            {
                Nome = nome.Padronizar(),
                TipoParteValor = TipoParte.PESSOA_FISICA.Valor,
                CNPJ = null,
                CPF = cpf,
                EstadoId = estado.Id,
                Endereco = endereco.Padronizar(),
                CEP = cep.ToString(),
                Cidade = cidade.Padronizar(),
                Bairro = bairro.Padronizar(),
                EnderecosAdicionais = enderecosAdicionais.Padronizar(),
                Telefone = telefone.HasValue ? telefone.Value.Numero : null,
                TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null,
                CelularDDD = celular.HasValue ? celular.Value.Area : null,
                Celular = celular.HasValue ? celular.Value.Numero : null,
                TelefonesAdicionais = telefonesAdicionais.Padronizar(),
                CarteiraDeTrabalho = carteiraTrabalho,
                DataCartaFianca = dataCartaFianca,
                ValorCartaFianca = valorCartaFianca ?? 0
            };
            parte.Validate();
            return parte;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string CNPJ { get; private set; }
        public int? CarteiraDeTrabalho { get; private set; }
        public string TipoParteValor { get; private set; }
        public TipoParte TipoParte => TipoParte.PorValor(TipoParteValor);

        public string EstadoId { get; private set; }
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);
        public string Endereco { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Bairro { get; private set; }

        public string EnderecosAdicionais { get; private set; }

        public string TelefoneDDD { get; private set; }
        public string Telefone { get; private set; }
        public string FaxDDD { get; private set; }
        public string Fax { get; private set; }
        public string CelularDDD { get; private set; }
        public string Celular { get; private set; }
        public string TelefonesAdicionais { get; private set; }
        //   public int? CodigoFornecedor { get; private set; }       


        public decimal ValorCartaFianca { get; private set; }
        public DateTime? DataCartaFianca { get; private set; }

        internal ParteBase ParteBase { get; private set; }

        public int? EmpresaCentralizadoraId { get; private set; }
        private void Validate()
        {
            if (Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }
        }

        /// <summary>
        /// Atualiza o <see cref="Profissional"/> <see cref="TipoParte.PESSOA_JURIDICA"/>
        /// </summary>
        public void AtualizarPJ(
            string nome, CNPJ cnpj, EstadoEnum estado, string endereco, int? cep, string cidade, string bairro,
            string enderecosAdicionais, Telefone? telefone, Telefone? celular, string telefonesAdicionais, DateTime? dataCartaFianca, decimal? valorCartaFianca)
        {           

            TipoParteValor = TipoParte.PESSOA_JURIDICA.Valor;
            Nome = nome.Padronizar();
            CNPJ = cnpj;
            CPF = null;
            EstadoId = estado.Id;
            Endereco = !String.IsNullOrEmpty(cidade) ? endereco.Padronizar() : null;
            CEP = cep.HasValue? cep.ToString():null;
            Cidade = cidade.Padronizar();
            Bairro = bairro.Padronizar();
            EnderecosAdicionais = enderecosAdicionais.Padronizar();
            Telefone = telefone.HasValue ? telefone.Value.Numero : null;
            TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null;
            CelularDDD = celular.HasValue ? celular.Value.Area : null;
            Celular = celular.HasValue ? celular.Value.Numero : null;
            TelefonesAdicionais = telefonesAdicionais.Padronizar();
            DataCartaFianca = dataCartaFianca;
            ValorCartaFianca = valorCartaFianca ?? 0;

            Validate();
        }

        /// <summary>
        /// Atualiza o <see cref="Profissional"/> <see cref="TipoParte.PESSOA_FISICA"/>
        /// </summary>
        public void AtualizarPF(
            string nome, CPF cpf, EstadoEnum estado, string endereco, int? cep, string cidade, string bairro,
            string enderecosAdicionais, Telefone? telefone, Telefone? celular, string telefonesAdicionais, int? carteiraTrabalho, DateTime? dataCartaFianca, decimal? valorCartaFianca)
        {
            
            TipoParteValor = TipoParte.PESSOA_FISICA.Valor;
            CNPJ = null;
            Nome = nome.ToUpper();
            CPF = cpf;
            EstadoId = estado.Id;
            Endereco = endereco.Padronizar();
            CEP = cep.HasValue ? cep.ToString() : null;
            Cidade = cidade.Padronizar();
            Bairro = bairro.Padronizar();
            EnderecosAdicionais = enderecosAdicionais.Padronizar();
            Telefone = telefone.HasValue ? telefone.Value.Numero : null;
            TelefoneDDD = telefone.HasValue ? telefone.Value.Area : null;
            CelularDDD = celular.HasValue ? celular.Value.Area : null;
            Celular = celular.HasValue ? celular.Value.Numero : null;
            TelefonesAdicionais = telefonesAdicionais.Padronizar();
            CarteiraDeTrabalho = carteiraTrabalho;
            DataCartaFianca = dataCartaFianca;
            ValorCartaFianca = valorCartaFianca ?? 0;

            Validate();
        }
    }
}