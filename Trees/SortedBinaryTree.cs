using System;
using System.Collections.Generic;

namespace Lab2.Trees
{
    
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

        public IEnumerable<T> Sorted() => InOrder();
    }
}
