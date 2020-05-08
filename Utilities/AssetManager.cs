using SFML.Graphics;
using SFML.Audio;

namespace Tetris
{
    public class AssetManager
    {
        public Texture backgroundTexture { get; }
        public Texture blocksTexture { get; }
        public Texture outlineTexture { get; }
        public Texture previewImageTexture { get; }
        public Font menuFont { get; }
        public Font gameFont { get; }
        public SoundBuffer menuMoveSound { get; }
        public SoundBuffer menuSelectSound { get; }
        public SoundBuffer lineClearSound { get; }
        public SoundBuffer gameOverSound { get; }
        public SoundBuffer rotateSound { get; }

        public AssetManager()
        {
            // Load Textures
            backgroundTexture = new Texture(Constants.BACKGROUNDS_PATH);
            blocksTexture = new Texture(Constants.BLOCKS_PATH);
            outlineTexture = new Texture(Constants.OUTLINE_PATH);
            previewImageTexture = new Texture(Constants.TETRIMINOS_PATH);

            // Load Fonts
            gameFont = new Font(Constants.VCR_PATH);

            // Load Sounds
            menuMoveSound = new SoundBuffer(Constants.UI_MOVE_PATH);
            menuSelectSound = new SoundBuffer(Constants.UI_SELECT_PATH);
            lineClearSound = new SoundBuffer(Constants.LINE_CLEAR_PATH);
            gameOverSound = new SoundBuffer(Constants.GAME_OVER_PATH);
            rotateSound = new SoundBuffer(Constants.ROTATE_PATH);
        }


    }
}
