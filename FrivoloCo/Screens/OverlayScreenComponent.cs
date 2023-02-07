using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Water.Graphics;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens
{
    public class OverlayScreenComponent : DrawableGameComponent
    {
        public List<Screen> Screens = new();
        private ScreenManager screenManager;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        public OverlayScreenComponent(Game game, SpriteBatch _spriteBatch, GraphicsDevice _graphicsDevice, ScreenManager _screenManager) : base(game)
        {
            screenManager = _screenManager;
            spriteBatch = _spriteBatch;
            graphicsDevice = _graphicsDevice;
            
            game.Components.Add(this);
        }

        public void AddScreen(Screen screen)
        {
            Screens.Add(screenManager.InitializeScreen(screen));
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach(Screen screen in this.Screens)
            {
                screen.DrawChildren(gameTime, spriteBatch, graphicsDevice);
            }
            spriteBatch.End();
        }
    }
}