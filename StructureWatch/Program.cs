using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace wilhe1m.StructureWatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO use config for this
            OpenBrowser("https://localhost:5001/");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }


        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) {UseShellExecute = true}); // Works ok on windows
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url); // Works ok on linux
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Process.Start("open", url); // Not tested
        }
    }
}