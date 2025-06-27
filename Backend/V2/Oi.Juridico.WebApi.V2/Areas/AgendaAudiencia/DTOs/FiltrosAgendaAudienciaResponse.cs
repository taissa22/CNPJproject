using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Interface.IEntity;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudiencia.DTOs
{
    public class FiltrosAgendaAudienciaResponse
    {
        public List<AdvogadoEscritorioResponse>? ListaAdvogado { get; set; }
        public List<ComarcaResponse>? ListaComarca { get; set; }
        public List<EmpresaDoGrupoResponse>? ListaEmpresa { get; set; }
        public List<EscritorioResponse>? ListaEscritorio { get; set; }
        public List<EstadoResponse>? ListaEstado { get; set; }
        public List<PrepostoResponse>? ListaPreposto { get; set; }

        //public FiltrosAgendaAudienciaData Data { get; set; }
        //public bool Sucesso { get; set; }
        //public string Mensagem { get; set; }
        //public string UrlRedirect { get; set; }
        //public bool ExibeNotificacao { get; set; }
    }

    public class FiltrosAgendaAudienciaData
    {
        public List<AdvogadoEscritorioResponse>? ListaAdvogado { get; set; }
        public List<ComarcaResponse>? ListaComarca { get; set; }
        public List<EmpresaDoGrupoResponse>? ListaEmpresa { get; set; }
        public List<EscritorioResponse>? ListaEscritorio { get; set; }
        public List<EstadoResponse>? ListaEstado { get; set; }
        public List<PrepostoResponse>? ListaPreposto { get; set; }
    }

    public class ComarcaResponse : IDualListItem<long>
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }

    public class EmpresaDoGrupoResponse
    {
        public long Id { get; set; }
        public long ParteId { get; set; }
        public string Nome { get; set; }
        public bool Persistido { get; set; }
        public string Descricao { get { return Nome; } }
    }

    public class EstadoResponse
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public bool Persistido { get; set; }
    }

    public class PrepostoResponse : IDualListItem<long>
    {
        public long Id { get; set; }

        public string Descricao { get; set; }
    }

    public class EscritorioResponse
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }

    public class AdvogadoEscritorioResponse : IDualListItem<long>
    {
        public long Id { get; set; }

        public string Descricao { get; set; }

        public long? CodigoInterno { get; set; }
    }

}
