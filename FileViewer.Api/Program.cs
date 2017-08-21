using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileViewer.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory() + "\\App_Data");
            var baseAddress = string.Format(ConfigurationManager.AppSettings["BaseAddress"]);//"http://*:9000/" for all connections
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Server started at " + baseAddress);
                Console.ReadLine();
            }
        }
    }
}
