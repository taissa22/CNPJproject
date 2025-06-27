using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.EFRepository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Manutencao.EFREpositories
{
    public class JuroCorrecaoProcessoRepository : BaseCrudRepository<JuroCorrecaoProcesso, long>, IJuroCorrecaoProcessoRepository
    {
        private readonly JuridicoContext dbContext;

        public JuroCorrecaoProcessoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            dbContext = context;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarComTipoProcesso(long? codTipoProcesso, 
                                                                                         DateTime? dataInicio, 
                                                                                         DateTime? dataFim,
                                                                                         bool ascendente, string ordenacao,
                                                                                         int pagina, int quantidade)
        {
            return await dbContext.JuroCorrecaoProcesso.Include(x => x.TipoProcesso)
                                  .WhereIf(codTipoProcesso != null, x => x.Id == codTipoProcesso)
                                  .WhereIf((dataInicio.HasValue && dataFim.HasValue), 
                                            x => x.DataVigencia >= dataInicio.Value && x.DataVigencia <= dataFim.Value)
                                  .OrdenarPorPropriedade(ascendente, ordenacao, "DATAVIGENCIA")
                                  .Paginar(pagina, quantidade)
                                  .Select(JuroCorrecaoProcesso => new JuroCorrecaoProcessoDTO() 
                                  { 
                                    CodTipoProcesso = JuroCorrecaoProcesso.Id,
                                    NomTipoProcesso = JuroCorrecaoProcesso.TipoProcesso.Descricao,
                                    DataVigencia = JuroCorrecaoProcesso.DataVigencia.Value,
                                    ValorJuros = JuroCorrecaoProcesso.ValorJuros
                                  }).ToListAsync();
        }

        public async Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarParaExportacaoComTipoProcesso(long? codTipoProcesso,
                                                                                                       DateTime? dataInicio,
                                                                                                       DateTime? dataFim)
        {
            return await dbContext.JuroCorrecaoProcesso.Include(x => x.TipoProcesso)
                                  .WhereIf(codTipoProcesso != null, x => x.Id == codTipoProcesso)
                                  .WhereIf((dataInicio.HasValue && dataFim.HasValue),
                                            x => x.DataVigencia >= dataInicio.Value && x.DataVigencia <= dataFim.Value)
                                  .Select(JuroCorrecaoProcesso => new JuroCorrecaoProcessoDTO()
                                  {
                                      CodTipoProcesso = JuroCorrecaoProcesso.Id,
                                      NomTipoProcesso = JuroCorrecaoProcesso.TipoProcesso.Descricao,
                                      DataVigencia = JuroCorrecaoProcesso.DataVigencia.Value,
                                      ValorJuros = JuroCorrecaoProcesso.ValorJuros
                                  }).ToListAsync();
        }

        public async Task<JuroCorrecaoProcesso> ObterPorChavesCompostas(long codTipoProcesso, DateTime dataVigencia)
        {
            return await dbContext.JuroCorrecaoProcesso
                                  .Where(x => x.Id == codTipoProcesso && x.DataVigencia == dataVigencia)
                                  .FirstOrDefaultAsync();
        }

        public bool VerificarSeDataInseridaEMenorQueACadastrada(long codTipoProcesso, DateTime dataVigencia)
        {
            return dbContext.JuroCorrecaoProcesso
                            .Where(x => x.Id == codTipoProcesso && x.DataVigencia > dataVigencia)
                            .Any();
        }
    }
}

                    