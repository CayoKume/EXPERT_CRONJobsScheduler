﻿using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IntegrationsCore.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScopedSQLServerConnection(this IServiceCollection services)
        {
            services.AddScoped<ISQLServerConnection, SQLServerConnection>();

            return services;
        }
    }
}
