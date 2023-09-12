using System;

namespace Tetris;

public class BlockQueue
{
    private readonly Block[] _blocks =
    {
        new IBlock(),
        new OBlock(),
        new TBlock(),
        new SBlock(),
        new JBlock(),
        new ZBlock(),
        new LBlock()
    };

    private readonly Random _random = new();

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    public Block NextBlock { get; private set; }

    public Block GetAndUpdate()
    {
        var block = NextBlock;

        do
        {
            NextBlock = RandomBlock();
        } while (block.Id == NextBlock.Id);

        return block;
    }

    private Block RandomBlock()
    {
        return _blocks[_random.Next(_blocks.Length)];
    }
}