using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

        public ScreenManager Screen { get; private set; }
        public virtual string ProjectName { get; }

        public GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;

        private GameObjectManager gameObjectManager;
        public WaterGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.SynchronizeWithVerticalRetrace = true;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
            Graphics.ApplyChanges();
            
            gameObjectManager = new(GraphicsDevice, this);

            Screen = new(gameObjectManager, Window);
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));

            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Screen.ChangeScreen(new DefaultScreen());

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

            gameObjectManager.Input.KeyDown += Input_KeyDown;
        }

        private void Input_KeyDown(object sender, Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.F11:
                    Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    Graphics.ToggleFullScreen();
                    break;
                case Keys.F10:
                    UseExperimentalDrawingMode = !UseExperimentalDrawingMode;
                    break;
                case Keys.F9:
                    GC.Collect(2);
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
        }

        protected override void Update(GameTime gameTime)
        {
            gameObjectManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            gameObjectManager.Draw(gameTime, _spriteBatch, GraphicsDevice);
           
            base.Draw(gameTime);
        }
    }
}
