using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity
{
    public class GrupoEstados : EntityCrud<GrupoEstados, long>
    {

        public long GrupoId { get; set; }
        public GrupoDeEstados GrupoDeEstados { get; set; }
        public string EstadoId { get; set; }
        public Estado Estado { get; set; }

        public override AbstractValidator<GrupoEstados> Validator => throw new NotImplementedException();

        public override void PreencherDados(GrupoEstados data)
        {
            Id = data.Id;
            GrupoId = data.GrupoId;
            EstadoId = data.EstadoId;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

    }
}
