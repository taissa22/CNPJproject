using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.Processos
{
    public class AdvogadoEscritorio : EntityCrud<AdvogadoEscritorio, long>
    {
        public long CodigoProfissional { get; set; }        
        public string CodigoEstado { get; set; }
        public string NumeroAOBAdvogado { get; set; }
        public string NomeAdvogado { get; set; }
        public string NumeroCelular { get; set; }
        public string NumeroDDDCelular { get; set; }
        public string DescricaoEmail { get; set; }
        public bool IndicaContatoEscritorio { get; set; }
        public long? CodigoInterno { get; set; }

        public ICollection<Profissional> Profissionais { get; set; }

        public override AbstractValidator<AdvogadoEscritorio> Validator => new AdvogadoEscritorioValidator();

        public override void PreencherDados(AdvogadoEscritorio data)
        {
            CodigoProfissional = data.CodigoProfissional;
            CodigoEstado = data.CodigoEstado;
            NumeroAOBAdvogado = data.NumeroAOBAdvogado;
            NomeAdvogado = data.NomeAdvogado;
            NumeroCelular = data.NumeroCelular;
            NumeroDDDCelular = data.NumeroDDDCelular;
            DescricaoEmail = data.DescricaoEmail;
            IndicaContatoEscritorio = data.IndicaContatoEscritorio;


        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class AdvogadoEscritorioValidator : AbstractValidator<AdvogadoEscritorio>
    {
        public AdvogadoEscritorioValidator()
        {
        }
    }
}

