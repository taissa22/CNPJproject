namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusNegociacoes.DTO
{
    public class DeParaNegociacoesDTO
    {
        public decimal Id { get; set; }

        public short StatusAPPId { get; set; }

        public short? SubstatusAPPId { get; set; }

        public short StatusSisjurId { get; set; }

        //public bool CriaNegociacoes { get; set; }

        public byte TipoProcesso { get; set; }

    }
}
