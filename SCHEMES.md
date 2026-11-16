# Схеми до лабораторної роботи 2 - варіант 2

## 1. Діаграми класів

### 1.1. Загальна діаграма: хто що демонструє

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Program {
        -Main()
    }

    class CollectionsDemo {
        <<static>>
        +Run()
        -DemoGenericList()
        -DemoArrayList()
        -DemoArray()
        -DemoBoxing()
    }

    class TreeDemo {
        <<static>>
        +RunTask3()
        +RunTask4()
        -DemoSortingMethods(books)
        -DemoSortedTree(books)
        -DemoIterator(books)
    }

    class SampleData {
        <<static>>
        +CreateBooks() Book[]
    }

    class Book {
        +int SerialNumber
        +string Title
        +int PublicationYear
        +decimal PricePerCopy
        +int Copies
        +IncreasePrice(percent)
        +TotalCost() decimal
        +GetInfo() string
    }

    class BinaryTree~T~ {
        +Add(item) bool
        +PostOrder() IEnumerable~T~
    }

    class SortedBinaryTree~T~ {
        +Sorted() IEnumerable~T~
    }

    Program ..> Book : виводить інформацію, збільшує вартість
    Program ..> CollectionsDemo : запускає завдання 2
    Program ..> TreeDemo : запускає завдання 3 і 4
    CollectionsDemo ..> SampleData : бере чотири книги
    CollectionsDemo ..> Book : List, ArrayList, масив
    TreeDemo ..> SampleData : бере чотири книги
    TreeDemo ..> BinaryTree~T~ : будує дерево по рівнях
    TreeDemo ..> SortedBinaryTree~T~ : будує впорядковане дерево
    SampleData ..> Book : створює об'єкти
    BinaryTree~T~ <|-- SortedBinaryTree~T~ : успадковує
```

### 1.2. Клас Book і порівняння

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Book {
        +int SerialNumber
        +string Title
        +int PublicationYear
        +decimal PricePerCopy
        +int Copies
        +Book(serialNumber, title, publicationYear, pricePerCopy, copies)
        +IncreasePrice(percent)
        +TotalCost() decimal
        +GetInfo() string
        +Equals(other) bool
        +CompareTo(other) int
    }

    class IComparable~T~ {
        <<interface>>
        +CompareTo(other) int
    }

    class IEquatable~T~ {
        <<interface>>
        +Equals(other) bool
    }

    class IComparer~T~ {
        <<interface>>
        +Compare(x, y) int
    }

    class BookTitleComparer {
        +Compare(x, y) int
    }

    class BookPriceComparer {
        +Compare(x, y) int
    }

    IComparable~T~ <|.. Book : порядок за серійним номером
    IEquatable~T~ <|.. Book : рівність за серійним номером
    IComparer~T~ <|.. BookTitleComparer : порядок за назвою
    IComparer~T~ <|.. BookPriceComparer : порядок за вартістю
    BookTitleComparer ..> Book : порівнює книги
    BookPriceComparer ..> Book : порівнює книги
```

