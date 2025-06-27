using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;


namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarVaraCommand : Validatable, IValidatable
    {
        public int? VaraId { get;  set; }
        public int ComarcaId { get;  set; }
        public int TipoVaraId { get;  set; }
        public string Endereco { get;  set; }
        public int? OrgaoBBId { get;  set; }


        public override void Validate()
        {
            if (ComarcaId <= 0)
            {
                AddNotification(nameof(ComarcaId), "ComarcaId não pode ser vazio");
            }

            if (VaraId == null)
            {
                AddNotification(nameof(VaraId), "Vara não pode ser vazio");
            }

            if (VaraId.ToString().Length > 3)
            {
                AddNotification(nameof(VaraId), "O campo vara não pode conter mais que 3 caracteres.");
            }

            if (!String.IsNullOrEmpty(Endereco) && Endereco.Length > 50)
            {
                AddNotification(nameof(VaraId), "O campo endereco não pode conter mais que 50 caracteres.");
            }

            if (TipoVaraId <= 0)
            {
                AddNotification(nameof(TipoVaraId), "TipoVaraId não pode ser vazio");
            }
        }
    }
}
