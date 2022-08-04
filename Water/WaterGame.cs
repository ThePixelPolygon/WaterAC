using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Screens;

namespace Water
{
    public class WaterGame : Game
    {
        public static bool UseExperimentalDrawingMode { get; private set; } = false;

        public GameObjectScreen Screen { get; private set; }
        public virtual string ProjectName { get; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameObjectManager gameObjectManager;
        public WaterGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = true;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
            _graphics.ApplyChanges();
            
            gameObjectManager = new(GraphicsDevice);

            Screen = new(gameObjectManager, Window);
            Screen.RelativePosition = GraphicsDevice.Viewport.Bounds;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Screen.ChangeScreen(new DefaultScreen());

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));

            gameObjectManager.Input.KeyDown += Input_KeyDown;
        }

        private void Input_KeyDown(object sender, Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    Exit();
                    break;
                case Keys.F11:
                    _graphics.ToggleFullScreen();
                    break;
                case Keys.F10:
                    UseExperimentalDrawingMode = !UseExperimentalDrawingMode;
                    break;
            }
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
        }

        protected override void Initialize()
        {
#if DEBUG
            Window.Title = $"DEBUG: Water {Assembly.GetExecutingAssembly().GetName().Version} running \"{ProjectName ?? "Water Engine"}\"";
#else
            Window.Title = ProjectName ?? "Water Engine";
#endif
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            // TODO: Add your update logic here
            gameObjectManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            gameObjectManager.Draw(gameTime, _spriteBatch, GraphicsDevice);
            
            base.Draw(gameTime);
        }
    }
}
