using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Vara : Notifiable, IEntity, INotifiable
    {
        private Vara()
        {

        }

        public static Vara Criar(int VaraId, int ComarcaId, int TipoVaraId ,string Endereco, int? OrgaoBBId )
        {
            Vara vara = new Vara();
            vara.VaraId = VaraId;
            vara.ComarcaId = ComarcaId;
            vara.TipoVaraId = TipoVaraId;
            vara.Endereco = Endereco;
            vara.OrgaoBBId = OrgaoBBId;

            vara.Validate();
            return vara;
        }

        public void Atualizar(int VaraId, int ComarcaId, int TipoVaraId, string Endereco, int? OrgaoBBId)
        {
            this.VaraId = VaraId;
            this.ComarcaId = ComarcaId;
            this.TipoVaraId = TipoVaraId;
            this.Endereco = Endereco;
            this.OrgaoBBId = OrgaoBBId;

            Validate();
        }

        public int VaraId { get; private set; }      
        public int ComarcaId { get; private set; }       
        public Comarca Comarca { get; private set; }
        public int TipoVaraId { get; private set; } 
        public TipoVara TipoVara { get; private set; } 
        public string Endereco { get; private set; }    
        public int? OrgaoBBId { get; private set; } 
        public OrgaoBB OrgaoBB { get; private set; }
        public int Numero { get { return VaraId; }}

        

    public void Validate()
        {

            if (ComarcaId <= 0)
            {
                AddNotification(nameof(ComarcaId), "ComarcaId não pode ser vazio");
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