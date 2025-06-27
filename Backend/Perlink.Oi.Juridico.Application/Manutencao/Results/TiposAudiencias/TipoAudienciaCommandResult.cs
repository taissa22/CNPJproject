
using Perlink.Oi.Juridico.Infra.Enums;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias
{
    public class TipoAudienciaCommandResult : ICommandResult
    {
        public TipoAudienciaCommandResult(long codigoTipoAudiencia, string descricao, string sigla, bool ativo,
                                          bool ehCivelConsumidor, bool ehCivelEstrategico, bool ehTrabalhista,
                                          bool ehTrabalhistaAdmin, bool ehTributarioAdmin, bool ehTributarioJud,
                                          bool ehJuizado, bool ehAdministrativo, bool ehCivelAdmin, bool ehCriminalJud,
                                          bool ehCriminalAdmin, bool ehProcon, bool ehPex, bool ativoDePara, int? idMigracao,
                                          string descricaoMigracao, bool linkVirtual) 
        {
            CodigoTipoAudiencia = codigoTipoAudiencia; 
            Descricao = descricao;
            Sigla = sigla;
            Ativo = ativo;
            EhCivelConsumidor = ehCivelConsumidor;
            EhCivelEstrategico = ehCivelEstrategico;
            EhTrabalhista = ehTrabalhista;
            EhTrabalhistaAdmin = ehTrabalhistaAdmin;
            EhTributarioAdmin = ehTributarioAdmin;
            EhTributarioJud = ehTributarioJud;
            EhAdministrativo = ehAdministrativo;
            EhCivelAdmin = ehCivelAdmin;
            EhCriminalAdmin = ehCriminalAdmin;
            EhCriminalJud = ehCriminalJud;
            EhJuizado = ehJuizado;
            EhProcon = ehProcon;
            EhPex = ehPex;
            AtivoDePara = ativoDePara;
            IdMigracao = idMigracao;
            DescricaoMigracao = descricaoMigracao;
            LinkVirtual = linkVirtual;
        }

        public long CodigoTipoAudiencia { get; set; }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public bool Ativo { get; set; }

        public TipoProcesso TipoProcesso
        {
            get
            {
                if (EhCivelConsumidor)
                {
                    return TipoProcesso.CIVEL_CONSUMIDOR;
                }

                if (EhTrabalhista)
                {
                    return TipoProcesso.TRABALHISTA;
                }

                if (EhTrabalhistaAdmin)
                {
                    return TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                }

                if (EhTributarioAdmin)
                {
                    return TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (EhTributarioJud)
                {
                    return TipoProcesso.TRIBUTARIO_JUDICIAL;
                }

                if (EhJuizado)
                {
                    return TipoProcesso.JEC;
                }

                if (EhCivelEstrategico)
                {
                    return TipoProcesso.CIVEL_ESTRATEGICO;
                }

                if (EhAdministrativo)
                {
                    return TipoProcesso.ADMINISTRATIVO;
                }

                if (EhCivelAdmin)
                {
                    return TipoProcesso.CIVEL_ADMINISTRATIVO;
                }

                if (EhCriminalJud)
                {
                    return TipoProcesso.CRIMINAL_JUDICIAL;
                }

                if (EhProcon)
                {
                    return TipoProcesso.PROCON;
                }

                if (EhPex)
                {
                    return TipoProcesso.PEX;
                }

                if (EhCriminalAdmin)
                {
                    return TipoProcesso.CRIMINAL_ADMINISTRATIVO;
                }

                return TipoProcesso.NAO_DEFINIDO;
            }
        }

        public bool EhCivelConsumidor { get; set; }

        public bool EhCivelEstrategico { get; set; }

        public bool EhTrabalhista { get; set; }

        public bool EhTrabalhistaAdmin { get; set; }

        public bool EhTributarioAdmin { get; set; }

        public bool EhTributarioJud { get; set; }

        public bool EhJuizado { get; set; }

        public bool EhAdministrativo { get; set; }

        public bool EhCivelAdmin { get; set; }

        public bool EhCriminalJud { get; set; }

        public bool EhCriminalAdmin { get; set; }

        public bool EhProcon { get; set; }

        public bool EhPex { get; set; }

        public bool AtivoDePara { get; private set; }

        public int? IdMigracao { get; private set; }

        public string DescricaoMigracao { get; private set; }

        public bool LinkVirtual { get; set; }
    }
}
