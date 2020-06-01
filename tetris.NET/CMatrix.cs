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
    internal class CMatrix
    {
        private readonly Cell[,] _matrix;

        public int RowsCount => _matrix.GetLength(1);

        public int ColumnsCount => _matrix.GetLength(0);

        public Cell this[int column, int row]
        {
            get => _matrix[column, row]; 
            private set => _matrix[column, row] = value; 
        }

        public CMatrix(int columns, int rows)
        {
            _matrix = new Cell[columns, rows];

            for (var colI = 0; colI < columns; colI++)
                for (var rowI = 0; rowI < rows; rowI++)
                    _matrix[colI, rowI] = new Cell();
        }

        public void Clear() 
        {
            foreach (var cell in _matrix) cell.Clear();
        }

        public int ClearAndMoveFullRows()
        {
            var rowsCleared = 0;
            for (var row = 0; row < RowsCount; row++)
                if (IsFullRow(row))
                {
                    rowsCleared++;
                    ClearRow(row);
                    MoveContentDown(row);
                }
            return rowsCleared;
        }

        public bool IsFullRow(int rowNumber)
        {
            for (var column = 0; column < ColumnsCount; column++)
                if (!_matrix[column, rowNumber].HasElement)
                    return false;
            return true;
        }

        public void ClearRow(int rowNumber)
        {
            for (var column = 0; column < ColumnsCount; column++)
                _matrix[column, rowNumber].Clear();
        }

        public void MoveContentDown(int freeRow)
        {
            for (var rowI = freeRow; rowI > 0; rowI--)
                SwapRows(rowI, rowI - 1);
        }

        public void SwapRows(int rowA, int rowB)
        {
            for (var colI = 0; colI < ColumnsCount; colI++)
                _matrix[colI, rowA].Swap(_matrix[colI, rowB]);
        }

        public CMatrix RotateCW90()
        {
            var result = new CMatrix(RowsCount, ColumnsCount);

            for (var row = 0; row < RowsCount; row++)
                for (var column = 0; column < ColumnsCount; column++)
                    result[RowsCount - 1 - row, column] = _matrix[column, row];

            return result;
        }

        public bool CanOverlay(CMatrix subMatrix, int posCol, int posRow)
        {
            for (var row = 0; row < subMatrix.RowsCount; row++)
                for (var column = 0; column < subMatrix.ColumnsCount; column++)
                    if (subMatrix[column, row].HasElement)
                    {
                        if (column + posCol < 0 || posCol + column >= ColumnsCount)
                            return false;

                        if (row + posRow < 0 || posRow + row >= RowsCount)
                            return false;

                        if (this[column + posCol, row + posRow].HasElement)
                            return false;
                    }
            return true;
        }

        public void Merge(CMatrix subMatrix, int posCol, int posRow)
        {
            for (var row = 0; row < subMatrix.RowsCount; row++)
                for (var column = 0; column < subMatrix.ColumnsCount; column++)
                    if (subMatrix[column, row].HasElement)
                        this[column + posCol, row + posRow].Swap(subMatrix[column, row]);
        }

        public bool FillFirstNotFullRow(Brick element)
        {
            var full = true;
            var row = 0;
            while (row < RowsCount)
            {
                for (var column = 0; column < ColumnsCount; column++)
                {
                    if (_matrix[column, row].HasElement) continue;
                    _matrix[column, row].Set(element);
                    full = false;
                }
                if (!full) break;
                row++;
            }
            return row == RowsCount;
        }
    }
}