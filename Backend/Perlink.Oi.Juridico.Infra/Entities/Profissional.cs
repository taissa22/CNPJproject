using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;


#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Profissional : Notifiable, IEntity, INotifiable
    {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.
        private Profissional()
        {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        /// <summary>
        /// Instancia um <see cref="Profissional"/> definido como <see cref="TipoPessoa.PESSOA_FISICA"/>
        /// </summary>
        public static Profissional CriarPessoaFisica(
            string nome, CPF cpf,
            string endereco, string enderecosAdicionais,
            string bairro, EstadoEnum? estado, string cidade,
            int? cep, string email, Telefone? telefone, Telefone? celular,
            Telefone? fax, string telefonesAdicionais,
            bool? contador, bool? contadorPex,
            bool? advogado, string registroOAB, EstadoEnum? estadoOAB)
        {
            var profissional = new Profissional()
            {
                Nome = nome.Padronizar(),
                TipoPessoaValor = TipoPessoa.PESSOA_FISICA.Valor,
                CPF = cpf,
                Endereco = endereco.Padronizar(),
                EnderecosAdicionais = enderecosAdicionais,
                Bairro = bairro.Padronizar(),
                EstadoId = estado?.Id,
                Cidade = cidade.Padronizar(),
                CEP = cep,
                Email = email,
                TelefoneDDD = telefone?.Area,
                Telefone = telefone?.Numero,
                CelularDDD = celular?.Area,
                Celular = celular?.Numero,
                FaxDDD = fax?.Area,
                Fax = fax?.Numero,
                TelefonesAdicionais = telefonesAdicionais.Padronizar(),
                EhContador = contador.GetValueOrDefault(),
                EhContadorPex = contadorPex.GetValueOrDefault(),
                EhAdvogado = advogado.GetValueOrDefault(),
                RegistroOAB = registroOAB.Padronizar(),
                EstadoOABId = estadoOAB?.Id
            };
            profissional.Validate();
            return profissional;
        }

        /// <summary>
        /// Instancia um <see cref="Profissional"/> definido como <see cref="TipoPessoa.PESSOA_JURIDICA"/>
        /// </summary>
        public static Profissional CriarPessoaJuridica(
            string nome, CNPJ cnpj,
            string endereco, string enderecosAdicionais,
            string bairro, EstadoEnum? estado, string cidade,
            int? cep, string email, Telefone? telefone, Telefone? celular,
            Telefone? fax, string telefonesAdicionais,
            bool? contador, bool? contadorPex,
            bool? advogado, string registroOAB, EstadoEnum? estadoOAB)
        {
            var profissional = new Profissional()
            {
                Nome = nome.Padronizar(),
                TipoPessoaValor = TipoPessoa.PESSOA_JURIDICA.Valor,
                CNPJ = cnpj,
                Endereco = endereco.Padronizar(),
                EnderecosAdicionais = enderecosAdicionais.Padronizar(),
                Bairro = bairro.Padronizar(),
                EstadoId = estado?.Id,
                Cidade = cidade.Padronizar(),
                CEP = cep,
                Email = email.Padronizar(),
                TelefoneDDD = telefone?.Area,
                Telefone = telefone?.Numero,
                CelularDDD = celular?.Area,
                Celular = celular?.Numero,
                FaxDDD = fax?.Area,
                Fax = fax?.Numero,
                TelefonesAdicionais = telefonesAdicionais.Padronizar(),
                EhContador = contador.GetValueOrDefault(),
                EhContadorPex = contadorPex.GetValueOrDefault(),
                EhAdvogado = advogado.GetValueOrDefault(),
                RegistroOAB = registroOAB.Padronizar(),
                EstadoOABId = estadoOAB?.Id
            };
            profissional.Validate();
            return profissional;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public bool Ativo { get; private set; } = true;

        public string CPF { get; private set; }

        public string CNPJ { get; private set; }

        public string Email { get; private set; }

        public string TipoPessoaValor { get; private set; } = string.Empty;
        public TipoPessoa TipoPessoa => TipoPessoa.PorValor(TipoPessoaValor);



        public string EstadoId { get; private set; }
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);

        public string Endereco { get; private set; }

        public int? CEP { get; private set; }

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

        public bool EhAdvogado { get; private set; }

        public string RegistroOAB { get; private set; }

        public string EstadoOABId { get; private set; }
        public EstadoEnum EstadoOAB => EstadoEnum.PorId(EstadoOABId);

        public bool? IndAreaCivel { get; private set; }
        public bool? IndAreaJuizado { get; private set; }
        public bool? IndAreaRegulatoria { get; private set; }
        public bool? IndAreaTrabalhista { get; private set; }
        public bool? IndAreaTributaria { get; private set; }
        public bool IndAreaCivelAdministrativo { get; private set; }
        public bool IndAreaCriminalAdministrativo { get; private set; }
        public bool IndAreaCriminalJudicial { get; private set; }
        public bool IndAreaPEX { get; private set; }
        public bool IndAreaProcon { get; private set; }

        public bool? EhContador { get; private set; }

        public bool EhContadorPex { get; private set; }

        internal bool EhEscritorio { get; private set; }

        public int? SeqAdvogado { get; set; }

        internal ProfissionalBase? ProfissionalBase { get; private set; } = null;

        private void Validate()
        {
            if (Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }

            if (Endereco != null && Endereco.Length > 60)
            {
                AddNotification(nameof(Endereco), "O Endereço permite no máximo 60 caracteres.");
            }

            if (Bairro != null && Bairro.Length > 30)
            {
                AddNotification(nameof(Bairro), "O Bairro permite no máximo 30 caracteres.");
            }

            if (Cidade != null && Cidade.Length > 30)
            {
                AddNotification(nameof(Cidade), "A Cidade permite no máximo 30 caracteres.");
            }

            if (Email != null && Email.Length > 60)
            {
                AddNotification(nameof(Email), "O E-mail permite no máximo 60 caracteres.");
            }

            if (RegistroOAB != null && RegistroOAB.Length > 7)
            {
                AddNotification(nameof(RegistroOAB), "O RegistroOAB permite no máximo 7 caracteres.");
            }

            if (EhAdvogado && (RegistroOAB is null || EstadoOABId is null))
            {
                AddNotification(nameof(EhAdvogado), "Advogado do Autor requer 'Registro OAB' e 'Estado OAB'.");
            }

            if (CEP != null && CEP.ToString().Length > 8)
            {
                AddNotification(nameof(CEP), "O campo CEP permite no máximo 8 caracteres.");
            }
        }

        /// <summary>
        /// Atualiza o <see cref="Profissional"/> <see cref="TipoPessoa.PESSOA_FISICA"/>
        /// </summary>
        public void Atualizar(
            string nome, CPF cpf,
            string endereco, string enderecosAdicionais,
            string bairro, EstadoEnum? estado, string cidade,
            int? cep, string email, Telefone? telefone, Telefone? celular,
            Telefone? fax, string telefonesAdicionais,
            bool? contador, bool? contadorPex,
            bool? advogado, string registroOAB, EstadoEnum? estadoOAB)
        {
            if (TipoPessoa == TipoPessoa.PESSOA_JURIDICA)
            {
                CNPJ = null;
                TipoPessoaValor = TipoPessoa.PESSOA_FISICA.Valor;
            }

            Nome = nome.Padronizar();
            CPF = cpf;
            Endereco = endereco.Padronizar();
            EnderecosAdicionais = enderecosAdicionais.Padronizar();
            Bairro = bairro.Padronizar();
            EstadoId = estado?.Id;
            Cidade = cidade.Padronizar();
            CEP = cep;
            Email = email.Padronizar();
            TelefoneDDD = telefone?.Area;
            Telefone = telefone?.Numero;
            CelularDDD = celular?.Area;
            Celular = celular?.Numero;
            FaxDDD = fax?.Area;
            Fax = fax?.Numero;
            TelefonesAdicionais = telefonesAdicionais.Padronizar();
            EhContador = contador.GetValueOrDefault();
            EhContadorPex = contadorPex.GetValueOrDefault();
            EhAdvogado = advogado.GetValueOrDefault();
            RegistroOAB = registroOAB;
            EstadoOABId = estadoOAB?.Id;

            Validate();
        }

        /// <summary>
        /// Atualiza o <see cref="Profissional"/> <see cref="TipoPessoa.PESSOA_JURIDICA"/>
        /// </summary>
        public void Atualizar(
            string nome, CNPJ cnpj,
            string endereco, string enderecosAdicionais,
            string bairro, EstadoEnum? estado, string cidade,
            int? cep, string email, Telefone? telefone, Telefone? celular,
            Telefone? fax, string telefonesAdicionais,
            bool? contador, bool? contadorPex,
            bool? advogado, string registroOAB, EstadoEnum? estadoOAB)
        {
            if (TipoPessoa == TipoPessoa.PESSOA_FISICA)
            {
                CPF = null;
                TipoPessoaValor = TipoPessoa.PESSOA_JURIDICA.Valor;
            }

            Nome = nome.Padronizar();
            CNPJ = cnpj;
            Endereco = endereco.Padronizar();
            EnderecosAdicionais = enderecosAdicionais.Padronizar();
            Bairro = bairro.Padronizar();
            EstadoId = estado?.Id;
            Cidade = cidade.Padronizar();
            CEP = cep;
            Email = email.Padronizar();
            TelefoneDDD = telefone?.Area;
            Telefone = telefone?.Numero;
            CelularDDD = celular?.Area;
            Celular = celular?.Numero;
            FaxDDD = fax?.Area;
            Fax = fax?.Numero;
            TelefonesAdicionais = telefonesAdicionais.Padronizar();
            EhContador = contador.GetValueOrDefault();
            EhContadorPex = contadorPex.GetValueOrDefault();
            EhAdvogado = advogado.GetValueOrDefault();
            RegistroOAB = registroOAB;
            EstadoOABId = estadoOAB?.Id;

            Validate();
        }
    }
}