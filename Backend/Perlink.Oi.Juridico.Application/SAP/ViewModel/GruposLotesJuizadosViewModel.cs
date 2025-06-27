using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class GruposLotesJuizadosViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<GruposLotesJuizados, GruposLotesJuizadosViewModel>();

            mapper.CreateMap<GruposLotesJuizadosViewModel, GruposLotesJuizados>();
        }
    }
}
