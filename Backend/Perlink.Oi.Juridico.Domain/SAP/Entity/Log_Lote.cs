using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Log_Lote : EntityCrud<Log_Lote, long>
    {
        public override AbstractValidator<Log_Lote> Validator => new Log_LoteValidator();
        public string Operacao { get; set; }
        public DateTime DataLog { get; set; }
        public long? CodigoStatusPagamentoAntes { get; set; }
        public long? CodigoStatusPagamentoDepois { get; set; }
        public string DescricaoStatusPagamento { get; set; }

        public string NomeUsuario { get; set; }
        public string UsuarioCodigoRetro { get; set; }

        public override void PreencherDados(Log_Lote data)
        {
            NomeUsuario = data.NomeUsuario;
            Operacao = data.Operacao;
            DataLog = data.DataLog;
            CodigoStatusPagamentoAntes = data.CodigoStatusPagamentoAntes;
            CodigoStatusPagamentoDepois = data.CodigoStatusPagamentoDepois;
            DescricaoStatusPagamento = data.DescricaoStatusPagamento;
            UsuarioCodigoRetro = data.UsuarioCodigoRetro;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class Log_LoteValidator : AbstractValidator<Log_Lote>
    {
        public Log_LoteValidator()
        {
        }
    }
}