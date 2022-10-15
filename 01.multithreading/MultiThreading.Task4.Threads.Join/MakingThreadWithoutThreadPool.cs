using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class MakingThreadWithoutThreadPool
    {
        public void Entry(int countOfThreads)
        {
           DoWork(countOfThreads);
        }

        public void DoWork(object countOfThreads)
        {
            var number = (int) countOfThreads;
            Console.WriteLine($"task id: {Thread.CurrentThread.ManagedThreadId}, number: {number--}");
            if (number != 0)
            {
                Thread thread = new Thread(DoWork);
                thread.Start(number);
                thread.Join();
            }
        }
    }
}
