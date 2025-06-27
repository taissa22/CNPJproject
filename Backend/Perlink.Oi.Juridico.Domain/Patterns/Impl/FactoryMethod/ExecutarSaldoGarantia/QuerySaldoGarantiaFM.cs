using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Patterns.Interface.FactoryMethod;
using System;

namespace Perlink.Oi.Juridico.Domain.Patterns.Impl.FactoryMethod.ExecutarSaldoGarantia
{
    public class QuerySaldoGarantiaFM
    {
        public IQuerySaldoGarantiaFM EscolherPorTipoProcesso(long tipoProcesso)
        {
            switch (tipoProcesso)
            {
                case (long)TipoProcessoEnum.CivelConsumidor:
                    return new CivelConsumidorFM();
                case (long)TipoProcessoEnum.CivelEstrategico:
                    return new CivelEstrategicoFM();
                case (long)TipoProcessoEnum.JuizadoEspecial:
                    return new JuizadoEspecialFM();
                case (long)TipoProcessoEnum.Trabalhista:
                    return new TrabalhistaFM();
                case (long)TipoProcessoEnum.TributarioAdministrativo:
                    return new TributarioAdministrativoFM();
                case (long)TipoProcessoEnum.TributarioJudicial:
                    return new TributarioJudicialFM();
                default:
                    throw new MissingMethodException("Não foi possível encontrar o Factory informado (QueryFactoryMethod)");
            }
        }
    }
}
