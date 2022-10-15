/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static AutoResetEvent areWrite = new AutoResetEvent(true);
        private static AutoResetEvent areRead = new AutoResetEvent(true);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            int collectionLength = 10;
            var collection = new List<int>();
            
            Task task1 = new Task(() => FillCollection(collection, collectionLength));
            Task task2 = new Task(() => OutputCollection(collection, collectionLength));
            task1.Start();
            task2.Start();

            Console.ReadLine();
        }

        static void FillCollection(List<int> collection, int collectionLength)
        {
            for (int i = 0; i < collectionLength; i++)
            {
                areWrite.WaitOne();
                collection.Add(i);
                areRead.Set();
            }
        }
        static void OutputCollection(List<int> collection, int collectionLength)
        {
            for (int i = 0; i < collectionLength; i++)
            {
                areRead.WaitOne();
                Console.WriteLine(string.Join(", ", collection));
                areWrite.Set();
            }
        }
    }
}
