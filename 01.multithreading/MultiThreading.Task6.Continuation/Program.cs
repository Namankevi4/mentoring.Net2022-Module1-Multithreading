/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var task1 = Task.Run(() => {
                Console.WriteLine("Run task for case 1");
            });

            task1.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Continuation task is executing regardless of the result of the parent task");
            });

            var task2 = Task.Run(() =>
            {
                Console.WriteLine("Run task for case 2");
                throw new Exception();
            });

            task2.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Continuation task is executing when the parent task was completed without success");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var task3 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Run task for case 3");
                Thread.CurrentThread.Name = "task3 Name";
                throw new Exception();
            });

            task3.ContinueWith((antecedent) =>
            {
                Console.WriteLine(Thread.CurrentThread.Name);
                Console.WriteLine("Continuation task is executing when the parent task failed and parent task thread should be reused for continuation");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            var ts = new CancellationTokenSource();

            CancellationToken ct = ts.Token;

            var task4 = Task.Run(() =>
            {
                Console.WriteLine("Run task for case 4");
                ct.ThrowIfCancellationRequested();
            }, ct);

            task4.ContinueWith((antecedent) =>
            {
                var isThreadPool = Thread.CurrentThread.IsThreadPoolThread;
                Console.WriteLine("Continuation task is executing outside of the thread pool when the parent task is cancelled");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            ts.Cancel();
            Console.ReadLine();
        }
    }
}
