using FluentValidation;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity
{
    public class AlteracaoEmBloco : EntityCrud<AlteracaoEmBloco, long>
    {
        public override AbstractValidator<AlteracaoEmBloco> Validator => new AlteracaoEmBlocoValidator();

        public DateTime? DataCadastro { get; set; }
        public DateTime? DataExecucao { get; set; }
        public AlteracaoEmBlocoEnum? Status { get; set; }
        public string Arquivo { get; set; }
        public string CodigoDoUsuario { get; set; }
        public int ProcessosAtualizados { get; set; }
        public int ProcessosComErro { get; set; }
        public TipoProcessoEnum? CodigoTipoProcesso { get; set; }        
        //public string NomeUsuario { get; set; }

        public override void PreencherDados(AlteracaoEmBloco data)
        {
            DataCadastro = data.DataCadastro;
            DataExecucao = data.DataExecucao;
            Status = data.Status;
            Arquivo = data.Arquivo;
            CodigoDoUsuario = data.CodigoDoUsuario;
            ProcessosAtualizados = data.ProcessosAtualizados;
            ProcessosComErro = data.ProcessosComErro;
            CodigoTipoProcesso = data.CodigoTipoProcesso;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class AlteracaoEmBlocoValidator : AbstractValidator<AlteracaoEmBloco>
    {
        public AlteracaoEmBlocoValidator()
        {

        }
    }
}
