﻿namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501DedSuspRequestDTO
    {
        public byte? DedsuspIndtpdeducao { get; set; }
        public decimal? DedsuspVlrdedsusp { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
