using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace dotnetserver
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            try
            {
                databaseService.CreateDatabase();
                migrationService.ListMigrations();
                migrationService.MigrateUp();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return host;
        }
    }
}