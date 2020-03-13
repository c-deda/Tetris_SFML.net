using SFML.Graphics;
using SFML.System;
using System;

namespace Tetris
{
    public class GamePlayState : State
    {
        public Tower tower { get; set; }
        public int level { get; set; }
        public int totalLinesCleared { get; set; }
        public int frameCount { get; set; }
        public Tetrimino activePiece { get; set; }
        public Tetrimino activeOutline { get; set; }
        public Tetrimino nextPiece { get; set; }
        public Tetrimino heldPiece { get; set; }
        public BlockValue lastPiece { get; set; }
        public BlockValue[] randomPieceBag { get; set; }
        public int randomPieceIndex { get; set; }
        public bool canHold { get; set; }

        public Sprite gameBackground { get; set; }
        public Sprite blocks { get; set; }
        public Sprite outline { get; set; }
        public Sprite nextPieceImg { get; set; }
        public Sprite heldPieceImg { get; set; }
        public Text nextText { get; set; }
        public Text holdText { get; set; }
        public Text linesLabelText { get; set; }
        public Text linesValueText { get; set; }
        public Text levelLabelText { get; set; }
        public Text levelValueText { get; set; }

        public override void Init(ref GameData data)
        {
            // Setup Assets
            // Background
            gameBackground = new Sprite(data.asset.backgroundTexture, new IntRect(Constants.WIN_WIDTH, 0, Constants.WIN_WIDTH, Constants.WIN_HEIGHT));
            // Blocks
            blocks = new Sprite(data.asset.blocksTexture);
            // Outline
            outline = new Sprite(data.asset.outlineTexture);
            // Preview Images
            nextPieceImg = new Sprite(data.asset.previewImageTexture);
            nextPieceImg.Position = new Vector2f(Constants.BLOCK_SIZE * 16, Constants.BLOCK_SIZE * 3);
            heldPieceImg = new Sprite(data.asset.previewImageTexture);
            heldPieceImg.Position = new Vector2f(Constants.BLOCK_SIZE, Constants.BLOCK_SIZE * 3);
            // Text
            holdText = new Text("Hold", data.asset.gameFont);
            holdText.Position = new Vector2f(Constants.BLOCK_SIZE, Constants.BLOCK_SIZE / 2);
            nextText = new Text("Next", data.asset.gameFont);
            nextText.Position = new Vector2f((Constants.WIN_WIDTH - Constants.BLOCK_SIZE) - nextText.GetGlobalBounds().Width, Constants.BLOCK_SIZE / 2);
            levelLabelText = new Text("Level", data.asset.gameFont);
            levelLabelText.Position = new Vector2f((levelLabelText.GetGlobalBounds().Width / 2) - Constants.BLOCK_SIZE, Constants.BLOCK_SIZE * 10);
            linesLabelText = new Text("Lines", data.asset.gameFont);
            linesLabelText.Position = new Vector2f(Constants.WIN_WIDTH - linesLabelText.GetGlobalBounds().Width - (Constants.BLOCK_SIZE / 2), Constants.BLOCK_SIZE * 10);

            ResetGame();
        }

        public override void HandleInput(ref GameData data, SFML.Window.KeyEventArgs e)
        {
            switch (e.Code)
            {
                case SFML.Window.Keyboard.Key.Left:
                    LegalMoveLeftRight(-1);
                    break;
                case SFML.Window.Keyboard.Key.Right:
                    LegalMoveLeftRight(1);
                    break;
                case SFML.Window.Keyboard.Key.F:
                    TryRotate(activePiece, true);
                    break;
                case SFML.Window.Keyboard.Key.D:
                    TryRotate(activePiece, false);
                    break;
                case SFML.Window.Keyboard.Key.LShift:
                    HoldPiece();
                    break;
                case SFML.Window.Keyboard.Key.Space:
                    HardDrop(activePiece, 1);
                    DropActivePiece();
                    break;
                case SFML.Window.Keyboard.Key.Escape:
                    data.state.addState(new PauseState(), false);
                    break;
            }
        }

        public override void Update(ref GameData data)
        {
            // Check Game Over
            if (IsGameOver())
            {
                data.state.addState(new GameOverState(), false);
            }
            // Gravity
            frameCount++;
            if (frameCount >= Constants.MIN_GRAVITY - (level * Constants.GRAVITY_MOD))
            {
                LegalMoveUpDown(activePiece, 1);
                frameCount = 0;
            }
            // Check And Clear Lines
            totalLinesCleared += tower.ClearCycle();

            CheckLevelUp();

            UpdateOutline();
        }

