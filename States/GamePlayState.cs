using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System;

namespace Tetris
{
    public class GamePlayState : State
    {
        private Tower _tower;
        private int _level;
        private int _totalLinesCleared;
        private int _frameCount;
        private Tetrimino _activePiece;
        private Tetrimino _activeOutline;
        private Tetrimino _nextPiece;
        private Tetrimino _heldPiece;
        private BlockValue[] _randomPieceBag;
        private int _randomPieceIndex;
        private bool _canHold;

        private Sprite _gameBackground;
        private Sprite _blocks;
        private Sprite _outline;
        private Sprite _nextPieceImg;
        private Sprite _heldPieceImg;
        private Text _nextText;
        private Text _holdText;
        private Text _linesLabelText;
        private Text _linesValueText;
        private Text _levelLabelText;
        private Text _levelValueText;
        private Sound _blockLandingSound;
        private Sound _gameOverSound;
        private Sound _rotateSound;
        private Sound _holdSound;
        private Sound _lineClearSound;

        public override void Init(GameData data)
        {
            // Background
            _gameBackground = new Sprite(data.Asset.BackgroundTexture, new IntRect(Constants.WindowWidth, 0, Constants.WindowWidth, Constants.WindowHeight));
            // Blocks
            _blocks = new Sprite(data.Asset.BlocksTexture);
            // Outline
            _outline = new Sprite(data.Asset.OutlineTexture);
            // Preview Images
            _nextPieceImg = new Sprite(data.Asset.PreviewImageTexture);
            _nextPieceImg.Position = new Vector2f(Constants.BlockSize * 16, Constants.BlockSize * 3);
            _heldPieceImg = new Sprite(data.Asset.PreviewImageTexture);
            _heldPieceImg.Position = new Vector2f(Constants.BlockSize, Constants.BlockSize * 3);
            // Text
            _holdText = new Text("Hold", data.Asset.GameFont);
            _holdText.Position = new Vector2f(Constants.BlockSize, Constants.BlockSize / 2);
            _nextText = new Text("Next", data.Asset.GameFont);
            _nextText.Position = new Vector2f((Constants.WindowWidth - Constants.BlockSize) - _nextText.GetGlobalBounds().Width, Constants.BlockSize / 2);
            _levelLabelText = new Text("Level", data.Asset.GameFont);
            _levelLabelText.Position = new Vector2f((_levelLabelText.GetGlobalBounds().Width / 2) - Constants.BlockSize, Constants.BlockSize * 10);
            _linesLabelText = new Text("Lines", data.Asset.GameFont);
            _linesLabelText.Position = new Vector2f(Constants.WindowWidth - _linesLabelText.GetGlobalBounds().Width - (Constants.BlockSize / 2), Constants.BlockSize * 10);
            // Sounds
            _blockLandingSound = new Sound(data.Asset.MenuMoveSound);
            _holdSound = new Sound(data.Asset.MenuSelectSound);
            _rotateSound = new Sound(data.Asset.RotateSound);
            _gameOverSound = new Sound(data.Asset.GameOverSound);
            _lineClearSound = new Sound(data.Asset.LineClearSound);

            InitGameData();
        }

        public override void HandleInput(GameData data, SFML.Window.KeyEventArgs e)
        {
            switch (e.Code)
            {
                case SFML.Window.Keyboard.Key.Left:
                    LegalMoveLeftRight(-1);
                    break;
                case SFML.Window.Keyboard.Key.Right:
                    LegalMoveLeftRight(1);
                    break;
                case SFML.Window.Keyboard.Key.Down:
                    LegalMoveUpDown(_activePiece, 1);
                    break;
                case SFML.Window.Keyboard.Key.F:
                    TryRotate(_activePiece, true);
                    break;
                case SFML.Window.Keyboard.Key.D:
                    TryRotate(_activePiece, false);
                    break;
                case SFML.Window.Keyboard.Key.LShift:
                    HoldPiece();
                    break;
                case SFML.Window.Keyboard.Key.Space:
                    HardDrop(_activePiece, 1);
                    DropActivePiece();
                    break;
                case SFML.Window.Keyboard.Key.Escape:
                    data.State.AddState(new PauseState(), false);
                    break;
            }
        }

