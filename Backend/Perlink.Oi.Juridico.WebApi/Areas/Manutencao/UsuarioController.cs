using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Usuario;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao {

    /// <summary>
    /// Controller Usuario 
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/usuarios")]
    public class UsuarioController : ApiControllerBase {

        /// <summary>
        /// Recupera os Usuario 
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IUsuarioRepository repository
            ) {

            return Result(repository.ObterTodos());
        }

        [HttpGet("ativos")]
        public IEnumerable<UsuarioCommandResult> ObterAtivos([FromServices] IUsuarioRepository repository)
        {
            return repository.ObterAtivos();
        }

        [HttpGet("todos")]
        public IEnumerable<UsuarioCommandResult> ObterAtivosEInativos([FromServices] IUsuarioRepository repository)
        {
            return repository.ObterAtivosEInativos();
        }

        [HttpGet("prepostos-ativos")]
        public IEnumerable<UsuarioCommandResult> ObterPrepostosAtivos([FromServices] IUsuarioRepository repository)
        {
            return repository.ObterPrepostosAtivos();
        }

        [HttpGet("todos-prepostos")]
        public IEnumerable<UsuarioCommandResult> ObterTodosPrepostos([FromServices] IUsuarioRepository repository)
        {
            return repository.ObterTodosPrepostos();
        }
    }
}