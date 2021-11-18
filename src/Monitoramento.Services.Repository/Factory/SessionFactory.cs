using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Monitoramento.Services.Repository.Factory
{
    public class SessionFactory
    {
        public ISessionFactory BuildSessionFactory(string connectionString, string namespaceConfig)
        {
            //Repositories -------
            var uowType = typeof(CustomFluentConfiguration);
            var mappingsAssembly = uowType.Assembly;

            var mappings = mappingsAssembly.GetExportedTypes()
                .Where(type => type.Namespace != null && type.Namespace.StartsWith(namespaceConfig)).ToList();

            var dbConfiguration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(connectionString)
                    .IsolationLevel(IsolationLevel.ReadCommitted))
                .AddMappings(mappings)
                .BuildConfiguration().AddProperties(ConfigureProperties());

            return dbConfiguration.BuildSessionFactory();
        }

        private static IDictionary<string, string> ConfigureProperties()
        {
            return new Dictionary<string, string>
            {
                { "query.substitutions", "true = S; false = N" } //conversão de bool pra char nos mapeamentos
            };
        }
    }
}