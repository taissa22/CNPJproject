using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service {
    public class ParametroService : BaseCrudService<Parametro, string>, IParametroService {
        private readonly IParametroRepository repository;
        private readonly IPermissaoService permissaoService;
        public ParametroService(IParametroRepository repository, IPermissaoService permissaoService) : base(repository) {
            this.repository = repository;
            this.permissaoService = permissaoService;
        }

        public Parametro RecuperarPorNome(string codigoDoParametro) {
            return repository.RecuperarPorNome(codigoDoParametro);
        }

        public IList<Parametro> CarregarConfiguracao() {
            IList<Parametro> parametros = new List<Parametro>();

            foreach (var item in System.Enum.GetValues(typeof(ConfiguracoesParametro))) {
                Parametro parametro = repository.RecuperarPorNome(Convert.ToString(item));

                if (parametro != null) {
                    parametros.Add(parametro);
                }
            }

            return parametros;
        }

        public async Task<IList<string>> ObterPermissoesParametro() {
            return await permissaoService.PermissoesModulo(ObterPermissoes());
        }

        private List<PermissaoEnum> ObterPermissoes() {
            List<PermissaoEnum> permissoes = new List<PermissaoEnum>();
            //permissoes.Add(PermissaoEnum.m_HonorariosParametros);
            //permissoes.Add(PermissaoEnum.mPagamentoHonorariosJuridicos);
            //permissoes.Add(PermissaoEnum.m_HonorariosParametros);
            return permissoes;
        }
    }
}