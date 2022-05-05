using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;


namespace dotnetserver
{
    public interface IClientLogService
    {
        void SaveUserLog(ClientLog clientLog, string agent);
    }
    public class ClientLogService : IClientLogService
    {
        public static IConfiguration Configuration { get; set; }
        private readonly ILogger<ClientLogService> _logger;
        private readonly string _clientErrorLogArtifactPath = "Artifacts/ClientErrorLog.txt";
        public ClientLogService(ILogger<ClientLogService> logger, IConfiguration config)
        {
            _logger = logger;
            Configuration = config;
        }

        public void SaveUserLog(ClientLog clientLog, string agent)
        {
            string logMessage = $"{clientLog.Timestamp} LogLevel {clientLog.Level} Message: {clientLog.Message} user uses agent: {agent}\n";
            while (true)
            {
                try
                {
                    File.AppendAllText(_clientErrorLogArtifactPath, logMessage);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

    }
}