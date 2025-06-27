using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Extensions
{
    public static class EnumHelpers
    {
        public static T ParseOrDefault<T>(string value, T defaultValue) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out T result))
            {
                return result;
            }
            return defaultValue;
        }
    }

    public class PaginatedQueryResult<T>
    {
        public IReadOnlyCollection<T> Data { get; set; }
        public int Total { get; set; }
    }

    public class Pagination
    {
        public static int PagesToSkip(int quantidade, int total, int pagina)
        {
           var quantidadePaginas = Math.Ceiling((decimal)total / quantidade);

           var paginasParaPular = quantidadePaginas >= pagina ? pagina - 1 : (int)quantidadePaginas == 0 ? (int)quantidadePaginas : (int)quantidadePaginas - 1;

           return paginasParaPular * quantidade;
        }
    }

    public static class Logs
    {
        public static string ValidanoPermissao( string usuario) =>
           string.Format("Validando se o usuário '{0}' possui permissão.'", usuario);


        public static string PermissaoNegada(string permissao, string usuario) =>
            string.Format("Permissão '{0}' negada para '{1}'", permissao, usuario);

        public static string IniciandoOperacao(string operacao) =>
            string.Format("Iniciando '{0}'", operacao);

        public static string OperacaoFinalizada(string operacao) =>
            string.Format("Operação '{0}' Finalizada", operacao);

        public static string OperacaoComErro(string operacao) =>
            string.Format("Operação '{0}' com Erro", operacao);

        public static string Obtendo(string entidade) =>
            string.Format("Obtendo '{0}'", entidade);

        public static string Retornando(string entidade) =>
            string.Format("Retornando '{0}'", entidade);

        public static string ComandoInvalido(string command) =>
            string.Format("Comando inválido '{0}'", command);

        public static string ObtendoEntidade(string entity, int id) =>
            ObtendoEntidade(entity, id.ToString());

        public static string ObtendoEntidade(string entity, string id) =>
            string.Format("Obtendo entidade '{0}:{1}'", entity, id);

        public static string EntidadeNaoEncontrada(string entity, int id) =>
            EntidadeNaoEncontrada(entity, id.ToString());

        public static string EntidadeNaoEncontrada(string entity, string id) =>
            string.Format("Entidade '{0}:{1}' não encontrada", entity, id);

        public static string CriandoEntidade(string entity) =>
            string.Format("Criando entidade '{0}'", entity);

        public static string AtualizandoEntidade(string entity, int id) =>
            AtualizandoEntidade(entity, id.ToString());

        public static string AtualizandoEntidade(string entity, string id) =>
            string.Format("Atualizando entidade '{0}:{1}'", entity, id);

        public static string RemovendoEntidade(string entity, int id) =>
            RemovendoEntidade(entity, id.ToString());

        public static string RemovendoEntidade(string entity, string id) =>
            string.Format("Removendo entidade '{0}:{1}'", entity, id);

        public static string EntidadeInvalida(string entity) =>
            string.Format("Entidade inválida '{0}'", entity);

        public static string SalvandoEntidade(string entity) =>
            string.Format("Salvando entidade '{0}'", entity);

        public static string ValidandoDuplicidades() =>
            string.Format("Validando duplicidades");

        public static string ValidandoRegras() =>
            string.Format("Validando regras");
    }

}
