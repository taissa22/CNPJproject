using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Shared.Tools;
using System;
using System.Globalization;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia
{
    public class SaldoGarantiaResultadoViewModel
    {
        public long IdProcesso { get; set; }
        public string NumeroProcesso { get; set; }
        public string CodigoEstado { get; set; }
        public string DescricaoComarca { get; set; }
        public long CodigoVara { get; set; }
        public string DescricaoTipoVara { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string Ativo { get; set; }
        public string DescricaoBanco { get; set; }
        public string DescricaoEscritorio { get; set; }
        public DateTime? DataFinalizacaoContabil { get; set; }
        //public string DataFinalizacaoContabil { get; set; }
        public string DescricaoTipoGarantia { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal ValorCorrecaoPrincipal { get; set; }
        public decimal ValorAjusteCorrecao { get; set; }
        public decimal ValorJurosPrincipal { get; set; }
        public decimal ValorAjusteJuros { get; set; }
        public decimal ValorPagamentoPrincipal { get; set; }
        public decimal ValorPagamentoCorrecao { get; set; }
        public decimal ValorPagamentosJuros { get; set; }
        public decimal ValorLevantadoPrincipal { get; set; }
        public decimal ValorLevantadoCorrecao { get; set; }
        public decimal ValorLevantadoJuros { get; set; }
        public decimal ValorSaldoPrincipal { get; set; }
        public decimal ValorSaldoCorrecao { get; set; }
        public decimal ValorSaldoJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaResultadoViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo));
            //.ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.NullableToString("DD/MM/YYYY")));
        }
    }

    public class SaldoGarantiaExportacaoCCViewModel
    {
        [Name("Código Interno Processo")]
        public string IdProcesso { get; set; }
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Comarca")]
        public string DescricaoComarca { get; set; }
        [Name("Vara")]
        public string CodigoVara { get; set; }
        [Name("Tipo de Vara")]
        public string DescricaoTipoVara { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Banco")]
        public string DescricaoBanco { get; set; }
        [Name("Escritório")]
        public string DescricaoEscritorio { get; set; }
        [Name("Data Finalização Contábil")]
        public string DataFinalizacaoContabil { get; set; }
        [Name("Tipo Garantia")]
        public string DescricaoTipoGarantia { get; set; }
        [Name("Valor Principal")]
        public string ValorPrincipal { get; set; }
        [Name("Correção Principal")]
        public string ValorCorrecaoPrincipal { get; set; }
        [Name("Ajuste Correção")]
        public string ValorAjusteCorrecao { get; set; }
        [Name("Juros Principal")]
        public string ValorJurosPrincipal { get; set; }
        [Name("Ajuste Juros")]
        public string ValorAjusteJuros { get; set; }
        [Name("Pagamento Principal")]
        public string ValorPagamentoPrincipal { get; set; }
        [Name("Pagamento Correção")]
        public string ValorPagamentoCorrecao { get; set; }
        [Name("Pagamento Juros")]
        public string ValorPagamentosJuros { get; set; }
        [Name("Levantado Principal")]
        public string ValorLevantadoPrincipal { get; set; }
        [Name("Levantado Correção")]
        public string ValorLevantadoCorrecao { get; set; }
        [Name("Levantado Juros")]
        public string ValorLevantadoJuros { get; set; }
        [Name("Saldo Principal")]
        public string ValorSaldoPrincipal { get; set; }
        [Name("Saldo Correção")]
        public string ValorSaldoCorrecao { get; set; }
        [Name("Saldo Juros")]
        public string ValorSaldoJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoCCViewModel>()
                .ForMember(dest => dest.IdProcesso, opt => opt.MapFrom(orig => orig.IdProcesso.ToString()))
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                .ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.NullableToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
                .ForMember(dest => dest.ValorPrincipal, opt => opt.MapFrom(orig => orig.ValorPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorCorrecaoPrincipal, opt => opt.MapFrom(orig => orig.ValorCorrecaoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteCorrecao, opt => opt.MapFrom(orig => orig.ValorAjusteCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorJurosPrincipal, opt => opt.MapFrom(orig => orig.ValorJurosPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteJuros, opt => opt.MapFrom(orig => orig.ValorAjusteJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoPrincipal, opt => opt.MapFrom(orig => orig.ValorPagamentoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoCorrecao, opt => opt.MapFrom(orig => orig.ValorPagamentoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentosJuros, opt => opt.MapFrom(orig => orig.ValorPagamentosJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoJuros, opt => opt.MapFrom(orig => orig.ValorLevantadoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoPrincipal, opt => opt.MapFrom(orig => orig.ValorLevantadoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoCorrecao, opt => opt.MapFrom(orig => orig.ValorLevantadoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoPrincipal, opt => opt.MapFrom(orig => orig.ValorSaldoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoJuros, opt => opt.MapFrom(orig => orig.ValorSaldoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoCorrecao, opt => opt.MapFrom(orig => orig.ValorSaldoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));

        }
    }

    public class SaldoGarantiaExportacaoTrabalhistaViewModel
    {
        [Name("Código Interno Processo")]
        public string IdProcesso { get; set; }
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Comarca")]
        public string DescricaoComarca { get; set; }
        [Name("Vara")]
        public string CodigoVara { get; set; }
        [Name("Tipo de Vara")]
        public string DescricaoTipoVara { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Estratégico")]
        public string Estrategico { get; set; }
        [Name("Banco")]
        public string DescricaoBanco { get; set; }
        [Name("Escritório")]
        public string DescricaoEscritorio { get; set; }
        [Name("Data Finalização Contábil")]
        public string DataFinalizacaoContabil { get; set; }
        [Name("Tipo Garantia")]
        public string DescricaoTipoGarantia { get; set; }
        [Name("Valor Principal")]
        public string ValorPrincipal { get; set; }
        [Name("Correção Principal")]
        public string ValorCorrecaoPrincipal { get; set; }
        [Name("Ajuste Correção")]
        public string ValorAjusteCorrecao { get; set; }
        [Name("Juros Principal")]
        public string ValorJurosPrincipal { get; set; }
        [Name("Ajuste Juros")]
        public string ValorAjusteJuros { get; set; }
        [Name("Pagamento Principal")]
        public string ValorPagamentoPrincipal { get; set; }
        [Name("Pagamento Correção")]
        public string ValorPagamentoCorrecao { get; set; }
        [Name("Pagamento Juros")]
        public string ValorPagamentosJuros { get; set; }
        [Name("Levantado Principal")]
        public string ValorLevantadoPrincipal { get; set; }
        [Name("Levantado Correção")]
        public string ValorLevantadoCorrecao { get; set; }
        [Name("Levantado Juros")]
        public string ValorLevantadoJuros { get; set; }
        [Name("Saldo Principal")]
        public string ValorSaldoPrincipal { get; set; }
        [Name("Saldo Correção")]
        public string ValorSaldoCorrecao { get; set; }
        [Name("Saldo Juros")]
        public string ValorSaldoJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoTrabalhistaViewModel>()
                .ForMember(dest => dest.IdProcesso, opt => opt.MapFrom(orig => orig.IdProcesso.ToString()))
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                .ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.NullableToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
                .ForMember(dest => dest.Estrategico, opt => opt.MapFrom(orig => orig.Estrategico))
                .ForMember(dest => dest.ValorPrincipal, opt => opt.MapFrom(orig => orig.ValorPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorCorrecaoPrincipal, opt => opt.MapFrom(orig => orig.ValorCorrecaoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteCorrecao, opt => opt.MapFrom(orig => orig.ValorAjusteCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorJurosPrincipal, opt => opt.MapFrom(orig => orig.ValorJurosPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteJuros, opt => opt.MapFrom(orig => orig.ValorAjusteJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoPrincipal, opt => opt.MapFrom(orig => orig.ValorPagamentoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoCorrecao, opt => opt.MapFrom(orig => orig.ValorPagamentoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentosJuros, opt => opt.MapFrom(orig => orig.ValorPagamentosJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoCorrecao, opt => opt.MapFrom(orig => orig.ValorLevantadoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoJuros, opt => opt.MapFrom(orig => orig.ValorLevantadoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoPrincipal, opt => opt.MapFrom(orig => orig.ValorLevantadoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoPrincipal, opt => opt.MapFrom(orig => orig.ValorSaldoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoJuros, opt => opt.MapFrom(orig => orig.ValorSaldoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoCorrecao, opt => opt.MapFrom(orig => orig.ValorSaldoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }

    public class SaldoGarantiaExportacaoJECViewModel
    {
        [Name("Código Interno do Processo")]
        public string IdProcesso { get; set; }
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Comarca")]
        public string DescricaoComarca { get; set; }
        [Name("Juizado")]
        public string CodigoVara { get; set; }
        [Name("Tipo de Juizado")]
        public string DescricaoTipoVara { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Banco")]
        public string DescricaoBanco { get; set; }
        [Name("Escritório")]
        public string DescricaoEscritorio { get; set; }
        [Name("Data Finalização Contábil")]
        public string DataFinalizacaoContabil { get; set; }
        [Name("Tipo Garantia")]
        public string DescricaoTipoGarantia { get; set; }
        [Name("Valor Principal")]
        public string ValorPrincipal { get; set; }
        [Name("Correção Principal")]
        public string ValorCorrecaoPrincipal { get; set; }
        [Name("Ajuste Correção")]
        public string ValorAjusteCorrecao { get; set; }
        [Name("Juros Principal")]
        public string ValorJurosPrincipal { get; set; }
        [Name("Ajuste Juros")]
        public string ValorAjusteJuros { get; set; }
        [Name("Pagamento Principal")]
        public string ValorPagamentoPrincipal { get; set; }
        [Name("Pagamento Correção")]
        public string ValorPagamentoCorrecao { get; set; }
        [Name("Pagamento Juros")]
        public string ValorPagamentosJuros { get; set; }
        [Name("Levantado Principal")]
        public string ValorLevantadoPrincipal { get; set; }
        [Name("Levantado Correção")]
        public string ValorLevantadoCorrecao { get; set; }
        [Name("Levantado Juros")]
        public string ValorLevantadoJuros { get; set; }
        [Name("Saldo Principal")]
        public string ValorSaldoPrincipal { get; set; }
        [Name("Saldo Correção")]
        public string ValorSaldoCorrecao { get; set; }
        [Name("Saldo Juros")]
        public string ValorSaldoJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoJECViewModel>()
                .ForMember(dest => dest.IdProcesso, opt => opt.MapFrom(orig => orig.IdProcesso.ToString()))
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                .ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.NullableToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
                .ForMember(dest => dest.ValorPrincipal, opt => opt.MapFrom(orig => orig.ValorPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorCorrecaoPrincipal, opt => opt.MapFrom(orig => orig.ValorCorrecaoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteCorrecao, opt => opt.MapFrom(orig => orig.ValorAjusteCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorJurosPrincipal, opt => opt.MapFrom(orig => orig.ValorJurosPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorAjusteJuros, opt => opt.MapFrom(orig => orig.ValorAjusteJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoPrincipal, opt => opt.MapFrom(orig => orig.ValorPagamentoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentoCorrecao, opt => opt.MapFrom(orig => orig.ValorPagamentoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorPagamentosJuros, opt => opt.MapFrom(orig => orig.ValorPagamentosJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoJuros, opt => opt.MapFrom(orig => orig.ValorLevantadoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoPrincipal, opt => opt.MapFrom(orig => orig.ValorLevantadoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorLevantadoCorrecao, opt => opt.MapFrom(orig => orig.ValorLevantadoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoPrincipal, opt => opt.MapFrom(orig => orig.ValorSaldoPrincipal.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoJuros, opt => opt.MapFrom(orig => orig.ValorSaldoJuros.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorSaldoCorrecao, opt => opt.MapFrom(orig => orig.ValorSaldoCorrecao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }
    public class SaldoGarantiaExportacaoTributarioADMViewModel
    {
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Órgão")]
        public string NomeOrgao { get; set; }
        [Name("Competência")]
        public string NomeCompetencia { get; set; }
        [Name("Município")]
        public string NomeMunicipio { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Valor Total Pago c/ Garantias")]
        public string ValorTotalPagoGarantia { get; set; }
        [Name("Saldo de Depósitos e Bloqueios")]
        public string SaldoDepositoBloqueio { get; set; }
        [Name("Saldo de Garantia")]
        public string SaldoGarantia { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoTributarioADMViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
                .ForMember(dest => dest.SaldoDepositoBloqueio, opt => opt.MapFrom(orig => orig.SaldoDepositoBloqueio.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.SaldoGarantia, opt => opt.MapFrom(orig => orig.SaldoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorTotalPagoGarantia, opt => opt.MapFrom(orig => orig.ValorTotalPagoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }
    public class SaldoGarantiaExportacaoTributarioJudicialViewModel
    {
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Comarca")]
        public string DescricaoComarca { get; set; }
        [Name("Vara")]
        public string CodigoVara { get; set; }
        [Name("Tipo Vara")]
        public string DescricaoTipoVara { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Valor Total Pago c/ Garantias")]
        public string ValorTotalPagoGarantia { get; set; }
        [Name("Saldo de Depósitos e Bloqueios")]
        public string SaldoDepositoBloqueio { get; set; }
        [Name("Saldo de Garantias")]
        public string SaldoGarantia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoTributarioJudicialViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
                .ForMember(dest => dest.SaldoDepositoBloqueio, opt => opt.MapFrom(orig => orig.SaldoDepositoBloqueio.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.SaldoGarantia, opt => opt.MapFrom(orig => orig.SaldoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorTotalPagoGarantia, opt => opt.MapFrom(orig => orig.ValorTotalPagoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));
        }
    }

    public class SaldoGarantiaExportacaoCEViewModel
    {
        [Name("Código Interno Processo")]
        public string IdProcesso { get; set; }
        [Name("Processo")]
        public string NumeroProcesso { get; set; }
        [Name("Estado")]
        public string CodigoEstado { get; set; }
        [Name("Comarca")]
        public string DescricaoComarca { get; set; }
        [Name("Vara")]
        public string CodigoVara { get; set; }
        [Name("Tipo de Vara")]
        public string DescricaoTipoVara { get; set; }
        [Name("Empresa do Grupo")]
        public string DescricaoEmpresaGrupo { get; set; }
        [Name("Ativo")]
        public string Ativo { get; set; }
        [Name("Escritório")]
        public string DescricaoEscritorio { get; set; }
        [Name("Data Finalização Contábil")]
        public string DataFinalizacaoContabil { get; set; }
        [Name("Valor Total Pago c/ Garantias")]
        public string ValorTotalPagoGarantia { get; set; }
        [Name("Saldo de Depósitos e Bloqueios")]
        public string SaldoDepositoBloqueio { get; set; }
        [Name("Saldo de Garantias")]
        public string SaldoGarantia { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<SaldoGarantiaResultadoDTO, SaldoGarantiaExportacaoCEViewModel>()
                .ForMember(dest => dest.NumeroProcesso, opt => opt.MapFrom(orig => $"'{orig.NumeroProcesso}"))
                 .ForMember(dest => dest.IdProcesso, opt => opt.MapFrom(orig => orig.IdProcesso.ToString()))
                .ForMember(dest => dest.DataFinalizacaoContabil, opt => opt.MapFrom(orig => orig.DataFinalizacaoContabil.NullableToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(orig => orig.Ativo))
               .ForMember(dest => dest.SaldoDepositoBloqueio, opt => opt.MapFrom(orig => orig.SaldoDepositoBloqueio.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.SaldoGarantia, opt => opt.MapFrom(orig => orig.SaldoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))))
                .ForMember(dest => dest.ValorTotalPagoGarantia, opt => opt.MapFrom(orig => orig.ValorTotalPagoGarantia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))));

        }

    }
}