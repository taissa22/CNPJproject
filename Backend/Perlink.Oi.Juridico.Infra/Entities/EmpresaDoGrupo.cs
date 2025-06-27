using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EmpresaDoGrupo : Notifiable, IEntity, INotifiable
    {
        
        private const string _centro = "Centro possui mais do que 4 números.";        
        private string _obrigatorio = "Obritatório informar {0}.";
        private const string _interfaceBBNaoPreenchida = "Como o flag de 'gerar arquivo BB' está habilitado, é obrigatório informar a interface BB";        

        private EmpresaDoGrupo()
        {
        }     

        public static EmpresaDoGrupo Criar(DataString razaoSocial, DataString cnpj, int? regionalId, DataString centroSapId, int? empresaCentralizadoraId, int? empresaSapId, DataString endereco, DataString bairro,
            EstadoEnum estado, DataString cidade, DataString cep, DataString telefoneDDD, DataString telefone, DataString faxDDD, DataString fax, int? fornecedorDefault, int? centroCustoId, bool gerarArquivoBB, int? interfaceBB, bool? empRecuperanda, bool empTrio) {
            EmpresaDoGrupo empresaGrupo = new EmpresaDoGrupo()
            {
                TipoParteValor = TipoParte.EMPRESA_DO_GRUPO.Valor,
                CNPJ = cnpj,
                Nome = razaoSocial,
                RegionalId = regionalId,
                CodCentroSap = centroSapId,
                EmpresaCentralizadoraId = empresaCentralizadoraId,
                CodigoEmpresaSap = empresaSapId,
                Endereco = endereco,
                Bairro = bairro,
                EstadoId = estado.Id,
                Cidade = cidade,
                CEP = cep,
                TelefoneDDD = telefoneDDD, 
                Telefone = telefone,
                FaxDDD = faxDDD,
                Fax = fax,
                FornecedorId = fornecedorDefault,
                CentroCustoId = centroCustoId,
                GeraArquivoBB = gerarArquivoBB,
                DiretorioBB = interfaceBB,
                EmpRecuperanda = empRecuperanda,
                EmpTrio = empTrio?"S":"N"
            };
            empresaGrupo.Validate();
            return empresaGrupo;
        }

        public static EmpresaDoGrupo Obter(int id, string nome) {
            EmpresaDoGrupo empresaGrupo = new EmpresaDoGrupo() {
                Id = id,
                Nome = nome
            };
            return empresaGrupo;
        }

        public void Atualizar(DataString razaoSocial, DataString cnpj, int? regionalId, DataString? centroSapId, int? empresaCentralizadoraId, int? empresaSapId, DataString? endereco, DataString? bairro,
            string estado, DataString? cidade, DataString? cep, DataString telefoneDDD, DataString? telefone, DataString faxDDD, DataString? fax, int? fornecedorDefault, int? centroCustoId, bool gerarArquivoBB, int? interfaceBB, bool? empRecuperanda, bool empTrio) {
            CNPJ = cnpj;
            Nome = razaoSocial;
            RegionalId = regionalId;
            CodCentroSap = centroSapId;
            EmpresaCentralizadoraId = empresaCentralizadoraId;
            CodigoEmpresaSap = empresaSapId;
            Endereco = endereco;
            Bairro = bairro;
            EstadoId = estado;
            Cidade = cidade;
            CEP = cep;
            Telefone = telefone;
            TelefoneDDD = telefoneDDD;
            Fax = fax;
            FaxDDD = faxDDD;
            FornecedorId = fornecedorDefault;
            CentroCustoId = centroCustoId;
            GeraArquivoBB = gerarArquivoBB;
            DiretorioBB = interfaceBB;
            EmpRecuperanda = empRecuperanda;
            EmpTrio = empTrio ? "S" : "N";
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        internal string TipoParteValor { get; private set; } = string.Empty;
        public string CNPJ { get; private set; }
        public string TelefoneDDD { get; private set; }
        public string Telefone { get; private set; }
        public string FaxDDD { get; private set; }
        public string Fax { get; private set; }

        public string EstadoId { get; private set; }
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);
        public string Endereco { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Bairro { get; private set; }

        internal int? RegionalId { get; private set; }
        public Regional Regional { get; private set; }

        public string CodCentroSap { get; private set; }

        internal int? CentroCustoId { get; private set; }
        public CentroCusto CentroCusto { get; private set; }

        internal int? FornecedorId { get; private set; }
        public Fornecedor Fornecedor { get; private set; }

        public bool GeraArquivoBB { get; private set; }

        internal int? EmpresaCentralizadoraId { get; private set; }
        public EmpresaCentralizadora EmpresaCentralizadora { get; private set; }

        internal int? CodigoEmpresaSap { get; private set; }
        public EmpresaSap EmpresaSap { get; private set; }

        public int? DiretorioBB { get; private set; }

        internal ParteBase ParteBase { get; private set; }

        public string EmpTrio { get; set; }

        public bool? EmpRecuperanda { get; private set; }

        public void Validate()
        {
            if (Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres.");
            }            

            if (string.IsNullOrEmpty(CodCentroSap)) {
                AddNotification(nameof(CodCentroSap), string.Format(_obrigatorio, "centro."));
            } else if (CodCentroSap.Length > 4) {
                AddNotification(nameof(CodCentroSap), _centro);
            }

            if (RegionalId < 1) {
                AddNotification(nameof(RegionalId), string.Format(_obrigatorio, "regional."));
            }

            if (EmpresaCentralizadoraId < 1) {
                AddNotification(nameof(EmpresaCentralizadoraId), string.Format(_obrigatorio, "empresa centralizadora."));
            }

            if (CodigoEmpresaSap < 1) {
                AddNotification(nameof(CodigoEmpresaSap), string.Format(_obrigatorio, "empresa sap."));
            }

            if (GeraArquivoBB) {
                if (DiretorioBB < 1) {
                    AddNotification(nameof(DiretorioBB), _interfaceBBNaoPreenchida);
                }
            }
        }       
    }
}