using System;
using System.Collections.Generic;
using System.Text;

namespace DiningPhilosophers.Classes
{
    class DinningTable
    {
        public const int DEFAULT_SIZE = 5;

        private Chopstick[] _chopsticks;
        private Philosopher[] _philosophers;

        public int Size { get; private set; } = DEFAULT_SIZE;

        private void Init()
        {
            _chopsticks = new Chopstick[Size];

            for (int i = 0; i < Size; ++i)
            {
                _chopsticks[i] = new Chopstick(i);
            }

            _philosophers = new Philosopher[Size];

            for (int i = 0; i < Size; ++i)
            {
                _philosophers[i] = new Philosopher(_chopsticks[i], _chopsticks[(i + 1) % Size], "Phil " + (i + 1));
            }
        }

        public DinningTable()
        {
            Init();
        }

        public DinningTable(int tableSize)
        {
            Size = tableSize;
            Init();
        }

        public bool StartPhilosopher(int index)
        {
            if (index >= Size)
            {
                return false;
            }

            if (_philosophers[index].isRuning)
            {
                return true;
            }

            _philosophers[index].Start();
            return true;
        }

        public void StartAll()
        {
            foreach (var phil in _philosophers)
            {
                if (!phil.isRuning)
                {
                    phil.Start();
                }
            }
        }

        public void TerminateAll()
        {
            foreach (var phil in _philosophers)
            {
                if (phil.isRuning)
                {
                    phil.Terminate();
                }
            }
        }

        public bool TerminatePhilosopher(int index)
        {
            if (index >= Size)
            {
                return false;
            }

            if (!_philosophers[index].isRuning)
            {
                return true;
            }

            _philosophers[index].Terminate();
            return true;
        }

        public bool GetPhilosopherStatus(int index, out string info)
        {
            info = string.Empty;
            
            if (index >= Size)
            {
                return false;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Is running: {_philosophers[index].isRuning}");
            sb.AppendLine(_philosophers[index].GetFrame());

            info = sb.ToString();
            return true;
        }

        public string GetAllPhilosopherStatus()
        {
            var sb = new StringBuilder();

            foreach (var phil in _philosophers)
            {
                sb.AppendLine($"Is running: {phil.isRuning}");
                sb.AppendLine(phil.GetFrame());
            }
            
            return sb.ToString();
        }
    }
}
