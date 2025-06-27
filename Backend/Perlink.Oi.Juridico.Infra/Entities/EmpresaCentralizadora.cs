using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EmpresaCentralizadora : Notifiable, IEntity, INotifiable
    {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private EmpresaCentralizadora()
        {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        public static EmpresaCentralizadora Criar(DataString nome, IEnumerable<Convenio> convenios)
        {
            var empresaCentralizadora = new EmpresaCentralizadora()
            {
                Nome = nome.ToString()
            };
            empresaCentralizadora.AtualizarConvenios(convenios);
            empresaCentralizadora.Validate();
            return empresaCentralizadora;
        }

        public int Codigo { get; private set; }
        public string Nome { get; private set; }
        public int Ordem { get; private set; }

        private readonly HashSet<Convenio> conveniosSet = new HashSet<Convenio>();
        public IReadOnlyCollection<Convenio> Convenios => conveniosSet;
        
        private readonly HashSet<FechamentoCCMedia> FechamentoCCMediaSet = new HashSet<FechamentoCCMedia>();
        public IReadOnlyCollection<FechamentoCCMedia> FechamentosCCMedia => FechamentoCCMediaSet;


        public void Validate()
        {
            if (Nome.Length > 100)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 100 caracteres.");
            }

            AddNotifications(conveniosSet);
        }

        public void Atualizar(DataString nome, IEnumerable<Convenio> convenios)
        {
            Nome = nome.ToString();
            AtualizarConvenios(convenios);
            Validate();
        }

        private void AtualizarConvenios(IEnumerable<Convenio> convenios)
        {
            conveniosSet.Clear();
            foreach (var convenio in convenios)
            {
                convenio.AdicionarEmpresaCentralizadora(this);
                conveniosSet.Add(convenio);
            }
        }
    }
}