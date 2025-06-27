using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Oi.Juridico.WebApi.V2.Areas
{
    public abstract class ApiControllerBase : ControllerBase
    {
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
