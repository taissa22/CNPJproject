using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class TipoProcessoService : BaseCrudService<TipoProcesso, long>, ITipoProcessoService
    {
        private readonly ITipoProcessoRepository repository;
        private IPermissaoService permissaoService;

        public TipoProcessoService(ITipoProcessoRepository repository, IPermissaoService permissaoService) : base(repository)
        {
            this.repository = repository;
            this.permissaoService = permissaoService;
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosConsultaLote()
        {

            // Valido as permissões do usuário pra cada tipo de processo:
            List<long> Ids = new List<long>();

            if (permissaoService.TemPermissao(PermissaoEnum.f_ConsultaLotesSapCC))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));

            if (permissaoService.TemPermissao(PermissaoEnum.f_ConsultaLotesSapTrabalhista))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));

            if (permissaoService.TemPermissao(PermissaoEnum.f_ConsultaLotesSapJEC))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));

            if (permissaoService.TemPermissao(PermissaoEnum.f_ConsultaLotesSapCE))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));

            if (permissaoService.TemPermissao(PermissaoEnum.f_ConsultaLotesSapPEX))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoCategoriaPagamento()
        {
            // Valido as permissões do usuário pra cada tipo de processo:
            List<long> Ids = new List<long>();

            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoCC))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoCE))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoJuiz))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoTrab))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoTribAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioAdministrativo));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoTribJud))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioJudicial));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Administrativo));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoProc))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Procon));
            if(permissaoService.TemPermissao(PermissaoEnum.f_CategoriasDePagamentoPex))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosCriaLote()
        {

            // Valido as permissões do usuário pra cada tipo de processo:
            List<long> Ids = new List<long>();

            if (permissaoService.TemPermissao(PermissaoEnum.f_CriarLotesCivelConsumidor))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));

            if (permissaoService.TemPermissao(PermissaoEnum.f_CriarLotesCivelEstrategico))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));

            if (permissaoService.TemPermissao(PermissaoEnum.f_CriarLotesJuizado))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));

            if (permissaoService.TemPermissao(PermissaoEnum.f_CriarLotesPex))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));

            if (permissaoService.TemPermissao(PermissaoEnum.f_CriarLotesTrabalhista))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarParaConsultaSaldoDeGarantia()
        {
            // Valido as permissões do usuário pra cada tipo de processo:
            List<long> Ids = new List<long>();

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasCC"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasCE"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasJEC"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasTrab"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasTribAdm"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioAdministrativo));

            if (permissaoService.TemPermissao("f_ConsultaSaldoGarantiasTribJud"))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioJudicial));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosEstornaLancamento()
        {
            List<long> Ids = new List<long>();
            if (permissaoService.TemPermissao(PermissaoEnum.f_EstornaLancamentosCC))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));
            if (permissaoService.TemPermissao(PermissaoEnum.f_EstornaLancamentosCE))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));
            if (permissaoService.TemPermissao(PermissaoEnum.f_EstornaLancamentosPex))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));
            if (permissaoService.TemPermissao(PermissaoEnum.f_EstornaLancamentosTrab))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));
            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoVigenciaCivil()
        {
            List<long> Ids = new List<long>();

            if (permissaoService.TemPermissao(PermissaoEnum.f_JurosCivelConsumidor))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));
            if (permissaoService.TemPermissao(PermissaoEnum.f_JurosCivelEstrategico))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoTipoAudiencia()
        {
            List<long> Ids = new List<long>();

            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaCivCon))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaCivEst))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaTrab))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaTrabAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TrabalhistaAdministrativo));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaTribAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioAdministrativo));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaTribJud))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioJudicial));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Administrativo));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaCivAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelAdministrativo));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaCriJud))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CriminalJudicial));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaCriAdm))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.CriminalAdministrativo));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaJec))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaProc))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Procon));
            if (permissaoService.TemPermissao(PermissaoEnum.f_TipoDeAudienciaPex))
                Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));

            return await repository.RecuperarTodosSAP(Ids.ToArray());
        }
    }
}