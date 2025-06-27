using Oi.Juridico.WebApi.V2.Areas.Processos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.DTOs
{
    public class FiltrosProcessoCriminal
    {
       //Comum
        public short cod_tipo_processo { get; set; }
        public int? cod_escritorio { get; set; }
        public SituacaoProcessoEnum? situacao { get; set; }
        public string? cod_estado { get; set; }
        public long? cod_empresa { get; set; }
        public TipoFiltroDocumentoParteEnum? tipo_filtro_documento_parte { get; set; }
        public string? documento_parte { get; set; }
        public TipoFiltroEnum? tipo_filtro_nome_parte { get; set; }
        public string? nome_parte { get; set; }
        public short? cod_assunto { get; set; }
        public decimal? cod_tipo_participacao { get; set; }
        public TipoCriticidadeEnum? cod_criticidade { get; set; }
        public ProcedimentosChecadosEnum? checado { get; set; }

        //Criminal Judicial
        public TipoFiltroNumeroProcessoEnum? tipo_filtro_numero_processo1 { get; set; }
        public TipoFiltroEnum? tipo_filtro_numero_processo2 { get; set; }
        public string? numero_processo { get; set; }
        public string? cpf_testemunha { get; set; }
        public short? cod_acao { get; set; }
        public short? cod_comarca { get; set; }

        //Criminal Adm
        public TipoFiltroEnum? tipo_filtro_procedimento { get; set; }
        public string? filtro_procedimento { get; set; }
        public long? cod_orgao { get; set; }
        public short? cod_competencia { get; set; }
        public short? cod_municipio { get; set; }
        public short? cod_tipo_procedimento { get; set; }

        //Paginacao
        private int _page = 1;
        private int _size = 10;

        public int page { get { return _page; } set { _page = value; } }
        public int size { get { return _size; } set { _size = value; } }
    }
}
