using AutoMapper;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Shared.Application.Conversores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers {
    public class ApuracaoOutliersConfigurationProfile : Profile{
        public ApuracaoOutliersConfigurationProfile() {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            FechamentoJecDisponivelViewModel.Mapping(this);
            AgendarApuracaoOutliersDTO.Mapping(this);
            AgendarApuracaoOutliersViewModel.Mapping(this);
            ApuracaoOutliersDownloadBaseFechamentoViewModel.Mapping(this);
            ApuracaoOutliersDownloadResultadoViewModel.Mapping(this);
            ListaProcessosBaseFechamentoJec.Mapping(this);
            ApuracaoOutlierDownloadArquivoViewModel.Mapping(this);
        }
    }
}
