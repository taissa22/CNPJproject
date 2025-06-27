namespace Perlink.Oi.Juridico.Infra.Seedwork.Notifying
{
    public abstract class Validatable : Notifiable, IValidatable
    {
        public bool Valid => !HasNotifications;

        public bool Invalid => !Valid;

        public abstract void Validate();
    }
}
