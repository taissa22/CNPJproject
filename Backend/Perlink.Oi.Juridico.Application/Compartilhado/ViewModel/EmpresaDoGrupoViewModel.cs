using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class EmpresaDoGrupoViewModel : BaseViewModel<long>
    {
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
        public long? CentroCusto { get; set; }
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

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EmpresaDoGrupoViewModel, Parte>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.TipoParte, opt => opt.MapFrom(orig => orig.TipoParte))
                //.ForMember(dest => dest.Cgc, opt => opt.MapFrom(orig => orig.Cgc))
                //.ForMember(dest => dest.Cpf, opt => opt.MapFrom(orig => orig.Cpf))
                //.ForMember(dest => dest.CarteiraTrabalho, opt => opt.MapFrom(orig => orig.CarteiraTrabalho))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome));
                //.ForMember(dest => dest.Endereco, opt => opt.MapFrom(orig => orig.Endereco))
                //.ForMember(dest => dest.DddTelefone, opt => opt.MapFrom(orig => orig.DddTelefone))
                //.ForMember(dest => dest.Telefone, opt => opt.MapFrom(orig => orig.Telefone))
                //.ForMember(dest => dest.DddFax, opt => opt.MapFrom(orig => orig.DddFax))
                //.ForMember(dest => dest.Fax, opt => opt.MapFrom(orig => orig.Fax))
                //.ForMember(dest => dest.Cep, opt => opt.MapFrom(orig => orig.Cep))
                //.ForMember(dest => dest.SiglaEstado, opt => opt.MapFrom(orig => orig.SiglaEstado))
                //.ForMember(dest => dest.Cidade, opt => opt.MapFrom(orig => orig.Cidade))
                //.ForMember(dest => dest.Competencia, opt => opt.MapFrom(orig => orig.Competencia))
                //.ForMember(dest => dest.Bairro, opt => opt.MapFrom(orig => orig.Bairro))
                //.ForMember(dest => dest.Regional, opt => opt.MapFrom(orig => orig.Regional))
                //.ForMember(dest => dest.Rg, opt => opt.MapFrom(orig => orig.Rg))
                //.ForMember(dest => dest.CodigoCentroSap, opt => opt.MapFrom(orig => orig.CodigoCentroSap))
                //.ForMember(dest => dest.SiglaArquivoSap, opt => opt.MapFrom(orig => orig.SiglaArquivoSap))
                //.ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
                //.ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(orig => orig.CentroCusto))
                //.ForMember(dest => dest.IndicadorGeraArquivo, opt => opt.MapFrom(orig => orig.IndicadorGeraArquivo))
                //.ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodigoEmpresaCentralizadora))
                //.ForMember(dest => dest.CodigoEmpresaSap, opt => opt.MapFrom(orig => orig.CodigoEmpresaSap))
                //.ForMember(dest => dest.ValorCartaFianca, opt => opt.MapFrom(orig => orig.ValorCartaFianca))
                //.ForMember(dest => dest.DataCartaFianca, opt => opt.MapFrom(orig => orig.DataCartaFianca))
                //.ForMember(dest => dest.EnderecoAdicionais, opt => opt.MapFrom(orig => orig.EnderecoAdicionais))
                //.ForMember(dest => dest.TelefoneAdicionais, opt => opt.MapFrom(orig => orig.TelefoneAdicionais))
                //.ForMember(dest => dest.CodigoMigracaoSistema, opt => opt.MapFrom(orig => orig.CodigoMigracaoSistema))
                //.ForMember(dest => dest.IndicadorCpfCgcValido, opt => opt.MapFrom(orig => orig.IndicadorCpfCgcValido))
                //.ForMember(dest => dest.CodigoDiretorioBancoBrasil, opt => opt.MapFrom(orig => orig.CodigoDiretorioBancoBrasil));

            mapper.CreateMap<Parte, EmpresaDoGrupoViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.TipoParte, opt => opt.MapFrom(orig => orig.TipoParte))
                //.ForMember(dest => dest.Cgc, opt => opt.MapFrom(orig => orig.Cgc))
                //.ForMember(dest => dest.Cpf, opt => opt.MapFrom(orig => orig.Cpf))
                //.ForMember(dest => dest.CarteiraTrabalho, opt => opt.MapFrom(orig => orig.CarteiraTrabalho))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome));
                //.ForMember(dest => dest.Endereco, opt => opt.MapFrom(orig => orig.Endereco))
                //.ForMember(dest => dest.DddTelefone, opt => opt.MapFrom(orig => orig.DddTelefone))
                //.ForMember(dest => dest.Telefone, opt => opt.MapFrom(orig => orig.Telefone))
                //.ForMember(dest => dest.DddFax, opt => opt.MapFrom(orig => orig.DddFax))
                //.ForMember(dest => dest.Fax, opt => opt.MapFrom(orig => orig.Fax))
                //.ForMember(dest => dest.Cep, opt => opt.MapFrom(orig => orig.Cep))
                //.ForMember(dest => dest.SiglaEstado, opt => opt.MapFrom(orig => orig.SiglaEstado))
                //.ForMember(dest => dest.Cidade, opt => opt.MapFrom(orig => orig.Cidade))
                //.ForMember(dest => dest.Competencia, opt => opt.MapFrom(orig => orig.Competencia))
                //.ForMember(dest => dest.Bairro, opt => opt.MapFrom(orig => orig.Bairro))
                //.ForMember(dest => dest.Regional, opt => opt.MapFrom(orig => orig.Regional))
                //.ForMember(dest => dest.Rg, opt => opt.MapFrom(orig => orig.Rg))
                //.ForMember(dest => dest.CodigoCentroSap, opt => opt.MapFrom(orig => orig.CodigoCentroSap))
                //.ForMember(dest => dest.SiglaArquivoSap, opt => opt.MapFrom(orig => orig.SiglaArquivoSap))
                //.ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
                //.ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(orig => orig.CentroCusto))
                //.ForMember(dest => dest.IndicadorGeraArquivo, opt => opt.MapFrom(orig => orig.IndicadorGeraArquivo))
                //.ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodigoEmpresaCentralizadora))
                //.ForMember(dest => dest.CodigoEmpresaSap, opt => opt.MapFrom(orig => orig.CodigoEmpresaSap))
                //.ForMember(dest => dest.ValorCartaFianca, opt => opt.MapFrom(orig => orig.ValorCartaFianca))
                //.ForMember(dest => dest.DataCartaFianca, opt => opt.MapFrom(orig => orig.DataCartaFianca))
                //.ForMember(dest => dest.EnderecoAdicionais, opt => opt.MapFrom(orig => orig.EnderecoAdicionais))
                //.ForMember(dest => dest.TelefoneAdicionais, opt => opt.MapFrom(orig => orig.TelefoneAdicionais))
                //.ForMember(dest => dest.CodigoMigracaoSistema, opt => opt.MapFrom(orig => orig.CodigoMigracaoSistema))
                //.ForMember(dest => dest.IndicadorCpfCgcValido, opt => opt.MapFrom(orig => orig.IndicadorCpfCgcValido))
                //.ForMember(dest => dest.CodigoDiretorioBancoBrasil, opt => opt.MapFrom(orig => orig.CodigoDiretorioBancoBrasil));
        }
    }

    public class EmpresaDoGrupoListaViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EmpresaDoGrupoListaViewModel, EmpresaDoGrupoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Descricao));

            mapper.CreateMap<EmpresaDoGrupoDTO,EmpresaDoGrupoListaViewModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Nome));
        }
    }
}