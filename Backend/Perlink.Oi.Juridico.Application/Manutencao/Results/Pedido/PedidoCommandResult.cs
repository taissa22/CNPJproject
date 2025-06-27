using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Pedido
{
    public class PedidoCommandResult
    {
        public PedidoCommandResult(int id, int? idMigracao, string descricao, bool ativo, string descricaoPara, bool ativoPara)
        {
            Id = id;
            Descricao = descricao;
            IdMigracao = idMigracao;
            Ativo = ativo;
            DescricaoPara = descricaoPara;
            AtivoPara = ativoPara;
        }

        public int Id { get; private set; }

        public int? IdMigracao { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }

        public string DescricaoPara { get; private set; }
        public bool AtivoPara { get; private set; }
    }
}