        public override void Update(GameData data)
        {
            // Check Game Over
            if (IsGameOver())
            {
                _gameOverSound.Play();
                data.State.AddState(new GameOverState(), true);
            }
            // Gravity
            _frameCount++;
            if (_frameCount >= Constants.MinimumGravity - (_level * Constants.GravityModifier))
            {
                LegalMoveUpDown(_activePiece, 1);
                _frameCount = 0;
            }
            // Check And Clear Lines
            if (_tower.ClearState == ClearState.Clear)
            {
                _lineClearSound.Play();
            }
            else if (_tower.ClearState == ClearState.Move)
            {
                _blockLandingSound.Play();
            }
            _totalLinesCleared += _tower.ClearCycle();

            CheckLevelUp();

            UpdateOutline();
        }

        private void UpdateText(GameData data)
        {
            _levelValueText = new Text(_level.ToString(), data.Asset.GameFont);
            _levelValueText.Position = new Vector2f((_levelLabelText.GetGlobalBounds().Width / 2), Constants.BlockSize * 12);
            _linesValueText = new Text(_totalLinesCleared.ToString(), data.Asset.GameFont);
            _linesValueText.Position = new Vector2f(Constants.WindowWidth - _linesLabelText.GetGlobalBounds().Width + (Constants.BlockSize / 2), Constants.BlockSize * 12);
        }

        public override void Draw(GameData data)
        {
            data.Window.Clear();

            data.Window.Draw(_gameBackground);

            // Preview Images
            DrawPreviewImage(data, _nextPieceImg, _nextPiece);
            DrawPreviewImage(data, _heldPieceImg, _heldPiece);

            DrawPile(data);

            // Draw Active Outline
            DrawActivePiece(data, _activeOutline);

            // Draw Active Block
            DrawActivePiece(data, _activePiece);

            // Text
            UpdateText(data);
            data.Window.Draw(_holdText);
            data.Window.Draw(_nextText);
            data.Window.Draw(_levelLabelText);
            data.Window.Draw(_levelValueText);
            data.Window.Draw(_linesLabelText);
            data.Window.Draw(_linesValueText);

            data.Window.Display();
        }

        private void DrawPile(GameData data)
        {
            for (int row = 0; row < Constants.TowerHeight; row++)
            {
                for (int col = 0; col < Constants.TowerWidth; col++)
                {
                    BlockValue towerValue = _tower.GetTile(row, col);

                    if (towerValue != BlockValue.Empty)
                    {
                        DrawBlock(data, _blocks, towerValue, row, col);
                    }
                }
            }
        }

        private void DrawActivePiece(GameData data, Tetrimino piece)
        {
            for (int i = 0; i < 4; i++)
            {
                Block b = piece.GetBlockAt(i);

                DrawBlock(data, _outline, b.Type, b.GetRow(), b.GetCol());
            }
        }

        private void DrawBlock(GameData data, Sprite toDraw, BlockValue blockValue, int row, int col)
        {
            int textureOffset = -1;

            switch (blockValue)
            {
                case BlockValue.IShape:
                    textureOffset = 0;
                    break;
                case BlockValue.LShape:
                    textureOffset = 1;
                    break;
                case BlockValue.JShape:
                    textureOffset = 2;
                    break;
                case BlockValue.SShape:
                    textureOffset = 3;
                    break;
                case BlockValue.ZShape:
                    textureOffset = 4;
                    break;
                case BlockValue.TShape:
                    textureOffset = 5;
                    break;
                case BlockValue.OShape:
                    textureOffset = 6;
                    break;
                case BlockValue.Cleared:
                    textureOffset = 7;
                    break;
            }

            toDraw.TextureRect = new IntRect(textureOffset * Constants.BlockSize, 0, Constants.BlockSize, Constants.BlockSize);
            toDraw.Position = new Vector2f((5 * Constants.BlockSize) + (col * Constants.BlockSize), Constants.BlockSize + (row * Constants.BlockSize));
            data.Window.Draw(toDraw);
        }

