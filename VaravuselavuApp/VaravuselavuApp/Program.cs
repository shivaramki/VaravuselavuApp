using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VaravuselavuStandard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;

                    builder.AddJsonFile("appsettings.json",
                                            optional: true, reloadOnChange: true)
                                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                            optional: true, reloadOnChange: true)
                                      .AddJsonFile("configuration.json", optional: true, reloadOnChange: true)
                                      .AddJsonFile($"configuration.{env.EnvironmentName}.json",
                                            optional: true, reloadOnChange: true);


                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(
                            new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            builder.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    builder.AddEnvironmentVariables();

                    if (args != null)
                    {
                        builder.AddCommandLine(args);
                    }


                })
                 .UseStartup<Startup>()
                 .Build();


        }
    }
}
