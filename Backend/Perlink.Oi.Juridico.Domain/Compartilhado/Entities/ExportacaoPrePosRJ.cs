using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class ExportacaoPrePosRJ : EntityCrud<ExportacaoPrePosRJ, long>
    {
        public override AbstractValidator<ExportacaoPrePosRJ> Validator => new ExportacaoPrePosRJValidator();

        // cod_exportacao_pre_pos_rj number(4),

        public DateTime? DataExtracao { get; set; } // data_extracao date
        public DateTime? DataExecucao { get; set; } // data_execucao date
        public bool? NaoExpurgar { get; set; } // expurgar char (1)
        public string ArquivoJec { get; set; } // arquivo_jec varchar2(70)
        public string ArquivoTrabalhista { get; set; } //arquivo_trabalhista varchar2(70)
        public string ArquivoCivelConsumidor { get; set; } //arquivo_civelconsumidor varchar2(70)
        public string ArquivoCivelEstrategico { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoPex { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoTributarioJudicial { get; set; } //arquivo_civelestrategico varchar2(70)
        public string ArquivoAdministrativo { get; set; } //arquivo_civelestrategico varchar2(70)

        public override void PreencherDados(ExportacaoPrePosRJ data)
        {
            DataExtracao = data.DataExtracao;
            DataExecucao = data.DataExecucao;
            NaoExpurgar = data.NaoExpurgar;
            ArquivoJec = data.ArquivoJec;
            ArquivoTrabalhista = data.ArquivoTrabalhista;
            ArquivoCivelConsumidor = data.ArquivoCivelConsumidor;
            ArquivoCivelEstrategico = data.ArquivoCivelEstrategico;
            ArquivoPex = data.ArquivoPex;
            ArquivoTributarioJudicial = data.ArquivoTributarioJudicial;
            ArquivoAdministrativo = data.ArquivoAdministrativo;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ExportacaoPrePosRJValidator : AbstractValidator<ExportacaoPrePosRJ>
    {
        public ExportacaoPrePosRJValidator()
        {

        }
    }

}
