using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class ContratoProcesso : EntityCrud<ContratoProcesso, long>
    {
        public override AbstractValidator<ContratoProcesso> Validator => new ContratoProcessoValidator();


        public long CodigoProcesso { get; set; }
        public string CodigoEstado { get; set; }
        public long? CodigoMunicipio { get; set; }
        public string NumeroContrato { get; set; }
        public string NumeroContratoInformado { get; set; }
        public string NomeProcurador { get; set; }
        public string CPF_CNPJ_PROCURADOR { get; set; }
        public string NomeAcionista { get; set; }
        public string CPF_CNPJ_ACIONISTA { get; set; }
        public DateTime? DataSolicitacaoRIC { get; set; }
        public DateTime? DataInclusaoRIC { get; set; }
        public string NomeArquivoRIC { get; set; }
        public long? IdTeseInicial { get; set; }
        public DateTime? DataTeseInicial { get; set; }
        public long IdSituacaoContrato { get; set; }
        public long? IdAutoDocumentoGED { get; set; }
        public string NomeLocalidade { get; set; }
        public string DescricaoObservacao { get; set; }
        public IList<ContratoPedidoProcesso> ContratoPedidoProcesso { get; set; }
        public Processo Processo { get; set; }

        //  public IList<Fornecedor> Fornecedores { get; set; }

        public override void PreencherDados(ContratoProcesso data)
        {
            CodigoProcesso = data.CodigoProcesso;
            CodigoEstado = data.CodigoEstado;
            CodigoMunicipio = data.CodigoMunicipio;
            NumeroContrato = data.NumeroContrato;
            NumeroContratoInformado = data.NumeroContratoInformado;
            NomeProcurador = data.NomeProcurador;
            CPF_CNPJ_PROCURADOR = data.CPF_CNPJ_PROCURADOR;
            NomeAcionista = data.NomeAcionista;
            CPF_CNPJ_ACIONISTA = data.CPF_CNPJ_ACIONISTA;
            DataSolicitacaoRIC = data.DataSolicitacaoRIC;
            DataInclusaoRIC = data.DataInclusaoRIC;
            NomeArquivoRIC = data.NomeArquivoRIC;
            IdTeseInicial = data.IdTeseInicial;
            DataTeseInicial = data.DataTeseInicial;
            IdSituacaoContrato = data.IdSituacaoContrato;
            IdAutoDocumentoGED = data.IdAutoDocumentoGED;
            NomeLocalidade = data.NomeLocalidade;
            DescricaoObservacao = data.DescricaoObservacao;

        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class ContratoProcessoValidator : AbstractValidator<ContratoProcesso>
        {
            public ContratoProcessoValidator()
            {
           


            }
        }
    }
}