using AutoMapper;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO
{
    public class AgendarApuracaoOutliersDTO
    {
        public int CodEmpresaCentralizadora { get; set; }
        public string MesAnoFechamento { get; set; }
        public string DataFechamento { get; set; }
        public string Observacao { get; set; }
        public string FatorDesvioPadrao { get; set; }
        public string DataSolicitacao { get; set; }
        public string NomeUsuario { get; set; }
        public string Status { get; set; }
        public string DataFinalizacao { get; set; }
        public string ArquivoBaseFechamento { get; set; }
        public string ArquivoResultado { get; set; }
        public string MgsStatusErro { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendarApuracaoOutliersDTO, AgendarApuracaoOutliers>()
                .ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodEmpresaCentralizadora))
                .ForMember(dest => dest.MesAnoFechamento, opt => opt.MapFrom(orig => orig.MesAnoFechamento))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(orig => orig.DataFechamento))
                .ForMember(dest => dest.DataSolicitacao, opt => opt.MapFrom(orig => orig.DataSolicitacao))
                .ForMember(dest => dest.FatorDesvioPadrao, opt => opt.MapFrom(orig => orig.FatorDesvioPadrao))
                .ForMember(dest => dest.Observacao, opt => opt.MapFrom(orig => orig.Observacao))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(orig => orig.NomeUsuario))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
                .ForMember(dest => dest.DataFinalizacao, opt => opt.MapFrom(orig => orig.DataFinalizacao))
                .ForMember(dest => dest.ArquivoBaseFechamento, opt => opt.MapFrom(orig => orig.ArquivoBaseFechamento))
                .ForMember(dest => dest.ArquivoResultado, opt => opt.MapFrom(orig => orig.ArquivoResultado))
                .ForMember(dest => dest.MgsStatusErro, opt => opt.MapFrom(orig => orig.MgsStatusErro));
        }
    }
}
