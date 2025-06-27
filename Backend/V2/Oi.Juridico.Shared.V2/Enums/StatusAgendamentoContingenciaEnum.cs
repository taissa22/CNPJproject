using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum StatusAgendamentoContingenciaEnum
    {
        [Description("Na Fila")]
        NaFila = 0,
        [Description("Processando")]
        Processando = 1,
        [Description("Encerrado")]
        Processado = 2,
        [Description("Erro")]
        Erro = 3,
        [Description("Cancelado")]
        Cancelado = 4,
        [Description("Verificando duplicidade")]
        VerificandoDuplicidade = 5,
        [Description("Recuperando Informacoes")]
        RecuperandoInformacoes = 6
    }
}
