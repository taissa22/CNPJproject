using System;

namespace Shared.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OrdemExecucao : Attribute
    {
        public OrdemExecucao(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
