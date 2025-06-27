using Flunt.Validations;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Impl.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Entities
{
    public class TipoAudiencia : Entity
    {
        #region "ATTRIBUTE"
        private IList<AudienciaProcesso> _audienciasProcesso;
        #endregion

        #region "CONSTRUCTOR"
        protected TipoAudiencia() 
        {
            _audienciasProcesso = new List<AudienciaProcesso>();
        }

        public TipoAudiencia(string descricao, string sigla, TipoProcessoEnum? tipoProcesso, bool estaAtivo = true) 
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "TipoAudiencia.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 100, "TipoAudiencia.Descricao", "Descrição só pode ter no máximo 100 caracteres.")
                .HasMaxLen(sigla, 4, "TipoAudiencia.Sigla", "Sigla só pode ter no máximo 4 caracteres.")
                .IsNotNull(tipoProcesso, "TipoAudiencia.TipoProcesso", "Tipo de Processo é obrigatório.")
            );

            if (Valid)
            {
                Descricao = descricao;
                Sigla = sigla.ToUpper();
                EstaAtivo = estaAtivo;
                ConfigurarTipoProcesso(tipoProcesso.Value);

                _audienciasProcesso = new List<AudienciaProcesso>();
            }
        }
        #endregion

        #region "PROPERTY"
        public long CodigoTipoAudiencia { get; private set; }

        public string Descricao { get; private set; }

        public string Sigla { get; private set; }

        public bool EstaAtivo { get; private set; }

        public bool EhCivelConsumidor { get; private set; }

        public bool EhCivelEstrategico { get; private set; }

        public bool EhTrabalhista { get; private set; }

        public bool EhTrabalhistaAdmin { get; private set; }
        
        public bool EhTributarioAdmin { get; private set; }

        public bool EhTributarioJud { get; private set; }

        public bool EhJuizado { get; private set; }

        public bool EhAdministrativo { get; private set; }

        public bool EhCivelAdmin { get; private set; }

        public bool EhCriminalJud { get; private set; }

        public bool EhCriminalAdmin { get; private set; }

        public bool EhProcon { get; private set; }

        public bool EhPex { get; private set; }

        public bool LinkVirtual { get; private set; }
        #endregion

        #region "RELATIONSHIP"
        public ICollection<AudienciaProcesso> AudienciasProcesso { get { return _audienciasProcesso.ToArray(); } }
        #endregion

        #region "METHOD"
        public void Atualizar(string descricao, string sigla, TipoProcessoEnum? tipoProcesso, bool estaAtivo = true)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "TipoAudiencia.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 100, "TipoAudiencia.Descricao", "Descrição só pode ter no máximo 100 caracteres.")
                .HasMaxLen(sigla, 4, "TipoAudiencia.Sigla", "Sigla só pode ter no máximo 4 caracteres.")
                .IsNotNull(tipoProcesso, "TipoAudiencia.TipoProcesso", "Tipo de Processo é obrigatório.")
            );

            if (Valid)
            {
                Descricao = descricao;
                Sigla = sigla.ToUpper();
                EstaAtivo = estaAtivo;
                ConfigurarTipoProcesso(tipoProcesso.Value);
            }
        }

        public void AdicionarAudienciaProcesso(AudienciaProcesso audienciaProcesso)
        {
            // Adaptação de validação
            var validate = audienciaProcesso.Validar();

            if(!validate.IsValid)
            {
                AddNotification("TipoAudiencia.AudienciaProcessoInvalido", "Audiencia Processo é inválido.");
                return;
            }

            _audienciasProcesso.Add(audienciaProcesso);
        }

        private void ConfigurarTipoProcesso (TipoProcessoEnum tipoProcesso)
        {
            EhCivelConsumidor = false;
            EhCivelEstrategico = false;
            EhTrabalhista = false;
            EhTrabalhistaAdmin = false;
            EhTributarioAdmin = false;
            EhTributarioJud = false;
            EhJuizado = false; 
            EhAdministrativo = false;
            EhCivelAdmin = false;
            EhCriminalJud = false;
            EhCriminalAdmin = false;
            EhProcon = false;
            EhPex = false;

            switch(tipoProcesso)
            {
                case TipoProcessoEnum.CivelConsumidor:
                    EhCivelConsumidor = true;
                    break;
                case TipoProcessoEnum.CivelEstrategico:
                    EhCivelEstrategico = true;
                    break;
                case TipoProcessoEnum.Trabalhista:
                    EhTrabalhista = true;
                    break;
                case TipoProcessoEnum.TrabalhistaAdministrativo:
                    EhTrabalhistaAdmin = true;
                    break;
                case TipoProcessoEnum.TributarioAdministrativo:
                    EhTributarioAdmin = true;
                    break;
                case TipoProcessoEnum.TributarioJudicial:
                    EhTributarioJud = true;
                    break;
                case TipoProcessoEnum.JuizadoEspecial:
                    EhJuizado = true;
                    break;
                case TipoProcessoEnum.Administrativo:
                    EhAdministrativo = true;
                    break;
                case TipoProcessoEnum.CivelAdministrativo:
                    EhCivelAdmin = true;
                    break;
                case TipoProcessoEnum.CriminalJudicial:
                    EhCriminalJud = true;
                    break;
                case TipoProcessoEnum.CriminalAdministrativo:
                    EhCriminalAdmin = true;
                    break;
                case TipoProcessoEnum.Procon:
                    EhProcon = true;
                    break;
                case TipoProcessoEnum.Pex:
                    EhPex = true;
                    break;
            }
        }
        #endregion
    }
}
