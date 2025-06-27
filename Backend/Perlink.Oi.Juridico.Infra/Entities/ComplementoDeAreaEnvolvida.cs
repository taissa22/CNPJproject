using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ComplementoDeAreaEnvolvida : Notifiable, IEntity, INotifiable
    {
        private ComplementoDeAreaEnvolvida()
        {
        }

        public static ComplementoDeAreaEnvolvida Criar(string nome, bool ativo, TipoProcesso tipoProcesso)
        {
            var complementoDeAreaEnvolvida = new ComplementoDeAreaEnvolvida();

            complementoDeAreaEnvolvida.Nome = nome.ToUpper();
            complementoDeAreaEnvolvida.Ativo = ativo;

            switch (tipoProcesso.Id)
            {
                case 2:
                    complementoDeAreaEnvolvida.EhTrabalhista = true;
                    break;

                case 3:
                    complementoDeAreaEnvolvida.EhAdministrativo = true;
                    break;

                case 5:
                    complementoDeAreaEnvolvida.EhTributario = true;
                    break;

                case 9:
                    complementoDeAreaEnvolvida.EhCivelEstrategico = true;
                    break;

                case 13:
                    complementoDeAreaEnvolvida.EhCriminal = true;
                    break;
            }

            complementoDeAreaEnvolvida.Validate();
            return complementoDeAreaEnvolvida;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public bool Ativo { get; private set; }
        public bool EhCivelEstrategico { get; private set; }
        public bool EhTrabalhista { get; private set; }
        public bool EhTributario { get; private set; }
        public bool EhAdministrativo { get; private set; }
        public bool EhCriminal { get; private set; }

        public TipoProcesso TipoProcesso
        {
            get
            {
                if (EhTrabalhista)
                {
                    return TipoProcesso.TRABALHISTA;
                }

                if (EhTributario)
                {
                    return TipoProcesso.TRIBUTARIO_JUDICIAL;
                }

                if (EhCivelEstrategico)
                {
                    return TipoProcesso.CIVEL_ESTRATEGICO;
                }

                if (EhAdministrativo)
                {
                    return TipoProcesso.ADMINISTRATIVO;
                }

                if (EhCriminal)
                {
                    return TipoProcesso.CRIMINAL;
                }

                return TipoProcesso.NAO_DEFINIDO;
            }
        }

        public void Atualizar(int id, string nome, bool ativo)
        {
            Id = id;
            Nome = nome.ToUpper();
            Ativo = ativo;
            Validate();
        }

        public void Remover(int complementoDeAreaEnvolvidaId)
        {
            Id = complementoDeAreaEnvolvidaId;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo nome não pode estar vazio");
            }
            if (Nome?.Length > 100)
            {
                AddNotification(nameof(Nome), "O Campo Nome pode conter no máximo 100 caracteres");
            }
            var tiposPermitidos = new[] { TipoProcesso.TRABALHISTA, TipoProcesso.TRIBUTARIO_JUDICIAL,
                                          TipoProcesso.CIVEL_ESTRATEGICO,TipoProcesso.ADMINISTRATIVO,
                                          TipoProcesso.CRIMINAL};

            if (!tiposPermitidos.Any(x => x == TipoProcesso))
            {
                AddNotification("Tipo Processo", "Tipo de processo diferente do esperado");
            }
        }
    }
}