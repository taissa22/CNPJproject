using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO
{
    public class AlteracaoEmBlocoRetornoDTO
    {
        public long Id { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataExecucao { get; set; }
        public AlteracaoEmBlocoEnum? Status { get; set; }
        public string Arquivo { get; set; }
        public string CodigoDoUsuario { get; set; }
        public int? ProcessosAtualizados { get; set; }
        public int? ProcessosComErro { get; set; }
        public TipoProcessoEnum? CodigoTipoProcesso { get; set; }
        public string NomeUsuario { get; set; }
    }
}
