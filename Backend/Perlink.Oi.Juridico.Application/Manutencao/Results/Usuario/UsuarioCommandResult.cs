namespace Perlink.Oi.Juridico.Application.Manutencao.Results.Usuario
{
    public class UsuarioCommandResult
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public string nomeCompleto => this.Ativo ? this.Nome : this.Nome + " [Inativo]";
    }
}