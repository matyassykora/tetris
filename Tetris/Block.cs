using System.Collections.Generic;

namespace Tetris;

public abstract class Block
{
    private readonly Position _offset;

    private int _rotationState;

    public Block()
    {
        _offset = new Position(StartOffset.Row, StartOffset.Column);
    }

    public abstract int Id { get; }


    protected abstract Position[][] Tiles { get; }
    protected abstract Position StartOffset { get; }

    public IEnumerable<Position> TilePositions()
    {
        foreach (var position in Tiles[_rotationState])
            yield return new Position(position.Row + _offset.Row, position.Column + _offset.Column);
    }

    public void RotateClockwise()
    {
        _rotationState = (_rotationState + 1) % Tiles.Length;
    }

    public void RotateCounterclockwise()
    {
        if (_rotationState == 0)
            _rotationState = Tiles.Length - 1;
        else _rotationState--;
    }

    public void Move(int rows, int columns)
    {
        _offset.Row += rows;
        _offset.Column += columns;
    }

    public void Reset()
    {
        _rotationState = 0;
        _offset.Row = StartOffset.Row;
        _offset.Column = StartOffset.Column;
    }
}