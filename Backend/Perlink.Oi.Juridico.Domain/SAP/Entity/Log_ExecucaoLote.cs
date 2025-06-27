using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Log_ExecucaoLote : EntityCrud<Log_ExecucaoLote, long>
    {
        public override AbstractValidator<Log_ExecucaoLote> Validator => new Log_ExecucaoLoteValidator();
        public DateTime DataLog { get; set; }
        public string DescricaoLogExecucaoLote { get; set; }

        public override void PreencherDados(Log_ExecucaoLote data)
        {
            DataLog = data.DataLog;
            DescricaoLogExecucaoLote = data.DescricaoLogExecucaoLote;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class Log_ExecucaoLoteValidator : AbstractValidator<Log_ExecucaoLote>
    {
        public Log_ExecucaoLoteValidator()
        {
        }
    }

}