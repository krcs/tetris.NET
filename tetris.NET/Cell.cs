/*
 tetris.NET
 Author: Krzysztof Cieślak (K!)  
 */
namespace tetris.NET
{
    public class Cell<T>
    {
        private T _element;

        public bool HasElement { get; private set; }

        public Cell()
        {
            Clear();
        }

        public Cell(T element)
        {
            Set(element);
        }

        public void Set(T element)
        {
            if (!Equals(element, default(T)))
            {
                HasElement = true;
                _element = element;
            }
            else
                Clear();
        }

        public void Clear()
        {
            _element = default(T);
            HasElement = false;
        }

        public T Get()
        {
            var result = Peek();
            if (HasElement) Clear();
            return result;
        }

        public T Peek()
        {
            return _element;
        }

        public void Swap(Cell<T> cell)
        {
            if (!Equals(cell, default(T)))
            {
                var buf = Get();
                Set(cell.Get());
                cell.Set(buf);
            }
            else
                Clear();
        }
    }
}