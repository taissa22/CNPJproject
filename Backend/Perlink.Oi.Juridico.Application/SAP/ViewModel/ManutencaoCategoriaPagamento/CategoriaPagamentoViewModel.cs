using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using Shared.Tools;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoViewModel : BaseViewModel<long>
    {
        // Despesas Judiciais 
        public long CodigoTipoProcesso { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public string Descricao { get; set; }
        public bool IndicaAtivo { get; set; }
        public long? CodMaterial { get; set; }
        public bool IndicaEnvioSAP { get; set; }
        public bool IndicaNumeroGuia { get; set; }
        public bool IndEncerraProcessoContabil { get; set; }

        // Garantias
        public string DescricaoClasseGarantia { get; set; }
        public long? GrupoCorrecao { get; set; }
        public bool EscritorioPodeSolicitar { get; set; }

        //Pagamentos 
        public bool IndicadorFinalizacaoContabil { get; set; }
        public string FornecedoresPermitidos { get; set; }
        public bool InfluenciaContingenciaMedia { get; set; }
        public string Justificativa { get; set; }
        
        public bool RequerComprovanteSolicitacao { get; set; }
        public bool RequerDataVencimentoDocumento { get; set; }
        public bool IndicaHistorica { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamento, CategoriaPagamentoViewModel>();

            mapper.CreateMap<CategoriaPagamentoViewModel, CategoriaPagamento>();
        }
    }

    public class CategoriaPagamentoPadraoViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Exige No. Guia")]
        public string NumeroGuia { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Fornecedores Permitidos")]
        public string FornecedoresPermitidos { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Estratégico (DExPARA migração de processo)")]
        public string DescricaoEstrategico { get; set; }

        [Name("Correspondente Cível Estratégico Ativo")]
        public string AtivoEstrategico { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoPadraoViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.IndicadorNumeroGuia.RetornaSimNao()))
                 .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ?   $"'{orig.Descricao}" : orig.Descricao));

            mapper.CreateMap<CategoriaPagamentoExtrategicoExportacaoDTO, CategoriaPagamentoPadraoViewModel>()
         .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
         .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
         .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
         .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
         .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.IndicadorNumeroGuia.RetornaSimNao()))
          .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
          .ForMember(dest => dest.DescricaoEstrategico, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoEstrategico, @"^\d+$") ? $"'{orig.DescricaoEstrategico}" : orig.DescricaoEstrategico))
            .ForMember(dest => dest.AtivoEstrategico, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo ));


        }


    }
    
    public class CategoriaPagamentoCCGarantiasViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Grupo de Correção")]
        public string GrupoCorrecao { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Estratégico (DExPARA migração de processo)")]
        public string DescricaoEstrategico { get; set; }

        [Name("Correspondente Cível Estratégico Ativo")]
        public string AtivoEstrategico { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoCCGarantiasViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

                mapper.CreateMap<CategoriaPagamentoExtrategicoExportacaoDTO, CategoriaPagamentoCCGarantiasViewModel>()
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoEstrategico, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoEstrategico, @"^\d+$") ? $"'{orig.DescricaoEstrategico}" : orig.DescricaoEstrategico))
                  .ForMember(dest => dest.AtivoEstrategico, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));
        }
    }

    public class CategoriaPagamentoBasicoFinalzContabilViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Estratégico (DExPARA migração de processo)")]
        public string DescricaoEstrategico { get; set; }

        [Name("Correspondente Cível Estratégico Ativo")]
        public string AtivoEstrategico { get; set; }

     

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExtrategicoExportacaoDTO, CategoriaPagamentoBasicoFinalzContabilViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoEstrategico, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoEstrategico, @"^\d+$") ? $"'{orig.DescricaoEstrategico}" : orig.DescricaoEstrategico))
                .ForMember(dest => dest.AtivoEstrategico, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));
        }
    }


    public class CategoriaPagamentoCCPagViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Influenciar a Contingência")]
        public string InfluenciaContingenciaMedia { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Encerrar Processos Contabilmente")]
        public string EncerraProcessoContabil { get; set; }
        [Name("Fornecedores Permitidos")]
        public string FornecedoresPermitidos { get; set; }        
        [Name("Justificativa")]
        public string DescricaoJustificativa { get; set; }
        [Name("% Responsabilidade Oi")]
        public string ReponsabilidadeOi { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Estratégico (DExPARA migração de processo)")]
        public string DescricaoEstrategico { get; set; }

        [Name("Correspondente Cível Estratégico Ativo")]
        public string AtivoEstrategico { get; set; }
   

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExtrategicoExportacaoDTO, CategoriaPagamentoCCPagViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.InfluenciaContingenciaMedia, opt => opt.MapFrom(orig => orig.IndicadorContingencia.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EncerraProcessoContabil, opt => opt.MapFrom(orig => (orig.IndEncerraProcessoContabil.HasValue && orig.IndEncerraProcessoContabil.Value).RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoEstrategico, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoEstrategico, @"^\d+$") ? $"'{orig.DescricaoEstrategico}" : orig.DescricaoEstrategico))
                .ForMember(dest => dest.AtivoEstrategico, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));

        }
    }

    public class CategoriaPagamentoCEDespesasViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoCEDespesasViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }

    public class CategoriaPagamentoCE_Pag_Gar_ViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoCE_Pag_Gar_ViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }

    public class CategoriaPagamentoBasicoViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoBasicoViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }

    public class CategoriaPagamentoBasicoClassGarantiaViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoBasicoClassGarantiaViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }
    public class CategoriaPagamentoProconDespesaViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }

        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Exige No. Guia")]
        public string NumeroGuia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoProconDespesaViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.IndicadorNumeroGuia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }

    public class CategoriaPagamentoTrabGarantiasViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Influenciar a Contingência por Média")]
        public string InfluenciaContingenciaMedia { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Grupo de Correção")]
        public string GrupoCorrecao { get; set; }       


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoTrabGarantiasViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.InfluenciaContingenciaMedia, opt => opt.MapFrom(orig => orig.IndicadorContingencia.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));

        }
    }
    public class CategoriaPagamentoTrabPagViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Influenciar a Contingência por Média")]
        public string InfluenciaContingenciaMedia { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Histórica")]
        public string Historica { get; set; }
        [Name("% Responsabilidade Oi")]
        public string ReponsabilidadeOi { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoTrabPagViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Historica, opt => opt.MapFrom(orig => orig.IndicadorHistorico.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.InfluenciaContingenciaMedia, opt => opt.MapFrom(orig => orig.IndicadorContingencia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }

    public class CategoriaPagamentoTrabDespesasJudViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoTrabDespesasJudViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoJuizadoGarantiaViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Grupo de Correção")]
        public string GrupoCorrecao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoJuizadoGarantiaViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoJuizadoPagViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Influenciar a Contingência")]
        public string InfluenciaContingenciaMedia { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }        
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }        
        [Name("Justificativa")]
        public string DescricaoJustificativa { get; set; }
        [Name("Fornecedores Permitidos")]
        public string FornecedoresPermitidos { get; set; }
        [Name("% de Responsabilidade Oi")]
        public string ReponsabilidadeOi { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoJuizadoPagViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.InfluenciaContingenciaMedia, opt => opt.MapFrom(orig => orig.IndicadorContingencia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }


    public class CategoriaPagamentoJuizadoDespViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Exige No. Guia")]
        public string NumeroGuia { get; set; }
        [Name("Fornecedores Permitidos")]
        public string FornecedoresPermitidos { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoJuizadoDespViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.IndicadorNumeroGuia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoBasicoAdmViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Exige No. Guia")]
        public string NumeroGuia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoBasicoAdmViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.NumeroGuia, opt => opt.MapFrom(orig => orig.IndicadorNumeroGuia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoPexDespesasJudViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Exige No. Guia")]
        public string NumeroGuia { get; set; }
        [Name("Escritório Pode Solicitar")]
        public string EscritorioPodeSolicitar { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoPexDespesasJudViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EscritorioPodeSolicitar, opt => opt.MapFrom(orig => (orig.IndEscritorioSolicitaLan.HasValue && orig.IndEscritorioSolicitaLan.Value).RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoPexGarantiasViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Grupo de Correção")]
        public string GrupoCorrecao { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Escritório Pode Solicitar")]
        public string EscritorioPodeSolicitar { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoPexGarantiasViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EscritorioPodeSolicitar, opt => opt.MapFrom(orig => (orig.IndEscritorioSolicitaLan.HasValue && orig.IndEscritorioSolicitaLan.Value).RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }

    public class CategoriaPagamentoPexHonoViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Registrar em Processos Finalizados Contabilmente")]
        public string IndicadorFinalizacaoContabil { get; set; }
        [Name("Escritório Pode Solicitar")]
        public string EscritorioPodeSolicitar { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoPexHonoViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.IndicadorFinalizacaoContabil.RetornaSimNao()))
                .ForMember(dest => dest.EscritorioPodeSolicitar, opt => opt.MapFrom(orig => (orig.IndEscritorioSolicitaLan.HasValue && orig.IndEscritorioSolicitaLan.Value).RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }
    }
    public class CategoriaPagamentoPexPagamentosViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Influenciar a Contingência")]
        public string InfluenciaContingenciaMedia { get; set; }
        [Name("Justificativa")]
        public string DescricaoJustificativa { get; set; }
        [Name("Requer Data Vencimento Documento")]
        public string RequerDataVencimentoDocumento { get; set; }
        [Name("Requer Comprovante na Solicitação")]
        public string RequerComprovanteSolicitacao { get; set; }
        [Name("Escritório Pode Solicitar")]
        public string EscritorioPodeSolicitar { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoExportacaoDTO, CategoriaPagamentoPexPagamentosViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.InfluenciaContingenciaMedia, opt => opt.MapFrom(orig => orig.IndicadorContingencia.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.RequerDataVencimentoDocumento, opt => opt.MapFrom(orig => orig.IndicadorRequerDataVencimento.HasValueAndTrue().RetornaSimNao()))
                .ForMember(dest => dest.RequerComprovanteSolicitacao, opt => opt.MapFrom(orig => orig.IndComprovanteSolicitacao.HasValueAndTrue().RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EscritorioPodeSolicitar, opt => opt.MapFrom(orig => orig.IndEscritorioSolicitaLan.HasValueAndTrue().RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao));
        }        
    }

    public class CategoriaPagamentoCEGarantiasConsumidorViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Categoria de Pagamento Correspondente Cível Consumidor (DExPARA migração de processo)")]
        public string DescricaoConsumidor { get; set; }

        [Name("Correspondente Cível Consumidor Ativo")]
        public string AtivoConsumidor { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoConsumidorExportacaoDTO, CategoriaPagamentoCEGarantiasConsumidorViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoConsumidor, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoConsumidor, @"^\d+$") ? $"'{orig.DescricaoConsumidor}" : orig.DescricaoConsumidor))
                .ForMember(dest => dest.AtivoConsumidor, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));

        }
    }

    public class CategoriaPagamentoPadraoConsumidorViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Consumidor (DExPARA migração de processo)")]
        public string DescricaoConsumidor { get; set; }

        [Name("Correspondente Cível Consumidor Ativo")]
        public string AtivoConsumidor { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoConsumidorExportacaoDTO, CategoriaPagamentoPadraoConsumidorViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoConsumidor, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoConsumidor, @"^\d+$") ? $"'{orig.DescricaoConsumidor}" : orig.DescricaoConsumidor))
                .ForMember(dest => dest.AtivoConsumidor, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));

        }
    }

    public class CategoriaPagamentoBasicoFinalzContabilConsumidorViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }

        [Name("Categoria de Pagamento Correspondente Cível Consumidor (DExPARA migração de processo)")]
        public string DescricaoConsumidor { get; set; }

        [Name("Correspondente Cível Consumidor Ativo")]
        public string AtivoConsumidor { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoConsumidorExportacaoDTO, CategoriaPagamentoBasicoFinalzContabilConsumidorViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoConsumidor, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoConsumidor, @"^\d+$") ? $"'{orig.DescricaoConsumidor}" : orig.DescricaoConsumidor))
                .ForMember(dest => dest.AtivoConsumidor, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));
        }
    }

    public class CategoriaPagamentoCCPagConsumidorViewModel
    {
        [Name("Código")]
        public string Codigo { get; set; }
        [Name("Descrição da Categoria de Pagamento")]
        public string Descricao { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Código do Material SAP")]
        public string CodigoMaterialSAP { get; set; }
        [Name("Envia SAP")]
        public string EnvioSap { get; set; }
        [Name("Classe de Garantia")]
        public string DescricaoClasseGarantia { get; set; }
        [Name("Categoria de Pagamento Correspondente Cível Consumidor (DExPARA migração de processo)")]
        public string DescricaoConsumidor { get; set; }

        [Name("Correspondente Cível Consumidor Ativo")]
        public string AtivoConsumidor { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoConsumidorExportacaoDTO, CategoriaPagamentoCCPagConsumidorViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.indAtivo.RetornaSimNao()))
                .ForMember(dest => dest.CodigoMaterialSAP, opt => opt.MapFrom(orig => orig.CodigoMaterialSAP.HasValidValue() ? $"'{orig.CodigoMaterialSAP}" : null))
                .ForMember(dest => dest.EnvioSap, opt => opt.MapFrom(orig => orig.indEnvioSap.RetornaSimNao()))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.Descricao, @"^\d+$") ? $"'{orig.Descricao}" : orig.Descricao))
                .ForMember(dest => dest.DescricaoConsumidor, opt => opt.MapFrom(orig => System.Text.RegularExpressions.Regex.IsMatch(orig.DescricaoConsumidor, @"^\d+$") ? $"'{orig.DescricaoConsumidor}" : orig.DescricaoConsumidor))
                .ForMember(dest => dest.AtivoConsumidor, opt => opt.MapFrom(orig => orig.VaidaDescricaoAtivo));
        }
    }
}
