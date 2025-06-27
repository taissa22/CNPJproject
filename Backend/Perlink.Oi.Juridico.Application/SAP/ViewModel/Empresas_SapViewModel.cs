using AutoMapper;
using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using Shared.Domain.Impl;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class Empresas_SapViewModel : BaseViewModel<long>
    {
        public AbstractValidator<Empresas_SapViewModel> Validator => new Empresas_SapValidator();

        public string Sigla { get; set; }
        public string Nome { get; set; }
        public bool IndicaEnvioArquivoSolicitacao { get; set; }
        public bool IndicaAtivo { get; set; }
        public string CodigoOrganizacaoCompra { get; set; }
        public bool ConfirmaSiglaRepetidaNaAlteracao { get; set; }

        

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<Empresas_SapViewModel, Empresas_Sap>()
            .ForMember(dest => dest.Sigla, opt => opt.MapFrom(orig => orig.Sigla.ToUpper()))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(orig => orig.Nome.ToUpper()))
            .ForMember(dest => dest.CodigoOrganizacaoCompra, opt => opt.MapFrom(orig => orig.CodigoOrganizacaoCompra.ToUpper()));

            mapper.CreateMap<Empresas_Sap, Empresas_SapViewModel>();

        }

        internal class Empresas_SapValidator : AbstractValidator<Empresas_SapViewModel>
        {
            public Empresas_SapValidator()
            {
                RuleFor(x => x.Sigla)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(4).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
                RuleFor(x => x.Nome)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
                RuleFor(x => x.CodigoOrganizacaoCompra)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null).
                    MaximumLength(4).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
            }
        }
    }
}