        public void DrawPreviewImage(GameData data, Sprite pieceImg, Tetrimino piece)
        {
            if (piece != null)
            {
                int textureOffset = 0;

                switch (piece.GetBlockValue())
                {
                    case BlockValue.IShape:
                        textureOffset = 0;
                        break;
                    case BlockValue.LShape:
                        textureOffset = 1;
                        break;
                    case BlockValue.JShape:
                        textureOffset = 2;
                        break;
                    case BlockValue.SShape:
                        textureOffset = 3;
                        break;
                    case BlockValue.ZShape:
                        textureOffset = 4;
                        break;
                    case BlockValue.TShape:
                        textureOffset = 5;
                        break;
                    case BlockValue.OShape:
                        textureOffset = 6;
                        break;
                }

                pieceImg.TextureRect = new IntRect(textureOffset * Constants.PreviewImageSize, 0, Constants.PreviewImageSize, Constants.PreviewImageSize);
                data.Window.Draw(pieceImg);
            }
        }

        public void InitGameData()
        {
            _tower = new Tower();
            _level = 0;
            _totalLinesCleared = 0;
            _randomPieceIndex = 0;
            _canHold = true;
            _randomPieceBag = new BlockValue[] { BlockValue.IShape,
                                                 BlockValue.JShape,
                                                 BlockValue.LShape,
                                                 BlockValue.OShape,
                                                 BlockValue.SShape,
                                                 BlockValue.TShape,
                                                 BlockValue.ZShape };

            ShuffleRandomPieceBag();
            SpawnNextPiece();
        }

        public void ShuffleRandomPieceBag()
        {
            var rand = new Random();

            for (int i = 0; i < 7; i++)
            {
                int swapWith = rand.Next(7);
                BlockValue temp = _randomPieceBag[swapWith];
                _randomPieceBag[swapWith] = _randomPieceBag[i];
                _randomPieceBag[i] = temp;
            }
        }

        public void CheckLevelUp()
        {
            if (_totalLinesCleared >= _level * Constants.LinesModifier)
            {
                _level++;
            }
        }

        public bool IsGameOver()
        {
            return _tower.CheckPieceOverlap(_activePiece);
        }

        public void LegalMoveLeftRight(int moveVal)
        {
            // Copy Active Piece And Simulate Move
            Tetrimino copy = GeneratePiece(_activePiece.GetBlockValue());
            _activePiece.MakeCopy(copy);
            copy.MoveLeftRight(moveVal);

            // If In Bounds, Perform Actual Move
            if (copy.IsInBounds(_tower))
            {
                _activePiece.MoveLeftRight(moveVal);
            }
        }

        public void LegalMoveUpDown(Tetrimino piece, int moveVal)
        {
            // Check For Collisions
            if (!CheckForMoveCollision(piece, moveVal))
            {
                _activePiece.MoveUpDown(moveVal);
            }
            else
            {
                DropActivePiece();
            }
        }

        public void HardDrop(Tetrimino piece, int moveVal)
        {
            if (CheckForMoveCollision(piece, moveVal))
            {
                piece.MoveUpDown(moveVal-1);
            }
            else
            {
                HardDrop(piece, moveVal+1);
            }
        }

        public void UpdateOutline()
        {
            // Hard Drop Preview
            _activePiece.MakeCopy(_activeOutline);
            HardDrop(_activeOutline, 1);
        }

        public void SpawnNextPiece()
        {
            if (_randomPieceIndex > 6)
            {
                _randomPieceIndex = 0;
                ShuffleRandomPieceBag();
            }

            if (_nextPiece == null)
            {
                _nextPiece = GeneratePiece(_randomPieceBag[_randomPieceIndex]);
                _randomPieceIndex++;
            }

            _activePiece = _nextPiece;
            _activePiece.Spawn();
            _nextPiece = GeneratePiece(_randomPieceBag[_randomPieceIndex]);
            _randomPieceIndex++;

            // Create Piece For Outline
            _activeOutline = GeneratePiece(_activePiece.GetBlockValue());
        }

