using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab2.Trees
{
    /// <summary>
    /// Узагальнене бінарне дерево (завдання 3).
    /// Обмеження where T : class забороняє структури (значущі типи) як елементи дерева.
    /// Add додає елемент у перше вільне місце по рівнях (повне бінарне дерево).
    /// </summary>
    public class BinaryTree<T> : IEnumerable<T> where T : class
    {
        protected internal TreeNode<T> Root { get; protected set; }

        /// <summary>Лічильник змін: ітератор за ним помічає зміну дерева під час обходу.</summary>
        protected internal int Version { get; protected set; }

        public int Count { get; protected set; }

        public virtual bool Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            TreeNode<T> node = new TreeNode<T>(item);
            if (Root == null)
            {
                Root = node;
            }
            else
            {
                Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
                queue.Enqueue(Root);
                while (queue.Count > 0)
                {
                    TreeNode<T> current = queue.Dequeue();
                    if (current.Left == null) { current.Left = node; break; }
                    queue.Enqueue(current.Left);
                    if (current.Right == null) { current.Right = node; break; }
                    queue.Enqueue(current.Right);
                }
            }
            Count++;
            Version++;
            return true;
        }

        /// <summary>Пошук за Equals (дерево не впорядковане, тому обхід усіх вузлів).</summary>
        public virtual bool Contains(T item)
        {
            foreach (T value in PreOrder())
                if (Equals(value, item)) return true;
            return false;
        }

        public int Height() => Height(Root);

        private static int Height(TreeNode<T> node)
            => node == null ? 0 : 1 + Math.Max(Height(node.Left), Height(node.Right));

        // ---- Рекурсивні обходи (для порівняння з власним ітератором) ----

        /// <summary>Прямий: корінь, ліве, праве.</summary>
        public IEnumerable<T> PreOrder() => PreOrder(Root);

        /// <summary>Центрований: ліве, корінь, праве.</summary>
        public IEnumerable<T> InOrder() => InOrder(Root);

        /// <summary>Зворотний: ліве, праве, корінь.</summary>
        public IEnumerable<T> PostOrder() => PostOrder(Root);

        private static IEnumerable<T> PreOrder(TreeNode<T> node)
        {
            if (node == null) yield break;
            yield return node.Value;
            foreach (T v in PreOrder(node.Left)) yield return v;
            foreach (T v in PreOrder(node.Right)) yield return v;
        }

        private static IEnumerable<T> InOrder(TreeNode<T> node)
        {
            if (node == null) yield break;
            foreach (T v in InOrder(node.Left)) yield return v;
            yield return node.Value;
            foreach (T v in InOrder(node.Right)) yield return v;
        }

        private static IEnumerable<T> PostOrder(TreeNode<T> node)
        {
            if (node == null) yield break;
            foreach (T v in PostOrder(node.Left)) yield return v;
            foreach (T v in PostOrder(node.Right)) yield return v;
            yield return node.Value;
        }

        // ---- Власний ітератор: foreach обходить дерево у зворотному порядку (варіант 2) ----

        public IEnumerator<T> GetEnumerator() => new PostOrderEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
