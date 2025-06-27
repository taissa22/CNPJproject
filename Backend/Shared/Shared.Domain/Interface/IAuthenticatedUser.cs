namespace Shared.Domain.Interface {
    public interface IAuthenticatedUser
    {
        string Login { get; }
        string Name { get; }
    }
}
