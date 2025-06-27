using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Infra.Entities;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra
{
    internal static class IncludesExtensions
    {
        public static IQueryable<Orgao> WithIncludes(this IQueryable<Orgao> orgaos)
        {
            return orgaos.Include(x => x.Competencias);
        }

        public static IQueryable<Usuario> WithIncludes(this IQueryable<Usuario> usuarios)
        {
            return usuarios.Include(x => x.Grupos);
        }

        public static IQueryable<EscritorioDoUsuario> WithIncludes(this IQueryable<EscritorioDoUsuario> usuarios)
        {
            return usuarios.Include(x => x.Escritorio)
                           .Include(x => x.Usuario);
        }

        public static IQueryable<Processo> WithIncludes(this IQueryable<Processo> processos)
        {
            return processos
                .Include(x => x.Lancamentos)
                .Include(x => x.Estabelecimento)
                .Include(x => x.EmpresaDoGrupo)
                .Include(x => x.Procedimento);
        }

        public static IQueryable<Parte> WithIncludes(this IQueryable<Parte> partes)
        {
            return partes;
        }

        public static IQueryable<EmpresaDoGrupo> WithIncludes(this IQueryable<EmpresaDoGrupo> empresasDoGrupo)
        {
            return empresasDoGrupo
                .Include(x => x.Regional)
                .Include(x => x.CentroCusto)
                .Include(x => x.Fornecedor)
                .Include(x => x.EmpresaCentralizadora)
                .Include(x => x.EmpresaCentralizadora.Convenios)
                .Include(x => x.EmpresaSap);
        }

        public static IQueryable<EmpresaCentralizadora> WithIncludes(this IQueryable<EmpresaCentralizadora> empresasCentralizadoras)
        {
            return empresasCentralizadoras
                .Include(x => x.Convenios);
        }

        public static IQueryable<Lancamento> WithIncludes(this IQueryable<Lancamento> lancamentos)
        {
            return lancamentos.Include(x => x.Parte)
                              .Include(x => x.Processo);
        }

        public static IQueryable<LoteLancamento> WithIncludes(this IQueryable<LoteLancamento> lancamentos)
        {
            return lancamentos.Include(x => x.Lancamento);
        }

        public static IQueryable<AdvogadoDoEscritorio> WithIncludes(this IQueryable<AdvogadoDoEscritorio> advogados)
        {
            return advogados.Include(x => x.Escritorio);
        }

        public static IQueryable<AudienciaDoProcesso> WithIncludes(this IQueryable<AudienciaDoProcesso> audiencias)
        {
            return audiencias.Include(x => x.Escritorio)
                             .Include(x => x.TipoAudiencia)
                             .Include(x => x.Preposto)
                             .Include(x => x.AdvogadoEscritorio)
                             .Include(x => x.Processo)
                             .Include(x => x.Processo.Vara)
                             .Include(x => x.Processo.Vara.Comarca)
                             .Include(x => x.Processo.Comarca)
                             .ThenInclude(x => x.Estado)
                             .Include(x => x.Processo.TipoVara)
                             .Include(x => x.Processo.Escritorio);
        }

        public static IQueryable<Comarca> WithIncludes(this IQueryable<Comarca> comarcas)
        {
            return comarcas
                  .Include(x => x.ComarcaBB)
                  .Include(x => x.Estado);
        }
        public static IQueryable<Vara> WithIncludes(this IQueryable<Vara> varas)
        {
            return varas
                  .Include(x => x.OrgaoBB.TribunalBB)
                  .Include(x => x.OrgaoBB.ComarcaBB)
                  .Include(x => x.TipoVara);
        }

        public static IQueryable<FatoGerador> WithIncludes(this IQueryable<FatoGerador> fatosGeradores)
        {
            return fatosGeradores;
        }

        public static IQueryable<DocumentoLancamento> WithIncludes(this IQueryable<DocumentoLancamento> documentosLancamentos)
        {
            return documentosLancamentos;
        }

        public static IQueryable<DocumentoQuitacaoLancamento> WithIncludes(this IQueryable<DocumentoQuitacaoLancamento> documentosQuitacoesLancamentos)
        {
            return documentosQuitacoesLancamentos;
        }

        public static IQueryable<PedidoProcesso> WithIncludes(this IQueryable<PedidoProcesso> pedidosProcessos)
        {
            return pedidosProcessos
                .Include(x => x.Pedido)
                .Include(x => x.Processo);
        }

        public static IQueryable<ParteProcesso> WithIncludes(this IQueryable<ParteProcesso> partesProcessos)
        {
            return partesProcessos
                .Include(x => x.Parte);
        }

        public static IQueryable<FechamentoProvisaoTrabalhista> WithIncludes(this IQueryable<FechamentoProvisaoTrabalhista> source)
        {
            return source;
        }
        
        public static IQueryable<PartePedidoProcesso> WithIncludes(this IQueryable<PartePedidoProcesso> partesPedidosProcesso)
        {
            return partesPedidosProcesso
                .Include(x => x.Processo)
                .Include(x => x.Parte)
                .Include(x => x.Pedido);
        }
        
        public static IQueryable<Procedimento> WithIncludes(this IQueryable<Procedimento> procedimentos)
        {
            return procedimentos
                .Include(x => x.TipoDeParticipacao1)
                .Include(x => x.TipoDeParticipacao2);
        }
        
        public static IQueryable<TipoDocumento> WithIncludes(this IQueryable<TipoDocumento> tiposDocumentos)
        {
            return tiposDocumentos
                .Include(x => x.TipoPrazo);
        }

        public static IQueryable<Cotacao> WithIncludes(this IQueryable<Cotacao> cotacoes)
        {
            return cotacoes
                .Include(x => x.Indice);
        }

        public static IQueryable<PendenciaProcesso> WithIncludes(this IQueryable<PendenciaProcesso> pendenciasProcessos)
        {
            return pendenciasProcessos
                .Include(x => x.TipoPendencia)
                .Include(x => x.Processo);
        }

        public static IQueryable<PrazoProcesso> WithIncludes(this IQueryable<PrazoProcesso> prazosProcessos)
        {
            return prazosProcessos
                .Include(x => x.TipoPrazo)
                .Include(x => x.Processo);
        }        
             
        public static IQueryable<OrientacaoJuridica> WithIncludes(this IQueryable<OrientacaoJuridica> orientacoesJuridicas)
        {
            return orientacoesJuridicas
                .Include(x => x.TipoOrientacaoJuridica);
        }
             
        public static IQueryable<OrgaoBB> WithIncludes(this IQueryable<OrgaoBB> orgaos)
        {
            return orgaos
                .Include(x => x.ComarcaBB)
                .Include(x => x.TribunalBB);
        }

        public static IQueryable<Estado> WithIncludes(this IQueryable<Estado> estados)
        {
            return estados
                .Include(x => x.Municipios);
        }
        
        public static IQueryable<Municipio> WithIncludes(this IQueryable<Municipio> municipios)
        {
            return municipios
                .Include(x => x.Estado);
        }
        public static IQueryable<Pedido> WithIncludes(this IQueryable<Pedido> pedidos)
        {
            return pedidos;
                //.Include(x => x.GrupoPedido);
        }
        public static IQueryable<DecisaoObjetoProcesso> WithIncludes(this IQueryable<DecisaoObjetoProcesso> decisaoObjetoProcessos)
        {
            return decisaoObjetoProcessos;
        }
        public static IQueryable<ValorObjetoProcesso> WithIncludes(this IQueryable<ValorObjetoProcesso> valorObjetoProcessos)
        {
            return valorObjetoProcessos;
        }
        
        public static IQueryable<Esfera> WithIncludes(this IQueryable<Esfera> esfera)
        {
            return esfera;
                              
        }
        public static IQueryable<IndiceCorrecaoEsfera> WithIncludes(this IQueryable<IndiceCorrecaoEsfera> indiceCorrecaoEsfera)
        {
            return indiceCorrecaoEsfera
                 .Include(x => x.Indice); 
        }

        public static IQueryable<Assunto> WithIncludes(this IQueryable<Assunto> assuntos)
        {
            return assuntos;                
                 
        }
        public static IQueryable<UsuarioOperacaoRetroativa> WithIncludes(this IQueryable<UsuarioOperacaoRetroativa> usuarioOperacaoRetroativa)
        {
            return usuarioOperacaoRetroativa
                 .Include(x => x.Usuario);
        }
      

        public static IQueryable<Evento> WithIncludes(this IQueryable<Evento> evento)
        {
            return evento;

        }
        public static IQueryable<DecisaoEvento> WithIncludes(this IQueryable<DecisaoEvento> decisaoEvento)
        {
            return decisaoEvento;
        }
        public static IQueryable<EventoDependente> WithIncludes(this IQueryable<EventoDependente> eventoDependente)
        {
            return eventoDependente;
        }

    }
}