namespace Tetris;

public class OBlock : Block
{
    protected override Position[][] Tiles { get; } =
    {
        new Position[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
    };

    protected override Position StartOffset => new(0, 4);

    public override int Id => 2;
}