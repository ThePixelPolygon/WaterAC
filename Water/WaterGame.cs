using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Screens;

namespace Water
{
    public class WaterGame : Game
    {
        public GameObjectScreen Screen { get; private set; }
        public virtual string ProjectName { get; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameObjectManager gameObjectManager;
        public WaterGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.SynchronizeWithVerticalRetrace = false;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = false;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(8.33);
            _graphics.ApplyChanges();
            Screen = new();
            Screen.RelativePosition = GraphicsDevice.Viewport.Bounds;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            gameObjectManager = new(GraphicsDevice);
            Screen.ChangeScreen(new DefaultScreen());
            gameObjectManager.AddObject(Screen);
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
#if DEBUG
            Screen.AddScreen(new DebugOverlay(gameObjectManager, Screen, Window));
#endif
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
                

            // TODO: Add your update logic here
            gameObjectManager.Update(gameTime);
            Window.Title = ProjectName ?? "Water Engine";
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            gameObjectManager.Draw(gameTime, _spriteBatch, GraphicsDevice);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
