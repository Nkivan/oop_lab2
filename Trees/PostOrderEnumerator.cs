using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab2.Trees
{
    
    public class PostOrderEnumerator<T> : IEnumerator<T> where T : class
    {
        private readonly BinaryTree<T> _tree;
        private readonly Stack<TreeNode<T>> _stack = new Stack<TreeNode<T>>();
        private TreeNode<T> _node;     
        private TreeNode<T> _lastVisited;
        private T _current;
        private bool _started;
        private bool _finished;
        private int _version;

        public PostOrderEnumerator(BinaryTree<T> tree)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            Reset();
        }

        public T Current
        {
            get
            {
                if (!_started || _finished)
                    throw new InvalidOperationException("Ітератор не встановлений на елемент (викличте MoveNext).");
                return _current;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_version != _tree.Version)
                throw new InvalidOperationException("Дерево було змінено під час обходу.");
            if (_finished) return false;
            _started = true;

            PushLeftChain();
            while (_stack.Count > 0)
            {
                TreeNode<T> top = _stack.Peek();

    
                if (top.Right != null && _lastVisited != top.Right)
                {
                    _node = top.Right;
                    PushLeftChain();
                    continue;
                }

                _stack.Pop();
                _lastVisited = top;
                _current = top.Value;
                return true;
            }

            _finished = true;
            _current = null;
            return false;
        }

        private void PushLeftChain()
        {
            while (_node != null)
            {
                _stack.Push(_node);
                _node = _node.Left;
            }
        }

        public void Reset()
        {
            _stack.Clear();
            _node = _tree.Root;
            _lastVisited = null;
            _current = null;
            _started = false;
            _finished = false;
            _version = _tree.Version;
        }

        public void Dispose() { }
    }
}
