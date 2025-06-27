using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
	public class EscritorioViewModel : BaseViewModel<long>
	{
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
		public bool? IndicadorAdvogado { get; set; }
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
		public static void Mapping(Profile mapper)
		{
			mapper.CreateMap<EscritorioViewModel, Profissional>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
				.ForMember(dest => dest.CodigoTipoPessoa, opt => opt.MapFrom(orig => orig.CodigoTipoPessoa))
				.ForMember(dest => dest.NomeProfissional, opt => opt.MapFrom(orig => orig.NomeProfissional))
				.ForMember(dest => dest.CgcProfissional, opt => opt.MapFrom(orig => orig.CgcProfissional))
				.ForMember(dest => dest.CpfProfissional, opt => opt.MapFrom(orig => orig.CpfProfissional))
				.ForMember(dest => dest.EnderecoProfissional, opt => opt.MapFrom(orig => orig.EnderecoProfissional))
				.ForMember(dest => dest.Cep, opt => opt.MapFrom(orig => orig.Cep))
				.ForMember(dest => dest.CodigoEstado, opt => opt.MapFrom(orig => orig.CodigoEstado))
				.ForMember(dest => dest.Cidade, opt => opt.MapFrom(orig => orig.Cidade))
				.ForMember(dest => dest.Bairro, opt => opt.MapFrom(orig => orig.Bairro))
				.ForMember(dest => dest.Telefone, opt => opt.MapFrom(orig => orig.Telefone))
				.ForMember(dest => dest.DddTelefone, opt => opt.MapFrom(orig => orig.DddTelefone))
				.ForMember(dest => dest.Fax, opt => opt.MapFrom(orig => orig.Fax))
				.ForMember(dest => dest.DddFax, opt => opt.MapFrom(orig => orig.DddFax))
				.ForMember(dest => dest.Celular, opt => opt.MapFrom(orig => orig.Celular))
				.ForMember(dest => dest.DddCelular, opt => opt.MapFrom(orig => orig.DddCelular))
				.ForMember(dest => dest.Site, opt => opt.MapFrom(orig => orig.Site))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(orig => orig.Email))
				.ForMember(dest => dest.IndicadorEscritorio, opt => opt.MapFrom(orig => orig.IndicadorEscritorio))
				.ForMember(dest => dest.IndicadorContador, opt => opt.MapFrom(orig => orig.IndicadorContador))
				.ForMember(dest => dest.IndicadorAreaTrabalhista, opt => opt.MapFrom(orig => orig.IndicadorAreaTrabalhista))
				.ForMember(dest => dest.IndicadorAreaRegulatoria, opt => opt.MapFrom(orig => orig.IndicadorAreaRegulatoria))
				.ForMember(dest => dest.IndicadorAreaTributaria, opt => opt.MapFrom(orig => orig.IndicadorAreaTributaria))
				.ForMember(dest => dest.IndicadorAlterarValorInternet, opt => opt.MapFrom(orig => orig.IndicadorAlterarValorInternet))
				.ForMember(dest => dest.CodigoAdvogado, opt => opt.MapFrom(orig => orig.CodigoAdvogado))
				.ForMember(dest => dest.CodigoDespesa, opt => opt.MapFrom(orig => orig.CodigoDespesa))
				.ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
				.ForMember(dest => dest.AlertarEm, opt => opt.MapFrom(orig => orig.AlertarEm))
				.ForMember(dest => dest.IndicadorAdvogado, opt => opt.MapFrom(orig => orig.IndicadorAdvogado))
				.ForMember(dest => dest.NumeroOabAdvogado, opt => opt.MapFrom(orig => orig.NumeroOabAdvogado))
				.ForMember(dest => dest.IndicadorAreaJuizado, opt => opt.MapFrom(orig => orig.IndicadorAreaJuizado))
				.ForMember(dest => dest.CodigoEstadoOab, opt => opt.MapFrom(orig => orig.CodigoEstadoOab))
				.ForMember(dest => dest.CodigoGrupoLoteJuizado, opt => opt.MapFrom(orig => orig.CodigoGrupoLoteJuizado))
				.ForMember(dest => dest.IndicadorCivelEstrategico, opt => opt.MapFrom(orig => orig.IndicadorCivelEstrategico))
				.ForMember(dest => dest.IndicadorCriminalAdm, opt => opt.MapFrom(orig => orig.IndicadorCriminalAdm))
				.ForMember(dest => dest.IndicadorIndCriminalJudicial, opt => opt.MapFrom(orig => orig.IndicadorIndCriminalJudicial))
				.ForMember(dest => dest.IndicadorCivelAdm, opt => opt.MapFrom(orig => orig.IndicadorCivelAdm))
				.ForMember(dest => dest.IndicadorProcom, opt => opt.MapFrom(orig => orig.IndicadorProcom))
				.ForMember(dest => dest.IndicadorPex, opt => opt.MapFrom(orig => orig.IndicadorPex))
				.ForMember(dest => dest.IndicadorContadorPex, opt => opt.MapFrom(orig => orig.IndicadorContadorPex))
				.ForMember(dest => dest.IndicadorAreaCivel, opt => opt.MapFrom(orig => orig.IndicadorAreaCivel))
				.ForMember(dest => dest.EnderecoAdicionais, opt => opt.MapFrom(orig => orig.EnderecoAdicionais))
				.ForMember(dest => dest.TelefoneAdicionais, opt => opt.MapFrom(orig => orig.TelefoneAdicionais))
				.ForMember(dest => dest.CodigoProfissionalSap, opt => opt.MapFrom(orig => orig.CodigoProfissionalSap))
				.ForMember(dest => dest.IndicadorAtivo, opt => opt.MapFrom(orig => orig.IndicadorAtivo));


			mapper.CreateMap<Profissional, EscritorioViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
				.ForMember(dest => dest.NomeProfissional, opt => opt.MapFrom(orig => orig.NomeProfissional))
				.ForMember(dest => dest.CgcProfissional, opt => opt.MapFrom(orig => orig.CgcProfissional))
				.ForMember(dest => dest.CpfProfissional, opt => opt.MapFrom(orig => orig.CpfProfissional))
				.ForMember(dest => dest.EnderecoProfissional, opt => opt.MapFrom(orig => orig.EnderecoProfissional))
				.ForMember(dest => dest.Cep, opt => opt.MapFrom(orig => orig.Cep))
				.ForMember(dest => dest.CodigoEstado, opt => opt.MapFrom(orig => orig.CodigoEstado))
				.ForMember(dest => dest.Cidade, opt => opt.MapFrom(orig => orig.Cidade))
				.ForMember(dest => dest.Bairro, opt => opt.MapFrom(orig => orig.Bairro))
				.ForMember(dest => dest.Telefone, opt => opt.MapFrom(orig => orig.Telefone))
				.ForMember(dest => dest.DddTelefone, opt => opt.MapFrom(orig => orig.DddTelefone))
				.ForMember(dest => dest.Fax, opt => opt.MapFrom(orig => orig.Fax))
				.ForMember(dest => dest.DddFax, opt => opt.MapFrom(orig => orig.DddFax))
				.ForMember(dest => dest.Celular, opt => opt.MapFrom(orig => orig.Celular))
				.ForMember(dest => dest.DddCelular, opt => opt.MapFrom(orig => orig.DddCelular))
				.ForMember(dest => dest.Site, opt => opt.MapFrom(orig => orig.Site))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(orig => orig.Email))
				.ForMember(dest => dest.IndicadorEscritorio, opt => opt.MapFrom(orig => orig.IndicadorEscritorio))
				.ForMember(dest => dest.IndicadorContador, opt => opt.MapFrom(orig => orig.IndicadorContador))
				.ForMember(dest => dest.IndicadorAreaTrabalhista, opt => opt.MapFrom(orig => orig.IndicadorAreaTrabalhista))
				.ForMember(dest => dest.IndicadorAreaRegulatoria, opt => opt.MapFrom(orig => orig.IndicadorAreaRegulatoria))
				.ForMember(dest => dest.IndicadorAreaTributaria, opt => opt.MapFrom(orig => orig.IndicadorAreaTributaria))
				.ForMember(dest => dest.IndicadorAlterarValorInternet, opt => opt.MapFrom(orig => orig.IndicadorAlterarValorInternet))
				.ForMember(dest => dest.CodigoAdvogado, opt => opt.MapFrom(orig => orig.CodigoAdvogado))
				.ForMember(dest => dest.CodigoDespesa, opt => opt.MapFrom(orig => orig.CodigoDespesa))
				.ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
				.ForMember(dest => dest.AlertarEm, opt => opt.MapFrom(orig => orig.AlertarEm))
				.ForMember(dest => dest.IndicadorAdvogado, opt => opt.MapFrom(orig => orig.IndicadorAdvogado))
				.ForMember(dest => dest.NumeroOabAdvogado, opt => opt.MapFrom(orig => orig.NumeroOabAdvogado))
				.ForMember(dest => dest.IndicadorAreaJuizado, opt => opt.MapFrom(orig => orig.IndicadorAreaJuizado))
				.ForMember(dest => dest.CodigoEstadoOab, opt => opt.MapFrom(orig => orig.CodigoEstadoOab))
				.ForMember(dest => dest.CodigoGrupoLoteJuizado, opt => opt.MapFrom(orig => orig.CodigoGrupoLoteJuizado))
				.ForMember(dest => dest.IndicadorCivelEstrategico, opt => opt.MapFrom(orig => orig.IndicadorCivelEstrategico))
				.ForMember(dest => dest.IndicadorCriminalAdm, opt => opt.MapFrom(orig => orig.IndicadorCriminalAdm))
				.ForMember(dest => dest.IndicadorIndCriminalJudicial, opt => opt.MapFrom(orig => orig.IndicadorIndCriminalJudicial))
				.ForMember(dest => dest.IndicadorCivelAdm, opt => opt.MapFrom(orig => orig.IndicadorCivelAdm))
				.ForMember(dest => dest.IndicadorProcom, opt => opt.MapFrom(orig => orig.IndicadorProcom))
				.ForMember(dest => dest.IndicadorPex, opt => opt.MapFrom(orig => orig.IndicadorPex))
				.ForMember(dest => dest.IndicadorContadorPex, opt => opt.MapFrom(orig => orig.IndicadorContadorPex))
				.ForMember(dest => dest.IndicadorAreaCivel, opt => opt.MapFrom(orig => orig.IndicadorAreaCivel))
				.ForMember(dest => dest.EnderecoAdicionais, opt => opt.MapFrom(orig => orig.EnderecoAdicionais))
				.ForMember(dest => dest.TelefoneAdicionais, opt => opt.MapFrom(orig => orig.TelefoneAdicionais))
				.ForMember(dest => dest.CodigoProfissionalSap, opt => opt.MapFrom(orig => orig.CodigoProfissionalSap))
				.ForMember(dest => dest.IndicadorAtivo, opt => opt.MapFrom(orig => orig.IndicadorAtivo));

        }
    }
    public class EscritorioListaViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<EscritorioListaViewModel, EscritorioDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));

            mapper.CreateMap<EscritorioDTO, EscritorioListaViewModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
        }

    }
}
