namespace Shared.Application.ViewModel
{
    public class BaseViewModel<TType>
    {
        public TType Id { get; set; }
        public bool SomenteLeitura { get; set; }
    }
}
