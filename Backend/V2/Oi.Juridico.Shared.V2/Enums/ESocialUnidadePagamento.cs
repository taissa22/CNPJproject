using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialUnidadePagamento
    {
        [Display(Description = "Por hora")]
        PorHora = 1,
        [Display(Description = "Por dia")]
        PorDia = 2,
        [Display(Description = "Por semana")]
        PorSemana = 3,
        [Display(Description = "Por quinzena")]
        PorQuinzena = 4,
        [Display(Description = "Por mês")]
        PorMes = 5,
        [Display(Description = "Por tarefa")]
        PorTarefa = 6,
        [Display(Description = "Não aplicável - Salário exclusivamente variável")]
        NaoAplicavel = 7
    }
}
