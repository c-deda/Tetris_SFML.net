using SFML.Graphics;
using SFML.Audio;

namespace Tetris
{
    public class AssetManager
    {
        public Texture BackgroundTexture { get; }
        public Texture BlocksTexture { get; }
        public Texture OutlineTexture { get; }
        public Texture PreviewImageTexture { get; }
        public Font MenuFont { get; }
        public Font GameFont { get; }
        public SoundBuffer MenuMoveSound { get; }
        public SoundBuffer MenuSelectSound { get; }
        public SoundBuffer LineClearSound { get; }
        public SoundBuffer GameOverSound { get; }
        public SoundBuffer RotateSound { get; }

        public AssetManager()
        {
            // Load Textures
            BackgroundTexture = new Texture(Constants.BackgroundPath);
            BlocksTexture = new Texture(Constants.BlocksPath);
            OutlineTexture = new Texture(Constants.OutlinePath);
            PreviewImageTexture = new Texture(Constants.TetriminosPath);

            // Load Fonts
            GameFont = new Font(Constants.GameFontPath);

            // Load Sounds
            MenuMoveSound = new SoundBuffer(Constants.UIMoveSoundPath);
            MenuSelectSound = new SoundBuffer(Constants.UISelectSoundPath);
            LineClearSound = new SoundBuffer(Constants.LinesClearSoundPath);
            GameOverSound = new SoundBuffer(Constants.GameOverSoundPath);
            RotateSound = new SoundBuffer(Constants.RotateSoundPath);
        }


    }
}
