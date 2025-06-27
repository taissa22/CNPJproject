using Shared.Data.Interface;

namespace Shared.Data.Impl
{
    public abstract class AbstractHandler:IHandler
    {
        private IHandler _nextHandler;
        /// <summary>
        /// Configura o próximo objeto a ser executado, exemplo:
        /// obj1.SetNext(obj2).SetNext(obj3);
        /// </summary>
        /// <returns>Retorna próximo objeto</returns>
        /// <param name="handler"></param>
        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }
        /// <summary>
        /// Esse método deve ser sobrescrito para a implementação da responsabilidade de cada classe.
        /// Deve ser passado o objeto a ser executado.
        /// </summary>
        /// <param name="request"></param>
        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}
