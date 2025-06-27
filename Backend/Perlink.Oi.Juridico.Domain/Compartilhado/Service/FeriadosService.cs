using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class FeriadosService : BaseCrudService<Feriados, long>, IFeriadosService {
        private readonly IFeriadosRepository repository;

        public FeriadosService(IFeriadosRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<DateTime> ValidarDataNovaParcela(int quantidadeDias) {
            var result = DateTime.Today.AddDays(30);
            
            var dataValida = false;

            while (!dataValida) {
                if (result.DayOfWeek.GetHashCode() == 6) {
                    result = result.AddDays(+1);
                }else if (result.DayOfWeek.GetHashCode() == 0) {
                    result = result.AddDays(+1);
                }else if (repository.Existe(result).Result)
                    result = result.AddDays(+1);
                else {
                    dataValida = true;
                }

            }


            return result;
        }
    }
}
