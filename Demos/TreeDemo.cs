using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lab2.Trees;

namespace Lab2.Demos
{
    /// <summary>Завдання 3 (узагальнене бінарне дерево) і завдання 4 (порівняння, сортування, ітератор).</summary>
    public static class TreeDemo
    {
        // ======================= ЗАВДАННЯ 3 =======================
        public static void RunTask3()
        {
            Console.WriteLine("################ ЗАВДАННЯ 3: узагальнене бінарне дерево ################");
            Book[] books = SampleData.CreateBooks();

            BinaryTree<Book> tree = new BinaryTree<Book>();
            foreach (Book b in books) tree.Add(b);

            Console.WriteLine($"BinaryTree<Book>: Count = {tree.Count}, Height = {tree.Height()}");
            Console.WriteLine("Дерево заповнюється по рівнях: корінь №305, його діти №102 і №407, далі №210.");
            Console.WriteLine($"Прямий    (preorder):  {Serials(tree.PreOrder())}");
            Console.WriteLine($"Центрований (inorder): {Serials(tree.InOrder())}");
            Console.WriteLine($"Зворотний (postorder): {Serials(tree.PostOrder())}");
            Console.WriteLine($"Contains(№407) = {tree.Contains(books[2])}, Contains(нова книга №999) = " +
                              $"{tree.Contains(new Book(999, "Невідома", 2000, 1m, 1))}");

            Console.WriteLine();
            Console.WriteLine("Те саме дерево для іншого типу — string (узагальненість):");
            BinaryTree<string> words = new BinaryTree<string>();
            foreach (string w in new[] { "дерево", "вузол", "корінь", "лист" }) words.Add(w);
            Console.WriteLine($"   Зворотний обхід: {string.Join(", ", words.PostOrder())}");

            Console.WriteLine();
            Console.WriteLine("Обмеження where T : class не дозволяє структури, тому BinaryTree<int> не компілюється:");
            Console.WriteLine("   // BinaryTree<int> t = new BinaryTree<int>();   // помилка CS0452");
            Console.WriteLine();
        }

        // ======================= ЗАВДАННЯ 4 =======================
        public static void RunTask4()
        {
            Console.WriteLine("################ ЗАВДАННЯ 4: порівняння та власний ітератор ################");
            Book[] books = SampleData.CreateBooks();

            DemoSortingMethods(books);
            DemoSortedTree(books);
            DemoIterator(books);
        }

        private static void DemoSortingMethods(Book[] books)
        {
            Console.WriteLine("--- Сортування через IComparable<T> / IComparer<T> ---");

            List<Book> list = new List<Book>(books);
            list.Sort();                                              // IComparable<Book>: за серійним номером
            Console.WriteLine($"List.Sort()                  (IComparable<Book>, за №):      {Serials(list)}");

            list.Sort(new BookTitleComparer());                       // IComparer<Book>: за назвою
            Console.WriteLine($"List.Sort(BookTitleComparer) (IComparer<Book>, за назвою):   {Titles(list)}");

            Book[] array = (Book[])books.Clone();
            Array.Sort(array, new BookPriceComparer());               // IComparer<Book>: за ціною
            Console.WriteLine($"Array.Sort(BookPriceComparer) (IComparer<Book>, за ціною):   {Prices(array)}");

            ArrayList arrayList = new ArrayList(books);
            arrayList.Sort();                                         // негенеричний IComparable
            Console.WriteLine($"ArrayList.Sort()             (IComparable, за №):            " +
                              $"{string.Join(", ", arrayList.Cast<Book>().Select(b => b.SerialNumber))}");
            Console.WriteLine();
        }

        private static void DemoSortedTree(Book[] books)
        {
            Console.WriteLine("--- SortedBinaryTree<Book>: дерево з порівнянням ---");

            SortedBinaryTree<Book> byNumber = new SortedBinaryTree<Book>();           // IComparable<Book>
            SortedBinaryTree<Book> byTitle = new SortedBinaryTree<Book>(new BookTitleComparer());
            SortedBinaryTree<Book> byPrice = new SortedBinaryTree<Book>(new BookPriceComparer());
            foreach (Book b in books)
            {
                byNumber.Add(b);
                byTitle.Add(b);
                byPrice.Add(b);
            }

            Console.WriteLine($"Порядок додавання:                 {Serials(books)}");
            Console.WriteLine($"За №     (IComparable<T>):         {Serials(byNumber.Sorted())}");
            Console.WriteLine($"За назвою (IComparer<T>):          {Titles(byTitle.Sorted())}");
            Console.WriteLine($"За ціною  (IComparer<T>):          {Prices(byPrice.Sorted())}");
            Console.WriteLine($"Двійковий пошук Contains(№210) = {byNumber.Contains(books[3])}, " +
                              $"Contains(нова книга №999) = {byNumber.Contains(new Book(999, "Невідома", 2000, 1m, 1))}");
            Console.WriteLine($"Повторне Add(№305): {byNumber.Add(books[0])} (дублікати не додаються), Count = {byNumber.Count}");

            try
            {
                new SortedBinaryTree<object>();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Тип без порівняння і без IComparer<T> відхиляється: {ex.Message}");
            }
            Console.WriteLine();
        }

        private static void DemoIterator(Book[] books)
        {
            Console.WriteLine("--- Власний ітератор (IEnumerable<T> + IEnumerator<T>), обхід зворотний (postorder) ---");
            BinaryTree<Book> tree = new BinaryTree<Book>();
            foreach (Book b in books) tree.Add(b);

            Console.WriteLine("foreach по дереву:");
            foreach (Book b in tree) Console.WriteLine("   " + b);

            Console.WriteLine("Те саме вручну через IEnumerator<T>:");
            IEnumerator<Book> enumerator = tree.GetEnumerator();
            while (enumerator.MoveNext()) Console.Write($"   №{enumerator.Current.SerialNumber}");
            Console.WriteLine();

            enumerator.Reset();
            enumerator.MoveNext();
            Console.WriteLine($"Після Reset() та MoveNext() Current = №{enumerator.Current.SerialNumber}");

            bool same = tree.SequenceEqual(tree.PostOrder());
            Console.WriteLine($"Збігається з рекурсивним PostOrder(): {same}");

            SortedBinaryTree<Book> sorted = new SortedBinaryTree<Book>();
            foreach (Book b in books) sorted.Add(b);
            Console.WriteLine($"SortedBinaryTree, foreach (postorder): {Serials(sorted)}");

            try
            {
                foreach (Book b in tree) tree.Add(new Book(999, "Нова", 2000, 1m, 1));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Зміна дерева під час foreach: {ex.Message}");
            }
            Console.WriteLine();
        }

        private static string Serials(IEnumerable<Book> items) => string.Join(", ", items.Select(b => "№" + b.SerialNumber));
        private static string Titles(IEnumerable<Book> items) => string.Join(", ", items.Select(b => b.Title));
        private static string Prices(IEnumerable<Book> items) => string.Join(", ", items.Select(b => b.PricePerCopy.ToString("F2")));
    }
}
