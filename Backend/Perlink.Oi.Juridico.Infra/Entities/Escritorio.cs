using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Escritorio : Notifiable, IEntity, INotifiable
    {
        private Escritorio()
        {
        }

        public static Escritorio Criar(string nome,
            bool ativo,
            string endereco,
            bool? civelEstrategico,
            string tipoPessoa,
            string CPFCNPJ,
            int? Cep,
            string cidade,
            string email,
            string estado,
            string bairro,
            bool indAdvogado,
            bool indAreaCivel,
            bool indAreaJuizado,
            bool indAreaRegulatoria,
            bool indAreaTrabalhista,
            bool indAreaTributaria,
            bool indAreaCivelAdministrativo,
            bool indAreaCriminalAdministrativo,
            bool indAreaCriminalJudicial,
            bool indAreaPEX,
            bool indAreaProcon,
            int? alertaEm,
            string codProfissionalSAP,
            string site,
            bool ehContadorPex,
            string? CNPJ,
            string telefone,
            string TelefoneDDD,
            string celular,
            string CelularDDD,
            string Fax,
            string FaxDDD,
            EstadoSelecionado[] selecionadosJec,
            EstadoSelecionado[] selecionadosCivelConsumidor,
            bool? enviar_app_preposto = false
            )
        {
            Escritorio escritorio = new Escritorio();

            escritorio.Nome = nome.ToUpper();
            escritorio.Ativo = ativo;
            escritorio.EhEscritorio = true;
            escritorio.CivelEstrategico = civelEstrategico;
            escritorio.TipoPessoaValor = tipoPessoa;
            escritorio.CPF = CPFCNPJ;
            escritorio.CEP = Cep;
            escritorio.Cidade = cidade;
            escritorio.Endereco = endereco;
            escritorio.Telefone = telefone;
            escritorio.Email = email;
            escritorio.EstadoId = estado;
            escritorio.Bairro = bairro;
            escritorio.EhAdvogado = indAdvogado;

            escritorio.IndAreaCivel = indAreaCivel;
            escritorio.IndAreaJuizado = indAreaJuizado;
            escritorio.IndAreaRegulatoria = indAreaRegulatoria;
            escritorio.IndAreaTrabalhista = indAreaTrabalhista;
            escritorio.IndAreaTributaria = indAreaTributaria;
            escritorio.IndAreaCivelAdministrativo = indAreaCivelAdministrativo;
            escritorio.IndAreaCriminalAdministrativo = indAreaCriminalAdministrativo;
            escritorio.IndAreaCriminalJudicial = indAreaCriminalJudicial;
            escritorio.IndAreaPEX = indAreaPEX;
            escritorio.IndAreaProcon = indAreaProcon;
            escritorio.AlertaEm = alertaEm;
            escritorio.CodProfissionalSAP = codProfissionalSAP;
            escritorio.Site = site;
            escritorio.EhContadorPex = ehContadorPex;
            escritorio.CNPJ = CNPJ;
            escritorio.Telefone = telefone;
            escritorio.TelefoneDDD = TelefoneDDD;
            escritorio.Celular = celular;
            escritorio.CelularDDD = CelularDDD;
            escritorio.Fax = Fax;
            escritorio.FaxDDD = FaxDDD;
            escritorio.selecionadosJec = selecionadosJec;
            escritorio.selecionadosCivelConsumidor = selecionadosCivelConsumidor;
            escritorio.Enviar_App_Preposto = enviar_app_preposto;
            return escritorio;
        }

        public void AtualizarSequecialAdvogado(int sequecial)
        {
            this.SeqAdvogado = sequecial;
        }

        public void Atualizar(int id,
                                string nome,
                                bool ativo,
                                string endereco,
                                bool? civelEstrategico,
                                string tipoPessoa,
                                string CPFCNPJ,
                                int? Cep,
                                string cidade,
                                string email,
                                string? estado,
                                string? bairro,
                                bool indAdvogado,
                                bool indAreaCivel,
                                bool indAreaJuizado,
                                bool indAreaRegulatoria,
                                bool indAreaTrabalhista,
                                bool indAreaTributaria,
                                bool indAreaCivelAdministrativo,
                                bool indAreaCriminalAdministrativo,
                                bool indAreaCriminalJudicial,
                                bool indAreaPEX,
                                bool indAreaProcon,
                                int? alertaEm,
                                string codProfissionalSAP,
                                string site,
                                bool ehContadorPex,
                                string? CNPJ,
                                string telefone,
                                string telefoneDDD,
                                string celular,
                                string CelularDDD,
                                string Fax,
                                string FaxDDD,
                                EstadoSelecionado[] selecionadosJec,
                                EstadoSelecionado[] selecionadosCivelConsumidor,
                                bool? enviar_app_preposto = false
                                )

        {
            this.Id = id;
            this.Nome = nome.ToUpper();
            this.EhEscritorio = true;
            this.Ativo = ativo;
            this.Endereco = endereco;
            this.CivelEstrategico = civelEstrategico;
            this.TipoPessoaValor = tipoPessoa;
            this.CPF = CPFCNPJ;
            this.CEP = CEP;
            this.Cidade = cidade;
            this.Email = email;
            this.EstadoId = estado;
            this.Bairro = bairro;
            this.EhAdvogado = indAdvogado;
            this.IndAreaCivel = indAreaCivel;
            this.IndAreaJuizado = indAreaJuizado;
            this.IndAreaRegulatoria = indAreaRegulatoria;
            this.IndAreaTrabalhista = indAreaTrabalhista;
            this.IndAreaTributaria = indAreaTributaria;
            this.IndAreaCivelAdministrativo = indAreaCivelAdministrativo;
            this.IndAreaCriminalAdministrativo = indAreaCriminalAdministrativo;
            this.IndAreaCriminalJudicial = indAreaCriminalJudicial;
            this.IndAreaPEX = indAreaPEX;
            this.IndAreaProcon = indAreaProcon;
            this.AlertaEm = alertaEm;
            this.CodProfissionalSAP = codProfissionalSAP;
            this.Site = site;
            this.EhContadorPex = ehContadorPex;
            this.CNPJ = CNPJ;
            this.Telefone = telefone;
            this.TelefoneDDD = telefoneDDD;
            this.Celular = celular;
            this.CelularDDD = CelularDDD;
            this.Fax = Fax;
            this.FaxDDD = FaxDDD;
            this.selecionadosCivelConsumidor = selecionadosCivelConsumidor;
            this.selecionadosJec = selecionadosJec;
            this.Enviar_App_Preposto = enviar_app_preposto;
            Validate();
        }

        public int Id { get; private set; }
        public string? Nome { get; private set; }
        public bool Ativo { get; private set; }
        internal bool EhEscritorio { get; private set; }
        public string? Endereco { get; private set; }
        public bool? CivelEstrategico { get; private set; }

        public string? TipoPessoaValor { get; private set; } = string.Empty;
        public string? CPF { get; private set; } = string.Empty;
        public int? CEP { get; private set; }
        public string? Cidade { get; private set; } = string.Empty;
        public string? Email { get; private set; } = string.Empty;
        public string? EstadoId { get; private set; } = string.Empty;
        public string? Bairro { get; private set; } = string.Empty;

        public bool EhAdvogado { get; private set; }

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
        public bool EhContadorPex { get; private set; }
        public int? AlertaEm { get; private set; }
        public string? CodProfissionalSAP { get; private set; } = string.Empty;
        public string? Site { get; private set; }
        public string? CNPJ { get; private set; }
        public string TelefoneDDD { get; private set; }
        public string Telefone { get; private set; }
        public string? FaxDDD { get; private set; }
        public string? Fax { get; private set; }
        public string? CelularDDD { get; private set; }
        public string? Celular { get; private set; }
        public int? LoteJuizado { get; private set; }
        public string? RegistroOAB { get; private set; }
        public string? EstadoOABId { get; private set; }
        public int? SeqAdvogado { get; private set; }
        public bool? Enviar_App_Preposto { get; private set; }

        [NotMapped]
        public EstadoSelecionado[] selecionadosJec { get; private set; }

        [NotMapped]
        public EstadoSelecionado[] selecionadosCivelConsumidor { get; private set; }

        internal ProfissionalBase? ProfissionalBase { get; private set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }

        public class EstadoSelecionado
        {
            public string Id { get; set; }
            public bool Selecionado { get; set; }
        }
    }
}