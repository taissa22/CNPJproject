using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.Interface;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.GrupoDeEstados.Controllers
{
    /// <summary>
    /// Controller para Agendamento de alteração em bloco
    /// </summary>
    [Route("grupo-de-estado")]
    [ApiController]    
    public class GrupoDeEstadosController : ApiControllerBase
    {
        private readonly IGrupoDeEstadosAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public GrupoDeEstadosController(IGrupoDeEstadosAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Atualiza o grupo e os relacionamentos com os estados
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromBody] IList<GrupoDeEstadosDTO> grp)
        {
            return Result(appService.Atualizar(grp));
        }

        /// <summary>
        /// Retorna uma lista de empresas do grupo contabil sap
        /// </summary>
        [HttpGet]
        public IActionResult ListarGrupoEstados()
        {
            return Ok(appService.ListarGrupoDeEstados());
        }

        /// <summary>
        /// Retorna uma lista de empresas do grupo contabil sap
        /// </summary>
        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar()
        {
            var resultado = await appService.Exportar();

            StringBuilder csv = new StringBuilder();
            csv.AppendLine($"CÓDIGO; GRUPO; EMPRESAS DO GRUPO;");
            if (resultado.Count() > 0)
            {
                foreach (var registro in resultado)
                {
                    foreach (var linha in registro.GrupoEstados)
                    {
                        csv.Append($"\"{registro.Id}\";");
                        csv.Append($"\"{registro.NomeGrupo}\";");
                        csv.Append($"\"{linha.Estado.NomeEstado}\";");
                        csv.AppendLine(string.Empty);
                    }
                }
            }

            string nomeArquivo = $"Grupo_Empresa_Contabil_SAP_{DateTime.Now:ddMMyyyy_HHmmss}.csv";
            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
            return File(bytes, "text/csv", nomeArquivo);
        }


    }
}
