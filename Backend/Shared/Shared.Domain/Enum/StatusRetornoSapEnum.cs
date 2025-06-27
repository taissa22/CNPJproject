using System.ComponentModel;

namespace Shared.Domain.Enum {
    public enum StatusRetornoSapEnum {
        [Description("Criado")]
        Criado = 1,
        [Description("Não foi criado")]
        NaoFoiCriado = 2
    }
}
