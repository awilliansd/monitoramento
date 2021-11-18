using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monitoramento.Configuration.SwaggerConfig
{
    public static class SwaggerSettings
    {
        public static void AddConfigurationSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("Api para monitoramento,");
                sb.AppendLine(
                    "  aplicação desenvolvida na plataforma .Net utilizando ASP.NET Core e atualmente hospedadada em container docker.");

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Monitoramento",
                        Version = "v1",
                        Description = sb.ToString(),
                        Contact = new OpenApiContact
                        {
                            Name = "Google",
                            Url = new Uri("http://www.google.com.br")
                        }
                    });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                    var versions = methodInfo.DeclaringType?.GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions!.Any(v => $"v{v.ToString()}" == docName);
                });

                c.OperationFilter<SwaggerOperationFilter>();
                c.DocumentFilter<SwaggerDocumentFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autorização JWT no cabeçalho usando esquema Bearer. Exemplo: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.DescribeAllParametersInCamelCase();

                var rootApp = new DirectoryInfo(AppContext.BaseDirectory);
                var files = rootApp.GetFiles("*.xml");

                foreach (var file in files)
                {
                    c.IncludeXmlComments(file.FullName, true);
                }
            });
        }

        // ReSharper disable once InconsistentNaming
        public static void AddConfigurationSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "monitoramento/v1/swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                        { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "monitoramento/v1/swagger";
                c.SwaggerEndpoint("/monitoramento/v1/swagger/v1/swagger.json", "Monitoramento V1");
            });
        }
    }
}