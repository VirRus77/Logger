using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Logger.Web
{
    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)
                ?? throw new NullReferenceException();
            //path = Path.Combine(path, "wwwroot");

            CreateHostBuilder(args)
                .UseContentRoot(path)
                .Build()
                .Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder => { webBuilder.UseStartup<Startup>(); }
                );
    }
}
