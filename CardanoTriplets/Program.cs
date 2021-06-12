using System;
using System.Threading;
using System.Threading.Tasks;

namespace CardanoTriplets
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            for (long i = 0; i < Convert.ToInt64(args[0]); ++i)
            {
                Console.Out.WriteLine(
                    "Totale: " + FindTriplets(long.Parse(args[i + 1]))
                );
            }

            Console.ReadKey();
        }

        private static int FindTriplets(long maxBound)
        {
            /*inital equation (a+sqrt((b^2)*c))^(1/3) + (a-sqrt((b^2)*c))^(1/3) for a,b,c integer become: ((a+1)^2)*(8*a-1)=(27*b^2)*c */
            /*from here we get "c"*/

            var totTriplets = 0;

            /*not every natural value of "c" is covered by a value of "a" then we must iterate..*/
            var opt = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.For(0, maxBound, opt, (long b) =>
            {
                Parallel.For(0, maxBound, opt, (long a) =>
                {
                    double c = ((a+1.0)*(a+1.0)*((8.0*a)-1))/(27.0*(b*b));

                    if (a + b + c > maxBound)
                        return;

                    if (c % 1 != 0)
                        return;

                    var innerRoot = Math.Pow(b * b * c, 1.0 / 2.0);
                    double result = Math.Pow(a + innerRoot, 1.0 / 3.0) + Math.Pow(a - innerRoot, 1.0 / 3.0);

                    /*NaN in this case is not a problem*/
                    if (result == 1 || double.IsNaN(result))
                    {
                        Console.WriteLine($"{maxBound}: a:{a} b:{b} c={c}");

                        Interlocked.Increment(ref totTriplets);

                    }
                });
            });

            return totTriplets;
        }
    }
}