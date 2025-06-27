using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Profissional : EntityCrud<Profissional, long>
    {
        public override AbstractValidator<Profissional> Validator => new ProfissionalValidator();
               
        public string CodigoTipoPessoa { get; set; }
        public string CgcProfissional { get; set; }
        public string CpfProfissional { get; set; }
        public string NomeProfissional { get; set; }
        public string EnderecoProfissional { get; set; }
        public long? Cep { get; set; }
        public string CodigoEstado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Telefone { get; set; }
        public string DddTelefone { get; set; }
        public string Fax { get; set; }
        public string DddFax { get; set; }
        public string Celular { get; set; }
        public string DddCelular { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public bool? IndicadorEscritorio { get; set; }
        public bool? IndicadorContador { get; set; }
        public bool? IndicadorAreaTrabalhista { get; set; }
        public bool? IndicadorAreaRegulatoria { get; set; }
        public bool? IndicadorAreaTributaria { get; set; }
        public bool? IndicadorAlterarValorInternet { get; set; }
        public long? CodigoAdvogado { get; set; }
        public long? CodigoDespesa { get; set; }
        public long? Fornecedor { get; set; }
        public long? AlertarEm { get; set; }
        public  bool?  IndicadorAdvogado { get; set; }
        public string NumeroOabAdvogado { get; set; }
        public bool? IndicadorAreaJuizado { get; set; }
        public string CodigoEstadoOab { get; set; }
        public long? CodigoGrupoLoteJuizado { get; set; }
        public bool? IndicadorCivelEstrategico { get; set; }
        public bool? IndicadorCriminalAdm { get; set; }
        public bool? IndicadorIndCriminalJudicial { get; set; }
        public bool? IndicadorCivelAdm { get; set; }
        public bool? IndicadorProcom { get; set; }
        public bool? IndicadorPex { get; set; }
        public bool? IndicadorContadorPex { get; set; }
        public bool? IndicadorAreaCivel { get; set; }
        public string EnderecoAdicionais { get; set; }
        public string TelefoneAdicionais { get; set; }
        public string CodigoProfissionalSap { get; set; }
        public bool? IndicadorAtivo { get; set; }

        public AdvogadoEscritorio AdvogadoEscritorio { get; set; }
        public IList<Processo> ProcessosProfissional { get; set; }
        public IList<Fornecedor> FornecedorProfissionais { get; set; }
        public IList<Fornecedor> FornecedorEscritorios { get; set; }
        public override void PreencherDados(Profissional data)
        {           
            CodigoTipoPessoa = data.CodigoTipoPessoa;
            CgcProfissional = data.CgcProfissional;
            CpfProfissional = data.CpfProfissional;
            NomeProfissional = data.NomeProfissional;
            EnderecoProfissional = data.EnderecoProfissional;
            Cep = data.Cep;
            CodigoEstado = data.CodigoEstado;
            Cidade = data.Cidade;
            Bairro = data.Bairro;
            Telefone = data.Telefone;
            DddTelefone = data.DddTelefone;
            Fax = data.Fax;
            DddFax = data.DddFax;
            Celular = data.Celular;
            DddCelular = data.DddCelular;
            Site = data.Site;
            Email = data.Email;
            IndicadorEscritorio = data.IndicadorEscritorio;
            IndicadorContador = data.IndicadorContador;
            IndicadorAreaCivel = data.IndicadorAreaCivel;
            IndicadorAreaTrabalhista = data.IndicadorAreaTrabalhista;
            IndicadorAreaRegulatoria = data.IndicadorAreaRegulatoria;
            IndicadorAreaTributaria = data.IndicadorAreaTributaria;
            IndicadorAlterarValorInternet = data.IndicadorAlterarValorInternet;
            CodigoAdvogado = data.CodigoAdvogado;
            CodigoDespesa = data.CodigoDespesa;
            Fornecedor = data.Fornecedor;
            AlertarEm = data.AlertarEm;
            IndicadorAdvogado = data.IndicadorAdvogado;
            NumeroOabAdvogado = data.NumeroOabAdvogado;
            IndicadorAreaJuizado = data.IndicadorAreaJuizado;
            CodigoEstadoOab = data.CodigoEstadoOab;
            CodigoGrupoLoteJuizado = data.CodigoGrupoLoteJuizado;
            IndicadorCivelEstrategico = data.IndicadorCivelEstrategico;
            IndicadorCriminalAdm = data.IndicadorCriminalAdm;
            IndicadorIndCriminalJudicial = data.IndicadorIndCriminalJudicial;
            IndicadorCivelAdm = data.IndicadorCivelAdm;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ProfissionalValidator : AbstractValidator<Profissional>
    {
        public ProfissionalValidator()
        {

        }    
    }
}
