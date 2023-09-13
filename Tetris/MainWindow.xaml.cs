using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int MaxDelay = 500;
    private const int MinDelay = 100;
    private const int DelayStep = 25;

    private readonly ImageSource[] _blockImages =
    {
        new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative))
    };

    private readonly Image[,] _imageControls;

    private readonly ImageSource[] _tileImages =
    {
        new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative))
    };

    private GameState _gameState = new();

    public MainWindow()
    {
        InitializeComponent();
        _imageControls = SetupGameCanvas(_gameState.GameGrid);
    }

    private void DrawGrid(GameGrid gameGrid)
    {
        for (var i = 0; i < gameGrid.Rows; i++)
        for (var j = 0; j < gameGrid.Columns; j++)
        {
            var id = gameGrid[i, j];
            _imageControls[i, j].Opacity = 1;
            _imageControls[i, j].Source = _tileImages[id];
        }
    }

    private void DrawBlock(Block block)
    {
        foreach (var position in block.TilePositions())
        {
            _imageControls[position.Row, position.Column].Opacity = 1;
            _imageControls[position.Row, position.Column].Source = _tileImages[block.Id];
        }
    }

    private void Draw(GameState gameState)
    {
        DrawGrid(gameState.GameGrid);
        DrawDropGhost(gameState.CurrentBlock);
        DrawBlock(gameState.CurrentBlock);
        DrawNextBlock(gameState.BlockQueue);
        DrawHeldBlock(gameState.HeldBlock);
        ScoreTextBlock.Text = $"Score: {_gameState.Score}";
    }

    private Image[,] SetupGameCanvas(GameGrid gameGrid)
    {
        var imageControls = new Image[gameGrid.Rows, gameGrid.Columns];
        var cellSize = 25;

        for (var i = 0; i < gameGrid.Rows; i++)
        for (var j = 0; j < gameGrid.Columns; j++)
        {
            var imageControl = new Image
            {
                Width = cellSize,
                Height = cellSize
            };

            Canvas.SetTop(imageControl, (i - 2) * cellSize + 10);
            Canvas.SetLeft(imageControl, j * cellSize);
            GameCanvas.Children.Add(imageControl);
            imageControls[i, j] = imageControl;
        }

        return imageControls;
    }

    private void DrawNextBlock(BlockQueue blockQueue)
    {
        var next = blockQueue.NextBlock;
        NextImage.Source = _blockImages[next.Id];
    }

    private async Task GameLoop()
    {
        Draw(_gameState);

        while (!_gameState.GameOver)
        {
            var delay = Math.Max(MinDelay, MaxDelay - _gameState.Score * DelayStep);
            await Task.Delay(delay);
            _gameState.MoveDown();
            Draw(_gameState);
        }

        GameOverMenu.Visibility = Visibility.Visible;
        FinalScoreTextBlock.Text = $"Score: {_gameState.Score}";
    }

    private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_gameState.GameOver) return;

        switch (e.Key)
        {
            case Key.A:
                _gameState.MoveLeft();
                break;
            case Key.D:
                _gameState.MoveRight();
                break;
            case Key.S:
                _gameState.MoveDown();
                break;
            case Key.E:
                _gameState.RotateClockwise();
                break;
            case Key.Q:
                _gameState.RotateCounterclockwise();
                break;
            case Key.F:
                _gameState.HoldBlock();
                break;
            case Key.Space:
                _gameState.DropBlock();
                break;
            default: return;
        }

        Draw(_gameState);
    }

    private void DrawDropGhost(Block block)
    {
        var dropDistance = _gameState.BlockDropDistance();

        foreach (var position in block.TilePositions())
        {
            _imageControls[position.Row + dropDistance, position.Column].Opacity = 0.3;
            _imageControls[position.Row + dropDistance, position.Column].Source = _tileImages[block.Id];
        }
    }

    private void DrawHeldBlock(Block? heldBlock)
    {
        if (heldBlock == null)
            HoldImage.Source = _blockImages[0];
        else
            HoldImage.Source = _blockImages[heldBlock.Id];
    }

    private async void GameCanvas_OnLoaded(object sender, RoutedEventArgs e)
    {
        await GameLoop();
    }

    private async void PlayAgain_OnClick(object sender, RoutedEventArgs e)
    {
        _gameState = new GameState();
        GameOverMenu.Visibility = Visibility.Hidden;
        await GameLoop();
    }
}