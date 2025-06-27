using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Acoes
{
   public class AcoesEstrategicoCommandResult
    {
        public AcoesEstrategicoCommandResult(int id, string descricao, bool ativo, bool ativoDePara, bool ehCivelEstrategico, bool ehCivelConsumidor, bool ehTrabalhista, bool ehTributaria, int? idMigracao, string descricaoMigracao)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            AtivoDePara = ativoDePara;
            EhCivelEstrategico = ehCivelEstrategico;
            EhCivelConsumidor = ehCivelConsumidor;
            EhTrabalhista = ehTrabalhista;
            EhTributaria = ehTributaria;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public bool AtivoDePara { get; private set; }        
        public bool EhCivelEstrategico { get; private set; }
        public bool EhCivelConsumidor { get; private set; }
        public bool EhTrabalhista { get; private set; }
        public bool EhTributaria { get; private set; }
        public int? IdMigracao { get; private set; }
        public string DescricaoMigracao { get; private set; }
    }
}
