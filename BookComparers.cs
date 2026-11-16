using System;
using System.Collections.Generic;

namespace Lab2
{
    public class BookTitleComparer : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.Title, y.Title, StringComparison.CurrentCulture);
        }
    }

    public class BookPriceComparer : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.PricePerCopy.CompareTo(y.PricePerCopy);
        }
    }
}
