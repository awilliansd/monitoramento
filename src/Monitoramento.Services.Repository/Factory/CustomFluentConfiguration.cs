using FluentNHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace Monitoramento.Services.Repository.Factory
{
    public static class CustomFluentConfiguration
    {
        public static FluentConfiguration AddMappings(this FluentConfiguration config, List<Type> mappings)
        {
            foreach (var mappin in mappings)
                config.Mappings(m => m.FluentMappings.Add(mappin));

            return config;
        }
    }
}