using System;

namespace Program_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Computing lg lg N:");
            while (true)
            {
                // This gets an input from the user
                Console.Write("\nEnter N: ");
                long n = long.Parse(Console.ReadLine());
                long lg = Lg(n);
                // The log that was just computed gets put back in to find the log of the log
                lg = Lg(lg);
                // The output gets written to the console
                Console.WriteLine("lg lg ({0}) = {1}.", n, lg);
            }
        }

        // This function computes the log of a given N
        static long Lg(long n)
        {
            long x= 0;
            while (n > 1)
            {
                n /= 2;
                x++;
            }

            return x;
        }
    }
 }
