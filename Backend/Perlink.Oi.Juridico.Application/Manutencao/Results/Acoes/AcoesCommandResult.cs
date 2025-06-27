using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Acoes
{
   public class AcoesCommandResult
    {
        public AcoesCommandResult(  int id, 
                                    string descricao, 
                                    bool ativo,
                                    bool ativoDePara, 
                                    NaturezaAcaoBB naturezaAcaoBB,
                                    bool ehCivelEstrategico, 
                                    bool ehCivelConsumidor,
                                    bool ehTrabalhista,
                                    bool ehTributaria, 
                                    int? idMigracao, 
                                    string descricaoMigracao,
                                    bool? enviarAppPreposto)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            AtivoDePara = ativoDePara;
            NaturezaAcaoBB = naturezaAcaoBB;
            EhCivelEstrategico = ehCivelEstrategico;
            EhCivelConsumidor = ehCivelConsumidor;
            EhTrabalhista = ehTrabalhista;
            EhTributaria = ehTributaria;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
            EnviarAppPreposto = enviarAppPreposto;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }

        public bool AtivoDePara { get; private set; }

        public int? NaturezaAcaoBBId { get; private set; } = null;
        public NaturezaAcaoBB NaturezaAcaoBB { get; private set; } = null;

        public bool EhCivelEstrategico { get; private set; }
        public bool EhCivelConsumidor { get; private set; }
        public bool EhTrabalhista { get; private set; }
        public bool EhTributaria { get; private set; }
        public int? IdMigracao { get; private set; }
        public string DescricaoMigracao { get; private set; }
        public bool? EnviarAppPreposto { get; private set; }

    }
}
