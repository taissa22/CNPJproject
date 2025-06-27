using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class CotacaoIndiceTrabalhistaRepository : ICotacaoIndiceTrabalhistaRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<ICotacaoIndiceTrabalhistaRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CotacaoIndiceTrabalhistaRepository(IDatabaseContext databaseContext, ILogger<ICotacaoIndiceTrabalhistaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<CotacaoIndiceTrabalhista> ObterBase(DateTime dataCorrecao, DateTime dataBaseDe, DateTime dataBaseAte, CotacaoIndiceTrabalhistaSort sort, bool ascending )
        {
            string logName = "Cotação Indice Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<CotacaoIndiceTrabalhista> query = DatabaseContext.CotacaoIndiceTrabalhistas.AsNoTracking();

            switch (sort)
            {
                case CotacaoIndiceTrabalhistaSort.DataCorrecao:
                    query = query.SortBy(x => x.DataCorrecao, ascending);
                    break;

                case CotacaoIndiceTrabalhistaSort.DataBase:
                    query = query.SortBy(x => x.DataBase, ascending);
                    break;

                case CotacaoIndiceTrabalhistaSort.ValorCotacao:
                    query = query.SortBy(x => x.ValorCotacao, ascending);
                    break;

                default:
                    query = query.SortBy(x => x.DataCorrecao, ascending);
                    break;
            }

            dataBaseAte = new DateTime(dataBaseAte.Year,dataBaseAte.Month,DateTime.DaysInMonth(dataBaseAte.Year, dataBaseAte.Month));
            
            return query.Where(x => x.DataBase.Year == dataCorrecao.Year &&
                                    x.DataBase.Month == dataCorrecao.Month &&
                                    x.DataCorrecao >= dataBaseDe  &&
                                    x.DataCorrecao  <= dataBaseAte);
        }

        public CommandResult<IReadOnlyCollection<CotacaoIndiceTrabalhista>> Obter(DateTime dataCorrecao, DateTime dataBaseDe, DateTime dataBaseAte, CotacaoIndiceTrabalhistaSort sort, bool ascending)
        {
            string logName = "Cotação Indice Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(dataCorrecao, dataBaseDe,dataBaseAte, sort, ascending).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<CotacaoIndiceTrabalhista>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>> ObterPaginado(DateTime dataCorrecao, DateTime dataBaseDe, DateTime dataBaseAte, CotacaoIndiceTrabalhistaSort sort, bool ascending, int pagina, int quantidade)
        {
            string logName = "Cotação Indice Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(dataCorrecao, dataBaseDe, dataBaseAte, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<CotacaoIndiceTrabalhista>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>>.Valid(resultado);
        }


        public CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>> CorrigirArquivo(List<CotacaoIndiceTrabalhista> lista)
        {
            string logName = "Cotação Indice Trabalhista";
            List<DateTime> listaDataBase = new List<DateTime>();
            listaDataBase.AddRange(lista.Select(f => f.DataBase));

           

            IQueryable<CotacaoIndiceTrabalhista> query = DatabaseContext.CotacaoIndiceTrabalhistas.AsNoTracking();
            int cotacaoDataBaseExistente = query.Count(d => listaDataBase.Contains(d.DataBase));
            if(cotacaoDataBaseExistente > 0)
            {
              
            }

            var resultado = new PaginatedQueryResult<CotacaoIndiceTrabalhista>()
            {
                //Total = total,
                //Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };
            return CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>>.Valid(resultado);
        }

        public CommandResult<string> Validar(List<CotacaoIndiceTrabalhista> lista, DataTable tab)
        {
            string logName = "Cotação Indice Trabalhista";
            List<DateTime> listaDataBase = new List<DateTime>();
            IQueryable<CotacaoIndiceTrabalhista> query = DatabaseContext.CotacaoIndiceTrabalhistas.AsNoTracking();
            listaDataBase.AddRange(lista.Select(f => f.DataBase));
           
            int cotacaoDataBaseExistente = query.Count(d => listaDataBase.Contains(d.DataBase));
            var DataBaseExistente = query.Where(d => listaDataBase.Contains(d.DataBase));

            if (tab.Columns.Count != 3)
            {
                return CommandResult<string>.Valid("Arquivo não possui a quantidade de colunas esperadas pelo sistema.");
            }
            if (cotacaoDataBaseExistente > 0)
            {
                return CommandResult<string>.Valid("Já existe uma cotação cadastrada para o período.");
            }
            if(lista.Count( d => d.DataBase > DateTime.Now) > 0)
            {
                return CommandResult<string>.Valid("Não é possível carregar uma cotação com data maior que hoje.");
            }
            if (lista.Count(d => d.ValorCotacao <= 0) > 0)
            {
                return CommandResult<string>.Valid("Não é possível carregar uma cotação com valor menor ou igual a 0.");
            }

            return CommandResult<string>.Valid("");
        }

        public CommandResult<PaginatedQueryResult<TempCotacaoIndiceTrab>> ObterPaginadoTemp(CotacaoIndiceTrabalhistaSort sort, bool ascending, int pagina, int quantidade)
        {
            string logName = "Cotação Indice Trabalhista";

            var listaBase = DatabaseContext.TempCotacaoIndiceTrabalhistas.AsNoTracking();

            switch (sort)
            {
                case CotacaoIndiceTrabalhistaSort.DataCorrecao:
                    listaBase = listaBase.SortBy(x => x.DataCorrecao, ascending);
                    break;

                case CotacaoIndiceTrabalhistaSort.DataBase:
                    listaBase = listaBase.SortBy(x => x.DataBase, ascending);
                    break;

                case CotacaoIndiceTrabalhistaSort.ValorCotacao:
                    listaBase = listaBase.SortBy(x => x.ValorCotacao, ascending);
                    break;

                default:
                    listaBase = listaBase.SortBy(x => x.DataCorrecao, ascending);
                    break;
            }

            var total = listaBase.Count();

            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TempCotacaoIndiceTrab>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TempCotacaoIndiceTrab>>.Valid(resultado);
        }

        public CommandResult<string> Validar(IFormFile arquivo, DateTime dataCorrecao, ref List<CotacaoIndiceTrabalhista> listaCotacao)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                {
                    return CommandResult<string>.Valid("Arquivo esta vazio.");
                }

                using (var reader = new StreamReader(arquivo.OpenReadStream()))
                {
                    var teste = reader.ReadToEnd();

                    DataTable tab = csvToDataTable(teste, ';');

                    if(tab.Columns.Count < 3)
                    {
                        return CommandResult<string>.Valid("Arquivo não possui a quantidade de colunas esperadas pelo sistema.");
                    }

                    var lista = AjustarLista(tab, dataCorrecao);

                    listaCotacao = lista;

                    return Validar(lista, tab);
                }
            }
            catch (Exception erro)
            {
                if (erro.Message.Contains("was not recognized as a valid DateTime"))
                {
                    return CommandResult<string>.Valid("Ano e mês informados devem ser válidos.");
                }
                if (erro.Message.Contains("was not in a correct format"))
                {
                    return CommandResult<string>.Valid("O valor da cotação contem mais de 9 casas decimais.");
                }

                return CommandResult<string>.Valid(erro.Message);
            }
        }


        #region METODOS
        public List<CotacaoIndiceTrabalhista> AjustarLista(DataTable dt, DateTime dataAnoMes)
        {
            List<CotacaoIndiceTrabalhista> lst = new List<CotacaoIndiceTrabalhista>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(CotacaoIndiceTrabalhista.Criar(Convert.ToDateTime(String.Format("{0}/{1}/{2}", "1", dt.Rows[i][1].ToString().PadLeft(2, '0'), dt.Rows[i][0])), Convert.ToDateTime(String.Format("{0}/{1}/{2}", "1", dataAnoMes.Month, dataAnoMes.Year)), Convert.ToDecimal(string.IsNullOrEmpty(dt.Rows[i][2].ToString()) ? Convert.ToDecimal("1,000000000") : dt.Rows[i][2])));

            }


            return lst;
        }
        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        private DataTable csvToDataTable(string fileName, char splitCharacter)
        {
            StreamReader sr = new StreamReader(GenerateStreamFromString(fileName));
            string myStringRow = sr.ReadLine();
            var rows = myStringRow.Split(splitCharacter);
            DataTable CsvData = new DataTable();
            foreach (string column in rows)
            {
                //creates the columns of new datatable based on first row of csv
                CsvData.Columns.Add(column);
            }
            myStringRow = sr.ReadLine();
            while (myStringRow != null)
            {
                //runs until string reader returns null and adds rows to dt 
                rows = myStringRow.Split(splitCharacter);
                CsvData.Rows.Add(rows);
                myStringRow = sr.ReadLine();
            }
            sr.Close();
            sr.Dispose();
            return CsvData;
        }

        #endregion
    }
}