using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using TelefoneVO = Perlink.Oi.Juridico.Infra.ValueObjects.Telefone;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Orgao : Notifiable, IEntity, INotifiable
    {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private Orgao()
        {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        public static Orgao Criar(HugeDataString nome, Telefone? telefone, TipoOrgao tipoOrgao, IEnumerable<Competencia> competencias)
        {
            var orgao = new Orgao()
            {
                Nome = nome.ToString(),
                TipoParteValor = tipoOrgao.Valor,
                TelefoneDDD = telefone?.Area,
                Telefone = telefone?.Numero,
                SequencialCompetencia = 0
            };
            orgao.AtualizarCompetencias(competencias);
            orgao.Validate();
            return orgao;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        internal string TipoParteValor { get; private set; }
        public TipoOrgao TipoOrgao => TipoOrgao.PorValor(TipoParteValor);

        public string? TelefoneDDD { get; private set; }
        public string? Telefone { get; private set; }

        internal int SequencialCompetencia { get; private set; }

        private readonly HashSet<Competencia> competenciaSet = new HashSet<Competencia>();
        public IReadOnlyCollection<Competencia> Competencias => competenciaSet;

        internal ParteBase ParteBase { get; private set; }

        public void Validate()
        {
            if (!Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }

            if (!string.IsNullOrEmpty(TelefoneDDD) && !TelefoneVO.IsValid(TelefoneDDD + Telefone))
            {
                AddNotification(nameof(Telefone), "O campo Telefone permite no máximo 13 caracteres.");
            }

            AddNotifications(competenciaSet);
        }

        private void AtualizarCompetencias(IEnumerable<Competencia> competencias)
        {
            competenciaSet.Clear();
            foreach (var competencia in competencias)
            {
                if (competencia.Orgao is null)
                {
                    SequencialCompetencia++;
                    competencia.AdicionarOrgao(this, SequencialCompetencia);
                }
                competenciaSet.Add(competencia);
            }
        }

        public void Atualizar(HugeDataString nome, Telefone? telefone, IEnumerable<Competencia> competencias)
        {
            Nome = nome.ToString();
            TelefoneDDD = telefone?.Area;
            Telefone = telefone?.Numero;
            AtualizarCompetencias(competencias);
            Validate();
        }
    }
}