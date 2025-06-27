using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
     public sealed class OrientacaoJuridica : Notifiable, IEntity, INotifiable
    {
        private OrientacaoJuridica()
        {
        }
        public static OrientacaoJuridica Criar(int codTipoOrientacaoJuridica, string nome, string descricao, string palavrasChave, bool trabalhista, bool ativo)
        {
            var orientacao = new OrientacaoJuridica()
            {
                Nome = nome.ToString(),
                CodTipoOrientacaoJuridica = codTipoOrientacaoJuridica,
                Descricao = descricao,
                PalavrasChave = palavrasChave,
                EhTrabalhista = trabalhista,
                Ativo = ativo
            };

            orientacao.Validate();
            return orientacao;
        }


        public int CodOrientacaoJuridica { get; private set; }
        public int CodTipoOrientacaoJuridica { get; private set; }
        public TipoOrientacaoJuridica TipoOrientacaoJuridica { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string  PalavrasChave { get; private set; }
        public bool  EhTrabalhista { get; private set; }
        public bool Ativo { get; private set; }

        public void Validate()
        {
            if (!Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }           
        }
        public void Atualizar(int codOrientacaoJuridica, int codTipoOrientacaoJuridica, string nome, string descricao, string palavrasChave, bool trabalhista, bool ativo)
        {
            Nome = nome.ToString();
            CodOrientacaoJuridica = codOrientacaoJuridica;
            Descricao = descricao;
            PalavrasChave = palavrasChave;
            EhTrabalhista = trabalhista;
            Ativo = ativo;
            CodTipoOrientacaoJuridica = codTipoOrientacaoJuridica;
        }

    }
}
