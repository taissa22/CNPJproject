﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados
{
   public class CategoriaDePagamentoConsumidorDTO
    {
        public CategoriaDePagamentoConsumidorDTO(long codigo, string descricao, long tipoLancamento, bool indAtivo, long? codigoMaterialSAP, bool indEnvioSap, long? clgarCodigoClasseGarantia, string descricaoclassegarantia, bool indicadorNumeroGuia, long? tipoFornecedorPermitido, string fornecedoresPermitidos, bool? indEscritorioSolicitaLan, long? grpcgIdGrupoCorrecaoGar, string grupoCorrecao, bool? indEncerraProcessoContabil, bool? indComprovanteSolicitacao, bool? indicadorRequerDataVencimento, bool indicadorContingencia, bool indicadorCivelConsumidor, bool indicadorCivelEstrategico, bool indicadorTrabalhista, bool indicadorTributarioJudicial, bool ind_TributarioAdministrativo, bool indicadorBaixaGarantia, bool indBaixaPagamento, string indicadorBloqueioDeposito, bool indicadorJuizado, bool indicadorAdministrativo, bool indicadorHistorico, bool indicadorProcon, bool indicadorPex, long? tmgarCodigoMovicadorGarantia, bool indicadorFinalizacaoContabil, string descricaoJustificativa, long? pagamentoA, string responsabilidadeOi, bool ativo, long? codMigConsumidor, string descricaoConsumidor)
        {
            Codigo = codigo;
            Descricao = descricao;
            TipoLancamento = tipoLancamento;
            IndAtivo = indAtivo;
            CodigoMaterialSAP = codigoMaterialSAP;
            IndEnvioSap = indEnvioSap;
            ClgarCodigoClasseGarantia = clgarCodigoClasseGarantia;
            Descricaoclassegarantia = descricaoclassegarantia;
            IndicadorNumeroGuia = indicadorNumeroGuia;
            TipoFornecedorPermitido = tipoFornecedorPermitido;
            FornecedoresPermitidos = fornecedoresPermitidos;
            IndEscritorioSolicitaLan = indEscritorioSolicitaLan;
            GrpcgIdGrupoCorrecaoGar = grpcgIdGrupoCorrecaoGar;
            GrupoCorrecao = grupoCorrecao;
            IndEncerraProcessoContabil = indEncerraProcessoContabil;
            IndComprovanteSolicitacao = indComprovanteSolicitacao;
            IndicadorRequerDataVencimento = indicadorRequerDataVencimento;
            IndicadorContingencia = indicadorContingencia;
            IndicadorCivelConsumidor = indicadorCivelConsumidor;
            IndicadorCivelEstrategico = indicadorCivelEstrategico;
            IndicadorTrabalhista = indicadorTrabalhista;
            IndicadorTributarioJudicial = indicadorTributarioJudicial;
            Ind_TributarioAdministrativo = ind_TributarioAdministrativo;
            IndicadorBaixaGarantia = indicadorBaixaGarantia;
            IndBaixaPagamento = indBaixaPagamento;
            IndicadorBloqueioDeposito = indicadorBloqueioDeposito;
            IndicadorJuizado = indicadorJuizado;
            IndicadorAdministrativo = indicadorAdministrativo;
            IndicadorHistorico = indicadorHistorico;
            IndicadorProcon = indicadorProcon;
            IndicadorPex = indicadorPex;
            TmgarCodigoMovicadorGarantia = tmgarCodigoMovicadorGarantia;
            IndicadorFinalizacaoContabil = indicadorFinalizacaoContabil;
            DescricaoJustificativa = descricaoJustificativa;
            PagamentoA = pagamentoA;
            ResponsabilidadeOi = responsabilidadeOi;
            Ativo = ativo;
            CodMigConsumidor = codMigConsumidor;
            DescricaoConsumidor = descricaoConsumidor;
        }

        // Despesas Judiciais 
        public long Codigo { get; set; }

        public string Descricao { get; set; }

        public long TipoLancamento { get; set; }

        public bool IndAtivo { get; set; }

        public long? CodigoMaterialSAP { get; set; }

        public bool IndEnvioSap { get; set; }

        public long? ClgarCodigoClasseGarantia { get; set; }

        public string Descricaoclassegarantia { get; set; }

        public bool IndicadorNumeroGuia { get; set; }


        public long? TipoFornecedorPermitido { get; set; }

        public string FornecedoresPermitidos { get; set; }

        public bool? IndEscritorioSolicitaLan { get; set; }

        // Garantias
        public long? GrpcgIdGrupoCorrecaoGar { get; set; }

        public string GrupoCorrecao { get; set; }

        //Pagamentos 
        public bool? IndEncerraProcessoContabil { get; set; }

        public bool? IndComprovanteSolicitacao { get; set; }

        public bool? IndicadorRequerDataVencimento { get; set; }

        public bool IndicadorContingencia { get; set; }

        public bool IndicadorCivelConsumidor { get; set; }

        public bool IndicadorCivelEstrategico { get; set; }

        public bool IndicadorTrabalhista { get; set; }

        public bool IndicadorTributarioJudicial { get; set; }

        public bool Ind_TributarioAdministrativo { get; set; }

        public bool IndicadorBaixaGarantia { get; set; }

        public bool IndBaixaPagamento { get; set; }

        public string IndicadorBloqueioDeposito { get; set; }

        public bool IndicadorJuizado { get; set; }

        public bool IndicadorAdministrativo { get; set; }

        public bool IndicadorHistorico { get; set; }

        public bool IndicadorProcon { get; set; }

        public bool IndicadorPex { get; set; }

        public long? TmgarCodigoMovicadorGarantia { get; set; }

        public bool IndicadorFinalizacaoContabil { get; set; }

        public string DescricaoJustificativa { get; set; }

        public long? PagamentoA { get; set; }
        public string ResponsabilidadeOi { get; set; }

        public bool Ativo { get; set; }

        public long? CodMigConsumidor { get; set; }

        public string DescricaoConsumidor { get; set; }
    }
}
