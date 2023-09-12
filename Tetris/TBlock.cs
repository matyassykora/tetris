namespace Tetris;

public class TBlock : Block
{
    public override int Id => 3;

    protected override Position[][] Tiles { get; } =
    {
        new Position[] { new(0, 1), new(1, 0), new(1, 1), new(1, 2) },
        new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 1) },
        new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 1) },
        new Position[] { new(0, 1), new(1, 0), new(1, 1), new(2, 1) }
    };

    protected override Position StartOffset => new(0, 3);
}