        public override void Draw(ref GameData data)
        {
            data.window.Clear();

            data.window.Draw(gameBackground);
            // Preview Images
            DrawPreviewImage(ref data, nextPieceImg, nextPiece);
            DrawPreviewImage(ref data, heldPieceImg, heldPiece);
            // Pile
            for (int row = 0; row < Constants.TOWER_HEIGHT; row++)
            {
                for (int col = 0; col < Constants.TOWER_WIDTH; col++)
                {
                    BlockValue towerValue = tower.GetTile(row, col);

                    if (towerValue != BlockValue.EMPTY)
                    {
                        DrawBlock(ref data, blocks, towerValue, row, col);
                    }
                }
            }
            // Draw Active Outline
            for (int i = 0; i < 4; i++)
            {
                Block b = activeOutline.GetBlockAt(i);

                DrawBlock(ref data, outline, b.type, b.GetRow(), b.GetCol());
            }
            // Draw Active Block
            for (int i = 0; i < 4; i++)
            {
                Block b = activePiece.GetBlockAt(i);

                DrawBlock(ref data, blocks, b.type, b.GetRow(), b.GetCol());
            }

            // Text
            levelValueText = new Text(level.ToString(), data.asset.gameFont);
            levelValueText.Position = new Vector2f((levelLabelText.GetGlobalBounds().Width / 2), Constants.BLOCK_SIZE * 12);
            linesValueText = new Text(totalLinesCleared.ToString(), data.asset.gameFont);
            linesValueText.Position = new Vector2f(Constants.WIN_WIDTH - linesLabelText.GetGlobalBounds().Width + (Constants.BLOCK_SIZE / 2), Constants.BLOCK_SIZE * 12);

            data.window.Draw(holdText);
            data.window.Draw(nextText);
            data.window.Draw(levelLabelText);
            data.window.Draw(levelValueText);
            data.window.Draw(linesLabelText);
            data.window.Draw(linesValueText);

            data.window.Display();
        }

        private void DrawBlock(ref GameData data, Sprite toDraw, BlockValue blockValue, int row, int col)
        {
            int textureOffset = -1;

            switch (blockValue)
            {
                case BlockValue.ISHAPE:
                    textureOffset = 0;
                    break;
                case BlockValue.LSHAPE:
                    textureOffset = 1;
                    break;
                case BlockValue.JSHAPE:
                    textureOffset = 2;
                    break;
                case BlockValue.SSHAPE:
                    textureOffset = 3;
                    break;
                case BlockValue.ZSHAPE:
                    textureOffset = 4;
                    break;
                case BlockValue.TSHAPE:
                    textureOffset = 5;
                    break;
                case BlockValue.OSHAPE:
                    textureOffset = 6;
                    break;
                case BlockValue.CLEARED:
                    textureOffset = 7;
                    break;
            }

            toDraw.TextureRect = new IntRect(textureOffset * Constants.BLOCK_SIZE, 0, Constants.BLOCK_SIZE, Constants.BLOCK_SIZE);
            toDraw.Position = new Vector2f((5 * Constants.BLOCK_SIZE) + (col * Constants.BLOCK_SIZE), Constants.BLOCK_SIZE + (row * Constants.BLOCK_SIZE));
            data.window.Draw(toDraw);
        }

        public void DrawPreviewImage(ref GameData data, Sprite pieceImg, Tetrimino piece)
        {
            if (piece != null)
            {
                int textureOffset = 0;

                switch (piece.GetBlockValue())
                {
                    case BlockValue.ISHAPE:
                        textureOffset = 0;
                        break;
                    case BlockValue.LSHAPE:
                        textureOffset = 1;
                        break;
                    case BlockValue.JSHAPE:
                        textureOffset = 2;
                        break;
                    case BlockValue.SSHAPE:
                        textureOffset = 3;
                        break;
                    case BlockValue.ZSHAPE:
                        textureOffset = 4;
                        break;
                    case BlockValue.TSHAPE:
                        textureOffset = 5;
                        break;
                    case BlockValue.OSHAPE:
                        textureOffset = 6;
                        break;
                }

                pieceImg.TextureRect = new IntRect(textureOffset * Constants.PREV_IMG_SIZE, 0, Constants.PREV_IMG_SIZE, Constants.PREV_IMG_SIZE);
                data.window.Draw(pieceImg);
            }
        }

        public void ResetGame()
        {
            tower = new Tower();
            level = 0;
            totalLinesCleared = 0;
            randomPieceIndex = 0;
            canHold = true;
            randomPieceBag = new BlockValue[] { BlockValue.ISHAPE,
                                                BlockValue.JSHAPE,
                                                BlockValue.LSHAPE,
                                                BlockValue.OSHAPE,
                                                BlockValue.SSHAPE,
                                                BlockValue.TSHAPE,
                                                BlockValue.ZSHAPE };

            ShuffleRandomPieceBag();
            SpawnNextPiece();
        }

