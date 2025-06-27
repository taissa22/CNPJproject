using CsvHelper;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Dtos;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Repositories;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Services
{
    public class ParametrizacaoClosingService
    {
        private readonly ParametrizacaoClosingRepository _parametrizacaoClosingRepository;

        private readonly ParametroJuridicoContext _parametroJuridicoContext;

        public ParametrizacaoClosingService(ParametrizacaoClosingRepository parametrizacaoClosingRepository, ParametroJuridicoContext parametroJuridicoContext)
        {
            _parametrizacaoClosingRepository = parametrizacaoClosingRepository;
            _parametroJuridicoContext = parametroJuridicoContext;
        }

        public async Task<bool> ValidarRegraClassificacaoClosing(AtualizarRequest requestDTO)
        {
            try
            {
                List<string> lst_parametros = new List<string> { "COD_TEC_CLOSING_FIBRA", "COD_TEC_CLOSING_MOVEL" };
                var tipoTecnologia = await _parametroJuridicoContext.RecuperaConteudoParametroJuridicoPorListaDeId(lst_parametros);
                var validar = await _parametrizacaoClosingRepository.ValidarRegraClassificacaoClosing(requestDTO, tipoTecnologia);

                return validar;

            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
