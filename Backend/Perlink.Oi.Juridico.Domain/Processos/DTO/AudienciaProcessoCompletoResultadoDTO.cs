using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO
{
    public class AudienciaProcessoCompletoResultadoDTO
    {
        public long CodProcesso { get; set; }

        public long SeqAudiencia { get; set; }

        public string SiglaEstado { get; set; }

        public string Comarca { get; set; }

        public int CodVara { get; set; }

        public string TipoVara { get; set; }

        public string DataAudiencia { get; set; }

        public string HorarioAudiencia { get; set; }

        public string TipoAudiencia { get; set; }

        public string Preposto { get; set; }

        public string EscritorioAudiencia { get; set; }

        public string AdvogadoAudiencia { get; set; }

        public string PrepostoAcompanhante { get; set; }

        public string EscritorioAcompanhante { get; set; }

        public string AdvogadoAcompanhante { get; set; }

        public string TipoProcesso { get; set; }

        private string _estrategico;
        public string Estrategico 
        {
            get 
            {
                return _estrategico.EndsWith("S") ? "Sim" : "Não";
            }
            set
            {
                _estrategico = value;
            } 
        }

        public string NumeroProcesso { get; set; }

        public string ClassificacaoHierarquica { get; set; }

        public string EmpresaGrupo { get; set; }

        public string Endereco { get; set; }

        public string EscritorioProcesso { get; set; }

        public string DddAdvogadoEscritorio { get; set; }

        public string NroAdvogadoEscritorio { get; set; }

        public string DddAdvogadoAcompanhante { get; set; }

        public string NroAdvogadoAcompanhante { get; set; }

        private string _migrado;
        public string Migrado
        {
            get
            {
                return _migrado.EndsWith("S") ? "Sim" : "Não";
            }
            set
            {
                _migrado = value;
            }
        }

        public string Reclamadas { get; set; }

        public string Reclamante { get; set; }

        public string Pedido { get; set; }
        
    }
}