        public void ShuffleRandomPieceBag()
        {
            var rand = new Random();

            for (int i = 0; i < 7; i++)
            {
                int swapWith = rand.Next(7);
                BlockValue temp = randomPieceBag[swapWith];
                randomPieceBag[swapWith] = randomPieceBag[i];
                randomPieceBag[i] = temp;
            }
        }

        public void CheckLevelUp()
        {
            if (totalLinesCleared >= level * Constants.LINES_MOD)
            {
                level++;
            }
        }

        public bool IsGameOver()
        {
            return tower.CheckPieceOverlap(activePiece);
        }

        public void LegalMoveLeftRight(int moveVal)
        {
            // Copy Active Piece And Simulate Move
            Tetrimino copy = GeneratePiece(activePiece.GetBlockValue());
            activePiece.MakeCopy(copy);
            copy.MoveLeftRight(moveVal);

            // If In Bounds, Perform Actual Move
            if (copy.IsInBounds(tower))
            {
                activePiece.MoveLeftRight(moveVal);
            }
        }

        public void LegalMoveUpDown(Tetrimino piece, int moveVal)
        {
            // Check For Collisions
            if (!CheckForMoveCollision(piece, moveVal))
            {
                activePiece.MoveUpDown(moveVal);
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
                piece.MoveUpDown(moveVal - 1);
            }
            else
            {
                HardDrop(piece, moveVal + 1);
            }
        }

        public void UpdateOutline()
        {
            // Hard Drop Preview
            activePiece.MakeCopy(activeOutline);
            HardDrop(activeOutline, 1);
        }

        public void SpawnNextPiece()
        {
            if (randomPieceIndex > 6)
            {
                randomPieceIndex = 0;
                ShuffleRandomPieceBag();
            }

            if (nextPiece == null)
            {
                nextPiece = GeneratePiece(randomPieceBag[randomPieceIndex]);
                randomPieceIndex++;
            }

            activePiece = nextPiece;
            activePiece.Spawn();
            nextPiece = GeneratePiece(randomPieceBag[randomPieceIndex]);
            randomPieceIndex++;

            // Create Piece For Outline
            activeOutline = GeneratePiece(activePiece.GetBlockValue());
        }

        public Tetrimino GeneratePiece(BlockValue shape)
        {
            switch (shape)
            {
                case BlockValue.ISHAPE:
                    return new IShape();
                case BlockValue.LSHAPE:
                    return new LShape();
                case BlockValue.JSHAPE:
                    return new JShape();
                case BlockValue.SSHAPE:
                    return new SShape();
                case BlockValue.ZSHAPE:
                    return new ZShape();
                case BlockValue.TSHAPE:
                    return new TShape();
                case BlockValue.OSHAPE:
                    return new OShape();
            }

            throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid argument for spawning tetrimino", shape));
        }

        public void HoldPiece()
        {
            if (canHold)
            {
                activePiece.Despawn();

                if (heldPiece == null)
                {
                    heldPiece = activePiece;
                    SpawnNextPiece();
                }
                else
                {
                    Tetrimino tempPiece = heldPiece;
                    heldPiece = activePiece;
                    activePiece = tempPiece;
                    activePiece.Spawn();
                    activeOutline = GeneratePiece(activePiece.GetBlockValue());
                }
            }

            canHold = false;
        }

        public bool CheckForMoveCollision(Tetrimino piece, int moveVal)
        {
            // Copy Active Piece And Simulate Move
            Tetrimino copy = GeneratePiece(piece.GetBlockValue());
            piece.MakeCopy(copy);
            copy.MoveUpDown(moveVal);

            // Check If Block Collides With Pile
            if (tower.CheckPieceOverlap(copy))
            {
                return true;
            }

            // Check If Block Collides With Bottom
            if (!copy.IsInBounds(tower))
            {
                return true;
            }
            
            return false;
        }

        public void DropActivePiece()
        {
            activePiece.CopyValuesToTower(tower);
            SpawnNextPiece();
            canHold = true;
        }

        public void TryRotate(Tetrimino piece, bool clockwise)
        {
            // Copy Active Piece And Simulate Rotation
            Tetrimino copy = GeneratePiece(piece.GetBlockValue());
            piece.MakeCopy(copy);
            copy.Rotate(clockwise);

            // If Everything Looks Good, Rotate Active Piece
            if (copy.IsInBounds(tower))
            {
                piece.Rotate(clockwise);
            }
            // If It Doesn't, Try To Apply An Offset To The Rotation
            else
            {
                int rotationOffsetValue = -1;

                while (!copy.IsInBounds(tower))
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
        }
    }
}
