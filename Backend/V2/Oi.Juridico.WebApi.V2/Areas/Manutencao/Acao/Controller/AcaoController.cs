using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.Acao.Dtos;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System.Linq;
using System.Linq.Expressions;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.Acao.Controllers
{
    [Route("Acao")]
    [ApiController]
    public class AcaoController : ControllerBase
    {
        #region Propriedades 

        private readonly ParametroJuridicoContext _parametroJuridico;
        private ManutencaoDbContext _manutencaoContext;

        #endregion Propriedades 

        #region Construtor

        public AcaoController(ParametroJuridicoContext parametroJuridico, ManutencaoDbContext manutencaoContext)
        {
            _parametroJuridico = parametroJuridico;
            _manutencaoContext = manutencaoContext;
        }

        #endregion Construtor

        #region Metodos Http

        [HttpGet("ObterAcao")]
        public async Task<IActionResult> ObterAcao(CancellationToken ct, [FromQuery] int pagina, [FromQuery] int quantidade = 8, [FromQuery] string coluna = "id", [FromQuery] string direcao = "asc", [FromQuery] int tipoProcesso = 0, string? pesquisa = null)
        {

            var query = _manutencaoContext.Acao.AsNoTracking().Select(x => new AcaoResponse
            {
                Id = x.CodAcao,
                Descricao = x.DscAcao,
                EnviarAppPreposto = x.EnviarAppPreposto == "N" ? "NÃO" : "SIM",
                Ativo = x.IndAtivo == "N" ? "NÃO" : "SIM",
                IndJEC = x.IndAcaoJuizado,
                IndProcon = x.IndProcon,
                IndCivelConsumidor = x.IndAcaoCivel,
                IndCivelEstrategico = x.IndCivelEstrategico,
                IndTrabalhista = x.IndAcaoTrabalhista,
                IndTributarioJudicial = x.IndAcaoTributaria,
                IndCriminalJudicial = x.IndCriminalJudicial,
                IndPex = x.IndAcaoPex,
                NaturezaAcaoBBId = x.BbnatIdBbNatAcao,
                NaturezaAcaoBBDesc = GetDescricaoNaturezaBBGrid(tipoProcesso, x.BbnatIdBbNatAcao, ref _manutencaoContext),
                IndRequerEscritorio = x.IndRequerEscritorio == null || x.IndRequerEscritorio == "N" ? "NÃO" : "SIM",
                AcaoCivelEstrategicoId = GetAcaoCivelEstrategicoIdGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelEstrategicoDesc = GetAcaoCivelEstrategicoDescricaoGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelConsumidorId = GetAcaoCivelConsumidorIdGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelConsumidorDesc = GetAcaoCivelConsumidorDescricaoGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext)
            });
             

            if (pesquisa != null)
                query = query.Where(x => x.Descricao.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()));

            switch (tipoProcesso)
            {
                case (int)TipoProcessoEnum.JuizadoEspecial:
                    query = query.Where(x => x.IndJEC == "S");
                    break;

                case (int)TipoProcessoEnum.Procon:
                    query = query.Where(x => x.IndProcon == "S");
                    break;

                case (int)TipoProcessoEnum.CivelConsumidor:
                    query = query.Where(x => x.IndCivelConsumidor == "S");
                    break;

                case (int)TipoProcessoEnum.CivelEstrategico:
                    query = query.Where(x => x.IndCivelEstrategico == "S");
                    break;

                case (int)TipoProcessoEnum.CriminalJudicial:
                    query = query.Where(x => x.IndCriminalJudicial == "S");
                    break;

                case (int)TipoProcessoEnum.Pex:
                    query = query.Where(x => x.IndPex == "S");
                    break;

                case (int)TipoProcessoEnum.Trabalhista:
                    query = query.Where(x => x.IndTrabalhista == "S");
                    break;

                case (int)TipoProcessoEnum.TributarioJudicial:
                    query = query.Where(x => x.IndTributarioJudicial == "S");
                    break;

                default:
                    break;
            }
             

            var data = await query.ToListAsync(ct); ;            
            //Ordenação
            switch (coluna)
            {
                case "naturezaBB":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.NaturezaAcaoBBDesc).ToList() : data.OrderByDescending(x => x.NaturezaAcaoBBDesc).ToList();
                    }
                    break;

                case "acaoCivelEstrategicoDesc":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.AcaoCivelEstrategicoDesc).ToList() : data.OrderByDescending(x => x.AcaoCivelEstrategicoDesc).ToList();
                    }
                    break;

                case "acaoCivelConsumidorDesc":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.AcaoCivelConsumidorDesc).ToList() : data.OrderByDescending(x => x.AcaoCivelConsumidorDesc).ToList();
                    }
                    break;


                case "id":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Id).ToList() : data.OrderByDescending(x => x.Id).ToList();
                    }
                    break;

                case "descricao":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Descricao).ToList() : data.OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                case "enviarAppPreposto":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.EnviarAppPreposto).ToList() : data.OrderByDescending(x => x.EnviarAppPreposto).ToList();
                    }
                    break;

                case "requerEscritorio":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.IndRequerEscritorio).ToList() : data.OrderByDescending(x => x.IndRequerEscritorio).ToList();
                    }
                    break;

                default:
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Id).ToList() : data.OrderByDescending(x => x.Id).ToList();
                    }
                    break; 
            }
            var total = data.Count;
            data = data.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

            return Ok(new { data, total });
        }
         

        [HttpGet("ObterNaturezaAcaoBB")]
        public IActionResult ObterNaturezaAcaoBB()
        {
            var naturezaBB = _manutencaoContext.BbNaturezasAcoes.AsNoTracking().OrderBy(x => x.Nome).Select(x => new NaturezaBBResponse
            {
                Id = x.Id,
                Descricao = x.Nome
            });

            return Ok(new { naturezaBB });
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, int id, int tipoProcesso)
        {
            try
            {
                var acao = _manutencaoContext.Acao.FirstOrDefault(a => a.CodAcao == id);

                if (acao is null)
                {
                    return BadRequest("Ação não encontrado");
                }

                if (_manutencaoContext.Processo.Where(P => P.CodAcao == id).Count() > 0 || _manutencaoContext.AndamentoProcesso.Where(P => P.CodAcao == id).Count() > 0)
                {
                    return BadRequest("Esta Ação não pode ser excluída pois está associada a outros processos!");
                }

                if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                {
                    var migAcao = _manutencaoContext.MigAcao.Where(ma => ma.CodAcaoCivel == id).FirstOrDefault();

                    if (migAcao != null)
                    {
                        _manutencaoContext.Remove(migAcao);
                    }
                }

                if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                {
                    var migAcao = _manutencaoContext.MigAcao.Where(ma => ma.CodAcaoCivelEstrat == id).FirstOrDefault();

                    if (migAcao != null)
                    {
                        _manutencaoContext.Remove(migAcao);
                    }
                }

                _manutencaoContext.Remove(acao);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Exportar")]
        public async Task<IActionResult> Exportar(CancellationToken ct, [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc", [FromQuery] int tipoProcesso = 0, [FromQuery] string? pesquisa = "")
        {
            var query = _manutencaoContext.Acao.AsNoTracking().Select(x => new AcaoResponse
            {
                Id = x.CodAcao,
                Descricao = x.DscAcao,
                EnviarAppPreposto = x.EnviarAppPreposto == "N" ? "NÃO" : "SIM",
                Ativo = x.IndAtivo == "N" ? "NÃO" : "SIM",
                IndJEC = x.IndAcaoJuizado,
                IndProcon = x.IndProcon,
                IndCivelConsumidor = x.IndAcaoCivel,
                IndCivelEstrategico = x.IndCivelEstrategico,
                IndTrabalhista = x.IndAcaoTrabalhista,
                IndTributarioJudicial = x.IndAcaoTributaria,
                IndCriminalJudicial = x.IndCriminalJudicial,
                IndPex = x.IndAcaoPex,
                NaturezaAcaoBBId = x.BbnatIdBbNatAcao,
                NaturezaAcaoBBDesc = GetDescricaoNaturezaBBGrid(tipoProcesso, x.BbnatIdBbNatAcao, ref _manutencaoContext),
                IndRequerEscritorio = x.IndRequerEscritorio == null || x.IndRequerEscritorio == "N" ? "NÃO" : "SIM",
                AcaoCivelEstrategicoId = GetAcaoCivelEstrategicoIdGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelEstrategicoDesc = GetAcaoCivelEstrategicoDescricaoGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelConsumidorId = GetAcaoCivelConsumidorIdGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext),
                AcaoCivelConsumidorDesc = GetAcaoCivelConsumidorDescricaoGrid(tipoProcesso, x.CodAcao, ref _manutencaoContext)
            });

            if (pesquisa != null)
                query = query.Where(x => x.Descricao.ToUpper().Trim().Contains(pesquisa.ToUpper().Trim()));

            switch (tipoProcesso)
            {
                case (int)TipoProcessoEnum.JuizadoEspecial:
                    query = query.Where(x => x.IndJEC == "S");
                    break;

                case (int)TipoProcessoEnum.Procon:
                    query = query.Where(x => x.IndProcon == "S");
                    break;

                case (int)TipoProcessoEnum.CivelConsumidor:
                    query = query.Where(x => x.IndCivelConsumidor == "S");
                    break;

                case (int)TipoProcessoEnum.CivelEstrategico:
                    query = query.Where(x => x.IndCivelEstrategico == "S");
                    break;

                case (int)TipoProcessoEnum.CriminalJudicial:
                    query = query.Where(x => x.IndCriminalJudicial == "S");
                    break;

                case (int)TipoProcessoEnum.Pex:
                    query = query.Where(x => x.IndPex == "S");
                    break;

                case (int)TipoProcessoEnum.Trabalhista:
                    query = query.Where(x => x.IndTrabalhista == "S");
                    break;

                case (int)TipoProcessoEnum.TributarioJudicial:
                    query = query.Where(x => x.IndTributarioJudicial == "S");
                    break;

                default:
                    break;
            }

            var data = await query.ToListAsync(ct); ;

            switch (coluna)
            {
                case "naturezaBB":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.NaturezaAcaoBBDesc).ToList() : data.OrderByDescending(x => x.NaturezaAcaoBBDesc).ToList();
                    }
                    break;

                case "acaoCivelEstrategicoDesc":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.AcaoCivelEstrategicoDesc).ToList() : data.OrderByDescending(x => x.AcaoCivelEstrategicoDesc).ToList();
                    }
                    break;

                case "acaoCivelConsumidorDesc":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.AcaoCivelConsumidorDesc).ToList() : data.OrderByDescending(x => x.AcaoCivelConsumidorDesc).ToList();
                    }
                    break;


                case "id":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Id).ToList() : data.OrderByDescending(x => x.Id).ToList();
                    }
                    break;

                case "descricao":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Descricao).ToList() : data.OrderByDescending(x => x.Descricao).ToList();
                    }
                    break;

                case "enviarAppPreposto":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.EnviarAppPreposto).ToList() : data.OrderByDescending(x => x.EnviarAppPreposto).ToList();
                    }
                    break;

                case "requerEscritorio":
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.IndRequerEscritorio).ToList() : data.OrderByDescending(x => x.IndRequerEscritorio).ToList();
                    }
                    break;

                default:
                    {
                        data = direcao == "asc" ? data.OrderBy(x => x.Id).ToList() : data.OrderByDescending(x => x.Id).ToList();
                    }
                    break;

            }


            StringBuilder csv = new StringBuilder();

            if (tipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial)
                csv.AppendLine($"Código;Descrição;Natureza da Ação BB;Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.Procon)
                csv.AppendLine($"Código;Descrição;Requer Preenchimento de Escritório;Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                csv.AppendLine($"Código;Descrição;Natureza da Ação BB;Correspondente Cível Estratégico(DExPARA Migração de Processo);Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                csv.AppendLine($"Código;Descrição;Ativo;Correspondente Cível Consumidor(DExPARA Migração de Processo);Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.CriminalJudicial)
                csv.AppendLine($"Código;Descrição;Ativo;Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.Pex)
                csv.AppendLine($"Código;Descrição;Natureza da Ação BB;Ativo;Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.Trabalhista)
                csv.AppendLine($"Código;Descrição;Enviar para o APP Preposto");

            else if (tipoProcesso == (int)TipoProcessoEnum.TributarioJudicial)
                csv.AppendLine($"Código;Descrição;Enviar para o APP Preposto");

            foreach (var x in data)
            {
                csv.Append($"\"{x.Id}\";");
                csv.Append($"\"{x.Descricao}\";");

                if (tipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial || tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor || tipoProcesso == (int)TipoProcessoEnum.Pex)
                    csv.Append($"\"{x.NaturezaAcaoBBDesc}\";");
                if (tipoProcesso == (int)TipoProcessoEnum.Procon)
                    csv.Append($"\"{x.IndRequerEscritorio}\";");
                if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                    csv.Append($"\"{x.AcaoCivelEstrategicoDesc}\";");
                if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                {
                    csv.Append($"\"{x.Ativo}\";");
                    csv.Append($"\"{x.AcaoCivelConsumidorDesc}\";");                    
                }

                if (tipoProcesso == (int)TipoProcessoEnum.CriminalJudicial || tipoProcesso == (int)TipoProcessoEnum.Pex)
                    csv.Append($"\"{x.Ativo}\";");

                csv.Append($"\"{x.EnviarAppPreposto}\";");
                csv.AppendLine("");
            }

            string nomeArquivo = $"ACAO_{RetornaDescricaoEnumProcesso(tipoProcesso).Replace(" ","_")}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());

            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();

            return File(bytes, "text/csv", nomeArquivo);
        }

        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] AcaoRequest dto)
        {
            try
            {
                if (VerificaAcaoExistente(dto))
                    return BadRequest("Já existe uma Ação com a descrição informada.");

                var acao = new Contextos.V2.ManutencaoContext.Entities.Acao()
                {
                    CodAcao = (short)_manutencaoContext.GetNextSequencial("ACAO"),
                    DscAcao = dto.Descricao.ToUpper().Trim(),
                    BbnatIdBbNatAcao = dto.NaturezaAcaoBB != null ? dto.NaturezaAcaoBB : (int?)null,
                    IndRequerEscritorio = PreencherRequerEscritorio(dto),
                    IndAtivo = RetornaStatusAtivo(dto.TipoProcesso, dto.Ativo),
                    IndPrincipalParalela = PreencherPrincipalParalela(dto.TipoProcesso),
                    IndAcaoJuizado = dto.TipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial ? "S" : "N",
                    IndProcon = dto.TipoProcesso == (int)TipoProcessoEnum.Procon ? "S" : "N",
                    EnviarAppPreposto = dto.EnviarAppPreposto == null || dto.EnviarAppPreposto == false ? "N" : "S",
                    IndAcaoCivel = dto.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor ? "S" : "N",
                    IndCivelEstrategico = dto.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico ? "S" : "N",
                    IndAcaoTrabalhista = dto.TipoProcesso == (int)TipoProcessoEnum.Trabalhista ? "S" : "N",
                    IndAcaoTributaria = dto.TipoProcesso == (int)TipoProcessoEnum.TributarioJudicial ? "S" : "N",
                    IndCriminalJudicial = dto.TipoProcesso == (int)TipoProcessoEnum.CriminalJudicial ? "S" : "N",
                    IndAcaoPex = dto.TipoProcesso == (int)TipoProcessoEnum.Pex ? "S" : "N",
                };

                if (dto.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                {
                    if (dto.AcoesCivelEstrategico != null)
                    {
                        var migAcao = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                        {
                            CodAcaoCivel = acao.CodAcao,
                            CodAcaoCivelEstrat = (short)dto.AcoesCivelEstrategico.Value
                        };

                        _manutencaoContext.MigAcao.Add(migAcao);
                    }
                }

                if (dto.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                {
                    if (dto.AcoesCivelConsumidor != null)
                    {
                        var migAcao = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                        {
                            CodAcaoCivel = (short)dto.AcoesCivelConsumidor.Value,
                            CodAcaoCivelEstrat = acao.CodAcao
                        };

                        _manutencaoContext.MigAcao.Add(migAcao);
                    }
                }

                _manutencaoContext.Acao.Add(acao);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut, Route("Alterar")]
        public async Task<IActionResult> AlterarAsync(CancellationToken ct, [FromBody] AcaoRequest dto)
        {
            try
            {
                if (VerificaAcaoExistente(dto))
                    return BadRequest("Já existe uma Ação com a descrição informada.");

                var acao = _manutencaoContext.Acao.Where(x => x.CodAcao == dto.Id).First();

                acao.DscAcao = dto.Descricao.ToUpper().Trim();
                acao.EnviarAppPreposto = dto.EnviarAppPreposto == null || dto.EnviarAppPreposto == false ? "N" : "S";

                if (dto.TipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial || dto.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                    acao.BbnatIdBbNatAcao = dto.NaturezaAcaoBB;

                if (dto.TipoProcesso == (int)TipoProcessoEnum.Procon)
                    acao.IndRequerEscritorio = dto.RequerEscritorio == null || dto.RequerEscritorio == false ? "N" : "S";

                if (dto.TipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                    AtualizaDadosAcaoCivelConsumidor(dto, acao);

                if (dto.TipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                {
                    acao.IndAtivo = dto.Ativo == "true" ? "S" : "N";
                    AtualizaDadosAcaoCivelEstrategico(dto, acao);
                }

                if (dto.TipoProcesso == (int)TipoProcessoEnum.CriminalJudicial || dto.TipoProcesso == (int)TipoProcessoEnum.Pex)
                    acao.IndAtivo = dto.Ativo == "true" ? "S" : "N";

                _manutencaoContext.Acao.Update(acao);

                await _manutencaoContext.SaveChangesAsync(ct);

                return Ok("Alterado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("ObterAcoesCivelEstrategico")]
        public IActionResult ObterAcoesCivelEstrategico()
        {
            var acaoCivelEstrategico = _manutencaoContext.Acao.Where(x => x.IndCivelEstrategico == "S").AsNoTracking().OrderBy(x => x.DscAcao).Select(x => new AcaoCivelEstrategicoResponse
            {
                Id = x.CodAcao,
                Descricao = x.IndAtivo == "S" ? x.DscAcao : x.DscAcao + " [INATIVO]"
            });

            return Ok(new { acaoCivelEstrategico });
        }

        [HttpGet("ObterAcoesCivelConsumidor")]
        public IActionResult ObterAcoesCivelConsumidor()
        {
            var acaoCivelConsumidor = _manutencaoContext.Acao.Where(x => x.IndAcaoCivel == "S").AsNoTracking().OrderBy(x => x.DscAcao).Select(x => new AcaoCivelConsumidorResponse
            {
                Id = x.CodAcao,
                Descricao = x.IndAtivo == "S" ? x.DscAcao : x.DscAcao + " [INATIVO]"
            });

            return Ok(new { acaoCivelConsumidor });
        }

        #endregion Metodos Http

        #region Metodos Auxiliares 

       

        private bool VerificaAcaoExistente(AcaoRequest dto)
        {
            var verificaAcaoExistente = _manutencaoContext.Acao.Where(x => x.DscAcao.ToUpper().Trim() == dto.Descricao.ToUpper().Trim());

            if (dto.Id > 0)
                verificaAcaoExistente = verificaAcaoExistente.Where(x => x.CodAcao != dto.Id);
            var existe = false;

            switch (dto.TipoProcesso)
            {
                case (int)TipoProcessoEnum.JuizadoEspecial:
                    existe = verificaAcaoExistente.Any(x => x.IndAcaoJuizado == "S");
                    break;

                case (int)TipoProcessoEnum.Procon:
                    existe = verificaAcaoExistente.Any(x => x.IndProcon == "S");
                    break;

                case (int)TipoProcessoEnum.CivelConsumidor:
                    existe = verificaAcaoExistente.Any(x => x.IndAcaoCivel == "S");
                    break;

                case (int)TipoProcessoEnum.CivelEstrategico:
                    existe = verificaAcaoExistente.Any(x => x.IndCivelEstrategico == "S");
                    break;

                case (int)TipoProcessoEnum.CriminalJudicial:
                    existe = verificaAcaoExistente.Any(x => x.IndCriminalJudicial == "S");
                    break;

                case (int)TipoProcessoEnum.Pex:
                    existe = verificaAcaoExistente.Any(x => x.IndAcaoPex == "S");
                    break;

                case (int)TipoProcessoEnum.Trabalhista:
                    existe = verificaAcaoExistente.Any(x => x.IndAcaoTrabalhista == "S");
                    break;

                case (int)TipoProcessoEnum.TributarioJudicial:
                    existe = verificaAcaoExistente.Any(x => x.IndAcaoTributaria == "S");
                    break;

                default:
                    break;
            }

            return existe;

        }

        private static string GetDescricaoNaturezaBBGrid(int tipoProcesso, decimal? bbnatIdBbNatAcao, ref ManutencaoDbContext _context)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial || tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor || tipoProcesso == (int)TipoProcessoEnum.Pex)
            {
                if (bbnatIdBbNatAcao != null)
                {
                    return _context.BbNaturezasAcoes.FirstOrDefault(bb => bb.Id == bbnatIdBbNatAcao).Nome;
                }
            }

            return string.Empty;
        }

        private static decimal? GetAcaoCivelEstrategicoIdGrid(int tipoProcesso, short codAcao, ref ManutencaoDbContext _context)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
            {
                if (_context.MigAcao.Any(mig => mig.CodAcaoCivel == codAcao))
                {
                    return _context.MigAcao.FirstOrDefault(mig => mig.CodAcaoCivel == codAcao)?.CodAcaoCivelEstrat;
                }
            }

            return null;
        }

        private static string GetAcaoCivelEstrategicoDescricaoGrid(int tipoProcesso, short codAcao, ref ManutencaoDbContext _context)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
            {
                if (_context.MigAcao.Any(mig => mig.CodAcaoCivel == codAcao))
                {
                    var codAcaoCE = _context.MigAcao.FirstOrDefault(mig => mig.CodAcaoCivel == codAcao)?.CodAcaoCivelEstrat;

                    var acao = _context.Acao.FirstOrDefault(a => a.CodAcao == codAcaoCE);

                    return acao.IndAtivo == "S" ? acao.DscAcao : acao.DscAcao + " [INATIVO]";
                }
            }

            return string.Empty;
        }

        private void AtualizaDadosAcaoCivelConsumidor(AcaoRequest dto, Contextos.V2.ManutencaoContext.Entities.Acao acao)
        {
            if (dto.AcoesCivelEstrategico != null)
            {
                var migAcao = _manutencaoContext.MigAcao.FirstOrDefault(ma => ma.CodAcaoCivel == acao.CodAcao);

                if (migAcao == null)
                {
                    var migAcaoEstrategicoNova = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                    {
                        CodAcaoCivel = acao.CodAcao,
                        CodAcaoCivelEstrat = (short)dto.AcoesCivelEstrategico.Value
                    };

                    _manutencaoContext.MigAcao.Add(migAcaoEstrategicoNova);
                }
                else
                {
                    if (migAcao.CodAcaoCivelEstrat != dto.AcoesCivelEstrategico)
                    {
                        _manutencaoContext.MigAcao.Remove(migAcao);

                        var migAcaoEstrategicoNova = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                        {
                            CodAcaoCivel = acao.CodAcao,
                            CodAcaoCivelEstrat = (short)dto.AcoesCivelEstrategico.Value
                        };

                        _manutencaoContext.MigAcao.Add(migAcaoEstrategicoNova);
                    }
                }
            }
            else
            {
                var migAcaoRemovida = _manutencaoContext.MigAcao.FirstOrDefault(ma => ma.CodAcaoCivel == acao.CodAcao);

                if (migAcaoRemovida != null)
                    _manutencaoContext.MigAcao.Remove(migAcaoRemovida);
            }
        }

        private string PreencherRequerEscritorio(AcaoRequest dto)
        {
            if (dto.TipoProcesso == (int)TipoProcessoEnum.Procon)
            {
                return dto.RequerEscritorio == null || dto.RequerEscritorio == false ? "N" : "S";
            }

            return string.Empty;
        }

        private string PreencherPrincipalParalela(int tipoProcesso)
        {
            switch (tipoProcesso)
            {
                case (int)TipoProcessoEnum.JuizadoEspecial:
                    return "PL";
                case (int)TipoProcessoEnum.Procon:
                    return "PL";
                case (int)TipoProcessoEnum.CivelConsumidor:
                    return "PR";
                case (int)TipoProcessoEnum.CivelEstrategico:
                    return "PR";
                case (int)TipoProcessoEnum.Trabalhista:
                    return "PR";
                case (int)TipoProcessoEnum.TributarioJudicial:
                    return "PR";
                case (int)TipoProcessoEnum.CriminalJudicial:
                    return "PR";
                case (int)TipoProcessoEnum.Pex:
                    return "PR";
                default:
                    return "PR";
            }
        }

        private static decimal? GetAcaoCivelConsumidorIdGrid(int tipoProcesso, short codAcao, ref ManutencaoDbContext _context)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
            {
                if (_context.MigAcao.Where(mig => mig.CodAcaoCivelEstrat == codAcao).Any())
                {
                    return _context.MigAcao.FirstOrDefault(mig => mig.CodAcaoCivelEstrat == codAcao).CodAcaoCivel;
                }
            }

            return null;
        }

        private static string GetAcaoCivelConsumidorDescricaoGrid(int tipoProcesso, short codAcao, ref ManutencaoDbContext _context)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
            {
                if (_context.MigAcao.Where(mig => mig.CodAcaoCivelEstrat == codAcao).Any())
                {
                    var codAcaoCC = _context.MigAcao.FirstOrDefault(mig => mig.CodAcaoCivelEstrat == codAcao).CodAcaoCivel;

                    var acao = _context.Acao.FirstOrDefault(a => a.CodAcao == codAcaoCC);

                    return acao.IndAtivo == "S" ? acao.DscAcao : acao.DscAcao + " [INATIVO]";
                }
            }

            return string.Empty;
        }

        private void AtualizaDadosAcaoCivelEstrategico(AcaoRequest dto, Contextos.V2.ManutencaoContext.Entities.Acao acao)
        {
            if (dto.AcoesCivelConsumidor != null)
            {
                var migAcao = _manutencaoContext.MigAcao.FirstOrDefault(ma => ma.CodAcaoCivelEstrat == acao.CodAcao);

                if (migAcao == null)
                {
                    var migAcaoNova = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                    {
                        CodAcaoCivel = (short)dto.AcoesCivelConsumidor.Value,
                        CodAcaoCivelEstrat = acao.CodAcao
                    };

                    _manutencaoContext.MigAcao.Add(migAcaoNova);
                }
                else
                {
                    if (migAcao.CodAcaoCivel != dto.AcoesCivelConsumidor)
                    {
                        _manutencaoContext.MigAcao.Remove(migAcao);

                        var migAcaoNova = new Contextos.V2.ManutencaoContext.Entities.MigAcao()
                        {
                            CodAcaoCivel = (short)dto.AcoesCivelConsumidor.Value,
                            CodAcaoCivelEstrat = acao.CodAcao
                        };

                        _manutencaoContext.MigAcao.Add(migAcaoNova);
                    }
                }
            }
            else
            {
                var migAcaoRemovida = _manutencaoContext.MigAcao.FirstOrDefault(ma => ma.CodAcaoCivelEstrat == acao.CodAcao);

                if (migAcaoRemovida != null)
                    _manutencaoContext.MigAcao.Remove(migAcaoRemovida);
            }
        }

        private string RetornaStatusAtivo(int tipoProcesso, string ativo)
        {
            if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico || tipoProcesso == (int)TipoProcessoEnum.CriminalJudicial || tipoProcesso == (int)TipoProcessoEnum.Pex)
                return ativo == "true" ? "S" : "N";
            else
                return "S";                    
        }

        private string RetornaDescricaoEnumProcesso(int tipoProcesso)
        {
            string ret = string.Empty;

            if (tipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial)
                ret = TipoProcessoEnum.JuizadoEspecial.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.Procon)
                ret = TipoProcessoEnum.Procon.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.CivelConsumidor)
                ret = TipoProcessoEnum.CivelConsumidor.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.CivelEstrategico)
                ret = TipoProcessoEnum.CivelEstrategico.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.CriminalJudicial)
                ret = TipoProcessoEnum.CriminalJudicial.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.Pex)
                ret = TipoProcessoEnum.Pex.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.Trabalhista)
                ret = TipoProcessoEnum.Trabalhista.Descricao();

            else if (tipoProcesso == (int)TipoProcessoEnum.TributarioJudicial)
                ret = TipoProcessoEnum.TributarioJudicial.Descricao();

            return RetiraAcentos(ret);
        }

        private String RetiraAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        #endregion Metodos Auxiliares 
    }
}
