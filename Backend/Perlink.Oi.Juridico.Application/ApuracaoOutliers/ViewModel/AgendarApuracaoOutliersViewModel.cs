using AutoMapper;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using Shared.Application.ViewModel;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel
{
    public class AgendarApuracaoOutliersViewModel : BaseViewModel<long>  {
        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal FatorDesvioPadrao { get; set; }
        public string Observacao { get; set; }
        public string NomeUsuario { get; set; }
        public string ArquivoBaseFechamento { get; set; }
        public string ArquivoResultado { get; set; }
        public AgendarApuracaoOutliersStatusEnum Status { get; set; }
        public string MgsStatusErro { get; set; }
        public string Descricao { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }
        public int NumeroMeses { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<AgendarApuracaoOutliersDTO, AgendarApuracaoOutliersViewModel>()
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

            mapper.CreateMap<ListarAgendamentosApuracaoOutliersDTO, AgendarApuracaoOutliersViewModel>()
                .ForMember(dest => dest.MesAnoFechamento, opt => opt.MapFrom(orig => orig.MesAnoFechamento))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(orig => orig.DataFechamento))
                .ForMember(dest => dest.DataFinalizacao, opt => opt.MapFrom(orig => orig.DataFinalizacao))
                .ForMember(dest => dest.DataSolicitacao, opt => opt.MapFrom(orig => orig.DataSolicitacao))
                .ForMember(dest => dest.FatorDesvioPadrao, opt => opt.MapFrom(orig => orig.FatorDesvioPadrao))
                .ForMember(dest => dest.Observacao, opt => opt.MapFrom(orig => orig.Observacao))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(orig => orig.NomeUsuario))
                .ForMember(dest => dest.ArquivoBaseFechamento, opt => opt.MapFrom(orig => orig.ArquivoBaseFechamento))
                .ForMember(dest => dest.ArquivoResultado, opt => opt.MapFrom(orig => orig.ArquivoResultado))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
                .ForMember(dest => dest.MgsStatusErro, opt => opt.MapFrom(orig => orig.MgsStatusErro))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.NumeroMeses <= 1 ? string.Format("{0}/{1} - {2} - {3} Mês {4}", orig.DataFechamento.ToString("MM"), orig.DataFechamento.Year, orig.DataFechamento.ToShortDateString(), orig.NumeroMeses,orig.IndFechamentoMensal? "- Fechamento Mensal":"") :
                                                                                                      string.Format("{0}/{1} - {2} - {3} Meses {4}", orig.DataFechamento.ToString("MM"), orig.DataFechamento.Year, orig.DataFechamento.ToShortDateString(), orig.NumeroMeses, orig.IndFechamentoMensal ? "- Fechamento Mensal" : "")))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodigoEmpresaCentralizadora))
                .ForMember(dest => dest.NumeroMeses, opt => opt.MapFrom(orig => orig.NumeroMeses));

            mapper.CreateMap<AgendarApuracaoOutliersViewModel, AgendarApuracaoOutliers>()
                .ForMember(dest => dest.MesAnoFechamento, opt => opt.MapFrom(orig => orig.MesAnoFechamento))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(orig => orig.DataFechamento))
                .ForMember(dest => dest.DataFinalizacao, opt => opt.MapFrom(orig => orig.DataFinalizacao))
                .ForMember(dest => dest.DataSolicitacao, opt => opt.MapFrom(orig => orig.DataSolicitacao))
                .ForMember(dest => dest.FatorDesvioPadrao, opt => opt.MapFrom(orig => orig.FatorDesvioPadrao))
                .ForMember(dest => dest.Observacao, opt => opt.MapFrom(orig => orig.Observacao))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(orig => orig.NomeUsuario))
                .ForMember(dest => dest.ArquivoBaseFechamento, opt => opt.MapFrom(orig => orig.ArquivoBaseFechamento))
                .ForMember(dest => dest.ArquivoResultado, opt => opt.MapFrom(orig => orig.ArquivoResultado))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(orig => orig.Status))
                .ForMember(dest => dest.MgsStatusErro, opt => opt.MapFrom(orig => orig.MgsStatusErro))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.CodigoEmpresaCentralizadora, opt => opt.MapFrom(orig => orig.CodigoEmpresaCentralizadora));
                
        }


    }
}
