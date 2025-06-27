using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class ExportacaoPrePosRJViewModel : BaseViewModel<long>
    {
        public DateTime? DataExtracao { get; set; } // data_extracao date
        public DateTime? DataExecucao { get; set; } // data_execucao date
        public bool? NaoExpurgar { get; set; } // expurgar char (1)
        public string ArquivoJec { get; set; } // arquivo_jec varchar2(70)
        public string ArquivoTrabalhista { get; set; } //arquivo_trabalhista varchar2(70)
        public string ArquivoCivelConsumidor { get; set; } //arquivo_civelconsumidor varchar2(70)
        public string ArquivoCivelEstrategico { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoPex { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoTributarioJudicial { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoAdministrativo { get; set; } //arquivo_civelestrategico varchar2(70)
        //public byte[] ArquivoZip { get; set; }
        //public string NomeArquivoZip { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ExportacaoPrePosRJ, ExportacaoPrePosRJViewModel>()
                .ForMember(dest => dest.DataExtracao, opt => opt.MapFrom(orig => orig.DataExtracao))
                .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(orig => orig.DataExecucao))
                .ForMember(dest => dest.NaoExpurgar, opt => opt.MapFrom(orig => orig.NaoExpurgar))
                .ForMember(dest => dest.ArquivoJec, opt => opt.MapFrom(orig => orig.ArquivoJec))
                .ForMember(dest => dest.ArquivoTrabalhista, opt => opt.MapFrom(orig => orig.ArquivoTrabalhista))
                .ForMember(dest => dest.ArquivoCivelConsumidor, opt => opt.MapFrom(orig => orig.ArquivoCivelConsumidor))
                .ForMember(dest => dest.ArquivoCivelEstrategico, opt => opt.MapFrom(orig => orig.ArquivoCivelEstrategico))
                .ForMember(dest => dest.ArquivoPex, opt => opt.MapFrom(orig => orig.ArquivoPex))
                .ForMember(dest => dest.ArquivoTributarioJudicial, opt => opt.MapFrom(orig => orig.ArquivoTributarioJudicial))
                .ForMember(dest => dest.ArquivoAdministrativo, opt => opt.MapFrom(orig => orig.ArquivoAdministrativo));

            mapper.CreateMap<ExportacaoPrePosRJViewModel, ExportacaoPrePosRJ>()
               .ForMember(dest => dest.DataExtracao, opt => opt.MapFrom(orig => orig.DataExtracao))
               .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(orig => orig.DataExecucao))
               .ForMember(dest => dest.NaoExpurgar, opt => opt.MapFrom(orig => orig.NaoExpurgar))
               .ForMember(dest => dest.ArquivoJec, opt => opt.MapFrom(orig => orig.ArquivoJec))
               .ForMember(dest => dest.ArquivoTrabalhista, opt => opt.MapFrom(orig => orig.ArquivoTrabalhista))
               .ForMember(dest => dest.ArquivoCivelConsumidor, opt => opt.MapFrom(orig => orig.ArquivoCivelConsumidor))
               .ForMember(dest => dest.ArquivoCivelEstrategico, opt => opt.MapFrom(orig => orig.ArquivoCivelEstrategico))
               .ForMember(dest => dest.ArquivoPex, opt => opt.MapFrom(orig => orig.ArquivoPex))
               .ForMember(dest => dest.ArquivoTributarioJudicial, opt => opt.MapFrom(orig => orig.ArquivoTributarioJudicial))
               .ForMember(dest => dest.ArquivoAdministrativo, opt => opt.MapFrom(orig => orig.ArquivoAdministrativo));
        }
    }

    public class ExportacaoPrePosRJListaViewModel
    {
        public byte[] ArquivoZip { get; set; }
        public string NomeArquivoZip { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ExportacaoPrePosRJListaViewModel, DownloadExportacaoPrePosRjDTO>()
                .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.ArquivoZip))
                .ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(orig => orig.NomeArquivoZip));

            mapper.CreateMap<DownloadExportacaoPrePosRjDTO, ExportacaoPrePosRJListaViewModel>()
               .ForMember(dest => dest.ArquivoZip, opt => opt.MapFrom(orig => orig.Arquivo))
               .ForMember(dest => dest.NomeArquivoZip, opt => opt.MapFrom(orig => orig.NomeArquivo));
        }
    }
}
