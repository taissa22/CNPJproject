using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using System.Globalization;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel {
    public class BorderoViewModel : BaseViewModel<long>
    {
        public long CodigoLote { get; set; }
        public string NomeBeneficiario { get; set; }
        public string CpfBeneficiario { get; set; }
        public string CnpjBeneficiario { get; set; }
        public string CidadeBeneficiario { get; set; }
        public string NumeroBancoBeneficiario { get; set; }
        public string DigitoBancoBeneficiario { get; set; }
        public string NumeroAgenciaBeneficiario { get; set; }
        public string DigitoAgenciaBeneficiario { get; set; }
        public string NumeroContaCorrenteBeneficiario { get; set; }
        public string DigitoContaCorrenteBeneficiario { get; set; }
        public decimal Valor { get; set; }
        public string Comentario { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BorderoViewModel, Bordero>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.NomeBeneficiario, opt => opt.MapFrom(orig => orig.NomeBeneficiario))
              .ForMember(dest => dest.CnpjBeneficiario, opt => opt.MapFrom(orig => orig.CnpjBeneficiario))
              .ForMember(dest => dest.CidadeBeneficiario, opt => opt.MapFrom(orig => orig.CidadeBeneficiario))
              .ForMember(dest => dest.NumeroBancoBeneficiario, opt => opt.MapFrom(orig => orig.NumeroBancoBeneficiario))
              .ForMember(dest => dest.DigitoBancoBeneficiario, opt => opt.MapFrom(orig => orig.DigitoBancoBeneficiario))
              .ForMember(dest => dest.NumeroAgenciaBeneficiario, opt => opt.MapFrom(orig => orig.NumeroAgenciaBeneficiario))
              .ForMember(dest => dest.DigitoAgenciaBeneficiario, opt => opt.MapFrom(orig => orig.DigitoAgenciaBeneficiario))
              .ForMember(dest => dest.NumeroContaCorrenteBeneficiario, opt => opt.MapFrom(orig => orig.NumeroContaCorrenteBeneficiario))
              .ForMember(dest => dest.DigitoContaCorrenteBeneficiario, opt => opt.MapFrom(orig => orig.DigitoContaCorrenteBeneficiario))
              .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor))
              .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
              .ForMember(dest => dest.CodigoLote, opt => opt.MapFrom(orig => orig.CodigoLote));
            mapper.CreateMap<Bordero, BorderoViewModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.NomeBeneficiario, opt => opt.MapFrom(orig => orig.NomeBeneficiario))
              .ForMember(dest => dest.CnpjBeneficiario, opt => opt.MapFrom(orig => orig.CnpjBeneficiario))
              .ForMember(dest => dest.CidadeBeneficiario, opt => opt.MapFrom(orig => orig.CidadeBeneficiario))
              .ForMember(dest => dest.NumeroBancoBeneficiario, opt => opt.MapFrom(orig => orig.NumeroBancoBeneficiario))
              .ForMember(dest => dest.DigitoBancoBeneficiario, opt => opt.MapFrom(orig => orig.DigitoBancoBeneficiario))
              .ForMember(dest => dest.NumeroAgenciaBeneficiario, opt => opt.MapFrom(orig => orig.NumeroAgenciaBeneficiario))
              .ForMember(dest => dest.DigitoAgenciaBeneficiario, opt => opt.MapFrom(orig => orig.DigitoAgenciaBeneficiario))
              .ForMember(dest => dest.NumeroContaCorrenteBeneficiario, opt => opt.MapFrom(orig => orig.NumeroContaCorrenteBeneficiario))
              .ForMember(dest => dest.DigitoContaCorrenteBeneficiario, opt => opt.MapFrom(orig => orig.DigitoContaCorrenteBeneficiario))
              .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor))
              .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario))
              .ForMember(dest => dest.CodigoLote, opt => opt.MapFrom(orig => orig.CodigoLote));
        }
    }

    public class BorderoExportarViewModel {
        [Name("Seq")]
        public string Id { get; set; }
        [Name("Beneficiário")]
        public string NomeBeneficiario { get; set; }
        [Name("CPF")]
        public string CpfBeneficiario { get; set; }
        [Name("CNPJ")]
        public string CnpjBeneficiario { get; set; }
        [Name("Banco")]
        public string NumeroBancoBeneficiario { get; set; }
        [Name("DV")]
        public string DigitoBancoBeneficiario { get; set; }
        [Name("Agência")]
        public string NumeroAgenciaBeneficiario { get; set; }
        [Name("DV")]
        public string DigitoAgenciaBeneficiario { get; set; }
        [Name("N° C/C")]
        public string NumeroContaCorrenteBeneficiario { get; set; }
        [Name("DV")]
        public string DigitoContaCorrenteBeneficiario { get; set; }
        [Name("Valor")]
        public string Valor { get; set; }
        [Name("Cidade")]
        public string CidadeBeneficiario { get; set; }
        [Name("Histórico")]
        public string Comentario { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<Bordero, BorderoExportarViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id.ToString()))
              .ForMember(dest => dest.NomeBeneficiario, opt => opt.MapFrom(orig => orig.NomeBeneficiario))
              .ForMember(dest => dest.CpfBeneficiario, opt => opt.MapFrom(orig =>  string.IsNullOrEmpty(orig.CpfBeneficiario)? "" : string.Format("'{0}", orig.CpfBeneficiario)))
              .ForMember(dest => dest.CnpjBeneficiario, opt => opt.MapFrom(orig => string.IsNullOrEmpty(orig.CnpjBeneficiario)? "" : string.Format("'{0}", orig.CnpjBeneficiario)))
              .ForMember(dest => dest.CidadeBeneficiario, opt => opt.MapFrom(orig => orig.CidadeBeneficiario))
              .ForMember(dest => dest.NumeroBancoBeneficiario, opt => opt.MapFrom(orig => string.IsNullOrEmpty(orig.NumeroBancoBeneficiario)? "" : string.Format("'{0}", orig.NumeroBancoBeneficiario)))
              .ForMember(dest => dest.DigitoBancoBeneficiario, opt => opt.MapFrom(orig => orig.DigitoBancoBeneficiario))
              .ForMember(dest => dest.NumeroAgenciaBeneficiario, opt => opt.MapFrom(orig => string.IsNullOrEmpty(orig.NumeroAgenciaBeneficiario)? "" : string.Format("'{0}", orig.NumeroAgenciaBeneficiario)))
              .ForMember(dest => dest.DigitoAgenciaBeneficiario, opt => opt.MapFrom(orig => orig.DigitoAgenciaBeneficiario))
              .ForMember(dest => dest.NumeroContaCorrenteBeneficiario, opt => opt.MapFrom(orig => string.IsNullOrEmpty(orig.NumeroContaCorrenteBeneficiario)? "" : string.Format("'{0}", orig.NumeroContaCorrenteBeneficiario)))
              .ForMember(dest => dest.DigitoContaCorrenteBeneficiario, opt => opt.MapFrom(orig => orig.DigitoContaCorrenteBeneficiario))
              .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
              .ForMember(dest => dest.Comentario, opt => opt.MapFrom(orig => orig.Comentario));
        }
    }
}
