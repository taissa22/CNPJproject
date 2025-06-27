using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

//#nullable enable

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarEmpresaDoGrupoCommand : Validatable, IValidatable
    {
        #region Dados da empresa
        //TODO: verificar o que fazer, reclamou dos ? por diferença de verção do c# retirei 
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public int Regional { get; set; }
        public int EmpresaCentralizadora { get; set; } 
        public string Endereco { get; set; } 
        public string Bairro { get; set; } 
        public string Estado { get; set; } 
        public string Cidade { get; set; } 
        public string Cep { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public string TelefoneDDD { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string FaxDDD { get; set; } = string.Empty;
        public bool EmpRecuperanda { get; set; }
        public bool EmpTrio { get; set; }

        #endregion Dados da empresa

        #region Dados Sap

        public int EmpresaSap { get; set; }

        public int? Fornecedor { get; set; }
        public string CentroSap { get; set; } 

        public int? CentroCusto { get; set; }
        public bool GeraArquivoBB { get; set; }
        public int? InterfaceBB { get; set; }

        #endregion Dados Sap

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome é obrigatório");
            }
            if (!string.IsNullOrEmpty(Nome) && Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres");
            }

            if (string.IsNullOrEmpty(Cnpj))
            {
                AddNotification(nameof(Cnpj), "O CNPJ é obrigatório");
            }

            if (!CNPJ.IsValidForSisjur(Cnpj))
            {
                AddNotification(nameof(Cnpj), "CNPJ inválido");
            }            

            if (!string.IsNullOrEmpty(Endereco) && Endereco.Length > 400) {
                AddNotification(nameof(Endereco), "O campo endereço permite no máximo 400 caracteres");
            }

            if (!string.IsNullOrEmpty(Cep)) {
                if (Cep.Replace("-", "").Length > 8) {
                    AddNotification(nameof(Cep), "O campo CEP permite no máximo 8 caracteres");
                }
            }

            if (!string.IsNullOrEmpty(Bairro) && Bairro.Length > 30) {
                AddNotification(nameof(Bairro), "O campo bairro permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(Cidade) && Cidade.Length > 30) {
                AddNotification(nameof(Cidade), "O campo cidade permite no máximo 30 caracteres");
            }

            if (!string.IsNullOrEmpty(Estado)) {
                var estado = Infra.Enums.EstadoEnum.PorId(Estado);
                if (estado == null || (estado != null && estado.Nome == estado.Id)) {
                    AddNotification(nameof(Estado), "O Estado informado não existe");
                }
            }

            if (!string.IsNullOrEmpty(TelefoneDDD)) {
               
                if (TelefoneDDD.Length > 2) {
                    AddNotification(nameof(TelefoneDDD), "O campo ddd permite no máximo 2 números");
                }
            }

             if (!string.IsNullOrEmpty(Telefone)) {                
                if (Telefone.Length > 9) {
                    AddNotification(nameof(Telefone), "O campo telefone permite no máximo 9 caracteres");
                }                
            }

            if (!string.IsNullOrEmpty(FaxDDD)) {               
                if (FaxDDD.Length > 2) {
                    AddNotification(nameof(FaxDDD), "O campo ddd permite no máximo 2 números");
                }
            }

            if (!string.IsNullOrEmpty(Fax)) {               
                if (Fax.Length > 9) {
                    AddNotification(nameof(Fax), "O campo Fax permite no máximo 9 caracteres");
                }              
            }
        }
    }
}