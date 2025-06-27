﻿using System;

namespace Shared.Tools
{
    public static class ExcecaoExtensions
    {
        public static string TratarExcecao(this Exception execao)
        {
            while (execao.InnerException != null) execao = execao.InnerException;

            return execao.Message;
        }
    }
}
