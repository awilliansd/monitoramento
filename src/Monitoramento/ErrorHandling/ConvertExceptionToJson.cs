using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Monitoramento.ErrorHandling
{
    public class ConvertExceptionToJson
    {
        private readonly JsonSerializer _serializer;
        private ILogger _logger;

        public ConvertExceptionToJson(ILoggerFactory loggerFactory)
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            
            _logger = loggerFactory.CreateLogger<CustomException>();
        }

        public async Task Invoke(HttpContext context)
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;

            var exception = BuildException(ex);

            var customException = JsonConvert.SerializeObject(exception);
            _logger.LogError(customException);

            await using var writer = new StreamWriter(context.Response.Body);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "Erro interno de sistema.";
            _serializer.Serialize(writer, message);
            await writer.FlushAsync().ConfigureAwait(false);
        }

        private static CustomException BuildException(Exception exception)
        {
            var customException = new CustomException
            {
                App = "Monitoramento",
                Type = exception.GetType().FullName,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
            };

            if (exception.InnerException != null)
                customException.InnerException = BuildException(exception.InnerException);

            return customException;
        }
    }
}