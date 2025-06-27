using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Interface;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios
{
    /// <summary>
    /// Controller de grupo de empresa contabil sap
    /// </summary>
    [Route("grupo-empresa-contabil-sap")]
    [ApiController]
    [Authorize]
    public class GrupoEmpresaContabilSapController : ApiControllerBase
    {
        private readonly IGrupoEmpresaContailSapAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public GrupoEmpresaContabilSapController(IGrupoEmpresaContailSapAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Atualiza o grupo e os relacionamentos com as empresas
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromBody] IList<GrupoEmpresaContabilSapDTO> grp)
        {
            return Result(appService.Atualizar(grp));
        }

        /// <summary>
        /// Retorna uma lista de empresas do grupo contabil sap
        /// </summary>
        [HttpGet]
        public IActionResult ListarGrupoEmpresaContabilSap()
        {
            return Ok(appService.ListarGrupoEmpresaContabilSap());
        }

        /// <summary>
        /// Retorna uma lista de empresas do grupo contabil sap
        /// </summary>
        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar()
        {
            var resultado = await appService.Exportar();

            StringBuilder csv = new StringBuilder();
            csv.AppendLine($"CÓDIGO; GRUPO; EMPRESAS DO GRUPO; RECUPERANDA;");
            if (resultado.Count() > 0)
            {
                foreach (var registro in resultado)
                {
                    foreach (var linha in registro.GrupoEmpresaContabilSapParte)
                    {
                        string recuperanda = linha.GrupoEmpresaContabilSap.Recuperanda == true ? "Sim" : "Não";
                        csv.Append($"\"{registro.Id}\";");
                        csv.Append($"\"{registro.NomeGrupo}\";");
                        csv.Append($"\"{linha.Empresa.Nome}\";");
                        csv.Append($"\"{recuperanda}\";");
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