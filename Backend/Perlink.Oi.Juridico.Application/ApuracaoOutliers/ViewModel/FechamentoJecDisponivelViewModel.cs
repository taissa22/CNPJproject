using AutoMapper;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Shared.Application.ViewModel;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel {
    public class FechamentoJecDisponivelViewModel : BaseViewModel<long>{
        public string Descricao { get; set; }
        public string DataFechamento { get; set; }
        public string MesAnoFechamento { get; set; }
        public int NumeroMeses { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<FechamentoJecDisponivelDTO, FechamentoJecDisponivelViewModel>()
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => string.Format("{0}/{1} - {2} - {3} Meses{4}", orig.DataFechamento.ToString("MM"), orig.DataFechamento.Year, orig.DataFechamento.ToShortDateString(), orig.NumeroMeses,orig.IndFechamentoMensal? " - Fechamento Mensal":"")))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(orig => orig.DataFechamento.DataFormatada()))
                .ForMember(dest => dest.MesAnoFechamento, opt => opt.MapFrom(orig => orig.MesAnoFechamento.DataFormatada()))
                .ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodigoEmpresaCentralizadora));

        }
    }
}
