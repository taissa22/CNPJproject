using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao {
    public class LoteCriacaoResultadoEmpresaCentralizadoraViewModel {
        public long CodigoEmpresaCentralizadora { get; set; }
        public string DescricaoEmpresaCentralizadora { get; set; }
        public int TotalLote { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteCriacaoResultadoEmpresaCentralizadoraViewModel, LoteCriacaoResultadoEmpresaCentralizadoraDTO>();
            mapper.CreateMap<LoteCriacaoResultadoEmpresaCentralizadoraDTO, LoteCriacaoResultadoEmpresaCentralizadoraViewModel >();
        }
    }
}
