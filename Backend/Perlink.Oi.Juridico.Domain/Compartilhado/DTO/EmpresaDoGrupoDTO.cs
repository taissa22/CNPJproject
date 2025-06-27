namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class EmpresaDoGrupoDTO
    {
        public long Id { get; set; }
        public long ParteId { get; set; }
        public string Nome { get; set; }
        public bool Persistido { get; set; }

        //ATENÇÃO: Um commit alterou a propriedade Descricao para Nome o que gerou quebras em varias telas do front
        // incluimos a propriedade Descricao somente leitura para restaurar as telas quebradas sem maiores danos.
        public string Descricao { get { return Nome; } }
    }
}