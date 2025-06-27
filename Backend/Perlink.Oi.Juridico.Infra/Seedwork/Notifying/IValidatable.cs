namespace Perlink.Oi.Juridico.Infra.Seedwork.Notifying
{
    public interface IValidatable : INotifiable
    {
        bool Valid { get; }

        bool Invalid { get; }

        void Validate();
    }
}
