using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class borderoRepository : BaseCrudRepository<Bordero, long>, IBorderoRepository
    {
        private readonly JuridicoContext dbContext;

        public borderoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

     
        public async Task<Bordero> RecuperarBordero(long CodigoBordero)
        {
            var Bordero = await context.Set<Bordero>()
              .Where(filtro => filtro.Id.Equals(CodigoBordero))
              .AsNoTracking()
              .FirstOrDefaultAsync();

            return Bordero;

        }
        public async Task<IEnumerable<Bordero>> GetBordero(long codigoLote)
        {
            var retorno = await context.Set<Bordero>()
                                    .Where(p => p.CodigoLote == codigoLote)
                                    .OrderBy(b => b.NomeBeneficiario)
                                    .ToListAsync();
            return retorno;

        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }    

        public async Task CriacaoBordero(IList<BorderoDTO> borderos, Lote lote)
        {
              
            foreach (var borderositem in borderos)
            {
                Bordero bordero = new Bordero()
                {
                    CodigoLote = lote.Id,
                    Id = ++borderositem.Seq_Bordero,
                    NomeBeneficiario = borderositem.NomeBeneficiario,
                    CpfBeneficiario = borderositem.CpfBeneficiario,
                    CnpjBeneficiario = borderositem.CnpjBeneficiario,
                    NumeroBancoBeneficiario = borderositem.NumeroBancoBeneficiario,
                    CidadeBeneficiario = borderositem.CidadeBeneficiario,
                    DigitoBancoBeneficiario = borderositem.DigitoBancoBeneficiario,
                    NumeroAgenciaBeneficiario = borderositem.NumeroAgenciaBeneficiario,
                    DigitoAgenciaBeneficiario = borderositem.DigitoAgenciaBeneficiario,
                    NumeroContaCorrenteBeneficiario = borderositem.NumeroContaCorrenteBeneficiario,
                    DigitoContaCorrenteBeneficiario = borderositem.DigitoContaCorrenteBeneficiario,
                    Valor = borderositem.Valor,
                    Comentario = borderositem.Comentario
                };

                await base.Inserir(bordero);
               
            }
        }
     }
}
