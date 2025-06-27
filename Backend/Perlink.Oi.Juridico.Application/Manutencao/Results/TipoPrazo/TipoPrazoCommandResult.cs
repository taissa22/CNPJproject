using Perlink.Oi.Juridico.Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo
{
    public class TipoPrazoCommandResult
    {
        public TipoPrazoCommandResult(int id, string descricao, bool ativo, bool? eh_Civel_Consumidor, bool? eh_Trabalhista, bool? eh_Tributario_Administrativo, bool? eh_Tributario_Judicial, bool? eh_Juizado_Especial, bool eh_Civel_Estrategico, bool eh_Administrativo, bool eh_Criminal_Judicial, bool eh_Procon, bool eh_Pex_Juizado, bool eh_Pex_Consumidor, bool eh_Criminal_Administrativo, bool eh_Documento, bool? eh_Servico, bool? ativoDePara, int? idMigracao, string descricaoMigracao)
        {
            Id = id;
            Descricao = descricao;
            Ativo = ativo;
            Eh_Civel_Consumidor = eh_Civel_Consumidor;
            Eh_Trabalhista = eh_Trabalhista;
            Eh_Tributario_Administrativo = eh_Tributario_Administrativo;
            Eh_Tributario_Judicial = eh_Tributario_Judicial;
            Eh_Juizado_Especial = eh_Juizado_Especial;
            Eh_Civel_Estrategico = eh_Civel_Estrategico;
            Eh_Administrativo = eh_Administrativo;
            Eh_Criminal_Judicial = eh_Criminal_Judicial;
            Eh_Procon = eh_Procon;
            Eh_Pex_Juizado = eh_Pex_Juizado;
            Eh_Pex_Consumidor = eh_Pex_Consumidor;
            Eh_Criminal_Administrativo = eh_Criminal_Administrativo;
            Eh_Documento = eh_Documento;
            Eh_Servico = eh_Servico;
            AtivoDePara = ativoDePara;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; } = string.Empty;

        public bool Ativo { get; private set; } = true;



        //public string TipoProcesso { get; private set; }

        //public int CodTipoProcesso { get; private set; }
        //public TipoProcessoManutencao TipoProcesso { get; set; }


        public TipoProcessoManutencao TipoProcesso
        {
            get
            {
                if (Eh_Civel_Consumidor.HasValue && Eh_Civel_Consumidor.HasValue)
                {
                    return TipoProcessoManutencao.CIVEL_CONSUMIDOR;
                }

                if (Eh_Trabalhista.HasValue && Eh_Trabalhista.HasValue)
                {
                    return TipoProcessoManutencao.TRABALHISTA;
                }

                if (Eh_Tributario_Administrativo.HasValue && Eh_Tributario_Administrativo.HasValue)
                {
                    return TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (Eh_Tributario_Judicial.HasValue && Eh_Tributario_Judicial.HasValue)
                {
                    return TipoProcessoManutencao.TRIBUTARIO_JUDICIAL;
                }

                if (Eh_Juizado_Especial.HasValue && Eh_Juizado_Especial.HasValue)
                {
                    return TipoProcessoManutencao.JEC;
                }

                if (Eh_Civel_Estrategico)
                {
                    return TipoProcessoManutencao.CIVEL_ESTRATEGICO;
                }

                if (Eh_Administrativo)
                {
                    return TipoProcessoManutencao.ADMINISTRATIVO;
                }

                if (Eh_Criminal_Judicial)
                {
                    return TipoProcessoManutencao.CRIMINAL_JUDICIAL;
                }

                if (Eh_Procon)
                {
                    return TipoProcessoManutencao.PROCON;
                }

                if (Eh_Pex_Juizado)
                {
                    return TipoProcessoManutencao.PEX_JUIZADO;
                }

                if (Eh_Pex_Consumidor)
                {
                    return TipoProcessoManutencao.PEX_CONSUMIDOR;
                }

                if (Eh_Criminal_Administrativo)
                {
                    return TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO;
                }

                return TipoProcessoManutencao.NAO_DEFINIDO;
            }
        }


        public bool? Eh_Civel_Consumidor { get; private set; }
        public bool? Eh_Trabalhista { get; private set; }
        public bool? Eh_Tributario_Administrativo { get; private set; }
        public bool? Eh_Tributario_Judicial { get; private set; }
        public bool? Eh_Juizado_Especial { get; private set; }
        public bool Eh_Civel_Estrategico { get; private set; }
        public bool Eh_Administrativo { get; private set; }
        public bool Eh_Criminal_Judicial { get; private set; }
        public bool Eh_Procon { get; private set; }
        public bool Eh_Pex_Juizado { get; private set; }
        public bool Eh_Pex_Consumidor { get; private set; }
        public bool Eh_Criminal_Administrativo { get; private set; }
        public bool Eh_Documento { get; private set; }
        public bool? Eh_Servico { get; private set; }
        //public bool Eh_Civel_Administrativo { get; private set; }

        public bool? AtivoDePara { get; private set; }
        public int? IdMigracao { get; private set; }
        public string DescricaoMigracao { get; private set; }
   
    }
}
