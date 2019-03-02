using DiningPhilosophers.Classes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class TableServer
    {
        private TcpListener _listener;
        private bool _accept = false;

        private DinningTable _table;

        private const int BUFF_SIZE = 1024;
        private const int STD_PORT = 56789;

        public TableServer(int port = STD_PORT, int sizeOfTable = DinningTable.DEFAULT_SIZE)
        {
            _table = new DinningTable(sizeOfTable);
            _listener = new TcpListener(IPAddress.Loopback, 56789);
            
        }

        public async Task Listen()
        {
            _listener.Start();
            _accept = true;

            while (_accept)
            {
                Console.WriteLine("Waiting for client...");
                var client = await _listener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected. Waiting for data.");

                using (var stream = client.GetStream())
                {
                    string command = "";

                    while (command != null && _accept)
                    {
                        byte[] buffer = new byte[BUFF_SIZE];
                        await stream.ReadAsync(buffer, 0, buffer.Length);

                        var response = $"{ProcessCommand(buffer)}\n";
                        await stream.WriteAsync(Encoding.ASCII.GetBytes(response));
                    }
                }
            }
        }

        private string ProcessCommand(byte[] buffer)
        {
            var nullIndex = Array.IndexOf(buffer, (byte)0);
            nullIndex = (nullIndex == -1) ? buffer.Length : nullIndex;
            string command = Encoding.ASCII.GetString(buffer, 0, nullIndex).Trim();

            if (string.IsNullOrWhiteSpace(command))
            {
                return "Empty command - nothing to do";
            }

            if (command.StartsWith("help"))
            {
                var sb = new StringBuilder();
                sb.AppendLine("This is dining philosophers problem server");
                sb.AppendLine($"Current amount of philosophers {_table.Size}");
                sb.AppendLine("Commands: ");

                sb.AppendLine($"\tstartl - start all philosophers.");
                sb.AppendLine($"\tstopl - stop all philosophers.");
                sb.AppendLine($"\tstatusl - get information about all philosophers.");

                sb.AppendLine();

                sb.AppendLine($"\tstartp <indexes of philosophers> - start philosophers with matched indexes");
                sb.AppendLine($"\tstopp <indexes of philosophers> - stop philosophers with matched indexes");

                sb.AppendLine();

                sb.AppendLine($"\thelp - show help");
                sb.AppendLine($"\tstop0 - stop server");
                return sb.ToString();
            }

            if (command.StartsWith("exit0"))
            {
                _accept = false;
                _table.TerminateAll();
                return "Terminating server";
            }

            if (command == "startl")
            {
                _table.StartAll();
                return "All philosophers started";
            }

            if (command == "stopl")
            {
                _table.TerminateAll();
                return "All philosophers stoped";
            }

            if (command == "statusl")
            {
                return _table.GetAllPhilosopherStatus();
            }

            string[] args = command.Split(' ');

            if (args[0] == "startp")
            {
                if (args.Length < 2)
                {
                    return "startp - not enougth args";
                }

                try
                {
                    var idx = new List<int>();
                    for (int i = 1; i < args.Length; ++i)
                    {
                        idx.Add(int.Parse(args[i]));
                    }

                    foreach (var ix in idx)
                    {
                        if (!_table.StartPhilosopher(ix))
                        {
                            return $"No such philosopher {ix}";
                        }
                    }

                    return $"Philosophers {string.Join(' ', idx)} started";
                }
                catch
                {
                    return "startp - incorrect arg";
                }
            }

            if (args[0] == "stopp")
            {
                if (args.Length < 2)
                {
                    return "stopp - not enougth args";
                }

                try
                {
                    var idx = new List<int>();
                    for (int i = 1; i < args.Length; ++i)
                    {
                        idx.Add(int.Parse(args[i]));
                    }

                    foreach (var ix in idx)
                    {
                        if (!_table.TerminatePhilosopher(ix))
                        {
                            return $"No such philosopher {ix}";
                        }
                    }

                    return $"Philosophers {string.Join(' ', idx)} stoped";
                }
                catch
                {
                    return "stopp - incorrect arg";
                }
            }

            return $"Unknown command {command}";
        }
    }
}
