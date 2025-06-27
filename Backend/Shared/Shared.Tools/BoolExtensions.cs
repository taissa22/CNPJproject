using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shared.Tools
{
    public static class BoolExtensions
    {
        public static string RetornaSimNao(this bool valor)
        {
            return valor ? "Sim" : "Não";
        }
    }
}
