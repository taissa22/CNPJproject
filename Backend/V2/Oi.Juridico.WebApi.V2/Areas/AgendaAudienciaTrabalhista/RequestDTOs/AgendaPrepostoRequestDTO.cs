namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.RequestDTOs
{
    public class AgendaPrepostoRequestDTO
    {
        public int CodProcesso { get; set; }
        public short SeqAudiencia { get; set; }        
        public long CodParte { get; set; }
        public int? CodPreposto { get; set; }
        public DateTime DatAudiencia { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErrosDTO) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Any(), mensagensErro);
        }
    }
}
