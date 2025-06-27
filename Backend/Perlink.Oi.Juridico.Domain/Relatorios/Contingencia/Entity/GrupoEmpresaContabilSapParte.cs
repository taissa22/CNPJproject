using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Entity
{
    public class GrupoEmpresaContabilSapParte : EntityCrud<GrupoEmpresaContabilSapParte, long>
    {
        public long GrupoId { get; set; }
        public virtual GrupoEmpresaContabilSap GrupoEmpresaContabilSap { get; set; }
        public long EmpresaId { get; set; }
        public virtual Parte Empresa { get; set; }

        public override AbstractValidator<GrupoEmpresaContabilSapParte> Validator => new GrupoEmpresaContabilSapParteValidator();

        public override void PreencherDados(GrupoEmpresaContabilSapParte data)
        {
            Id = data.Id;
            GrupoId = data.GrupoId;
            EmpresaId = data.EmpresaId;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class GrupoEmpresaContabilSapParteValidator : AbstractValidator<GrupoEmpresaContabilSapParte>
    {
        public GrupoEmpresaContabilSapParteValidator()
        {
        }
    }
}