using AutoMapper;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra.Entities;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.AlteracaoBloco.ViewModel
{
    public class AlteracaoEmBlocoViewModel : BaseViewModel<long>
    {
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataExecucao { get; set; }
        public AlteracaoEmBlocoEnum? Status { get; set; }
        public string Arquivo { get; set; }
        public string CodigoDoUsuario { get; set; }
        public int ProcessosAtualizados { get; set; }
        public int ProcessosComErro { get; set; }
        public TipoProcessoEnum? CodigoTipoProcesso { get; set; }
        public string NomeUsuario { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AlteracaoEmBloco, AlteracaoEmBlocoViewModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(orig => orig.DataCadastro))
              .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(orig => orig.DataExecucao))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
              .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.Arquivo))
              .ForMember(dest => dest.CodigoDoUsuario, opt => opt.MapFrom(orig => orig.CodigoDoUsuario))
              .ForMember(dest => dest.ProcessosAtualizados, opt => opt.MapFrom(orig => orig.ProcessosAtualizados))
              .ForMember(dest => dest.ProcessosComErro, opt => opt.MapFrom(orig => orig.ProcessosComErro))
              .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso));

            mapper.CreateMap<AlteracaoEmBlocoViewModel, AlteracaoEmBloco>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(orig => orig.DataCadastro))
              .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(orig => orig.DataExecucao))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
              .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.Arquivo))
              .ForMember(dest => dest.CodigoDoUsuario, opt => opt.MapFrom(orig => orig.CodigoDoUsuario))
              .ForMember(dest => dest.ProcessosAtualizados, opt => opt.MapFrom(orig => orig.ProcessosAtualizados))
              .ForMember(dest => dest.ProcessosComErro, opt => opt.MapFrom(orig => orig.ProcessosComErro))
              .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso));

            mapper.CreateMap<AlteracaoEmBlocoRetornoDTO, AlteracaoEmBlocoViewModel>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
             .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(orig => orig.DataCadastro))
             .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(orig => orig.DataExecucao))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
             .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.Arquivo))
             .ForMember(dest => dest.CodigoDoUsuario, opt => opt.MapFrom(orig => orig.CodigoDoUsuario))
             .ForMember(dest => dest.ProcessosAtualizados, opt => opt.MapFrom(orig => orig.ProcessosAtualizados))
             .ForMember(dest => dest.ProcessosComErro, opt => opt.MapFrom(orig => orig.ProcessosComErro))
             .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso))
             .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(orig => orig.NomeUsuario));

        }
    }

    public class AlteracaoEmBlocoDownloadViewModel
    {
        public long Id { get; set; }
        public byte[] Arquivo { get; set; }
        public string NomeDoArquivo { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AlteracaoEmBlocoDTO, AlteracaoEmBlocoDownloadViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(orig => orig.Arquivo))
                .ForMember(dest => dest.NomeDoArquivo, opt => opt.MapFrom(orig => orig.NomeDoArquivo));
        }
    }
}
