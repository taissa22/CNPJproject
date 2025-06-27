namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2500InfocontratoDTO
    {
        public int IdF2500 { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public byte? InfocontrTpcontr { get; set; }
        public string InfocontrIndcontr { get; set; } = string.Empty;
        public DateTime? InfocontrDtadmorig { get; set; }
        public string InfocontrIndreint { get; set; } = string.Empty;
        public string InfocontrIndcateg { get; set; } = string.Empty;
        public string InfocontrIndnatativ { get; set; } = string.Empty;
        public string InfocontrIndmotdeslig { get; set; } = string.Empty;
        public string InfocontrIndunic { get; set; } = string.Empty;
        public string InfocontrMatricula { get; set; } = string.Empty;
        public short? InfocontrCodcateg { get; set; }
        public DateTime? InfocontrDtinicio { get; set; }
        public long IdEsF2500Infocontrato { get; internal set; }
        public string DescricaoTipoContrato { get; set; } = string.Empty;
        public string DescricaoCategoria { get; set; } = string.Empty;
        public string InfocomplCodcbo { get; set; } = string.Empty;
        public byte? InfocomplNatatividade { get; set; }
        public byte? InfovincTpregtrab { get; set; }
        public byte? InfovincTpregprev { get; set; }
        public DateTime? InfovincDtadm { get; set; }
        public decimal? InfovincTmpparc { get; set; }
        public byte? DuracaoTpcontr { get; set; }
        public DateTime? DuracaoDtterm { get; set; }
        public string DuracaoClauassec { get; set; } = string.Empty;
        public string DuracaoObjdet { get; set; } = string.Empty;
        public byte? SucessaovincTpinsc { get; set; }
        public string SucessaovincNrinsc { get; set; } = string.Empty;
        public string SucessaovincMatricant { get; set; } = string.Empty;
        public DateTime? SucessaovincDttransf { get; set; }
        public DateTime? InfodesligDtdeslig { get; set; }
        public string InfodesligMtvdeslig { get; set; } = string.Empty;
        public DateTime? InfodesligDtprojfimapi { get; set; }
        public DateTime? InfotermDtterm { get; set; }
        public string InfotermMtvdesligtsv { get; set; } = string.Empty;
        public byte? IdeestabTpinsc { get; set; }
        public string IdeestabNrinsc { get; set; } = string.Empty;
        public string InfovlrCompini { get; set; } = string.Empty;
        public string InfovlrCompfim { get; set; } = string.Empty;
        public decimal? InfovlrVrremun { get; set; }
        public decimal? InfovlrVrapi { get; set; }
        public decimal? InfovlrVr13api { get; set; }
        public decimal? InfovlrVrinden { get; set; }
        public decimal? InfovlrVrbaseindenfgts { get; set; }
        public string InfovlrPagdiretoresc { get; set; } = string.Empty;
        public string IndProprioTerceiro { get; set; } = string.Empty;
        public decimal? InfovincPensalim { get; set; }
        public decimal? InfovincPercaliment { get; set; }
        public decimal? InfovincVralim { get; set; }
        public DateTime? InfoprocjudDtsent { get; set; }
        public bool? InfovlrIdensd { get; set; }
        public bool? InfovlrIdenabono { get; set; }
        public int? InfovlrIndreperc { get; set; }
    }
}
