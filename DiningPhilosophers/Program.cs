using DiningPhilosophers.Classes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = new TableServer();
                server.Listen().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
