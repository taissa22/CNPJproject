namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository {
    internal class SqliteParameter {
        private string v;
        private string empresa;

        public SqliteParameter(string v, string empresa) {
            this.v = v;
            this.empresa = empresa;
        }
    }
}