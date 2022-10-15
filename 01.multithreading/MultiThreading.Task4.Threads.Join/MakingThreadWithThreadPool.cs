using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class MakingThreadWithThreadPool
    {
        private Semaphore _semaphoreObject;

        public void Entry(int countOfThreads)
        {
            _semaphoreObject = new Semaphore(1, countOfThreads);
            DoWork(countOfThreads);
        }

        public void DoWork(object countOfThreads)
        {
            var number = (int)countOfThreads;
            _semaphoreObject.WaitOne();
            Console.WriteLine($"task id: {Thread.CurrentThread.ManagedThreadId}, number: {number--}");
            _semaphoreObject.Release();
            if (number != 0)
            {
                ThreadPool.QueueUserWorkItem(DoWork, number);
            }
        }
    }
}
