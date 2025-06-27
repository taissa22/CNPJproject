using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Data.SAP.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Patterns.Impl.FactoryMethod.ExecutarSaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Data.Impl.FactoryMethod;
using Shared.Domain.Interface;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class SaldoGarantiaRepository : BaseCrudRepository<AgendamentoSaldoGarantia, long>, ISaldoGarantiaRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly ICriterioSaldoGarantiaRepository criterioSaldoGarantiaRepository;
        private readonly IClassesGarantiasRepository classesGarantiasRepository;
        private readonly IAuthenticatedUser user;
        private readonly IParteRepository parteRepository;
        private readonly IEstadoRepository estadoRepository;
        private readonly IProcessoRepository processoRepository;
        private readonly IBancoRepository bancoRepository;
        private readonly IProfissionalRepository profissionalRepository;
        private readonly IParametroRepository parametroRepository;

        public SaldoGarantiaRepository(JuridicoContext dbContext, IAuthenticatedUser user,
                                        ICriterioSaldoGarantiaRepository criterioSaldoGarantiaRepository,
                                        IClassesGarantiasRepository classesGarantiasRepository,
                                        IParteRepository parteRepository,
                                        IEstadoRepository estadoRepository,
                                        IProfissionalRepository profissionalRepository,
                                        IProcessoRepository processoRepository,
                                        IBancoRepository bancoRepository,
                                        IParametroRepository parametroRepository) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            this.criterioSaldoGarantiaRepository = criterioSaldoGarantiaRepository;
            this.classesGarantiasRepository = classesGarantiasRepository;
            this.user = user;
            this.parteRepository = parteRepository;
            this.profissionalRepository = profissionalRepository;
            this.estadoRepository = estadoRepository;
            this.processoRepository = processoRepository;
            this.bancoRepository = bancoRepository;
            this.parametroRepository = parametroRepository;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO)
        {
            var agendamento = new AgendamentoSaldoGarantia
            {
                //Id = await sequencialRepository.RecuperarSequencialAsync("AGENDAMENTO_SALDO_GARANTIA"),
                CodigoTipoProcesso = filtroDTO.TipoProcesso,
                CodigoStatusAgendamento = 1,
                CodigoUsuario = user.Login,
                DataAgendamento = DateTime.Now,
                NomeAgendamento = filtroDTO.NomeAgendamento
            };
            await Inserir(agendamento);
            await criterioSaldoGarantiaRepository.CriarCriteriosAsync(agendamento, filtroDTO);
            await CommitAsync();
        }



        public async Task<ICollection<SaldoGarantiaResultadoDTO>> ExecutarAgendamentoSaldoGarantia(AgendamentoSaldoGarantia agendamento)
        {
            var parametros = new List<OracleParameter>();
            StringBuilder query = new StringBuilder();
            var queryFactoryMethod = new QuerySaldoGarantiaFM().EscolherPorTipoProcesso(agendamento.CodigoTipoProcesso);
            query.AppendLine(queryFactoryMethod.RetornaSelect());
            query.AppendLine(queryFactoryMethod.RetornaFrom());
            query.AppendLine(queryFactoryMethod.RetornaJoin());
            query.AppendLine(WhereQuery(agendamento, parametros));
            query.AppendLine(queryFactoryMethod.RetornaGroupBy());
            query.AppendLine(queryFactoryMethod.RetornaOrderBy());
            query = queryFactoryMethod.CalcularSaldoQuery(query);

            var resultado = await dbContext.ExecutarQuery(query.ToString(), parametros.ToArray());
            var ResultadoDeserialize = JsonConvert.DeserializeObject<List<SaldoGarantiaResultadoDTO>>(resultado);
            if (agendamento.CodigoTipoProcesso == (long)TipoProcessoEnum.Trabalhista ||
                agendamento.CodigoTipoProcesso == (long)TipoProcessoEnum.CivelConsumidor ||
                agendamento.CodigoTipoProcesso == (long)TipoProcessoEnum.JuizadoEspecial)
                foreach (var item in ResultadoDeserialize)
                {
                    item.DescricaoEscritorio = profissionalRepository.RecuperarPorId(item.CodigoProfissional).Result.NomeProfissional;
                }

            return ResultadoDeserialize;
        }



        private string WhereQuery(AgendamentoSaldoGarantia agendamento, List<OracleParameter> oracleParameters)
        {
            StringBuilder query = new StringBuilder();
            //contrução da query where com critérios salvos
            foreach (var criterio in agendamento.CriteriosSaldoGarantias)
            {
                if (criterio.NomeCriterio.Contains("Lista") && criterio.Parametros != "*")
                {
                    //condições com lista In
                    query.AppendLine(GetQueryComIn_Oracle<string>(criterio.Criterio, criterio.Parametros, criterio.ValoresParametros.Split(",").ToList(), oracleParameters));
                }
                else
                {
                    //condições com um ou mais parâmetros
                    query.AppendLine($" and {criterio.Criterio}");
                    if (criterio.Parametros != "*") //Usando * para indicar quando deve adicionar parametro oracle
                        for (int i = 0; i <= criterio.Parametros.Split(";").Length - 1; i++)
                        {
                            oracleParameters.Add(new OracleParameter(criterio.Parametros.Split(";")[i], criterio.ValoresParametros.Split(";")[i]));
                        }
                }
            }

            return query.ToString();
        }
        private string GetQueryComIn_Oracle<Tipo>(string campo, string nomeParametro, object listaIn, List<OracleParameter> oracleParameters)
        {
            List<Tipo> lista = (List<Tipo>)listaIn;
            nomeParametro = nomeParametro.Contains(":") ? nomeParametro.Replace(":", "") : nomeParametro;

            StringBuilder query = new StringBuilder();
            if (lista != null && lista.Any())
            {
                query.Append($" and ({campo}) in (");

                foreach ((object valor, int index) in lista.Select((valor, index) => (valor, index)))
                {
                    query.Append($":{nomeParametro}_{index}");
                    query.Append(index != lista.Count() - 1 ? "," : string.Empty);
                    oracleParameters.Add(new OracleParameter($"{nomeParametro}_{index}", valor));
                }

                query.Append(")");
            }
            return query.ToString();
        }
        public async Task<ICollection<AgendamentoResultadoDTO>> ConsultarAgendamentos(OrdernacaoPaginacaoDTO filtroDTO)
        {
            var result = await dbContext.AgendamentoSaldoGarantias
                            .AsNoTracking()
                            .Where(a => a.CodigoUsuario == user.Login)
                            .Select(agendamento => new AgendamentoResultadoDTO
                            {
                                Id = agendamento.Id,
                                CodigoStatusAgendamento = agendamento.CodigoStatusAgendamento,
                                DataAgendamento = agendamento.DataAgendamento,
                                DataFinalizacao = agendamento.DataFinalizacao.HasValue ? agendamento.DataFinalizacao.Value.ToString("dd/MM/yyyy HH:mm") : "",
                                MensagemErro = agendamento.MensagemErro,
                                NomeAgendamento = agendamento.NomeAgendamento,
                                StatusAgendamento = EnumExtensions.GetDescricaoFromValue<StatusAgendamento>(agendamento.CodigoStatusAgendamento),
                                NomeArquivo = agendamento.NomeArquivoGerado,
                                TipoProcesso = agendamento.CodigoTipoProcesso
                            })
                            .OrdenarPorPropriedade(false, "DataAgendamento", "DataAgendamento")
                            .Paginar(filtroDTO.Pagina, filtroDTO.Quantidade)
                            .ToListAsync();
            return result;
        }


        public async Task<int> RecuperarTotalRegistros()
        {
            return await dbContext.AgendamentoSaldoGarantias
                             .AsNoTracking()
                             .Where(a => a.CodigoUsuario == user.Login)
                             .CountAsync();
        }
        public async Task<ICollection<KeyValuePair<string, string>>> ConsultarCriteriosPesquisa(long codigoAgendamento)
        {
            var criterios = await criterioSaldoGarantiaRepository.RecuperarPorAgendamento(codigoAgendamento);

            var dictCriterios = new Dictionary<string, string>();

            foreach (var criterio in criterios)
            {
                var valores = criterio.ValoresParametros.Split(";");

                var valor = FormatarValorCriterioPesquisa(valores, criterio.NomeCriterio);
                dictCriterios.Add(criterio.NomeCriterio[0].ToString().ToLower() + criterio.NomeCriterio.Substring(1), valor.ToString());
            }
            return dictCriterios;
        }

        private string FormatarValorCriterioPesquisa(string[] valores, string nomeCriterio)
        {
            var isBetween = valores.Length > 1;

            if (isBetween)
                return $"Entre {valores[0].FormataValor()} e {valores[1].FormataValor()}";

            // Se for um critério de lista, buscar o nome de cada valor
            string resultadoValor = valores[0];
            string[] listaValores = new string[] { };

            if (nomeCriterio.ToLower().StartsWith("lista"))
                listaValores = valores[0].Split(",");

            // Transformando valores da lista em cada valor real
            for (int i = 0; i < listaValores.Length; i++)
            {
                if (nomeCriterio == "ListaEmpresas")
                {
                    listaValores[i] = parteRepository.RecuperarPorId(Convert.ToInt64(listaValores[i])).Result.Nome;
                }
                else if (nomeCriterio == "ListaEstados")
                {
                    listaValores[i] = estadoRepository.RecuperarPorId(listaValores[i]).Result.NomeEstado;
                }
                else if (nomeCriterio == "ListaProcessos")
                {
                    listaValores[i] = processoRepository.RecuperarNumeroProcessoCartorio(long.Parse(listaValores[i]));
                }

                else if (nomeCriterio == "ListaRiscosPerdas")
                {
                    if (listaValores[i] == "PR") listaValores[i] = RiscoPerdaEnum.PR.Descricao();
                    if (listaValores[i] == "PO") listaValores[i] = RiscoPerdaEnum.PO.Descricao();
                    if (listaValores[i] == "RE") listaValores[i] = RiscoPerdaEnum.RE.Descricao();
                }
                else if (nomeCriterio == "ListaTipoGarantia")
                {
                    if (listaValores[i].Trim() == "1") listaValores[i] = TiposSaldosGarantiaEnum.Deposito.Descricao();
                    if (listaValores[i].Trim() == "2") listaValores[i] = TiposSaldosGarantiaEnum.Bloqueio.Descricao();
                    if (listaValores[i].Trim() == "3") listaValores[i] = TiposSaldosGarantiaEnum.Outros.Descricao();

                }
                else if (nomeCriterio == "ListaBancos")
                {
                    listaValores[i] = bancoRepository.RecuperarPorId(Convert.ToInt64(listaValores[i])).Result.NomeBanco;
                }
            }

            if (nomeCriterio.ToLower().StartsWith("lista"))
                resultadoValor = String.Join(", ", listaValores);

            return resultadoValor;
        }
        /// <summary>
        /// Recuperar Agendamentos e seus critérios.
        /// </summary>
        /// <param name="codigoAgendamento"></param>
        /// <returns></returns>
        public async Task<AgendamentoSaldoGarantia> RecuperarAgendamento(long codigoAgendamento)
        {
            return await dbContext.AgendamentoSaldoGarantias
                            .AsNoTracking()
                            .Include(a => a.CriteriosSaldoGarantias)
                            .Where(a => a.Id == codigoAgendamento)
                            .FirstOrDefaultAsync();
        }
    }
}