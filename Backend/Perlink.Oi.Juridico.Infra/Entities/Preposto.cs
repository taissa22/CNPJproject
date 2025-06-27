using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.ComponentModel.DataAnnotations.Schema;
#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Preposto : Notifiable, IEntity, INotifiable
    {
        private Preposto()
        {
        }

        public Preposto(int id, 
                        string nome, 
                        //string estadoId, 
                        bool ativo, 
                        bool ehCivelEstrategico, 
                        bool ehCivel, 
                        bool ehTrabalhista, 
                        bool ehJuizado, 
                        string? usuarioId, 
                        string nomeUsuario,  
                        bool? usuarioAtivo,
                        bool ehProcon, 
                        bool ehPex, 
                        bool ehEscritorio,
                        string? matricula)
        {
            Id = id;
            Nome = nome;
            //EstadoId = estadoId;
            Ativo = ativo;
            EhCivelEstrategico = ehCivelEstrategico;
            EhCivel = ehCivel;
            EhTrabalhista = ehTrabalhista;
            EhJuizado = ehJuizado;
            UsuarioId = usuarioId;
            NomeUsuario = nomeUsuario;
            UsuarioAtivo = usuarioAtivo;
            EhProcon = ehProcon;
            EhPex = ehPex;
            EhEscritorio = ehEscritorio;
            Matricula = matricula; 
        }

        public static Preposto Criar(string nome,
                                     //string estadoId,
                                     bool ativo,
                                     bool ehCivelEstrategico,
                                     bool ehCivel,
                                     bool ehTrabalhista,
                                     bool ehJuizado,
                                     string usuarioId,
                                     bool ehProcon,
                                     bool ehPex,
                                     bool ehEscritorio,
                                     string matricula)
        {
            Preposto preposto = new Preposto();
            preposto.Nome = nome;
            //preposto.EstadoId = estadoId;
            preposto.Ativo = ativo;
            preposto.EhCivelEstrategico = ehCivelEstrategico;
            preposto.EhCivel = ehCivel;
            preposto.EhTrabalhista = ehTrabalhista;
            preposto.EhJuizado = ehJuizado;
            preposto.UsuarioId = usuarioId;
            preposto.EhProcon = ehProcon;
            preposto.EhPex = ehPex;
            preposto.EhEscritorio = ehEscritorio;
            preposto.Matricula = matricula;

            preposto.Validate();

            return preposto;
        }

        public void Atualizar(string nome,
                              //string estadoId,
                              bool ativo,
                              bool ehCivelEstrategico,
                              bool ehCivel,
                              bool ehTrabalhista,
                              bool ehJuizado,
                              string usuarioId,
                              bool ehProcon,
                              bool ehPex,
                              bool ehEscritorio,
                              string matricula
                              )
        {
            this.Nome = nome;
            //this.EstadoId = estadoId;
            this.Ativo = ativo;
            this.EhCivelEstrategico = ehCivelEstrategico;
            this.EhCivel = ehCivel;
            this.EhTrabalhista = ehTrabalhista;
            this.EhJuizado = ehJuizado;
            this.UsuarioId = usuarioId;
            this.EhProcon = ehProcon;
            this.EhPex = ehPex;
            this.EhEscritorio = ehEscritorio;
            this.Matricula = matricula;

            Validate();
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        //public string EstadoId { get; private set; }
        public bool Ativo { get; private set; } = true;
        public bool EhCivelEstrategico { get; private set; } = false;
        public bool EhCivel { get; private set; }
        public bool EhTrabalhista { get; private set; }
        public bool EhJuizado { get; private set; }
        public string? UsuarioId { get; private set; } = null;
        [NotMapped]
        public string NomeUsuario { get; set; }
        [NotMapped]
        public bool? UsuarioAtivo { get; set; } = false;
        public bool EhProcon { get; private set; }
        public bool EhPex { get; private set; }
        public bool EhEscritorio { get; private set; }
        public string? Matricula { get; private set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }
    }
}