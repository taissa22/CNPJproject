using Perlink.Oi.Juridico.Domain.Compartilhado.Interface;

namespace Perlink.Oi.Juridico.Data.Transactions
{
    public class Uow : IUow
    {
        private readonly JuridicoContext _context;

        public Uow(JuridicoContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges(); 
        }

        public void Rollback() { }
    }
}
