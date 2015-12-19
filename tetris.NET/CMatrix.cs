/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System;

namespace tetris.NET
{
    public class CMatrix<T>
    {
        private readonly Cell<T>[,] _matrix;

        public int RowsCount => _matrix.GetLength(1);

        public int ColumnsCount => _matrix.GetLength(0);

        public Cell<T> this[int column, int row]
        {
            get { return _matrix[column, row]; }
            private set { _matrix[column, row] = value; }
        }

        public CMatrix(int columns, int rows)
        {
            if (columns == 0 || rows == 0)
                throw new ArgumentException("Columns and Rows cannot be zero.");

            _matrix = new Cell<T>[columns, rows];

            for (var colI = 0; colI < columns; colI++)
                for (var rowI = 0; rowI < rows; rowI++)
                    _matrix[colI, rowI] = new Cell<T>();
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

        public CMatrix<T> RotateCW90()
        {
            var result = new CMatrix<T>(RowsCount, ColumnsCount);

            for (var row = 0; row < RowsCount; row++)
                for (var column = 0; column < ColumnsCount; column++)
                    result[RowsCount - 1 - row, column] = _matrix[column, row];

            return result;
        }

        public bool CanOverlay(CMatrix<T> subMatrix, int posCol, int posRow)
        {
            if (subMatrix == null)
                throw new ArgumentNullException();

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

        public void Merge(CMatrix<T> subMatrix, int posCol, int posRow)
        {
            for (var row = 0; row < subMatrix.RowsCount; row++)
                for (var column = 0; column < subMatrix.ColumnsCount; column++)
                    if (subMatrix[column, row].HasElement)
                        this[column + posCol, row + posRow].Swap(subMatrix[column, row]);
        }

        public bool FillFirstNotFullRow(T element)
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