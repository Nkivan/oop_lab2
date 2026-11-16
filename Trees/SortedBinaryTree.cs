using System;
using System.Collections.Generic;

namespace Lab2.Trees
{
    /// <summary>
    /// Бінарне дерево з операцією порівняння (завдання 4): елементи розташовуються за правилом
    /// «менші — ліворуч, більші — праворуч». Порівняння — через IComparer&lt;T&gt;
    /// або (якщо його не передано) через IComparable&lt;T&gt; самого елемента.
    /// Центрований обхід (InOrder) повертає елементи у відсортованому порядку.
    /// </summary>
    public class SortedBinaryTree<T> : BinaryTree<T> where T : class
    {
        private readonly IComparer<T> _comparer;

        public SortedBinaryTree() : this(null) { }

        public SortedBinaryTree(IComparer<T> comparer)
        {
            if (comparer == null &&
                !typeof(IComparable<T>).IsAssignableFrom(typeof(T)) &&
                !typeof(IComparable).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException(
                    $"Тип {typeof(T).Name} не реалізує IComparable<{typeof(T).Name}>: передайте IComparer<T>.");
            }
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>Додає елемент на своє місце. Дублікати (Compare == 0) не додаються.</summary>
        public override bool Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (Root == null)
            {
                Root = new TreeNode<T>(item);
            }
            else
            {
                TreeNode<T> current = Root;
                while (true)
                {
                    int cmp = _comparer.Compare(item, current.Value);
                    if (cmp == 0) return false;

                    if (cmp < 0)
                    {
                        if (current.Left == null) { current.Left = new TreeNode<T>(item); break; }
                        current = current.Left;
                    }
                    else
                    {
                        if (current.Right == null) { current.Right = new TreeNode<T>(item); break; }
                        current = current.Right;
                    }
                }
            }
            Count++;
            Version++;
            return true;
        }

        /// <summary>Двійковий пошук: O(висота дерева).</summary>
        public override bool Contains(T item)
        {
            if (item == null) return false;
            TreeNode<T> current = Root;
            while (current != null)
            {
                int cmp = _comparer.Compare(item, current.Value);
                if (cmp == 0) return true;
                current = cmp < 0 ? current.Left : current.Right;
            }
            return false;
        }

        /// <summary>Елементи у відсортованому порядку (за використаним порівнянням).</summary>
        public IEnumerable<T> Sorted() => InOrder();
    }
}
