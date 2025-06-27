using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia.CsvHelperConfigurations
{
    public class DetalheContingenciaTrabalhistaPoMap : ClassMap<DetalheContingenciaTrabalhistaPoResponse>
    {
        public DetalheContingenciaTrabalhistaPoMap(string indPossuiHibrido)
        {
            var i = 0;

            Map(m => m.NomeEmpresaCentralizadora).Index(i++).Name("Empresa Centralizadora");
            Map(m => m.NomeEmpresaGrupo).Index(i++).Name("Empresa do Grupo");
            Map(m => m.ProprioTerceiro).Index(i++).Name("Tipo de Pedido");
            Map(m => m.RiscoPerda).Index(i++).Name("Risco");
            Map(m => m.DataFormatada).Index(i++).Name("Fechamento");

            Map(m => m.PedCodPedido).Index(i++).Name("Código do Pedido");
            Map(m => m.NomePedido).Index(i++).Name("Descrição do Pedido");
            
            Map(m => m.PercPerdaProvavel).Index(i++).Name(indPossuiHibrido == "S" ? "Percentual de Perda de Pedidos Histórico (Pré/Híbrido) (G)" : "Percentual de Perda de Pedidos Histórico (G)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValorMedioDesemPrincPr).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Médio Desembolsado de Principal por Pedido PR (Pré/Híbrido) (R$) (I1)" : "Valor Médio Desembolsado de Principal por Pedido PR (R$) (I1)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValorMedioDesemJurosPr).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Médio Desembolsado de Correção e Juros por Pedido PR (Pré/Híbrido) (R$) (I2)" : "Valor Médio Desembolsado de Correção e Juros por Pedido PR (R$) (I2)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.QtePedidosProvavel).Index(i++).Name(indPossuiHibrido == "S" ? "Quantidade de Pedidos Ativos sem desembolso [PR] (Pré/Híbrido) (Q)" : "Quantidade de Pedidos Ativos sem desembolso [PR] (Q)");
            
            Map(m => m.QtePedidosPossivelP).Index(i++).Name(indPossuiHibrido == "S" ? "Quantidade de Pedidos Não-Concluídos [PO] (Pré) (K)" : "Quantidade de Pedidos Não-Concluídos [PO] (K)");

            Map(m => m.QtePedidosRemoto).Index(i++).Name(indPossuiHibrido == "S" ? "Quantidade de Pedidos Não-Concluídos [RE] (Pré/Híbrido) (L)" : "Quantidade de Pedidos Não-Concluídos [RE] (L)");
            Map(m => m.PercPerdaPossivel).Index(i++).Name(indPossuiHibrido == "S" ? "Percentual de Perda de Pedidos Não-Concluídos [PO] (Pré/Híbrido) (T = Q / (Q + L))" : "Percentual de Perda de Pedidos Não-Concluídos [PO] (T = Q / (Q + L))").TypeConverter<DecimalToN2Converter>();
            
            Map(m => m.ValProvContPrincipalP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor Principal da Provisão de Contingência de Pedidos Não-Concluídos [PO] (Pré) (R$) (P1 = G * I1 * K * T)" : "Valor Principal da Provisão de Contingência de Pedidos Não-Concluídos [PO] (R$) (P1 = G * I1 * K * T)").TypeConverter<DecimalToN2Converter>();
            Map(m => m.ValProvContJurosP).Index(i++).Name(indPossuiHibrido == "S" ? "Valor de Correção e Juros da Provisão de Contingência de Pedidos Não-Concluídos [PO] (Pré) (R$) (P2 = G * I2 * K * T)" : "Valor de Correção e Juros da Provisão de Contingência de Pedidos Não-Concluídos [PO] (R$) (P2 = G * I2 * K * T)").TypeConverter<DecimalToN2Converter>();

            if (indPossuiHibrido == "S")
            {
                Map(m => m.QtePedidosPossivelH).Index(i++).Name("Quantidade de Pedidos Não-Concluídos [PO] (Híbrido) (R)");
                Map(m => m.PerResponsOi).Index(i++).Name("Percentual de Responsabilidade Oi (Híbrido) (U)");
                Map(m => m.ValProvContPrincipalH).Index(i++).Name("Valor Principal da Provisão de Contingência de Pedidos Não-Concluídos [PO] (Híbrido) (R$) (S1 = G * R * I1 * T * U)");
                Map(m => m.ValProvContJurosH).Index(i++).Name("Valor de Correção e Juros da Provisão de Contingência de Pedidos Não-Concluídos [PO] (Híbrido)  (R$) (S2 = G * R * I2 * T * U)");
                Map(m => m.ValProvContPrincipal).Index(i++).Name("Total do Valor Principal da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré/Híbrido) (R$) (V1 = P1 + S1)");
                Map(m => m.ValProvContJuros).Index(i++).Name("Total do Valor de Correção e Juros da Provisão de Contingência de Pedidos Ativos sem desembolso (Pré/Híbrido) (R$) (V2 = P2 + S2)");
            }
        }

    }
}
