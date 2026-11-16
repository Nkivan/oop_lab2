namespace Lab2
{
    public static class SampleData
    {
        public static Book[] CreateBooks() => new[]
        {
            new Book(305, "Кобзар", 1840, 250.00m, 100),
            new Book(102, "Лісова пісня", 1911, 180.50m, 50),
            new Book(407, "Тіні забутих предків", 1911, 150.00m, 80),
            new Book(210, "Захар Беркут", 1883, 200.00m, 30)
        };
    }
}
