using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao.CsvHelperMap
{
    public class DownloadCsvResponseMap : ClassMap<DownloadCsvResponse>
    {
        public DownloadCsvResponseMap()
        {
            Map(row => row.UsuarioSolicitante).Name("Usuário Solicitante");
            Map(row => row.Login).Name("Login");
            Map(row => row.UsuarioSolicitado).Name("Usuário Solicitado");
            Map(row => row.Email).Name("E-mail");
            Map(row => row.Escritorio).Name("Escritório");
            Map(row => row.Origem).Name("Origem");
            Map(row => row.SituacaoOrigemDescricao).Name("Situação Origem");
            Map(row => row.DataHoraSolicitacao).Name("Data/Hora da Solicitação");
            Map(row => row.StatusSolicitacao).Name("Status da Solicitação");
            Map(row => row.AdministradorAprovador).Name("Administrador Aprovador");
            Map(row => row.DataHoraReativacao).Name("Data/Hora da Ampliação");
            Map(row => row.DataValidadeSenha).Name("Data Validade Senha");
            Map(row => row.Perfil).Name("Perfil");
            Map(row => row.GestorResponsavel).Name("Gestor Responsável");
            Map(row => row.Aprovado).Name("Aprovado");
            Map(row => row.DataHora).Name("Data/Hora");
            Map(row => row.Observacao).Name("Motivo da Rejeição");

            Map(row => row.CodProfissional).Ignore();
            Map(row => row.SituacaoOrigem).Ignore();
            Map(row => row.StatusRenovacaoAcesso).Ignore();
            Map(row => row.StatusDaSolicitacaoDeAcessoEnum).Ignore();
        }
    }
}
