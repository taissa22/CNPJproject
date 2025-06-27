using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories.Implementations {
    internal class ParametroJuridicoRepository : IParametroJuridicoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private IParametroJuridicoProvider ParametroJuridicoProvider { get; }

        public ParametroJuridicoRepository(IDatabaseContext databaseContext, IParametroJuridicoProvider parametroJuridicoProvider) {
            DatabaseContext = databaseContext;
            ParametroJuridicoProvider = parametroJuridicoProvider;
        }

        public CommandResult<ParametroJuridico> Obter(string parametroId) {
            var resultado = ParametroJuridicoProvider.Obter(parametroId);
            if (!resultado.IsValid) {
                return CommandResult<ParametroJuridico>.Invalid(resultado.Mensagens);
            }
            
            return CommandResult<ParametroJuridico>.Valid(resultado.Dados);
        }
    }
}
