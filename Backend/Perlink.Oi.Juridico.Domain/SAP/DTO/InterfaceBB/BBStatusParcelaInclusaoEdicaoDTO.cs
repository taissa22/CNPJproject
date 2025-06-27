using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB
{
    public class BBStatusParcelaInclusaoEdicaoDTO
    {
        public long Id { get; set; }
        public long CodigoBB { get; set; }
        public string Descricao{get; set;}

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBStatusParcelaInclusaoEdicaoDTO, BBStatusParcelas>();
      
            mapper.CreateMap<BBStatusParcelas, BBStatusParcelaInclusaoEdicaoDTO>();
        }

    }
}
