namespace Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO
{
    public class GrupoEmpresaContabilSapDTO
    {
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public string NomeAnterior { get; set; }
        public bool Persistido { get; set; }
        public EmpresaDTO[] EmpresasGrupo { get; set; }
        public int QtdEmpresasIniciaisControle { get; set; }
        public bool Recuperanda { get; set; }
        public bool RecuperandaAnterior { get; set; }
    }
}