
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Data.Impl;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ValidacoesEspecificas
{
    #region Tipo Lancamento: Garantias
    public class CategoriaPagamentoGarantias_CC_Trab_Pex_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;


        public CategoriaPagamentoGarantias_CC_Trab_Pex_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }

        //Valida Garantias para os tipos de processos: Civel, Trabalhista e Pex
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorCivel || categoria.IndicadorTrabalhista || categoria.IndicadorPex) &&
                 categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Garantias)
            {
                if (categoria.ClgarCodigoClasseGarantia.HasValue && categoria.ClgarCodigoClasseGarantia != 1 && (categoria.IndicadorEnvioSap || (categoria.CodigoMaterialSap.HasValue && categoria.CodigoMaterialSap != 0)))
                    return $"Os campos Envio Sap e Código Material não podem ser informados quando a Classe de Garantia for diferente de Depósito";
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;
                //if (categoria.ClgarCodigoClasseGarantia.HasValue &&
                //        (categoria.ClgarCodigoClasseGarantia == 1 || categoria.ClgarCodigoClasseGarantia == 3) &&
                //        ((!categoria.GrpcgIdGrupoCorrecaoGar.HasValue || categoria.GrpcgIdGrupoCorrecaoGar.Value == 0)))
                //    return $"Grupo Correção deve ser informado";
                if (!categoria.ClgarCodigoClasseGarantia.HasValue)
                    return "Classe Garantia é obrigatório";

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }

    }

    public class CategoriaPagamento_Garantias_CE_Juizado_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Garantias_CE_Juizado_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }

        //Valida Garantias para os tipos de processos: Civel Estratégico e Juizado
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorCivelEstrategico || categoria.IndicadorJuizado) &&
                 categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Garantias)
            {
                if (categoria.ClgarCodigoClasseGarantia.HasValue && categoria.ClgarCodigoClasseGarantia != 1 && (categoria.IndicadorEnvioSap || categoria.CodigoMaterialSap.HasValue))
                    return $"Os campos Envio Sap e Código Material não podem ser informados quando a Classe de Garantia for diferente de Depósito";
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;
                if (!categoria.ClgarCodigoClasseGarantia.HasValue)
                    return "Classe Garantia é obrigatório";

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    public class CategoriaPagamento_Garantias_Tributarios_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Garantias_Tributarios_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Garantias para os tipos de processos: Tributários Adm e Jud
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorTributarioAdministrativo || categoria.IndicadorTributarioJudicial) &&
                 categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Garantias)
            {
                if (!categoria.ClgarCodigoClasseGarantia.HasValue)
                    return "Classe Garantia é obrigatório";

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
    #endregion Garantias

    #region Tipo de Lançamento: Pagamentos
    public class CategoriaPagamento_Pagamentos_Juizado_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Pagamentos_Juizado_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Pagamentos para os tipos de processos: Juizado
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if (categoria.IndicadorJuizado &&
                categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Pagamentos)
            {
                if (categoria.ClgarCodigoClasseGarantia.HasValue && categoria.ClgarCodigoClasseGarantia != 1 && (categoria.IndicadorEnvioSap || categoria.CodigoMaterialSap.HasValue))
                    return $"Os campos Envio Sap e Código Material não podem ser informados quando a Classe de Garantia é diferente a Depósito";
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;
                if (categoria.IndicadorEnvioSap && categoria.TipoFornecedorPermitido == null)
                    return $"Campo 'Fornecedor Permitido' é Obrigatório quando selecionado o Envia Sap";

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    public class CategoriaPagamento_Pagamentos_CC_CE_Pex_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Pagamentos_CC_CE_Pex_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Pagamentos para os tipos de processos: Cível, Cível Estratégico e Pex

        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorCivel || categoria.IndicadorCivelEstrategico || categoria.IndicadorPex) &&
                categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Pagamentos)
            {
                if (categoria.ClgarCodigoClasseGarantia.HasValue && categoria.ClgarCodigoClasseGarantia != 1 && (categoria.IndicadorEnvioSap || categoria.CodigoMaterialSap.HasValue))
                    return $"Os campos Envio Sap e Código Material não podem ser informados quando a Classe de Garantia for diferente de Depósito";
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    public class CategoriaPagamento_Pagamentos_Trabalhista_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Pagamentos_Trabalhista_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Pagamentos para os tipos de processos: Trabalhista
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorTrabalhista) &&
                categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.Pagamentos)
            {
                if (categoria.ClgarCodigoClasseGarantia.HasValue && categoria.ClgarCodigoClasseGarantia != 1 && (categoria.IndicadorEnvioSap || categoria.CodigoMaterialSap.HasValue))
                    return $"Os campos Envio Sap e Código Material não podem ser informados quando a Classe de Garantia for diferente de Depósito";
                if (categoria.Id != 0)
                    return service.ValidaHistorica(categoria).Result;
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    public class CategoriaPagamento_Pagamento_A_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Pagamento_A_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            return service.PagamentoAIsValido(categoria).Result;
        }
    }

    #endregion Pagamentos

    #region Tipo Lançamento: Despesas
    public class CategoriaPagamento_Despesas_CE_Trab_PEX_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Despesas_CE_Trab_PEX_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Pagamentos para os tipos de processos: Cível Estratégico, Trabalhista e Pex
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorCivelEstrategico || categoria.IndicadorTrabalhista || categoria.IndicadorPex) &&
                 (categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.DespesasJudiciais))
            {
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;


                return "";
            }
            else
            {
                return base.Handle(request);
            }

        }

    }
    public class CategoriaPagamento_Despesas_CC_Juizado_Handler : AbstractHandler
    {
        private readonly ICategoriaPagamentoService service;
        public CategoriaPagamento_Despesas_CC_Juizado_Handler(ICategoriaPagamentoService service)
        {
            this.service = service;
        }
        //Valida Pagamentos para os tipos de processos: Cível Consumidor, Juizado
        public override object Handle(object request)
        {
            var categoria = (CategoriaPagamento)request;
            if ((categoria.IndicadorCivel || categoria.IndicadorJuizado) &&
                 (categoria.CodigoTipoLancamento == (long)TipoLancamentoEnum.DespesasJudiciais))
            {
                if (categoria.Id != 0)
                    return service.EnvioSapIsValido(categoria).Result;


                if (categoria.IndicadorEnvioSap == true && categoria.TipoFornecedorPermitido == null)
                    return $"Campo 'Fornecedor Permitido' é Obrigatório";

                return "";
            }
            else
            {
                return base.Handle(request);
            }
        }

    }
    #endregion Despesas

}

