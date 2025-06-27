using FluentValidation;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Parte : EntityCrud<Parte, long>
    {
        public IList<Lote> Lotes { get; set; }

        public override AbstractValidator<Parte> Validator => new ParteValidator();

        // Comentado por questão de desempenho
        public string TipoParte { get; set; }

        public string Cgc { get; set; }
        public string Cpf { get; set; }
        public long? CarteiraTrabalho { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string DddTelefone { get; set; }
        public string Telefone { get; set; }
        public string DddFax { get; set; }
        public string Fax { get; set; }
        public string Cep { get; set; }
        public string SiglaEstado { get; set; }
        public string Cidade { get; set; }
        public long? Competencia { get; set; }
        public string Bairro { get; set; }
        public long? Regional { get; set; }
        public string Rg { get; set; }
        public string CodigoCentroSap { get; set; }
        public string SiglaArquivoSap { get; set; }
        public long? Fornecedor { get; set; }
        public long? CodigoCentroCusto { get; set; }
        public bool? IndicadorGeraArquivo { get; set; }
        public long? CodigoEmpresaCentralizadora { get; set; }
        public long? CodigoEmpresaSap { get; set; }
        public long? ValorCartaFianca { get; set; }
        public DateTime? DataCartaFianca { get; set; }
        public string EnderecoAdicionais { get; set; }
        public string TelefoneAdicionais { get; set; }
        public long? CodigoMigracaoSistema { get; set; }
        public bool? IndicadorCpfCgcValido { get; set; }
        public long? CodigoDiretorioBancoBrasil { get; set; }
        public IList<ParteProcesso> PartesProcessos { get; set; }
        public IList<Processo> ProcessoParte { get; set; }
        public Empresas_Sap Empresa_Sap { get; set; }
        public EmpresasCentralizadoras EmpresaCentralizadora { get; set; }
        public IList<GrupoEmpresaContabilSapParte> GrupoEmpresaContabilSapParte { get; set; }

        public override void PreencherDados(Parte data)
        {
            TipoParte = data.TipoParte;
            Cgc = data.Cgc;
            Cpf = data.Cpf;
            CarteiraTrabalho = data.CarteiraTrabalho;
            Nome = data.Nome;
            Endereco = data.Endereco;
            DddTelefone = data.DddTelefone;
            Telefone = data.Telefone;
            DddFax = data.DddFax;
            Fax = data.Fax;
            Cep = data.Cep;
            SiglaEstado = data.SiglaEstado;
            Cidade = data.Cidade;
            Competencia = data.Competencia;
            Bairro = data.Bairro;
            Regional = data.Regional;
            Rg = data.Rg;
            CodigoCentroSap = data.CodigoCentroSap;
            SiglaArquivoSap = data.SiglaArquivoSap;
            Fornecedor = data.Fornecedor;
            CodigoCentroCusto = data.CodigoCentroCusto;
            IndicadorGeraArquivo = data.IndicadorGeraArquivo;
            CodigoEmpresaCentralizadora = data.CodigoEmpresaCentralizadora;
            CodigoEmpresaSap = data.CodigoEmpresaSap;
            ValorCartaFianca = data.ValorCartaFianca;
            DataCartaFianca = data.DataCartaFianca;
            EnderecoAdicionais = data.EnderecoAdicionais;
            TelefoneAdicionais = data.TelefoneAdicionais;
            CodigoMigracaoSistema = data.CodigoMigracaoSistema;
            IndicadorCpfCgcValido = data.IndicadorCpfCgcValido;
            //CodigoDiretorioBancoBrasil = data.CodigoDiretorioBancoBrasil;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ParteValidator : AbstractValidator<Parte>
    {
        public ParteValidator()
        {
        }
    }
}