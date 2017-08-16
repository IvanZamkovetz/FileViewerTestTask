using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileViewer.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = string.Format("http://localhost:9000/");//"http://*:9000/" for all connections
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Server started");
                Console.ReadLine();
            }
        }
    }
}
