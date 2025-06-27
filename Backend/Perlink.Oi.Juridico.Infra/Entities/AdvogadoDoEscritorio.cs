using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AdvogadoDoEscritorio : Notifiable, IEntity, INotifiable
    {
        private AdvogadoDoEscritorio()
        {
        }

        public static AdvogadoDoEscritorio Criar(int id,int escritorioId, string nome , bool EhContato, string celular, string celularDDD, string email,string estado, string numeroOAB )
        {
            AdvogadoDoEscritorio advogado = new AdvogadoDoEscritorio();
            advogado.Id = id;
            advogado.EscritorioId = escritorioId;
            advogado.Nome = nome;
            advogado.Celular = celular;
            advogado.CelularDDD = celularDDD;
            advogado.EhContato = EhContato;
            advogado.Email = email;
            advogado.EstadoId = estado;
            advogado.NumeroOAB = numeroOAB;
            return advogado;

        }
        public void Atualizar(int escritorioId, int id, string nome, bool ehContato, string celular, string celularDDD, string email, string estado, string numeroOAB)
        {
            Id = id;
            Nome = nome;
            EhContato = ehContato;
            Celular = celular;
            CelularDDD = celularDDD;
            Email = email;
            EstadoId = estado;
            NumeroOAB = numeroOAB;
            EscritorioId = escritorioId;
            Validate();
        }

        public void AtualizarContato(bool ehContato)
        {
            EhContato = ehContato;
        }


        public AdvogadoDoEscritorio(int id, string nome) {
            Id = id;
            Nome = nome;
        }

        public int Id { get; private set; }

        public int EscritorioId { get; private set; }
        public Escritorio Escritorio { get; private set; }

        public string Nome { get; private set; }

        public string NumeroOAB { get; private set; }
        public string EstadoId { get; private set; }
        public EstadoEnum Estado => Enums.EstadoEnum.PorId(EstadoId);


        public string Celular { get; private set; }
        public string CelularDDD { get; private set; }
        public string Email { get; private set; }
        public bool EhContato { get; private set; }

        public void Validate()
        {

            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Nome não pode ser vazio");
            }

            if (String.IsNullOrEmpty(EstadoId))
            {
                AddNotification(nameof(Estado), "Estado não pode ser vazio");
            }
        }


    }
}