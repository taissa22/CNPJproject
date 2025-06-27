using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Shared.V2.Filters
{
    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new ContentResult { Content = "Task was cancelled", StatusCode = 499 };
            }
        }
    }
}
