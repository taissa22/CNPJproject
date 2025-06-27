using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity
{
    public class GrupoDeEstados : EntityCrud<GrupoDeEstados, long>
    {
        public string NomeGrupo { get; set; }
        public IEnumerable<GrupoEstados> GrupoEstados { get; set; }

        public override AbstractValidator<GrupoDeEstados> Validator => throw new NotImplementedException();

        public override void PreencherDados(GrupoDeEstados data)
        {
            Id = data.Id;
            NomeGrupo = data.NomeGrupo;
        }

        public void AtualizarNome(string novoNome)
        {
            this.NomeGrupo = novoNome;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }


    }
}