### 1.3. Узагальнене дерево та власний ітератор

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class IEnumerable~T~ {
        <<interface>>
        +GetEnumerator() IEnumerator~T~
    }

    class IEnumerator~T~ {
        <<interface>>
        +T Current
        +MoveNext() bool
        +Reset()
    }

    class IComparer~T~ {
        <<interface>>
        +Compare(x, y) int
    }

    class BinaryTree~T~ {
        +int Count
        +Add(item) bool
        +Contains(item) bool
        +Height() int
        +PreOrder() IEnumerable~T~
        +InOrder() IEnumerable~T~
        +PostOrder() IEnumerable~T~
        +GetEnumerator() IEnumerator~T~
    }

    class SortedBinaryTree~T~ {
        -IComparer~T~ comparer
        +Add(item) bool
        +Contains(item) bool
        +Sorted() IEnumerable~T~
    }

    class TreeNode~T~ {
        +T Value
        +TreeNode Left
        +TreeNode Right
    }

    class PostOrderEnumerator~T~ {
        -Stack stack
        +T Current
        +MoveNext() bool
        +Reset()
        +Dispose()
    }

    IEnumerable~T~ <|.. BinaryTree~T~ : реалізує
    BinaryTree~T~ <|-- SortedBinaryTree~T~ : успадковує і додає порівняння
    BinaryTree~T~ ..> TreeNode~T~ : зберігає вузли
    BinaryTree~T~ ..> PostOrderEnumerator~T~ : створює у GetEnumerator
    IEnumerator~T~ <|.. PostOrderEnumerator~T~ : реалізує
    PostOrderEnumerator~T~ ..> BinaryTree~T~ : читає корінь і лічильник змін
    SortedBinaryTree~T~ ..> IComparer~T~ : порівнює елементи
```

Обмеження `where T : class` стоїть на `BinaryTree<T>`, `SortedBinaryTree<T>` і `PostOrderEnumerator<T>`: структури не можуть бути елементами дерева.

## 2. Структура дерев на прикладі чотирьох книг

Книги додаються в порядку серійних номерів 305, 102, 407, 210.

`BinaryTree`: елементи додаються по рівнях, зліва направо.

```
№305 Кобзар                                  (корінь)
├── ліве:  №102 Лісова пісня
│   ├── ліве:  №210 Захар Беркут
│   └── праве: (порожньо)
└── праве: №407 Тіні забутих предків
```

`SortedBinaryTree` (за серійним номером): менші елементи ліворуч, більші праворуч.

```
№305 Кобзар                                  (корінь)
├── ліве  (менше):  №102 Лісова пісня
│   ├── ліве:  (порожньо)
│   └── праве (більше): №210 Захар Беркут
└── праве (більше): №407 Тіні забутих предків
```

Зворотний обхід (ліве, праве, корінь) обох дерев: №210, №102, №407, №305.
Центрований обхід: для `BinaryTree` - №210, №102, №305, №407, для `SortedBinaryTree` - №102, №210, №305, №407 (відсортовано).

## 3. Алгоритм методу MoveNext власного ітератора

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["MoveNext"]) --> B{"Дерево змінено під час обходу?"}
    B -->|"так"| X["InvalidOperationException"]
    B -->|"ні"| C{"Обхід уже завершено?"}
    C -->|"так"| R0(["Повернути false"])
    C -->|"ні"| D["Спуститися ліворуч від поточного вузла, кладучи вузли у стек"]
    D --> E{"Стек порожній?"}
    E -->|"так"| F["Позначити обхід завершеним"]
    F --> R0
    E -->|"ні"| G["top = вершина стеку, без вилучення"]
    G --> H{"Є праве піддерево, яке ще не пройдене?"}
    H -->|"так"| I["Перейти в праве піддерево, спуститися ліворуч, кладучи вузли у стек"]
    I --> E
    H -->|"ні"| J["Вилучити top зі стеку, запам'ятати як останній відвіданий, Current = значення top"]
    J --> K(["Повернути true"])
```

## 4. Взаємодія об'єктів: foreach по дереву

```mermaid
sequenceDiagram
    participant P as TreeDemo
    participant T as BinaryTree
    participant E as PostOrderEnumerator

    P->>T: foreach - GetEnumerator()
    T->>E: new PostOrderEnumerator(tree)
    E-->>P: ітератор

    loop поки MoveNext повертає true
        P->>E: MoveNext()
        Note over E: спуск ліворуч, перевірка правого піддерева, вилучення зі стеку
        E-->>P: true
        P->>E: Current
        E-->>P: книга
    end

    P->>E: MoveNext()
    E-->>P: false - обхід завершено
    Note over P,E: Якщо дерево змінили під час обходу, буде InvalidOperationException
```
