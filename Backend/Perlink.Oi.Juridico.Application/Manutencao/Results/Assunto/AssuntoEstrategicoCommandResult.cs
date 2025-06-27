using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto
{
    public class AssuntoEstrategicoCommandResult
    {
        public AssuntoEstrategicoCommandResult(int id, string descricao, bool ativo, bool ativoDePara, int? idMigracao, string descricaoMigracao)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            AtivoDePara = ativoDePara;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }
        public bool AtivoDePara { get; private set; }

        public int? IdMigracao { get; private set; }

        public string DescricaoMigracao { get; private set; }
    }
}
