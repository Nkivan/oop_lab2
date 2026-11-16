using System;
using System.Collections;

namespace Lab2
{
    /// <summary>Книга (клас, а не структура: за умовою елементами дерева структури бути не можуть).</summary>
    public class Book : IComparable<Book>, IComparable, IEquatable<Book>
    {
        public int SerialNumber { get; }
        public string Title { get; }
        public int PublicationYear { get; }
        public decimal PricePerCopy { get; private set; }
        public int Copies { get; }

        public Book(int serialNumber, string title, int publicationYear, decimal pricePerCopy, int copies)
        {
            if (serialNumber <= 0) throw new ArgumentException("Серійний номер має бути додатним.", nameof(serialNumber));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Назва не може бути порожньою.", nameof(title));
            if (publicationYear < 1450 || publicationYear > DateTime.Now.Year)
                throw new ArgumentException("Некоректний рік видання.", nameof(publicationYear));
            if (pricePerCopy < 0) throw new ArgumentException("Вартість не може бути від'ємною.", nameof(pricePerCopy));
            if (copies < 0) throw new ArgumentException("Кількість примірників не може бути від'ємною.", nameof(copies));

            SerialNumber = serialNumber;
            Title = title;
            PublicationYear = publicationYear;
            PricePerCopy = pricePerCopy;
            Copies = copies;
        }

        /// <summary>Збільшення вартості примірника на певний відсоток.</summary>
        public void IncreasePrice(decimal percent)
        {
            if (percent <= -100) throw new ArgumentException("Відсоток має бути більшим за -100.", nameof(percent));
            PricePerCopy = Math.Round(PricePerCopy * (1 + percent / 100m), 2);
        }

        /// <summary>Загальна вартість тиражу.</summary>
        public decimal TotalCost() => PricePerCopy * Copies;

        /// <summary>Інформація про книгу.</summary>
        public string GetInfo()
            => $"№{SerialNumber}: «{Title}», {PublicationYear} р., {PricePerCopy:F2} грн × {Copies} = {TotalCost():F2} грн";

        public override string ToString() => GetInfo();

        // Рівність за серійним номером: на цьому працюють Contains/Remove/IndexOf у колекціях і масивах.
        public bool Equals(Book other) => other != null && SerialNumber == other.SerialNumber;
        public override bool Equals(object obj) => Equals(obj as Book);
        public override int GetHashCode() => SerialNumber.GetHashCode();

        // Природний порядок — за серійним номером.
        public int CompareTo(Book other) => other == null ? 1 : SerialNumber.CompareTo(other.SerialNumber);

        // Негенеричний IComparable потрібен, наприклад, для ArrayList.Sort().
        int IComparable.CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Book book) return CompareTo(book);
            throw new ArgumentException("Об'єкт має бути типу Book.", nameof(obj));
        }
    }
}
