/*

Tetris.NET

MIT License

Copyright (c) 2014 Krzysztof Cieślak

Permission  is hereby granted, free  of charge,  to any person obtaining a copy
of this software and associated documentation files (the "Software"),  to  deal
in  the Software without restriction,  including without limitation the  rights
to  use, copy,  modify,  merge, publish, distribute,  sublicense,  and/or  sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this  permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE  IS PROVIDED  "AS IS",  WITHOUT WARRANTY OF ANY  KIND,  EXPRESS OR
IMPLIED,  INCLUDING  BUT NOT  LIMITED  TO  THE WARRANTIES  OF  MERCHANTABILITY,
FITNESS  FOR  A  PARTICULAR  PURPOSE AND NONINFRINGEMENT.  IN  NO  EVENT  SHALL
THE  AUTHORS  OR  COPYRIGHT  HOLDERS  BE  LIABLE  FOR  ANY  CLAIM,  DAMAGES  OR
OTHER  LIABILITY,  WHETHER  IN  AN  ACTION  OF  CONTRACT,  TORT  OR  OTHERWISE,
ARISING  FROM,  OUT  OF  OR  IN CONNECTION WITH  THE SOFTWARE  OR  THE  USE  OR
OTHER DEALINGS IN THE SOFTWARE.

*/

namespace tetris.NET
{
    internal class Cell
    {
        private Brick _element;

        public bool HasElement { get; private set; }

        public Cell() => Clear();

        public void Set(Brick element)
        {
            if (!Equals(element, default))
            {
                HasElement = true;
                _element = element;
            }
            else
                Clear();
        }

        public void Clear()
        {
            _element = default;
            HasElement = false;
        }

        public Brick Get()
        {
            var result = Peek();
            if (HasElement) Clear();
            return result;
        }

        public Brick Peek() => _element;

        public void Swap(Cell cell)
        {
            var buf = Get();
            Set(cell.Get());
            cell.Set(buf);
        }
    }
}