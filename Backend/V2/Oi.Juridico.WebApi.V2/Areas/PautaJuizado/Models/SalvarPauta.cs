using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class SalvarPauta
    {
        public string CodParteEmpresa { get; set; }

        public short CodComarca { get; set; }

        public short CodTipoVara { get; set; }

        public short CodVara { get; set; }

        public string DataAudiencia { get; set; }

        public int[] CodPreposto { get; set; }

        public int CodPrepostoPrincipal { get; set; } 

        public SalvarPauta()
        {

        }

        public string PreparaPrepostosParaSqlClauseIn(int[] codPreposto)
        {
            string strSql = String.Empty;

            for (int i = 0; i < codPreposto.Length; i++)
            {
                strSql = strSql + codPreposto[i] + ",";
            }

            return strSql.Substring(0, strSql.Length - 1);
        }

        public string[] PreparaEmpresasGrupo(string codParteEmpresa)
        {
            return codParteEmpresa.Split(","); 
        }

        public SalvarPauta PreparaModeloPorGrupo(SalvarPauta model, GrupoJuizadoVara grupoJuizadoVara)
        {
            model.CodComarca = grupoJuizadoVara.CodComarca;
            model.CodVara = grupoJuizadoVara.CodVara;
            model.CodTipoVara = grupoJuizadoVara.CodTipoVara;

            return model;
        }

        public string FormataIndicaPrepostoPrincipal(long prep, long prepPrincipal)
        {
            return prep == prepPrincipal ? "S" : "N";
        }

    }   
}
