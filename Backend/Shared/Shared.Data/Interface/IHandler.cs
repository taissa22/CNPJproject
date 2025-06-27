
namespace Shared.Data.Interface
{

    ///<summary>
    ///Estrutura utilizada para o DesignPattern de Chain of Responsability
    ///</summary>
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }
}
