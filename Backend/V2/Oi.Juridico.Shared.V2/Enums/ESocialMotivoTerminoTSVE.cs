using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialMotivoTerminoTSVE
    {
        [Display(Description = "EXONERAÇÃO DO DIRETOR NÃO EMPREGADO SEM JUSTA CAUSA, POR DELIBERAÇÃO DA ASSEMBLEIA, DOS SÓCIOS COTISTAS OU DA AUTORIDADE COMPETENTE")]
        DiretorExoneracao = 1,
        [Display(Description = "TÉRMINO DE MANDATO DO DIRETOR NÃO EMPREGADO QUE NÃO TENHA SIDO RECONDUZIDO AO CARGO")]
        DiretorTerminoMantado = 2,
        [Display(Description = "EXONERAÇÃO A PEDIDO DE DIRETOR NÃO EMPREGADO")]
        DiretorExoneracaoAPedido = 3,
        [Display(Description = "EXONERAÇÃO DO DIRETOR NÃO EMPREGADO POR CULPA RECÍPROCA OU FORÇA MAIOR")]
        DiretorExoneracaoCulpaReciproca = 4,
        [Display(Description = "MORTE DO DIRETOR NÃO EMPREGADO")]
        DiretorMorte = 5,
        [Display(Description = "EXONERAÇÃO DO DIRETOR NÃO EMPREGADO POR FALÊNCIA, ENCERRAMENTO OU SUPRESSÃO DE PARTE DA EMPRESA")]
        DiretorExoneracaoFalencia = 6,
        [Display(Description = "OUTROS")]
        Outros = 99
    }
}
