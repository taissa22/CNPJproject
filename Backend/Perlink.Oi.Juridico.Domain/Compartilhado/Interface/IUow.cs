namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface
{
    public interface IUow
    {
        void Commit();

        void Rollback();
    }
}
