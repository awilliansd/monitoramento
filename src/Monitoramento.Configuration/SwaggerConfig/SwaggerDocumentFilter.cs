using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Monitoramento.Configuration.SwaggerConfig
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var rpath = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
            {
                rpath.Add(key.Replace("v{version}", swaggerDoc.Info.Version), value);
            }

            swaggerDoc.Paths = rpath;
        }
    }
}