using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class GrupoPedido : Notifiable, IEntity, INotifiable
    {
        private GrupoPedido()
        {
        }

        public static GrupoPedido Criar(int id, string descricao, int tipoprocesso, float multamedia, float toleranciamultamedia)
        {
            var grupoPedido = new GrupoPedido()
            {
                Descricao = descricao,
                MultaMedia = multamedia,
                TipoProcessoId = tipoprocesso,
                ToleranciaMultaMedia = toleranciamultamedia
            };

             grupoPedido.Validate();
            return grupoPedido;
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public int? TipoProcessoId { get; private set; }
        public float? MultaMedia { get; private set; }
        public float? ToleranciaMultaMedia { get; private set; }

        public void Validate()
        {
            if (!Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "A descrição permite no máximo 50 caracteres.");
            }
        }

        public void Atualizar(int id, string descricao, int tipoprocesso, float multamedia, float toleranciamultamedia)
        {
            Id = id;
            Descricao = descricao;
            MultaMedia = multamedia;
            TipoProcessoId = tipoprocesso;
            ToleranciaMultaMedia = toleranciamultamedia;
            Validate();
        }
    }
}