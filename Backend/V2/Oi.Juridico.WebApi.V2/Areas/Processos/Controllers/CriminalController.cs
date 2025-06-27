using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.Contextos.V2.CriminalContext.Data;
using Oi.Juridico.Contextos.V2.CriminalContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.Processos.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.Controllers
{
    /// <summary>
    /// Api associação das permissões de módulos
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CriminalController : ControllerBase
    {
        private readonly CriminalContext _db;
        private readonly ILogger<CriminalController> _logger;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        public CriminalController(CriminalContext db, ILogger<CriminalController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Obtém os estados cadastrados no banco de dados
        /// </summary>
        /// <returns>Lista de estados</returns>
        [HttpGet, Route("Estados")]
        public async Task<IActionResult> ListarEstados()
        {
            var data = _db.Estado
                .AsNoTracking()
                .OrderBy(x => x.NomEstado)
                .Select(x => new { Id = x.CodEstado, Descricao = x.NomEstado })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém os municípios do estado cadastrados no banco de dados
        /// <param name="cod_estado">Código do estado</param>
        /// </summary>
        /// <returns>Lista de municípios</returns>
        [HttpGet, Route("Municipios")]
        public async Task<IActionResult> ListarMunicipios(string cod_estado)
        {
            var data = _db.Municipio
                .AsNoTracking()
                .Where(x => x.CodEstado == cod_estado)
                .OrderBy(x => x.NomMunicipio)
                .Select(x => new { Id = x.CodMunicipio, Descricao = x.NomMunicipio })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém Orgãos cadastrados no banco de dados
        /// </summary>
        /// <returns>Lista de Orgãos</returns>
        [HttpGet, Route("Orgaos")]
        public async Task<IActionResult> ListarOrgaos()
        {
            var data = _db.Parte
                .AsNoTracking()
                .Where(x => x.CodTipoParte == "2")
                .OrderBy(x => x.NomParte)
                .Select(x => new { Id = x.CodParte, Descricao = x.NomParte })
                .ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Obtém as competências do órgão cadastrados no banco de dados
        /// <param name="cod_parte">Código do órgão</param>
        /// </summary>
        /// <returns>Lista das competências do órgão,/returns>
        [HttpGet, Route("Competencias")]
        public async Task<IActionResult> ListarCompetencias(long cod_parte)
        {
            var data = _db.Competencia
                .AsNoTracking()
                .Where(x => x.CodParte == cod_parte)
                .OrderBy(x => x.NomCompetencia)
                .Select(x => new { Id = x.CodCompetencia, Descricao = x.NomCompetencia })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém os assuntos cadastrados no banco de dados
        /// <param name="ind_criminal_adm">Se é criminal adm (S ou N)</param>
        /// <param name="ind_criminal_judicial">Se é criminal juizado (S ou N)</param>
        /// </summary>
        /// <returns>Lista de assuntos,/returns>
        [HttpGet, Route("Assuntos")]
        public async Task<IActionResult> ListarAssuntos(string ind_criminal_adm, string ind_criminal_judicial)
        {
            var data = _db.Assunto
                .AsNoTracking()
                .Where(x => x.IndCriminalAdm == ind_criminal_adm && x.IndCriminalJudicial == ind_criminal_judicial)
                .OrderBy(x => x.DscAssunto)
                .Select(x => new { Id = x.CodAssunto, Descricao = x.DscAssunto })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém os Tipos de participação de processos criminais
        /// </summary>
        /// <returns>Lista de assuntos,/returns>
        [HttpGet, Route("TiposParticipacao")]
        public async Task<IActionResult> ListarTiposParticipacao()
        {
            var data = _db.TipoParticipacaoEmpresa
                .AsNoTracking()
                .Where(x => x.TpCodTipoProcesso == 13 && x.IndAtivo == "S")
                .OrderBy(x => x.Descricao)
                .Select(x => new { Id = x.Id, Descricao = x.Descricao })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém as empresas do grupo
        /// <param name="cod_tipo_processo">Tipo de processo</param>
        /// </summary>
        /// <returns>Lista de empresas do grupo</returns>
        [HttpGet, Route("EmpresasDoGrupo")]
        public async Task<IActionResult> ListarEmpresasDoGrupo(short cod_tipo_processo)
        {
            var usuario = _db.AcaUsuario
                .AsNoTracking()
                .Include(x => x.CodProfissional)
                .Where(x => x.CodUsuario == User.Identity!.Name)
                .FirstOrDefault();


            if (usuario!.CodProfissional!.Any())
            {
                var data = _db.Parte
                    .AsNoTracking()
                    .Where(x => x.CodTipoParte == "E")
                    .OrderBy(x => x.NomParte)
                    .Select(x => new { Id = x.CodParte, Descricao = x.NomParte })
                    .ToListAsync();

                return Ok(data);
            }
            else
            {
                var regionais = _db.UsuarioRegional.Where(x => x.CodUsuario == usuario.CodUsuario && x.CodTipoProcesso == cod_tipo_processo).Select(x => x.CodRegional).ToList();

                var data = _db.Parte
                    .AsNoTracking()
                    .Where(x => x.CodTipoParte == "E" && x.CodRegional != null && regionais.Contains(x.CodRegional.Value))
                    .OrderBy(x => x.NomParte)
                    .Select(x => new { Id = x.CodParte, Descricao = x.NomParte })
                    .ToListAsync();

                return Ok(data);
            }
        }

        /// <summary>
        /// Obtém os escritórios
        /// <param name="cod_tipo_processo">Tipo de processo</param>
        /// </summary>
        /// <returns>Lista de escritórios</returns>
        [HttpGet, Route("Escritorios")]
        public async Task<IActionResult> ListarEscritorios(short cod_tipo_processo)
        {
            var usuario = _db.AcaUsuario
                .AsNoTracking()
                .Include(x => x.CodProfissional)
                .Where(x => x.CodUsuario == User.Identity!.Name)
                .FirstOrDefault();


            if (usuario!.CodProfissional!.Any())
            {
                var escritorios = usuario!.CodProfissional.Select(x => x.CodProfissional).ToList();

                var data = _db.VEscritorios
                    .AsNoTracking()
                    .Where(x => ((x.IndCriminalAdm == "S" && cod_tipo_processo == 14) || (x.IndCriminalJudicial == "S" && cod_tipo_processo == 15))
                                && escritorios.Contains(x.CodProfissional.Value)
                                )
                    .OrderBy(x => x.NomProfissional)
                    .Select(x => new { Id = x.CodProfissional, Descricao = x.NomProfissional })
                    .ToListAsync();

                return Ok(data);
            }
            else
            {
                var data = _db.VEscritorios
                    .AsNoTracking()
                    .Where(x => ((x.IndCriminalAdm == "S" && cod_tipo_processo == 14) || (x.IndCriminalJudicial == "S" && cod_tipo_processo == 15)))
                    .OrderBy(x => x.NomProfissional)
                    .Select(x => new { Id = x.CodProfissional, Descricao = x.NomProfissional })
                    .ToListAsync();


                return Ok(data);
            }
        }

        /// <summary>
        /// Obtém os Tipos de Procedimento
        /// </summary>
        /// <returns>Lista de procedimentos</returns>
        [HttpGet, Route("TiposProcedimento")]
        public async Task<IActionResult> ListarTiposProcedimentos()
        {
            var data = _db.Procedimento
                .AsNoTracking()
                .Where(x => x.IndCriminalAdm == "S")
                .OrderBy(x => x.DscProcedimento)
                .Select(x => new { Id = x.CodProcedimento, Descricao = x.DscProcedimento })
                .ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Obtém os Tipos de Procedimento
        /// <param name="cod_estado">Código do estado</param>
        /// </summary>
        /// <returns>Lista de procedimentos</returns>
        [HttpGet, Route("Comarcas")]
        public async Task<IActionResult> ListarComarcas(string cod_estado)
        {
            var data = _db.Comarca
                .AsNoTracking()
                .Where(x => x.CodEstado == cod_estado)
                .OrderBy(x => x.NomComarca)
                .Select(x => new { Id = x.CodComarca, Descricao = x.NomComarca })
                .ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Obtém os Tipos de Procedimento
        /// </summary>
        /// <returns>Lista de procedimentos</returns>
        [HttpGet, Route("Acoes")]
        public async Task<IActionResult> ListarAcoes()
        {
            var data = _db.Acao
                .AsNoTracking()
                .Where(x => x.IndCriminalJudicial == "S")
                .OrderBy(x => x.DscAcao)
                .Select(x => new { Id = x.CodAcao, Descricao = x.DscAcao })
                .ToListAsync();

            return Ok(data);
        }
        /// <summary>
        /// Obtém os Ultimos Processos acessados pelo usuário
        /// <param name="cod_tipo_processo">Tipo de processo</param>
        /// </summary>
        /// <returns>Lista de processos</returns>
        [HttpGet, Route("ListarUltimosProcessos")]
        public async Task<IActionResult> ListarUltimosProcessos(CancellationToken ct, int cod_tipo_processo)
        {
            var query = _db.ProcessoAcessoRecentes
               .AsNoTracking()
               .Where(x => x.UsrCodUsuario == User.Identity!.Name)
               .Where(x => x.ProcCodProcessoNavigation.CodTipoProcesso == cod_tipo_processo);

            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.ParteProcesso).ThenInclude(x => x.CodParteNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.ParteProcesso).ThenInclude(x => x.CodTipoParticipacaoNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodProfissionalNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodProcedimentoNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodParteOrgaoNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.AssuntoProcesso).ThenInclude(x => x.AssCodAssuntoNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodMunicipioNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodComarcaNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodAcaoNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodVaraNavigation).ThenInclude(x => x.CodTipoVaraNavigation);
            query = query.Include(x => x.ProcCodProcessoNavigation).ThenInclude(x => x.CodCompetenciaNavigation);

            var total = await query.CountAsync(ct);

            var data = await query
                .OrderByDescending(x => x.DataUltimoAcesso)
                .Skip(0)
                .Take(10)
                .Select(p => ProcessoToProcessoResponse(p.ProcCodProcessoNavigation, p.DataUltimoAcesso))
            .ToListAsync(ct);

            return Ok(new
            {
                data,
                total
            });
        }

        private IQueryable<Processo> ListarProcessosQuery(FiltrosProcessoCriminal filtros)
        {
            #region Tipo de processo

            IQueryable<Processo> query = _db.Processo
                .AsNoTracking()
                .Where(x => x.CodTipoProcesso == filtros.cod_tipo_processo);
            #endregion

            #region INCLUDES
            query = query.Include(x => x.ParteProcesso).ThenInclude(x => x.CodParteNavigation);
            query = query.Include(x => x.ParteProcesso).ThenInclude(x => x.CodTipoParticipacaoNavigation);
            query = query.Include(x => x.CodProfissionalNavigation);
            query = query.Include(x => x.CodProcedimentoNavigation);
            query = query.Include(x => x.CodParteOrgaoNavigation);
            query = query.Include(x => x.AssuntoProcesso).ThenInclude(x => x.AssCodAssuntoNavigation);
            query = query.Include(x => x.CodMunicipioNavigation);
            query = query.Include(x => x.CodComarcaNavigation);
            query = query.Include(x => x.CodAcaoNavigation);
            query = query.Include(x => x.CodVaraNavigation).ThenInclude(x => x.CodTipoVaraNavigation);
            query = query.Include(x => x.CodParteEmpresaNavigation);
            query = query.Include(x => x.TpempIdTipoParticEmpresa);
            query = query.Include(x => x.CodCompetenciaNavigation);

            #endregion

            #region Usuário logado
            var usuario = _db.AcaUsuario
                .Include(x=>x.CodProfissional)
                .Include(x=>x.UsuarioRegional)
                .Where(x => x.CodUsuario == User.Identity!.Name).FirstOrDefault();

            if (usuario.CodProfissional.Count > 0)
            {
                var escritorios = usuario.CodProfissional.Select(x => x.CodProfissional).ToList();
                query = query.Where(x => x.CodProfissional!=null && escritorios.Contains(x.CodProfissional.Value));
            }
            else
            {
                var regionais = usuario.UsuarioRegional.Where(x=>x.CodTipoProcesso==filtros.cod_tipo_processo).Select(x => x.CodRegional).ToList();
                query = query.Where(x => x.CodParteEmpresaNavigation.CodRegional!=null && regionais.Contains(x.CodParteEmpresaNavigation.CodRegional.Value));
            }

            #endregion

            #region Escritório
            if (filtros.cod_escritorio != null)
                query = query.Where(x => x.CodProfissional == filtros.cod_escritorio || x.CodEscritorioAcompanhante == filtros.cod_escritorio);
            #endregion

            #region Situação do processo
            if (filtros.situacao != null)
            {
                switch (filtros.situacao)
                {
                    case Enums.SituacaoProcessoEnum.Ativos:
                        query = query.Where(x => x.IndProcessoAtivo == "S"); break;
                    case Enums.SituacaoProcessoEnum.Inativos:
                        query = query.Where(x => x.IndProcessoAtivo == "N"); break;
                }
            }
            #endregion

            #region Número do processo
            if (!String.IsNullOrEmpty(filtros.numero_processo))
            {
                string like = filtros.numero_processo.Replace("%", "");

                switch (filtros.tipo_filtro_numero_processo2)
                {
                    case Enums.TipoFiltroEnum.Comecando: like += "%"; break;
                    case Enums.TipoFiltroEnum.Terminando: like = "%" + like; break;
                    case Enums.TipoFiltroEnum.QualquerParte: like = "%" + like.Replace(" ", "%") + "%"; break;
                }

                switch (filtros.tipo_filtro_numero_processo1)
                {
                    case Enums.TipoFiltroNumeroProcessoEnum.Antigo:
                        query = query.Where(x => EF.Functions.Like(x.NumProcessoAntigo, like)); break;
                    case Enums.TipoFiltroNumeroProcessoEnum.Ambos:
                        query = query.Where(x => EF.Functions.Like(x.NroProcessoCartorio, like) || EF.Functions.Like(x.NumProcessoAntigo, like)); break;
                    default:
                        query = query.Where(x => EF.Functions.Like(x.NroProcessoCartorio, like)); break;
                }
            }
            #endregion

            #region Órgão
            if (filtros.cod_orgao != null)
            {
                query = query.Where(x => x.CodParteOrgao == filtros.cod_orgao.Value);
            }
            #endregion

            #region Competência
            if (filtros.cod_competencia != null)
            {
                query = query.Where(x => x.CodCompetencia == filtros.cod_competencia);
            }
            #endregion

            #region Estado
            if (!String.IsNullOrEmpty(filtros.cod_estado))
            {
                query = query.Where(x => x.CodEstado == filtros.cod_estado);
            }
            #endregion

            #region Município
            if (filtros.cod_municipio != null)
            {
                query = query.Where(x => x.CodMunicipio == filtros.cod_municipio);
            }
            #endregion

            #region Comarca
            if (filtros.cod_comarca != null)
            {
                query = query.Where(x => x.CodComarca == filtros.cod_comarca);
            }
            #endregion

            #region Empresa Do Grupo
            if (filtros.cod_empresa != null)
            {
                query = query.Where(x => x.CodParteEmpresa == filtros.cod_empresa);
            }
            #endregion

            #region CPF/CNPJ Parte
            if (!String.IsNullOrEmpty(filtros.documento_parte))
            {
                string documento = Regex.Replace(filtros.documento_parte, @"[^\d]", "");

                switch (filtros.tipo_filtro_documento_parte)
                {
                    case Enums.TipoFiltroDocumentoParteEnum.CNPJ:
                        query = query.Where(x => x.ParteProcesso.Where(y => y.CodParteNavigation.CgcParte == documento).Any()); break;
                    case Enums.TipoFiltroDocumentoParteEnum.CPF:
                        query = query.Where(x => x.ParteProcesso.Where(y => y.CodParteNavigation.CpfParte == documento).Any()); break;
                    default:
                        query = query.Where(x =>
                                            x.ParteProcesso.Where(y => y.CodParteNavigation.CgcParte == documento).Any()
                                         || x.ParteProcesso.Where(y => y.CodParteNavigation.CpfParte == documento).Any()
                         ); break;

                }
            }

            #endregion

            #region Tipo Procedimento
            if (filtros.cod_tipo_procedimento != null)
            {
                query = query.Where(x => x.CodProcedimento == filtros.cod_tipo_procedimento);
            }
            #endregion

            #region Nome Parte
            if (!String.IsNullOrEmpty(filtros.nome_parte))
            {
                string like = filtros.nome_parte.ToUpper().Replace("%", "");

                switch (filtros.tipo_filtro_nome_parte)
                {
                    case Enums.TipoFiltroEnum.Comecando: like += "%"; break;
                    case Enums.TipoFiltroEnum.Terminando: like = "%" + like; break;
                    case Enums.TipoFiltroEnum.QualquerParte: like = "%" + like.Replace(" ", "%") + "%"; break;
                }
                query = query.Where(x => x.ParteProcesso.Where(y => EF.Functions.Like(y.CodParteNavigation.NomParte, like)).Any());
            }
            #endregion

            #region Assunto
            if (filtros.cod_assunto != null)
            {
                query = query.Where(x => x.AssuntoProcesso.Where(a => a.AssCodAssunto == filtros.cod_assunto).Any());
            }
            #endregion

            #region Tipo Participação
            if (filtros.cod_tipo_participacao != null)
            {
                query = query.Where(x => x.ParteProcesso.Where(y => y.CodTipoParticipacao == filtros.cod_tipo_participacao).Any());
            }
            #endregion

            #region Criticidade
            if (filtros.cod_criticidade != null)
            {
                switch (filtros.cod_criticidade)
                {
                    case Enums.TipoCriticidadeEnum.Alta: query = query.Where(x => x.CodCriticidade == "A"); break;
                    case Enums.TipoCriticidadeEnum.Media: query = query.Where(x => x.CodCriticidade == "M"); break;
                    case Enums.TipoCriticidadeEnum.Baixa: query = query.Where(x => x.CodCriticidade == "B"); break;
                }
            }
            #endregion

            #region Chegado
            if (filtros.checado != null)
            {
                switch (filtros.checado)
                {
                    case Enums.ProcedimentosChecadosEnum.Sim: query = query.Where(x => x.IndChecado == "S"); break;
                    case Enums.ProcedimentosChecadosEnum.Nao: query = query.Where(x => x.IndChecado == "N"); break;
                }
            }

            #endregion

            #region Acao
            if (filtros.cod_acao != null)
            {
                query = query.Where(x => x.CodAcao == filtros.cod_acao);
            }
            #endregion

            #region Testemunha
            if (filtros.cpf_testemunha != null)
            {
                query = query.Where(x => x.TestemunhasProcessos.Where(t => t.Cpf == Regex.Replace(filtros.cpf_testemunha, @"[^\d]", "")).Count() > 0);
            }
            #endregion

            return query.OrderBy(x => x.CodProfissionalNavigation.NomProfissional)
                .ThenBy(x => x.NumeroProcSemFormatacao)
                .ThenBy(x => x.CodParteOrgaoNavigation.NomParte)
                .ThenBy(x => x.CodCompetenciaNavigation.NomCompetencia)
                .ThenBy(x => x.CodEstado)
                .ThenBy(x => x.CodMunicipio);
        }

        [HttpPost, Route("Processos")]
        public async Task<IActionResult> ListarProcessos(CancellationToken ct, [FromBody] FiltrosProcessoCriminal filtros)
        {
            if (filtros.cod_tipo_processo < 14 || filtros.cod_tipo_processo > 15)
                return Problem("Tipo de processo informado não é Criminal");

            var query = ListarProcessosQuery(filtros);

            var total = await query.CountAsync(ct);
            var dataquery = await query.Skip((filtros.page - 1) * filtros.size).Take(filtros.size).ToListAsync(ct);

            List<BuscaProcessoResponse> data = new List<BuscaProcessoResponse>();

            foreach (Processo p in dataquery)
            {
                if (!data.Where(x => x.id == p.CodProfissional).Any())
                {
                    data.Add(new BuscaProcessoResponse()
                    {
                        id = p.CodProfissional.Value,
                        nome = p.CodProfissionalNavigation.NomProfissional,
                        processos = new List<ProcessoResponse>()
                    });
                }

                data.Where(x => x.id == p.CodProfissional)
                    .First()
                    .processos.Add(ProcessoToProcessoResponse(p, null));
            }

            return Ok(new
            {
                data,
                total
            });
        }

        [HttpPost, Route("Download")]
        public async Task<IActionResult> Download(CancellationToken ct, [FromBody] FiltrosProcessoCriminal filtros)
        {
            if (filtros.cod_tipo_processo < 14 || filtros.cod_tipo_processo > 15)
                return Problem("Tipo de processo informado não é Criminal");

            DataTable tabelaRetorno = new DataTable();
            string filename = null;
            string titulo = null;

            #region Colunas
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>();


            switch (filtros.cod_tipo_processo)
            {
                case 14: //Administrativo

                    filename = "relatorio_juridico_criminal_administrativo";
                    titulo = "PROCEDIMENTOS CRIMINAIS ADMINISTRATIVOS";

                    ColumnNames.Add("TipoProcedimento", "Tipo de Procedimento");
                    ColumnNames.Add("SituacaoProcedimento", "Situação do procedimento");
                    ColumnNames.Add("NumeroProcedimento", "Numero do Procedimento");
                    ColumnNames.Add("Estado", "Estado");
                    ColumnNames.Add("Municipio", "Município");
                    ColumnNames.Add("Orgao", "Orgão");
                    ColumnNames.Add("Competencia", "Competencia");
                    ColumnNames.Add("Assuntos", "Assuntos");
                    ColumnNames.Add("TipoParte", "Tipo de Parte");
                    ColumnNames.Add("Partes", "Partes");
                    ColumnNames.Add("TipoParticipacaoEmpresa", "Tipo Participação da Empresa");
                    ColumnNames.Add("Escritorio", "Escritório");
                    ColumnNames.Add("DataInstauracao", "Data de Instauração");
                    ColumnNames.Add("Criticidade", "Criticidade");
                    ColumnNames.Add("EmpresaGrupo", "Empresa do Grupo");
                    break;
                case 15://Judicial

                    filename = "relatorio_juridico_criminal_judicial";
                    titulo = "PROCESSOS CRIMINAL JUDICIAL";

                    ColumnNames.Add("NumeroProcedimento", "Numero do Processo");
                    ColumnNames.Add("Acao", "Ação");
                    ColumnNames.Add("Estado", "Estado");
                    ColumnNames.Add("Comarca", "Comarca");
                    ColumnNames.Add("Vara", "Vara");
                    ColumnNames.Add("EmpresaGrupo", "Empresa do Grupo");
                    ColumnNames.Add("Escritorio", "Escritório");
                    ColumnNames.Add("SituacaoProcedimento", "Situação do processo");
                    ColumnNames.Add("Criticidade", "Criticidade");
                    ColumnNames.Add("TipoParticipacaoEmpresa", "Tipo Participação da Empresa");
                    ColumnNames.Add("Assuntos", "Assuntos");
                    ColumnNames.Add("TipoParte", "Tipo de Parte");
                    ColumnNames.Add("Partes", "Partes");
                    break;
            }

            foreach (String key in ColumnNames.Keys)
            {
                tabelaRetorno.Columns.Add(key);
            }

            #endregion

            var lista = ListarProcessosQuery(filtros).ToList();

            #region Preenche colunas

            foreach (Processo p in lista)
            {
                DataRow linha = tabelaRetorno.NewRow();
                tabelaRetorno.Rows.Add(linha);

                foreach (DataColumn coluna in tabelaRetorno.Columns)
                {
                    switch (coluna.ColumnName)
                    {
                        case "Acao":
                            {
                                linha[coluna.ColumnName] = p.CodAcaoNavigation.DscAcao;
                                break;
                            }
                        case "TipoProcedimento":
                            {
                                linha[coluna.ColumnName] = p.CodProcedimentoNavigation.DscProcedimento;
                                break;
                            }
                        case "SituacaoProcedimento":
                            {
                                linha[coluna.ColumnName] = p.IndProcessoAtivo == "S" ? "Ativo" : "Inativo";
                                break;
                            }
                        case "NumeroProcedimento":
                            {
                                linha[coluna.ColumnName] = p.NroProcessoCartorio;
                                break;
                            }
                        case "Estado":
                            {
                                linha[coluna.ColumnName] = p.CodEstado;
                                break;
                            }
                        case "Municipio":
                            {
                                linha[coluna.ColumnName] = p.CodMunicipio != null ? p.CodMunicipioNavigation.NomMunicipio : "";
                                break;
                            }
                        case "Comarca":
                            {
                                linha[coluna.ColumnName] = p.CodComarcaNavigation.NomComarca;
                                break;
                            }
                        case "Vara":
                            {
                                linha[coluna.ColumnName] = p.CodVaraNavigation != null ? p.CodVara + "ª " + p.CodVaraNavigation.CodTipoVaraNavigation.NomTipoVara : "";
                                break;
                            }
                        case "Orgao":
                            {
                                linha[coluna.ColumnName] = p.CodParteOrgao != null ? p.CodParteOrgaoNavigation.NomParte : "";
                                break;
                            }
                        case "Competencia":
                            {
                                linha[coluna.ColumnName] = p.CodCompetencia != null ? "FALTA MAPEAR" : "";
                                break;
                            }
                        case "Escritorio":
                            {
                                linha[coluna.ColumnName] = p.CodProfissionalNavigation.NomProfissional;
                                break;
                            }
                        case "DataInstauracao":
                            {
                                linha[coluna.ColumnName] = p.DatDistribuicao != null ? p.DatDistribuicao.Value.ToString("dd/MM/yyyy") : "";
                                break;
                            }
                        case "Criticidade":
                            {
                                linha[coluna.ColumnName] = formataCriticidade(p.CodCriticidade);
                                break;
                            }
                        case "EmpresaGrupo":
                            {
                                linha[coluna.ColumnName] = p.CodParteEmpresaNavigation.NomParte;
                                break;
                            }
                        default:
                            break;
                    }
                }

                List<KeyValuePair<string, string>> partes = p.ParteProcesso.Select(x => new KeyValuePair<string, string>(x.CodTipoParticipacaoNavigation.DscTipoParticipacao, x.CodParteNavigation.NomParte)).ToList();
                List<string> assuntos = p.AssuntoProcesso != null ? p.AssuntoProcesso.Select(x => x.AssCodAssuntoNavigation.DscAssunto).ToList() : new List<string>();
                List<string> participacoes = p.TpempIdTipoParticEmpresa.Select(x => x.Descricao).ToList();

                if (partes.Count > 0)
                {
                    linha["TipoParte"] = partes[0].Key;
                    linha["Partes"] = partes[0].Value;
                }

                if (participacoes.Count > 0)
                {
                    linha["TipoParticipacaoEmpresa"] = participacoes[0];
                }

                if (assuntos.Count > 0)
                {
                    linha["Assuntos"] = assuntos[0];
                }

                int qtdeItensMaiorLista = (int)Math.Max(partes.Count, participacoes.Count);

                for (int i = 1; i < qtdeItensMaiorLista; i++)
                {
                    DataRow linhaVazia = tabelaRetorno.NewRow();

                    tabelaRetorno.Rows.Add(linhaVazia);

                    if (partes.Count > i)
                    {
                        linhaVazia["TipoParte"] = partes[i].Key;
                        linhaVazia["Partes"] = partes[i].Value;
                    }

                    if (participacoes.Count > i)
                    {
                        linhaVazia["TipoParticipacaoEmpresa"] = participacoes[i];
                    }

                    if (assuntos.Count > i)
                    {
                        linhaVazia["Assuntos"] = assuntos[i];
                    }
                }
            }

            #endregion

            string header = $"OI JURÍDICO\n\n{titulo}\n\n";

            byte[] csv = Encoding.Unicode.GetBytes(header + ToCSV(tabelaRetorno).ToString());

            DateTime dtExportacao = DateTime.Now;
            string nomeArquivo = $"{filename}_{dtExportacao:yyyy_MM_dd}_{dtExportacao.Hour}h{dtExportacao.Minute}m{3}s_{dtExportacao.Second}.csv";
            return File(csv, "application/csv", nomeArquivo);
        }

        private static ProcessoResponse ProcessoToProcessoResponse(Processo p, DateTime? dataUltimoAcesso)
        {
            return new ProcessoResponse()
            {
                id = p.CodProcesso,
                nro_processo = p.NroProcessoCartorio,
                procedimento = p.CodProcedimento != null ? p.CodProcedimentoNavigation.DscProcedimento : "",
                orgao = p.CodParteOrgao != null ? p.CodParteOrgaoNavigation.NomParte : "",
                competencia = p.CodCompetencia != null ? p.CodCompetenciaNavigation.NomCompetencia : "",
                estado = p.CodEstado,
                municipio = p.CodMunicipio != null ? p.CodMunicipioNavigation.NomMunicipio : "",
                assunto = p.AssuntoProcesso != null ? p.AssuntoProcesso.Select(x => x.AssCodAssuntoNavigation.DscAssunto).ToList() : new List<string>(),
                instauracao = p.DatDistribuicao != null ? p.DatDistribuicao.Value.ToString("dd/MM/yyyy") : "",
                data_ultimo_acesso = dataUltimoAcesso != null ? dataUltimoAcesso.Value.ToString("dd/MM/yyyy") : "",
                status = p.IndProcessoAtivo == "S" ? "Ativo" : "Inativo",
                criticidade = formataCriticidade(p.CodCriticidade),
                acao = p.CodAcaoNavigation != null ? p.CodAcaoNavigation.DscAcao : "",
                comarca = p.CodComarcaNavigation != null ? p.CodComarcaNavigation.NomComarca : "",
                nro_processo_antigo = p.NumProcessoAntigo,
                vara = p.CodVaraNavigation != null ? p.CodVara + "ª " + p.CodVaraNavigation.CodTipoVaraNavigation.NomTipoVara : "",
                partes = p.ParteProcesso.GroupBy(x => x.CodTipoParticipacaoNavigation.DscTipoParticipacao).ToDictionary(g => g.Key, g => g.OrderBy(x => x.CodParteNavigation.NomParte).Select(x => x.CodParteNavigation.NomParte).ToList())
            };
        }

        private static StringBuilder ToCSV(DataTable dtData)
        {
            StringBuilder data = new StringBuilder();

            //Taking the column names.
            for (int column = 0; column < dtData.Columns.Count; column++)
            {
                if (column == dtData.Columns.Count - 1)
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(";", ","));
                else
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(";", ",") + ';');
            }

            data.Append(Environment.NewLine);//New line after appending columns.

            for (int row = 0; row < dtData.Rows.Count; row++)
            {
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    if (column == dtData.Columns.Count - 1)
                        data.Append(dtData.Rows[row][column].ToString()!.Replace(";", ","));
                    else
                        data.Append(dtData.Rows[row][column].ToString()!.Replace(";", ",") + ';');
                }

                //Making sure that end of the file, should not have a new line.
                if (row != dtData.Rows.Count - 1)
                    data.Append(Environment.NewLine);
            }
            return data;
        }

        private static string formataCriticidade(string CodCriticidade)
        {
            switch (CodCriticidade)
            {
                case "A": return "Alta";
                case "M": return "Média";
                case "B": return "Baixa";
                default: return "";
            }
        }
    }
}
