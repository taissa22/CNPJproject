using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class DadosProcessoEstornoViewModel
    {
        public string NumeroProcesso { get; set; }
        public string ClassificacaoHierarquica { get; set; }
        public long CodigoProcesso { get; set; }
        public string NomeComarca { get; set; }
        public string Vara { get; set; }
        public string NomeTipoVara { get; set; }
        public string UF { get; set; }
        public string DescricaoEmpresaDoGrupo { get; set; }
        public string DescricaoEscritorio { get; set; }
        public string ResponsavelInterno { get; set; }
        public List<DadosLancamentoEstornoViewModel> DadosLancamentosEstorno { get; set; }
        public List<PedidoEstornoDTO> Pedidos { get; set; }
        public ICollection<Reclamantes_ReclamadasDTO> Reclamantes { get; set; }
        public ICollection<Reclamantes_ReclamadasDTO> Reclamadas { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<DadosProcessoEstornoDTO, DadosProcessoEstornoViewModel>()
              .ForMember(dest => dest.Vara, opt => opt.MapFrom(orig => orig.CodigoVara));
        }
    }

    public class DadosLancamentoEstornoViewModel
    {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public string PedidoSAP { get; set; }
        public string DataRecebimentoFiscal { get; set; }
        public string DataPagamento { get; set; }
        public string DataLancamento { get; set; }
        public decimal Valor { get; set; }
        public string StatusPagamento { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public string CategoriaPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string Fornecedor { get; set; }
        public string Centro { get; set; }
        public string CentroCusto { get; set; }
        public string DataLevantamento { get; set; }
        public string ParteEfetivouLevantamento { get; set; }
        public string Comentario { get; set; }
        public int QuantidadeCredoresAssociados { get; set; }
        public decimal ValorCompromisso { get; set; }
        public bool ReduzirPagamentoCredor { get; set; }
        public bool CriarNovaParcelaFutura { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public string DescricaoTipoLancamento { get; set; }
        public long CodigoCompromisso { get; set; }
        public long CodigoParcela { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<DadosLancamentoEstornoDTO, DadosLancamentoEstornoViewModel>();
        }
    }
}
