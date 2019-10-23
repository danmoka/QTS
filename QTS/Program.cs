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
            try
            {
                Console.Write("Введите количество часов работы системы: ");
                QTS.WorkingTime = int.Parse(Console.ReadLine());

                Console.Write("Введите количество заявок в час: ");
                int requestCount = int.Parse(Console.ReadLine());

                Console.Write("Введите количество каналов: ");
                int channelCount = int.Parse(Console.ReadLine());

                Console.Write("Сгенерировать случайные данные? Y\\N: ");

                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Введите максимально возможное количество обрабатываемых заявок одним каналом: ");
                    int maxChannelCapacity = int.Parse(Console.ReadLine());
                    if (requestCount > 0 && channelCount > 0 && maxChannelCapacity > 0)
                        QTS.Start(requestCount, channelCount, maxChannelCapacity, null);
                }
                else
                {
                    Console.Write("Введите количество заявок обрабатываемых одним каналом: ");
                    int channelCapacity = int.Parse(Console.ReadLine());

                    Console.Write("Введите время обработки одной заявки в канале (доля от часа, например, 0,5 - 30 [мин]): ");
                    decimal busyTime = decimal.Parse(Console.ReadLine());

                    QTS.Start(requestCount, channelCount, channelCapacity, busyTime);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Что-то пошло не так!");
                return;
            }
        }
    }
}
