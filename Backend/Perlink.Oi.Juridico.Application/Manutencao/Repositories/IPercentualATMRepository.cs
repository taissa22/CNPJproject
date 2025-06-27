using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IPercentualATMRepository
    {
        CommandResult<PaginatedQueryResult<PercentualATM>> ObterPaginadoPercentualATM(int pagina, int quantidade,
             PercentualATMSort sort, bool ascending, DateTime DataVigencia, int codTipoProcesso);

        CommandResult<IReadOnlyCollection<PercentualATM>> Obter(PercentualATMSort sort, bool ascending, DateTime dataVigencia, int codTipoProcesso);
       
        CommandResult<IReadOnlyCollection<DateTime>> ObterComboVigencias(int codTipoProcesso);

        CommandResult<bool> ExistePercentualParaVigencia(DateTime dataVigencia, int codTipoProcesso);

        PercentualATM RecuperarVigenciaParaUF(string estado, DateTime dataVigencia, int codTipoProcesso);
        
        DateTime RecuperarUltimaVigenciaCadastrada(int codTipoProcesso);       
    }
}