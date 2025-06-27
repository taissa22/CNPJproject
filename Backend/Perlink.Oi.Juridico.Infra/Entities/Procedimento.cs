using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Procedimento : Notifiable, IEntity, INotifiable
    {

        private Procedimento()
        {
        }

        public static Procedimento Criar(string descricao, bool indAtivo, TipoDeParticipacao tipoDeParticipacao1, TipoDeParticipacao tipoDeParticipacao2, 
            bool indOrgao1, bool indOrgao2, bool indProvisionado, bool indPoloPassivoUnico, TipoProcesso tipoProcesso)
        {
            Procedimento procedimento = new Procedimento();

            procedimento.Descricao = descricao;
            procedimento.IndAtivo = indAtivo;
            procedimento.CodTipoParticipacao1 = tipoDeParticipacao1?.Codigo;
            procedimento.CodTipoParticipacao2 = tipoDeParticipacao2?.Codigo;
            procedimento.IndOrgao1 = indOrgao1;
            procedimento.IndOrgao2 = indOrgao2;
            procedimento.IndProvisionado = indProvisionado;
            procedimento.IndPoloPassivoUnico = indPoloPassivoUnico;

            switch (tipoProcesso.Id)
            {        
                case 3:
                    procedimento.IndAdministrativo = true;
                    break;
                case 4:
                    procedimento.IndTributario = true;
                    break;
                case 6:
                    procedimento.IndTrabalhistaAdm = true;
                    break;
                case 12:
                    procedimento.IndCivelAdm = true;
                    break;
                case 14:
                    procedimento.IndCriminalAdm = true;
                    break;              
            }

            procedimento.Validate();

            return procedimento;
        }

        public int Codigo { get; private set; }

        public string Descricao { get; private set; }

        public int? CodTipoParticipacao1 { get; private set; }
        public TipoDeParticipacao TipoDeParticipacao1 { get; private set; }

        public int? CodTipoParticipacao2 { get; private set; }
        public TipoDeParticipacao TipoDeParticipacao2 { get; private set; }

        public TipoProcesso TipoProcesso
        {
            get
            {
                if (IndAdministrativo.HasValue && (bool)IndAdministrativo)
                {
                    return TipoProcesso.ADMINISTRATIVO;
                }

                if (IndTrabalhistaAdm.HasValue && (bool)IndTrabalhistaAdm)
                {
                    return TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                }

                if (IndTributario.HasValue && (bool)IndTributario)
                {
                    return TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (IndCivelAdm)
                {
                    return TipoProcesso.CIVEL_ADMINISTRATIVO;
                }

                if (IndCriminalAdm)
                {
                    return TipoProcesso.CRIMINAL_ADMINISTRATIVO;
                }              

                return TipoProcesso.NAO_DEFINIDO;
            }
        }


        public bool? IndOrgao1 { get; private set; }

        public bool? IndOrgao2 { get; private set; }

        public bool? IndAdministrativo { get; private set; }

        public bool? IndTributario { get; private set; }

        public bool? IndTrabalhistaAdm { get; private set; }

        public bool IndProvisionado { get; private set; }

        public bool IndPoloPassivoUnico { get; private set; }

        public bool IndAtivo { get; private set; }

        public bool IndCriminalAdm { get; private set; }

        public bool IndCivelAdm { get; private set; } = false;

        public void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }

            if ((IndOrgao1.HasValue && IndOrgao1.Value) && (IndOrgao2.HasValue && IndOrgao2.Value))
            {
                AddNotification(nameof(IndOrgao1), "Os campos IndOrgao1 e IndOrgao2 não podem ser marcados ao mesmo tempo");
            }

            if ((IndOrgao2.HasValue && IndOrgao2.Value) && (IndPoloPassivoUnico))
            {
                AddNotification(nameof(IndOrgao1), "Os campos IndOrgao2 e IndPoloPassivoUnico não podem ser marcados ao mesmo tempo");
            }

            if (!new[] { TipoProcesso.ADMINISTRATIVO.Id, TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id,
                TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id, TipoProcesso.CIVEL_ADMINISTRATIVO.Id,
                TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id }.Contains(TipoProcesso.Id))
            {
                AddNotification("Tipo de Processo", "Tipo de Processo não permitido.");
            }

            if (new[] { TipoProcesso.ADMINISTRATIVO.Id, TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id,
                TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id}.Contains(TipoProcesso.Id)
                && CodTipoParticipacao1.GetValueOrDefault() <= 0)
            {
                AddNotification("1º Tipo de Participação", "Campo Requerido");
            }

            if (new[] { TipoProcesso.ADMINISTRATIVO.Id, TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id,
                TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id}.Contains(TipoProcesso.Id)
                && CodTipoParticipacao2.GetValueOrDefault() <= 0)
            {
                AddNotification("2º Tipo de Participação", "Campo Requerido");
            }
        }

        public void Atualizar(string descricao, bool indAtivo, TipoDeParticipacao tipoDeParticipacao1, TipoDeParticipacao tipoDeParticipacao2,
            bool indOrgao1, bool indOrgao2, bool indProvisionado, bool indPoloPassivoUnico)
        {
            Descricao = descricao;
            IndAtivo = indAtivo;
            CodTipoParticipacao1 = tipoDeParticipacao1?.Codigo;
            CodTipoParticipacao2 = tipoDeParticipacao2?.Codigo;
            IndOrgao1 = indOrgao1;
            IndOrgao2 = indOrgao2;
            IndProvisionado = indProvisionado;
            IndPoloPassivoUnico = indPoloPassivoUnico;

            Validate();
        }
    }
}


