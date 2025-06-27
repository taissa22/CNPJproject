using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Entity
{
    public class GrupoEmpresaContabilSap : EntityCrud<GrupoEmpresaContabilSap, long>
    {
        public string NomeGrupo { get; set; }
        public virtual IList<GrupoEmpresaContabilSapParte> GrupoEmpresaContabilSapParte { get; set; }
        public bool? Recuperanda { get; set; }

        public override AbstractValidator<GrupoEmpresaContabilSap> Validator => new GrupoEmpresaContabilSapValidator();

        public void AtualizarNome(string novoNome)
        {
            this.NomeGrupo = novoNome;
        }

        public void AtualizarFlagRecuperanda(bool recuperanda)
        {
            this.Recuperanda = recuperanda;
        }

        public override void PreencherDados(GrupoEmpresaContabilSap data)
        {
            Id = data.Id;
            NomeGrupo = data.NomeGrupo;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class GrupoEmpresaContabilSapValidator : AbstractValidator<GrupoEmpresaContabilSap>
    {
        public GrupoEmpresaContabilSapValidator()
        {
        }
    }
}