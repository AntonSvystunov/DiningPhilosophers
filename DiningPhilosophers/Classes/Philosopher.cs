using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace DiningPhilosophers.Classes
{
    class Philosopher
    {
        private Thread thread;

        private Chopstick left;
        private Chopstick right;

        private Random random;
        private Stopwatch sw;

        public string Name { get; set; }

        public bool isRuning { get; set; } = false;

        public long EatingTime { get; private set; }

        public long ThinkingTime { get; private set; }

        public long StarvationTime { get; private set; }

        public string LastFrame { get; set; }

        public Philosopher(Chopstick left, Chopstick right, string name = "Philosopher")
        {
            this.left = left;
            this.right = right;

            Name = name;

            random = new Random(Guid.NewGuid().GetHashCode());
            sw = new Stopwatch();
            thread = new Thread(run);
            
        }

        public void Start()
        {
            isRuning = true;
            thread.Start();
        }

        public void Terminate()
        {
            isRuning = false;
            //thread.Interrupt();
            thread.Join();
        }

        private void take()
        {
            sw.Restart();
            sw.Start();

            bool leftTaken = false,
                 rightTaken = false;

            while (!leftTaken || !rightTaken)
            {
                if (!leftTaken)
                {
                    Console.WriteLine($"{Name} tries to get left chopstick");

                    if (left.Get(1000))
                    {
                        leftTaken = true;
                        Console.WriteLine($"{Name} gets left chopstick");
                    }
                    else
                    {
                        Console.WriteLine($"{Name} cound not get left chopstick");
                    }
                }

                if (!rightTaken)
                {
                    Console.WriteLine($"{Name} tries to get right chopstick");
                    if (right.Get(1000))
                    {
                        rightTaken = true;
                        Console.WriteLine($"{Name} gets right chopstick");
                    }
                    else
                    {
                        Console.WriteLine($"{Name} count not get right chopstick");
                    }
                }
            }
                        
            sw.Stop();
            StarvationTime += sw.ElapsedMilliseconds;
            Console.WriteLine($"{Name} got two chopsticks!");
        }

        private void put()
        {
            right.Put();
            Console.WriteLine($"{Name} puts right chopstick");
            
            left.Put();
            Console.WriteLine($"{Name} puts left chopstick");
        }

        private void think()
        {
            Console.WriteLine($"{Name} starts thinking");
            sw.Reset();
            sw.Start();
            var sleepTime = random.Next(1000, 5000);
            Thread.Sleep(sleepTime);
            sw.Stop();
            ThinkingTime += sw.ElapsedMilliseconds;
            Console.WriteLine($"{Name} stops thinking");
        }

        private void eat()
        {
            //if (!isRuning) return;
            Console.WriteLine($"{Name} starts eating");
            sw.Reset();
            sw.Start();
            var sleepTime = random.Next(1000, 5000);
            Thread.Sleep(sleepTime);
            sw.Stop();
            EatingTime += sw.ElapsedMilliseconds;
            Console.WriteLine($"{Name} ends eating");
        }

        private void run()
        {
            while (isRuning)
            {
                think();
                take();                
                eat();                
                put();
            }
            Console.WriteLine($"{Name} stopped!");
            PrintInfo();
        }

        public string GetFrame()
        {
            SaveFrame();
            return LastFrame;
        }

        public void SaveFrame()
        {
            LastFrame = $"{Name}\n\tEating time: {EatingTime} ms\n\tThinking time: {ThinkingTime} ms\n\tStarvation time: {StarvationTime} ms\n\tStarvation per cent: {((EatingTime + ThinkingTime + StarvationTime) > 0 ? 100 * StarvationTime / (EatingTime + ThinkingTime + StarvationTime) : 0)}%";
        }

        public void PrintInfo()
        {
            if (LastFrame == null)
            {
                SaveFrame();
            }
            Console.WriteLine(LastFrame);
        }

        public void SetStatsToZero()
        {
            EatingTime = ThinkingTime = StarvationTime = 0;
            sw.Restart();
        }

    }
}
