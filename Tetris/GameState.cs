namespace Tetris;

public class GameState
{
    private Block _currentBlock;

    public GameState()
    {
        GameGrid = new GameGrid(22, 10);
        BlockQueue = new BlockQueue();
        CurrentBlock = BlockQueue.GetAndUpdate();
        CanHold = true;
    }

    public Block CurrentBlock
    {
        get => _currentBlock;
        private set
        {
            _currentBlock = value;
            _currentBlock.Reset();

            for (var i = 0; i < 2; i++)
            {
                _currentBlock.Move(1, 0);

                if (!BlockFits()) _currentBlock.Move(-1, 0);
            }
        }
    }

    public GameGrid GameGrid { get; }
    public BlockQueue BlockQueue { get; }
    public bool GameOver { get; private set; }
    public int Score { get; private set; }
    public Block? HeldBlock { get; private set; }
    public bool CanHold { get; private set; }

    public void HoldBlock()
    {
        if (!CanHold) return;

        if (HeldBlock == null)
        {
            HeldBlock = CurrentBlock;
            CurrentBlock = BlockQueue.GetAndUpdate();
        }
        else
        {
            (CurrentBlock, HeldBlock) = (HeldBlock, CurrentBlock);
        }

        CanHold = false;
    }

    private bool BlockFits()
    {
        foreach (var position in CurrentBlock.TilePositions())
            if (!GameGrid.IsEmpty(position.Row, position.Column))
                return false;
        return true;
    }

    public void RotateClockwise()
    {
        CurrentBlock.RotateClockwise();

        if (!BlockFits()) CurrentBlock.RotateCounterclockwise();
    }

    public void RotateCounterclockwise()
    {
        CurrentBlock.RotateCounterclockwise();

        if (!BlockFits()) CurrentBlock.RotateClockwise();
    }

    public void MoveLeft()
    {
        CurrentBlock.Move(0, -1);

        if (!BlockFits()) CurrentBlock.Move(0, 1);
    }

    public void MoveRight()
    {
        CurrentBlock.Move(0, 1);

        if (!BlockFits()) CurrentBlock.Move(0, -1);
    }

    private bool IsGameOver()
    {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }

    private void PlaceBlock()
    {
        foreach (var position in CurrentBlock.TilePositions())
            GameGrid[position.Row, position.Column] = CurrentBlock.Id;

        Score += GameGrid.ClearFullRows();

        if (IsGameOver())
        {
            GameOver = true;
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
    }

    public void MoveDown()
    {
        CurrentBlock.Move(1, 0);

        if (!BlockFits())
        {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }
}