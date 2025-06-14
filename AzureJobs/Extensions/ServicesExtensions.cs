﻿using Application.IntegrationsCore.Interfaces;
using Application.IntegrationsCore.Services;
using Domain.IntegrationsCore.Interfaces;
using Infrastructure.FlashCourier.DependencyInjection;
using Infrastructure.IntegrationsCore.DependencyInjection;
using Infrastructure.IntegrationsCore.Repositorys;
using Infrastructure.Jadlog.DependencyInjection;
using Infrastructure.LinxCommerce.DependencyInjection;
using Infrastructure.LinxMicrovix.Outbound.WebService.DependencyInjection;
using Infrastructure.TotalExpress.DependencyInjection;

namespace AzureJobs.Extensions
{
    public static class ServicesExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScopedSQLServerConnection();

                services.AddAuditServices();
                services.AddScopedLinxCommerceServices();
                services.AddScopedLinxMicrovixServices();
                services.AddScopedB2CLinxMicrovixServices();
                services.AddScopedFlashCourierServices();
                services.AddScopedTotalExpressServices();
                services.AddScopedJadlogServices();
            });

            return builder;
        }

        public static IServiceCollection AddAuditServices(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILoggerService, LoggerService>();

            return services;
        }
    }
}
