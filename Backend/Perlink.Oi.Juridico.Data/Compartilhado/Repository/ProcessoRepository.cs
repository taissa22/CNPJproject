using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class ProcessoRepository : BaseCrudRepository<Processo, long>, IProcessoRepository
    {
        private readonly JuridicoContext dbContext;
        public ProcessoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public string RecuperarNumeroProcessoCartorio(long ID)
        {
            var result = dbContext.Processos
                                   .Where(processo => processo.Id.Equals(ID))
                                   .AsNoTracking()
                                   .Select(nr => nr.NumeroProcessoCartorio).FirstOrDefault();
            return result;

        }

        public async Task<ICollection<Processo>> RecuperarPorIdentificador(string numeroProcesso, long codigoProcesso, long codigoTipoProcesso)
        {
            ICollection<Processo> processos = new Collection<Processo>();
            IQueryable<Processo> query = context.Set<Processo>()
                .Where(p => p.CodigoTipoProcesso == codigoTipoProcesso);

            if (!string.IsNullOrEmpty(numeroProcesso))
            {
                query = query.Where(p => p.NumeroProcessoCartorio == numeroProcesso);
            }
            else if (codigoProcesso > 0)
            {
                query = query.Where(p => p.Id == codigoProcesso);
            }

            if (!string.IsNullOrEmpty(numeroProcesso) || codigoProcesso > 0)
            {
                processos = await query.Select(p => new Processo()
                {
                    NumeroProcessoCartorio = p.NumeroProcessoCartorio,
                    Id = p.Id,
                    Comarca = p.Comarca,
                    CodigoVara = p.CodigoVara,
                    TipoVara = p.TipoVara,
                    CodigoClassificacaoProcesso = ConsultarClassificaoProcesso(p.CodigoClassificacaoProcesso),
                    CodigoParteEmpresa = p.CodigoParteEmpresa,
                    ResponsavelInterno = p.Usuario.Nome,
                    Parte = p.Parte,
                    Profissional = p.Profissional
                }).ToListAsync();
            }

            return processos;
        }
       private string ConsultarClassificaoProcesso ( string codigoClassificacaoProcesso )
        {

            switch (codigoClassificacaoProcesso.ToEnum(ClassificacaoHierarquicaEnum.U)) {
                case ClassificacaoHierarquicaEnum.P:
                    return ClassificacaoHierarquicaEnum.P.Descricao();
                case ClassificacaoHierarquicaEnum.S:
                    return ClassificacaoHierarquicaEnum.S.Descricao();
                default:
                    return ClassificacaoHierarquicaEnum.U.Descricao();
            }

        }
        public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso)
        {
            List<ProcessoDTO> processos = new List<ProcessoDTO>();
            IQueryable<Processo> query = dbContext.Processos
                                  .Where(processo => processo.NumeroProcessoCartorio.Equals(numeroProcesso) &&
                                                     processo.CodigoTipoProcesso.Equals(codigoTipoProcesso))
                                  .AsNoTracking();
           

            if (codigoTipoProcesso == TipoProcessoEnum.TributarioAdministrativo.GetHashCode()) {
                processos =  await query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcessoCartorio.ToString(),
                    Estado = "",
                    Comarca = "",
                    Vara = "",
                    TipoVara = "",
                    EmpresaGrupo = processo.Parte.Nome
                }).ToListAsync();
            } else {
                processos = await query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcessoCartorio.ToString(),
                    Estado = processo.Parte.SiglaEstado,
                    Comarca = processo.Comarca.Nome,
                    Vara = processo.Vara.CodigoVara.ToString(),
                    TipoVara = processo.TipoVara.NomeTipoVara,
                    EmpresaGrupo = processo.Parte.Nome
                }).ToListAsync();
               
            }
            return processos;
        }

        public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, IList<long> codigosEscritorio)
        {
            var processos = new List<ProcessoDTO>();
            var valuesEscritorio = string.Join(",", codigosEscritorio);

            string sql = @"SELECT processo.COD_PROCESSO as Id, 
                                   processo.NRO_PROCESSO_CARTORIO as NumeroProcesso, 
                                   Parte.COD_ESTADO as Estado, 
                                   Comarca.NOM_COMARCA as Comarca, 
                                   CAST(Vara.COD_VARA AS VARCHAR2(20)) as Vara, 
                                   TipoVara.NOM_TIPO_VARA as TipoVara, 
                                   Parte.NOM_PARTE as EmpresaGrupo 
                            FROM JUR.PROCESSO processo
                            INNER JOIN JUR.TIPO_VARA TipoVara ON processo.COD_TIPO_VARA = TipoVara.COD_TIPO_VARA
                            INNER JOIN JUR.VARA Vara ON processo.COD_COMARCA = Vara.COD_COMARCA AND processo.COD_VARA = Vara.COD_VARA AND processo.COD_TIPO_VARA = Vara.COD_TIPO_VARA
                            INNER JOIN JUR.COMARCA Comarca ON processo.COD_COMARCA = Comarca.COD_COMARCA
                            INNER JOIN JUR.PARTE Parte ON processo.COD_PARTE_EMPRESA = Parte.COD_PARTE
                            WHERE processo.NRO_PROCESSO_CARTORIO = '" + numeroProcesso + @"'
                            AND processo.COD_TIPO_PROCESSO = " + codigoTipoProcesso + @"
                            AND (processo.COD_PROFISSIONAL IN (" + valuesEscritorio + @") or processo.COD_ESCRITORIO_ACOMPANHANTE IN (" + valuesEscritorio + @"))";

            var query = await this.context.Query<ProcessoDTO>().FromSql(sql).ToListAsync();

            if (codigoTipoProcesso == TipoProcessoEnum.TributarioAdministrativo.GetHashCode())
            {
                processos = query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcesso.ToString(),
                    Estado = "",
                    Comarca = "",
                    Vara = "",
                    TipoVara = "",
                    EmpresaGrupo = processo.EmpresaGrupo.ToString()
                }).ToList();
            }
            else
            {
                processos = query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcesso.ToString(),
                    Estado = processo.Estado,
                    Comarca = processo.Comarca,
                    Vara = processo.Vara.ToString(),
                    TipoVara = processo.TipoVara,
                    EmpresaGrupo = processo.EmpresaGrupo.ToString()
                }).ToList();
            }

            return processos;
        }

        public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso)
        {
            List<ProcessoDTO> processos = new List<ProcessoDTO>();
            IQueryable<Processo> query = dbContext.Processos
                                  .Where(processo => processo.Id.Equals(codigoProcesso) &&
                                                     processo.CodigoTipoProcesso.Equals(codigoTipoProcesso))
                                  .AsNoTracking();


            if (codigoTipoProcesso == TipoProcessoEnum.TributarioAdministrativo.GetHashCode())
            {
                processos = await query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcessoCartorio.ToString(),
                    Estado = "",
                    Comarca = "",
                    Vara = "",
                    TipoVara = "",
                    EmpresaGrupo = processo.Parte.Nome
                }).ToListAsync();
            }
            else
            {
                processos = await query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcessoCartorio.ToString(),
                    Estado = processo.Parte.SiglaEstado,
                    Comarca = processo.Comarca.Nome,
                    Vara = processo.Vara.CodigoVara.ToString(),
                    TipoVara = processo.TipoVara.NomeTipoVara,
                    EmpresaGrupo = processo.Parte.Nome
                }).ToListAsync();

            }
            return processos;
        }

        public async Task<IEnumerable<ProcessoExportacaoPrePosRJDTO>> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, int qtdeMesesParaExportacao)
        {
            var resultado = await (from processos in context.Set<Processo>().AsNoTracking()
                                   where processos.CodigoTipoProcesso == (long)tipoProcesso
                                   && processos.DataCadastro < DateTime.Now.Date
                                   && (processos.DataFinalizacaoContabil == null || processos.DataFinalizacaoContabil > DateTime.Now.Date.AddMonths(qtdeMesesParaExportacao * -1))
                                   select new ProcessoExportacaoPrePosRJDTO()
                                   {
                                       Id = processos.Id,
                                       NumeroProcessoCartorio = processos.NumeroProcessoCartorio + "\t",
                                       DataFinalizacao = processos.DataFinalizacao,
                                       DataFinalizacaoContabil = processos.DataFinalizacaoContabil,
                                       PrePos = processos.PrePos == 0 ? "A DEFINIR" : processos.PrePos == 1 ? "PRÉ RJ" : "PÓS RJ",
                                       ConsiderarProvisao = processos.IndicadorConsiderarProvisao == true ? "SIM" : "NÃO"
                                   })
                                   .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<ExportacaoPrePosRJ>> ExpurgoPrePosRj(int parametro)
        {
            var resultado = await context.Set<ExportacaoPrePosRJ>()
                                .Where(a => Convert.ToDateTime(a.DataExecucao).Date <= DateTime.Now.Date.AddDays(parametro * -1) &&
                                a.NaoExpurgar == false)
                               .AsNoTracking()
                               .ToListAsync();
            return resultado;
        }
       
        public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso, IList<long> codigosEscritorio)
        {
            var processos = new List<ProcessoDTO>();
            var valuesEscritorio = string.Join(",", codigosEscritorio);

            string sql = @"SELECT processo.COD_PROCESSO as Id, 
                                  processo.NRO_PROCESSO_CARTORIO as NumeroProcesso, 
                                  Parte.COD_ESTADO Estado, 
                                  Comarca.NOM_COMARCA Comarca, 
                                  CAST(Vara.COD_VARA AS VARCHAR2(20)) Vara, 
                                  TipoVara.NOM_TIPO_VARA TipoVara, 
                                  Parte.NOM_PARTE EmpresaGrupo
                            FROM JUR.PROCESSO processo
                            INNER JOIN JUR.TIPO_VARA TipoVara ON processo.COD_TIPO_VARA = TipoVara.COD_TIPO_VARA
                            INNER JOIN JUR.VARA Vara ON processo.COD_COMARCA = Vara.COD_COMARCA AND processo.COD_VARA = Vara.COD_VARA AND processo.COD_TIPO_VARA = Vara.COD_TIPO_VARA
                            INNER JOIN JUR.COMARCA Comarca ON processo.COD_COMARCA = Comarca.COD_COMARCA
                            INNER JOIN JUR.PARTE Parte ON processo.COD_PARTE_EMPRESA = Parte.COD_PARTE
                            WHERE processo.COD_PROCESSO = " + codigoProcesso + @" 
                            AND processo.COD_TIPO_PROCESSO = " + codigoTipoProcesso + @"
                            AND (processo.COD_PROFISSIONAL IN (" + valuesEscritorio + @") or processo.COD_ESCRITORIO_ACOMPANHANTE IN (" + valuesEscritorio + @"))";

            var query = await this.context.Query<ProcessoDTO>().FromSql(sql).ToListAsync();

            if (codigoTipoProcesso == TipoProcessoEnum.TributarioAdministrativo.GetHashCode())
            {
                processos = query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcesso.ToString(),
                    Estado = "",
                    Comarca = "",
                    Vara = "",
                    TipoVara = "",
                    EmpresaGrupo = processo.EmpresaGrupo.ToString()
                }).ToList();
            }
            else
            {
                processos = query.Select(processo => new ProcessoDTO()
                {
                    Id = processo.Id,
                    NumeroProcesso = processo.NumeroProcesso.ToString(),
                    Estado = processo.Estado,
                    Comarca = processo.Comarca,
                    Vara = processo.Vara,
                    TipoVara = processo.TipoVara,
                    EmpresaGrupo = processo.EmpresaGrupo
                }).ToList();
            }

            return processos;
        }
    }
}