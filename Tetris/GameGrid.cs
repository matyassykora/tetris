namespace Tetris;

public class GameGrid
{
    private readonly int[,] _gameGridArray;

    public GameGrid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        _gameGridArray = new int[rows, columns];
    }

    public int Rows { get; }
    public int Columns { get; }

    public int this[int row, int column]
    {
        get => _gameGridArray[row, column];
        set => _gameGridArray[row, column] = value;
    }

    public bool IsInsideGrid(int row, int column)
    {
        return row >= 0 && row < Rows && column >= 0 && column < Columns;
    }

    public bool IsEmpty(int row, int column)
    {
        return IsInsideGrid(row, column) && _gameGridArray[row, column] == 0;
    }

    public bool IsRowFull(int row)
    {
        for (var i = 0; i < Columns; i++)
            if (_gameGridArray[row, i] == 0)
                return false;

        return true;
    }

    public bool IsRowEmpty(int row)
    {
        for (var i = 0; i < Columns; i++)
            if (_gameGridArray[row, i] != 0)
                return false;

        return true;
    }

    private void ClearRow(int row)
    {
        for (var i = 0; i < Columns; i++) _gameGridArray[row, i] = 0;
    }

    private void ShiftRowDown(int row, int numberOfRows)
    {
        for (var i = 0; i < Columns; i++)
        {
            _gameGridArray[row + numberOfRows, i] = _gameGridArray[row, i];
            _gameGridArray[row, i] = 0;
        }
    }

    public int ClearFullRows()
    {
        var cleared = 0;

        for (var i = Rows - 1; i >= 0; i--)
            if (IsRowFull(i))
            {
                ClearRow(i);
                cleared++;
            }
            else if (cleared > 0)
            {
                ShiftRowDown(i, cleared);
            }

        return cleared;
    }
}