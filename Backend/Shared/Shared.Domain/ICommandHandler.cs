using Shared.Domain.Commands;

namespace Shared.Domain
{
    public interface ICommandHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }
}
