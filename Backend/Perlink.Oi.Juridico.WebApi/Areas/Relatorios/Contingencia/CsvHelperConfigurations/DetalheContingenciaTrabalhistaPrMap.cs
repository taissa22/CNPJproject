using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.CsvHelperConfigurations
{
    public class DetalheContingenciaTrabalhistaPrMap : ClassMap<DetalheContingenciaTrabalhistaResponse>
    {
        public DetalheContingenciaTrabalhistaPrMap(string indPossuiHibrido)
        {
            var i = 0;

            Map(m => m.NomeEmpresaCentralizadora).Index(i++).Name("Empresa Centralizadora");
            Map(m => m.NomeEmpresaGrupo).Index(i++).Name("Empresa do Grupo");
            Map(m => m.ProprioTerceiro).Index(i++).Name("Tipo de Pedido");
            Map(m => m.RiscoPerda).Index(i++).Name("Risco");
            Map(m => m.DataFormatada).Index(i++).Name("Fechamento");

            Map(m => m.PedCodPedido).Index(i++).Name("Código do Pedido");
            Map(m => m.NomePedido).Index(i++).Name("Descrição do Pedido");
            Map(m => m.QtePedidosProvavelP).Index(i++).Name(indPossuiHibrido == "S" ? "Quantidade de Pedidos Ativos sem desembolso (Pré) (J)" : "Quantidade de Pedidos Ativos sem desembolso (J)");
            Map(m => m.PercPerdaProvavelP).Index(i++).Name(indPossuiHibrido == "S" ? "Percentual de Perda de Pedidos Histórico (Pré/Híbrido) (G)" : "Percentual de Perda de Pedidos Histórico (G)");
            Map(m => m.ExpectativaPerdaP).Index(i++).Name(indPossuiHibrido == "S" ? "Quantidade de Pedidos Ativos sem desembolso com Expectativa de Perda (Pré) (M = G * J)" : "Quantidade de Pedidos Ativos sem desembolso com Expectativa de Perda (M = G * J)");
            Map(m => m.ValorMedioDesembPrincipalP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Médio Desembolsado de Principal por Pedido PR (Pré/Híbrido) (R$) (I1)" : "Valor Médio Desembolsado de Principal por Pedido PR (R$) (I1)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValorMedioDesembolsoJurosP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Médio Desembolsado de Correção e Juros por Pedido PR (Pré/Híbrido) (R$) (I2)" : "Valor Médio Desembolsado de Correção e Juros por Pedido PR (R$) (I2)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValProvContPrincipalP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Principal da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré) (R$) (O1 = I1 * M)" : "Valor Principal da Provisão de Contingência de Pedidos Ativos sem desembolso (R$) (O1 = I1 * M)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValProvContJurosP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor de Correção e Juros da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré) (R$) (O2 = I2 * M) " : "Valor de Correção e Juros da Provisão de Contingência de Pedidos Ativos sem desembolso (R$) (O2 = I2 * M)").TypeConverter<DecimalToN2Converter>();

            if (indPossuiHibrido == "S")
            {
                Map(m => m.QtePedidosProvavelH).Index(i++).Name("Quantidade de Pedidos Ativos sem desembolso (Híbrido) (P)");
                Map(m => m.PerResponsOi).Index(i++).Name("Percentual de Responsabilidade Oi (Híbrido) (Q)");
                Map(m => m.ExpectativaPerdaH).Index(i++).Name("Quantidade de Pedidos Ativos sem desembolso com Expectativa de Perda (Híbrido) (R = G * P)");
                Map(m => m.ValProvContPrincipalH).Index(i++).Name("Valor Principal da Provisão de Contingência de Pedidos Ativos sem desembolso (Híbrido) (R$) (S1 = I1 * Q * R)");
                Map(m => m.ValProvContJurosH).Index(i++).Name("Valor de Correção e Juros da Provisão de Contingência de Pedidos Ativos sem desembolso (Híbrido) (R$) (S2 = I2 * Q * R)");
                Map(m => m.ValProvContPrincipal).Index(i++).Name("Total do Valor Principal da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré/Híbrido) (R$) (U1 = O1 + S1) ");
                Map(m => m.ValProvContJuros).Index(i++).Name("Total do Valor de Correção e Juros da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré/Híbrido) (R$) (U2 = O2 + S2)");
            }
        }
    }
}