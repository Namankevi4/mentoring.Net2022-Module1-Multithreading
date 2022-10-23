/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine(
                "Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task task = Task.Run(() =>
                {
                    int[] arr = Enumerable.Repeat(0, 10).Select(x => new Random().Next(0, 100)).ToArray();
                    Console.WriteLine($"origin array: {string.Join(",", arr)}");
                    return arr;
                })
                .ContinueWith((antecedent) =>
                {
                    var arr = antecedent.Result;
                    var randValue = new Random().Next(0, 100);
                    for (var index = 0; index < arr.Length; index++)
                    {
                        arr[index] = arr[index] * randValue;
                    }

                    Console.WriteLine($"array multiplied by {randValue}: {string.Join(",", arr)}");

                    return arr;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith((antecedent) =>
                {
                    var arr = antecedent.Result;
                    Array.Sort(arr);
                    Console.WriteLine($"sorted array by asc: {string.Join(",", arr)}");

                    return arr;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith((antecedent) =>
                {
                    var arr = antecedent.Result;
                    var average = arr.Sum() / arr.Length;
                    Console.WriteLine($"average: {average}");
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.Wait();

            Console.ReadLine();
        }
    }
}
