using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Water.Configuration;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Screens;
using Water.Utils;

namespace Water
{
    public class WaterGame : Game
    {
        public static bool UseExperimentalDrawingMode { get; private set; } = false;

        public GameScalingMode GameScalingMode { get; init; } = GameScalingMode.None;

        public Color BackgroundColor { get; set; } = Color.Black;

        public ConfigFile EngineConfig { get; set; }

        public int TargetWidth { get; set; } = 1920;
        public int TargetHeight { get; set; } = 1080;

        public float TargetAspectRatio => TargetWidth / TargetHeight;

        public ScreenManager Screen { get; private set; }
        public virtual string ProjectName { get; }

        public GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;

        private GameObjectManager gameObjectManager;
        public WaterGame()
        {
            LoadConfig();

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

            SoundEffect.MasterVolume = EngineConfig.EffectVolume * EngineConfig.MasterVolume;
            MediaPlayer.Volume = EngineConfig.MusicVolume * EngineConfig.MasterVolume;

            gameObjectManager.Input.KeyDown += Input_KeyDown;
        }

        private async void LoadConfig() // doing it this way so that we can get graphics/window settings from the start
        {
            EngineConfig = await JsonUtils.ReadAsync<ConfigFile>("Data/engineconfig.json");
        }

        private void Input_KeyDown(object sender, Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.F11: // TODO: fix fullscreen
                    //Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    //Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    //Graphics.ToggleFullScreen();
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
            switch (GameScalingMode)
            {
                case GameScalingMode.MaintainAspectRatio:
                    if (Graphics.IsFullScreen)
                    {
                        // TODO: get this to work
                    }
                    else
                    {
                        var oldWidth = Window.ClientBounds.Width;
                        var oldHeight = Window.ClientBounds.Height;
                        var newWidth = oldWidth;
                        var newHeight = oldHeight;



                        var fixedWidth = (int)Math.Round((float)oldHeight / 9 * 16);
                        var fixedHeight = (int)Math.Round((float)oldWidth / 16 * 9);

                        var widthDiff = Math.Abs(oldWidth - fixedWidth);
                        var heightDiff = Math.Abs(oldHeight - fixedWidth);

                        if (widthDiff < heightDiff)
                            newWidth = fixedWidth;
                        else
                            newHeight = fixedHeight;

                        Graphics.PreferredBackBufferWidth = newWidth;
                        Graphics.PreferredBackBufferHeight = newHeight;
                        Graphics.ApplyChanges();
                    }
                    break;
            }
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));
        }

        protected override async void OnExiting(object sender, EventArgs args)
        {
            await JsonUtils.WriteAsync("Data/engineconfig.json", EngineConfig);
            base.OnExiting(sender, args);
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
            GraphicsDevice.Clear(BackgroundColor);
            gameObjectManager.Draw(gameTime, _spriteBatch, GraphicsDevice);
           
            base.Draw(gameTime);
        }
    }

    public enum GameScalingMode
    {
        None,
        MaintainAspectRatio
    }
}
