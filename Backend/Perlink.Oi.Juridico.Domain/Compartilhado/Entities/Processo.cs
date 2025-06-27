using FluentValidation;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Processo : EntityCrud<Processo, long>
    {
        public override AbstractValidator<Processo> Validator => new ProcessoValidator();

       
        public long CodigoTipoProcesso { get; set; }
        public string NumeroProcessoCartorio { get; set; }
        public long? CodigoComarca { get; set; }
        public long? CodigoVara { get; set; }
        public long? CodigoTipoVara { get; set; }
        public long? CodigoProfissonal { get; set; }
        public long? CodigoParteEmpresa { get; set; }
        public long? CodigoEstabelecimento { get; set; }
        public long? CodigoParteOrgao { get; set; }
        public string ResponsavelInterno { get; set; }
        public bool? IndicaProcessoAtivo { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime? DataFinalizacaoContabil { get; set; }
        public string CodigoRiscoPerda { get; set; }
        public DateTime? DataCadastro { get; set; }
        public long? PrePos { get; set; }
        public bool? IndicadorConsiderarProvisao { get; set; }
        public bool? IndicadorMediacao { get; set; }
        public string CodigoClassificacaoProcesso { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public IList<PedidoProcesso> PedidoProcessos { get; set; }
        public Profissional Profissional { get; set; }
        public Comarca Comarca { get; set; }
        public TipoVara TipoVara { get; set; }
        public Vara Vara { get; set; }
        public Parte Parte { get; set; }
        public IList<AudienciaProcesso> AudienciaProcessos { get; set; }
        public List<ParteProcesso> PartesProcessos { get; set; }
        public IList<ContratoProcesso> ContratoProcesso { get; set; }
        public IList<CompromissoProcesso> CompromissoProcessos { get; set; }
        public IList<ParteProcesso> PartesProcesso { get; set; }
        public Usuario Usuario { get; set; }

        public override void PreencherDados(Processo data)
        {
            
            CodigoTipoProcesso = data.CodigoTipoProcesso;
            NumeroProcessoCartorio = data.NumeroProcessoCartorio;
            CodigoComarca = data.CodigoComarca;
            CodigoVara = data.CodigoVara;
            CodigoTipoProcesso = data.CodigoTipoProcesso;
            CodigoTipoVara = data.CodigoTipoVara;
            CodigoProfissonal = data.CodigoProfissonal;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ProcessoValidator : AbstractValidator<Processo>
    {
        public ProcessoValidator()
        {

        }    
    }
}