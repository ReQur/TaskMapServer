using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dotnetserver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                            serverOptions.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                            serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                            serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                            serverOptions.ConfigureHttpsDefaults(listenOptions =>
                            {
                                listenOptions.SslProtocols = SslProtocols.Tls12;
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}
