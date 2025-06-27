using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class LoteCanceladoViewModel : BaseViewModel<long>
    {
        public long CodigoLote { get; set; }
        public string DescricaoPagamento { get; set; }
        public long CodigoStatusPagamento { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteCanceladoDTO, LoteCanceladoViewModel>();
        }
    }
}
