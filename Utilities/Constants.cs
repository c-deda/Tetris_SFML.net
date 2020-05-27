namespace Tetris
{
    public static class Constants
    {
        public const string WindowTitle = "Tetris";
        public const int WindowHeight = 660;
        public const int WindowWidth = 600;
        public const int Framerate = 60;
        public const int TowerHeight = 20;
        public const int TowerWidth = 10;
        public const int TotalRotationValues = 4;
        public const int DespawnedPosition = -10;
        public const int BlockSize = 30;
        public const int PreviewImageSize = 90;
        public const int MinimumGravity = 55;
        public const int GravityModifier = 5;
        public const int LinesModifier = 10;
        public const string BlocksPath = "../../Assets/Textures/Tetris_Blocks.png";
        public const string OutlinePath = "../../Assets/Textures/Tetris_Outlines.png";
        public const string BackgroundPath = "../../Assets/Textures/Tetris_Backgrounds.png";
        public const string TetriminosPath = "../../Assets/Textures/Tetris_Tetriminos.png";
        public const string GameFontPath = "../../Assets/Fonts/VCR.ttf";
        public const string UIMoveSoundPath = "../../Assets/Sounds/UI_Move.wav";
        public const string UISelectSoundPath = "../../Assets/Sounds/UI_Select.wav";
        public const string LinesClearSoundPath = "../../Assets/Sounds/Line_Clear.wav";
        public const string GameOverSoundPath = "../../Assets/Sounds/Game_Over.wav";
        public const string RotateSoundPath = "../../Assets/Sounds/Rotate.wav";
    }
}
