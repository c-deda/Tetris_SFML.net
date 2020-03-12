using SFML.Graphics;

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

        public AssetManager()
        {
            // Load Textures
            backgroundTexture = new Texture(Constants.BACKGROUNDS_PATH);
            blocksTexture = new Texture(Constants.BLOCKS_PATH);
            outlineTexture = new Texture(Constants.OUTLINE_PATH);
            previewImageTexture = new Texture(Constants.TETRIMINOS_PATH);

            // Load Fonts;
            gameFont = new Font(Constants.VCR_PATH);
        }
    }
}
