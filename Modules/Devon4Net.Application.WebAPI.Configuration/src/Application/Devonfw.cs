﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Devon4Net.Application.WebAPI.Configuration.Common;
using Devon4Net.Infrastructure.Common;
using Devon4Net.Infrastructure.Common.Options;
using Devon4Net.Infrastructure.Common.Options.Devon;
using Devon4Net.Infrastructure.Extensions.Helpers;
using Devon4Net.Infrastructure.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Devon4Net.Application.WebAPI.Configuration.Application
{
    public static class Devonfw
    {
        private static IHostBuilder HostBuilder { get; set; }
        private static IConfiguration Configuration { get; set; }
        private static ConfigurationBuilder ConfigurationBuilder { get; set; }

        public static void Configure<T>(string[] args) where T : class
        {
            LoadConfiguration();
            CreateHostBuilder<T>(args);
            HostBuilder.Build().Run();
        }

        public static IWebHostBuilder InitializeDevonFw(this IWebHostBuilder builder)
        {
            LoadConfiguration();
            builder.UseConfiguration(Configuration);
            builder.UseSerilog();

            var useDetailedErrorsKey = Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:UseDetailedErrorsKey"];
            builder.UseSetting(WebHostDefaults.DetailedErrorsKey, useDetailedErrorsKey);

            var useIis = Convert.ToBoolean(Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:UseIIS"],
                System.Globalization.CultureInfo.InvariantCulture);

            if (useIis)
            {
                ConfigureIis(ref builder);
            }
            else
            {
                SetupKestrel.Configure(ref builder, Configuration);
            }

            builder.ConfigureServices(services => services.AddSingleton(Configuration));
            return builder;
        }

        public static void SetupDevonfw(this IServiceCollection services, ref IConfiguration configuration)
        {
            var devonOptions = services.GetTypedOptions<DevonfwOptions>(configuration, DevonFwConst.DevonFwAppSettingsNodeName);

            if (devonOptions == null || string.IsNullOrEmpty(devonOptions.Environment) || devonOptions.Kestrel == null)
            {
                throw new ApplicationException("Please check the devonfw options node in your configuration file");
            }

            services.ConfigureIIS(ref configuration);
            services.SetupKillSwitch(ref configuration);
            services.AddTransient(typeof(IObjectTypeHelper), typeof(ObjectTypeHelper));
            services.AddTransient(typeof(IJsonHelper), typeof(JsonHelper));
        }

        public static void InitializeDevonFw(this IApplicationBuilder app)
        {
            app.UseRequestLocalization();
            app.SetupDevonfwMiddleware();

            bool.TryParse(Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:UseSwagger"], out bool useSwagger);
            if (!useSwagger) return;
            
            var swaggerEndpoint = Configuration["Swagger:Endpoint:Url"];
            var swaggerName = Configuration["Swagger:Endpoint:Name"];
            
            if (!string.IsNullOrEmpty(swaggerEndpoint) && !string.IsNullOrEmpty(swaggerName)) app.ConfigureSwaggerApplication(swaggerEndpoint, swaggerName);
        }

        private static void CreateHostBuilder<T>(string[] args)  where T: class
        {
            HostBuilder = Host.CreateDefaultBuilder(args);
            HostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<T>();
                webBuilder.UseSerilog();
                webBuilder.UseConfiguration(Configuration);

                var useDetailedErrorsKey = Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:UseDetailedErrorsKey"];
                webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, useDetailedErrorsKey);

                var useIis = Convert.ToBoolean(Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:UseIIS"],
                    System.Globalization.CultureInfo.InvariantCulture);

                if (useIis)
                {
                    ConfigureIis(ref webBuilder);
                }
                else
                {
                    SetupKestrel.Configure(ref webBuilder, Configuration);
                }

            });
        }

        private static void ConfigureIis(ref IWebHostBuilder webBuilder)
        {
            webBuilder.UseIISIntegration();
        }

        private static void LoadConfiguration()
        {
            AddConfigurationSettingsFile("appsettings.json", false, true);
            AddConfigurationSettingsFile($"appsettings.{Configuration[$"{DevonFwConst.DevonFwAppSettingsNodeName}:Environment"]}.json", true, true);
            CheckExtraSettingsFiles();
        }

        private static void CheckExtraSettingsFiles()
        {
            Devon4NetLogger.Information($"CheckExtraSettingsFiles Initialized");
            var appSettingsList = new List<string>();
            Configuration.GetSection("ExtraSettingsFiles").Bind(appSettingsList);

            if (!appSettingsList.Any())
            {
                Devon4NetLogger.Information($"CheckExtraSettingsFiles does not contains any settings file to be managed");
                return;
            }

            Devon4NetLogger.Information($"CheckExtraSettingsFiles has detected the global settings : {appSettingsList}");
            ManageSettingsFiles(appSettingsList);
        }

        private static void ManageSettingsFiles(IReadOnlyCollection<string> settingsItemList)
        {
            Devon4NetLogger.Information($"Managing settings global settings files ...");
            if (settingsItemList == null || !settingsItemList.Any())
            {
                Devon4NetLogger.Information($"No global settings files found!");
                return;
            }

            foreach (var settingsItem in settingsItemList)
            {
                if (string.IsNullOrEmpty(settingsItem)) continue;;

                if (Directory.Exists(settingsItem))
                {
                    Devon4NetLogger.Information($"SettingsItem {settingsItem} is a directory. Checking the directory...");
                    ProcessDirectorySettings(settingsItem);
                    Devon4NetLogger.Information($"SettingsItem {settingsItem} processed");
                }
                else
                {
                    if (!File.Exists(settingsItem))
                    {
                        Devon4NetLogger.Error($"The provided settings file '{settingsItem}' does not exists as directory or file");
                        continue;
                    }

                    Devon4NetLogger.Information($"SettingsItem {settingsItem} is a file. Checking the file...");
                    AddConfigurationSettingsFile(settingsItem, true, false);
                    Devon4NetLogger.Information($"SettingsItem {settingsItem} processed");
                }
            }
        }

        private static void ProcessDirectorySettings(string settingsItemDirectory)
        {
            Devon4NetLogger.Information($"ProcessDirectorySettings {settingsItemDirectory}");
            if (string.IsNullOrEmpty(settingsItemDirectory) || string.IsNullOrWhiteSpace(settingsItemDirectory) || !Directory.Exists(settingsItemDirectory)) return;
            var fileNameList = FileOperations.GetFilesFromPath("*", settingsItemDirectory);

            foreach (var fileSettings in fileNameList)
            {
                Devon4NetLogger.Information($"Processing {fileSettings}");
                AddConfigurationSettingsFile(fileSettings, true, false, settingsItemDirectory);
            }
        }

        private static void AddConfigurationSettingsFile(string filename, bool optional, bool reloadOnChange, string defaultDirectory = null)
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrWhiteSpace(filename))
            {
                Devon4NetLogger.Information($"{filename} settings file does NOT exists!!!");
                return;
            }

            SetupConfigurationBuilder();

            var fileName = FileOperations.GetFileFullPath(filename, defaultDirectory);

            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return;

            ConfigurationBuilder.AddJsonFile(filename, optional, reloadOnChange);
            Configuration = ConfigurationBuilder.Build();
        }

        private static void SetupConfigurationBuilder()
        {
            if (ConfigurationBuilder != null) return;
            ConfigurationBuilder = new ConfigurationBuilder();
            ConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        }
    }
}