using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto
{
    public class AssuntoCommandResult
    {
        public AssuntoCommandResult(int id, string descricao, bool ativo, bool ativoDePara, int? idMigracao, string descricaoMigracao, string proposta, string negociacao, string codTipoContingencia)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            AtivoDePara = ativoDePara;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
            Proposta = proposta;
            Negociacao = negociacao;
            CodTipoContingencia = codTipoContingencia;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }

        public bool AtivoDePara { get; private set; }

        public int? IdMigracao { get; private set; }
        public string DescricaoMigracao { get; private set; }
        public string Proposta { get; private set; } = null;

        public string Negociacao { get; private set; } = null;

        public string CodTipoContingencia { get; set; }

    }
}
