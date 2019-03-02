using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiningPhilosophers.Classes
{
    class Chopstick
    {
        public int Id { get; set; }

        private Mutex _mutex;

        public Chopstick(int id)
        {
            Id = id;
            _mutex = new Mutex();
        }

        public bool Get(int timeout = -1)
        {
            return _mutex.WaitOne(timeout);
        }

        public void Put()
        {
            _mutex.ReleaseMutex();
        }
    }
}
