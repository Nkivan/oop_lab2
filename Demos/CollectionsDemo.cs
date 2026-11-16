using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab2.Demos
{
    
    public static class CollectionsDemo
    {
        public static void Run()
        {
            Console.WriteLine("################ ЗАВДАННЯ 2: колекції та масиви ################");
            DemoGenericList();
            DemoArrayList();
            DemoArray();
            DemoBoxing();
            PrintComparison();
        }

        
        private static void DemoGenericList()
        {
            Header("List<Book> — узагальнена колекція");
            Book[] d = SampleData.CreateBooks();
            List<Book> list = new List<Book> { d[0], d[1], d[2] };

            list.Add(d[3]);
            Console.WriteLine($"Додавання Add(): Count = {list.Count}");

            list.Remove(d[1]);                                        
            Console.WriteLine($"Видалення Remove(№{d[1].SerialNumber}): Count = {list.Count}");

            Book found = list.Find(b => b.SerialNumber == 407);      
            Console.WriteLine($"Пошук Find(407): {found}");
            Console.WriteLine($"Contains(№210) = {list.Contains(d[3])}, IndexOf(№210) = {list.IndexOf(d[3])}");

            found.IncreasePrice(10);                                
            Console.WriteLine($"Оновлення (+10%): {found}");
            list[0] = new Book(305, "Кобзар (нове видання)", 2020, 300m, 120);   
            Console.WriteLine($"Заміна list[0]: {list[0]}");

            Console.WriteLine("Прохід foreach:");
            foreach (Book b in list) Console.WriteLine("   " + b);
        }


        private static void DemoArrayList()
        {
            Header("ArrayList — звичайна (non-generic) колекція");
            Book[] d = SampleData.CreateBooks();
            ArrayList list = new ArrayList { d[0], d[1], d[2] };

            list.Add(d[3]);
            Console.WriteLine($"Додавання Add(): Count = {list.Count}");

            list.Remove(d[1]);
            Console.WriteLine($"Видалення Remove(№{d[1].SerialNumber}): Count = {list.Count}");

            Book found = null;                                     
            foreach (object o in list)
                if (((Book)o).SerialNumber == 407) found = (Book)o;
            Console.WriteLine($"Пошук (з приведенням (Book)): {found}");
            Console.WriteLine($"Contains(№210) = {list.Contains(d[3])}, IndexOf(№210) = {list.IndexOf(d[3])}");

            found.IncreasePrice(10);
            Console.WriteLine($"Оновлення (+10%): {found}");
            list[0] = new Book(305, "Кобзар (нове видання)", 2020, 300m, 120);
            Console.WriteLine($"Заміна list[0]: {(Book)list[0]}");

            Console.WriteLine("Прохід foreach:");
            foreach (Book b in list) Console.WriteLine("   " + b);

            
            list.Add("це не книга");
            Console.WriteLine("Додали рядок у ArrayList — компілятор не заперечив. Повторний прохід:");
            try
            {
                foreach (Book b in list) Console.WriteLine("   " + b.Title);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"   Помилка лише під час виконання: {ex.GetType().Name}");
            }
        }


        private static void DemoArray()
        {
            Header("Book[] — масив");
            Book[] d = SampleData.CreateBooks();
            Book[] arr = { d[0], d[1], d[2] };

            Array.Resize(ref arr, arr.Length + 1);             
            arr[arr.Length - 1] = d[3];
            Console.WriteLine($"Додавання (Array.Resize): Length = {arr.Length}");

            int index = Array.IndexOf(arr, d[1]);                
            Array.Copy(arr, index + 1, arr, index, arr.Length - index - 1);
            Array.Resize(ref arr, arr.Length - 1);
            Console.WriteLine($"Видалення №{d[1].SerialNumber} (зсув + Resize): Length = {arr.Length}");

            Book found = Array.Find(arr, b => b.SerialNumber == 407);
            Console.WriteLine($"Пошук Array.Find(407): {found}");
            Console.WriteLine($"Array.IndexOf(№210) = {Array.IndexOf(arr, d[3])}");

            found.IncreasePrice(10);
            Console.WriteLine($"Оновлення (+10%): {found}");
            arr[0] = new Book(305, "Кобзар (нове видання)", 2020, 300m, 120);
            Console.WriteLine($"Заміна arr[0]: {arr[0]}");

            Console.WriteLine("Прохід for:");
            for (int i = 0; i < arr.Length; i++) Console.WriteLine($"   [{i}] {arr[i]}");
        }

        
        private static void DemoBoxing()
        {
            Header("Упаковка (boxing) та розпакування (unboxing)");
            int x = 42;
            ArrayList nums = new ArrayList();
            nums.Add(x);                             
            int y = (int)nums[0];                 
            Console.WriteLine($"ArrayList: упаковка й розпакування, y = {y}");

            try { long z = (long)nums[0]; Console.WriteLine(z); }
            catch (InvalidCastException)
            {
                Console.WriteLine("Розпакувати в інший тип (long) не можна: InvalidCastException");
            }

            List<int> typed = new List<int> { x };      
            Console.WriteLine($"List<int>: typed[0] = {typed[0]} (без упаковки)");
        }

        private static void PrintComparison()
        {
            Header("Порівняння");
            Console.WriteLine("List<T>   : розмір динамічний, типобезпечний, без приведень і упаковки, зручні Find/Exists/Sort.");
            Console.WriteLine("ArrayList : розмір динамічний, але елементи object: приведення типів, помилки типів — лише у runtime,");
            Console.WriteLine("            для значущих типів (int тощо) — упаковка/розпакування.");
            Console.WriteLine("Масив     : розмір фіксований (додати/видалити = Resize/зсув), найшвидший доступ за індексом,");
            Console.WriteLine("            типобезпечний; пам'ять виділяється одним блоком.");
            Console.WriteLine();
        }

        private static void Header(string text)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {text} ---");
        }
    }
}
