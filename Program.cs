using System;
using System.Text;
using Lab2.Demos;

namespace Lab2
{
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Book book = SampleData.CreateBooks()[0];
            Console.WriteLine("################ Клас Book ################");
            Console.WriteLine(book.GetInfo());
            book.IncreasePrice(10);
            Console.WriteLine($"Після збільшення вартості на 10%: {book.GetInfo()}");
            Console.WriteLine($"Загальна вартість тиражу: {book.TotalCost():F2} грн");
            Console.WriteLine();

            CollectionsDemo.Run();
            TreeDemo.RunTask3();
            TreeDemo.RunTask4();

            Console.WriteLine("Натисніть Enter для завершення...");
            Console.ReadLine();
        }
    }
}
