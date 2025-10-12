﻿using Datory;
using Microsoft.Extensions.Configuration;
using System;

namespace XBLMS.Services
{
    public partial interface ISettingsManager
    {
        IConfiguration Configuration { get; set; }
        string ContentRootPath { get; }
        string WebRootPath { get; }
        string Version { get; }
        string VersionName { get; }
        string FrameworkDescription { get; }
        string OSArchitecture { get; set; }
        string OSDescription { get; }
        bool Containerized { get; }
        int CPUCores { get; }
        bool IsProtectData { get; }
        bool IsSafeMode { get; }
        bool IsDisablePlugins { get; }
        string SecurityKey { get; }
        DatabaseType DatabaseType { get; }
        string DatabaseConnectionString { get; }
        IDatabase Database { get; }
        string RedisConnectionString { get; }
        int MaxSites { get; }
        IRedis Redis { get; }
        public string AdminRestrictionHost { get; }
        public string[] AdminRestrictionAllowList { get; }
        public string[] AdminRestrictionBlockList { get; }
        public bool CorsIsOrigins { get; }
        public string[] CorsOrigins { get; }
        string Encrypt(string inputString, string securityKey = null);
        string Decrypt(string inputString, string securityKey = null);
        void SaveSettings(bool isProtectData, bool isSafeMode, bool isDisablePlugins, DatabaseType databaseType, string databaseConnectionString, string redisConnectionString, string adminRestrictionHost, string[] adminRestrictionAllowList, string[] adminRestrictionBlockList, bool corsIsOrigins, string[] corsOrigins);
        void ChangeDatabase(string configFilePath);
        IServiceProvider BuildServiceProvider();
        void Reload();
    }
}
