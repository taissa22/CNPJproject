namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs
{
    public class ListarEmpresasDoGrupoCommand
    {
        public string? Login  { get; set; }

        public bool Escritorio { get; set; }

        public bool Contador { get; set; }
    }
}