        public Tetrimino GeneratePiece(BlockValue shape)
        {
            switch (shape)
            {
                case BlockValue.IShape:
                    return new IShape();
                case BlockValue.LShape:
                    return new LShape();
                case BlockValue.JShape:
                    return new JShape();
                case BlockValue.SShape:
                    return new SShape();
                case BlockValue.ZShape:
                    return new ZShape();
                case BlockValue.TShape:
                    return new TShape();
                case BlockValue.OShape:
                    return new OShape();
            }

            throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid argument for spawning tetrimino", shape));
        }

        public void HoldPiece()
        {
            if (_canHold)
            {
                _activePiece.Despawn();

                if (_heldPiece == null)
                {
                    _heldPiece = _activePiece;
                    SpawnNextPiece();
                }
                else
                {
                    Tetrimino tempPiece = _heldPiece;
                    _heldPiece = _activePiece;
                    _activePiece = tempPiece;
                    _activePiece.Spawn();
                    _activeOutline = GeneratePiece(_activePiece.GetBlockValue());
                }

                _holdSound.Play();
            }

            _canHold = false;
        }

        public bool CheckForMoveCollision(Tetrimino piece, int moveVal)
        {
            // Copy Active Piece And Simulate Move
            Tetrimino copy = GeneratePiece(piece.GetBlockValue());
            piece.MakeCopy(copy);
            copy.MoveUpDown(moveVal);

            // Check If Block Collides With Pile
            if (_tower.CheckPieceOverlap(copy))
            {
                return true;
            }

            // Check If Block Collides With Bottom
            if (!copy.IsInBounds(_tower))
            {
                return true;
            }
            
            return false;
        }

        public void DropActivePiece()
        {
            _blockLandingSound.Play();
            _activePiece.CopyValuesToTowerData(_tower);
            SpawnNextPiece();
            _canHold = true;
        }

        public void TryRotate(Tetrimino piece, bool clockwise)
        {
            // Copy Active Piece And Simulate Rotation
            Tetrimino copy = GeneratePiece(piece.GetBlockValue());
            piece.MakeCopy(copy);
            copy.Rotate(clockwise);

            // If Everything Looks Good, Rotate Active Piece
            if (copy.IsInBounds(_tower))
            {
                piece.Rotate(clockwise);
            }
            // If It Doesn't, Try To Apply An Offset To The Rotation
            else
            {
                int rotationOffsetValue = -1;

                while (!copy.IsInBounds(_tower))
                {
                    rotationOffsetValue++;

                    switch(rotationOffsetValue)
                    {
                        case 0:
                            // 1 Space Left
                            copy.MoveLeftRight(-1);
                            break;
                        case 1:
                            // 2 Spaces Left
                            copy.MoveLeftRight(-1);
                            break;
                        case 2:
                            // 1 Space Right
                            copy.MoveLeftRight(3);
                            break;
                        case 3:
                            // 2 Spaces Right
                            copy.MoveLeftRight(1);
                            break;
                        case 4:
                            // 1 Space Down
                            copy.MoveLeftRight(-2);
                            copy.MoveUpDown(1);
                            break;
                        case 5:
                            // 2 Spaces Down
                            copy.MoveUpDown(1);
                            break;
                        default:
                            return;
                            
                    }
                }

                // Rotate And Apply Offset
                piece.Rotate(clockwise);
                switch (rotationOffsetValue)
                {
                    case 0:
                        piece.MoveLeftRight(-1);
                        break;
                    case 1:
                        piece.MoveLeftRight(-2);
                        break;
                    case 2:
                        piece.MoveLeftRight(1);
                        break;
                    case 3:
                        piece.MoveLeftRight(2);
                        break;
                    case 4:
                        piece.MoveUpDown(1);
                        break;
                    case 5:
                        piece.MoveUpDown(2);
                        break;
                }
            }

            _rotateSound.Play();
        }
    }
}
