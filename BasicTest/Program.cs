using System;
using System.Collections.Generic;

namespace BasicTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int i,j,sum=0,number;
            List<int> AllNum = new List<int>();

            try
            {
                for (j = 1; j < 10; j++)
                {
                    Console.WriteLine("Enter the number");
                    number = Convert.ToInt16(Console.ReadLine());

                    AllNum.Add(number);
                }
                for (i = 0; i <= AllNum[i]; i++)
                {
                    Console.WriteLine($"{i}");
                    sum = sum + AllNum[i];
                }
                Console.WriteLine(sum);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
