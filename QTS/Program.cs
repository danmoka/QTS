using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите количество заявок: ");
            int requestCount = 0;

            try
            {
                requestCount = int.Parse(Console.ReadLine());
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введенены некорректные данные!");
                return;
            }

            Console.Write("Введите количество каналов: ");
            int channelCount = 0;

            try
            {
                channelCount = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введенены некорректные данные!");
                return;
            }

            if(requestCount > 0 && channelCount > 0)
                QTS.Start(requestCount, channelCount);
            else
                Console.WriteLine();
        }
    }
}
