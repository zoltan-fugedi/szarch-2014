using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MedievalWarfare.WcfLib;

namespace MedievalWarfare.ServerHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var service = new ServiceHost(typeof(ServerMethods)))
            {
                service.Open();
                try
                {
                    Console.WriteLine(service.BaseAddresses.First().AbsoluteUri);
                }
                catch (Exception)
                {
                    
                    throw;
                }
                Console.ReadKey();
            }

        }
    }
}
