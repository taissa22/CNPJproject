using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.WebApi.Areas
{
    /// <summary>
    /// Controller base da api
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Retorna um Result de acordo com o CommandResult fornecido
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Result</returns>
        // CommandResult<T> : CommandResult
        protected IActionResult Result(CommandResult result)
        {
            switch (result.Tipo)
            {
                case ResultType.Valid:
                    return Ok();

                case ResultType.Forbidden:
                    return Forbid();

                case ResultType.Invalid:
                default:
                    return BadRequest(result.Mensagens);
            }
        }

        /// <summary>
        /// Retorna um Result de tipo genérico de acordo com o CommandResult fornecido
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns>Result de tipo genérico</returns>
        protected IActionResult Result<T>(CommandResult<T> result)
        {
            switch (result.Tipo)
            {
                case ResultType.Valid:
                    return Ok(result.Dados);

                case ResultType.Forbidden:
                    return Forbid();

                case ResultType.Invalid:
                default:
                    return BadRequest(result.Mensagens);
            }
        }
    }
}