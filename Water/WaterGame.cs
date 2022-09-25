using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Water.Configuration;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Screens;
using Water.Utils;

namespace Water
{
    public class WaterGame : Game
    {
        /// <summary>
        /// The way the entire game will be scaled
        /// </summary>
        public GameScalingMode GameScalingMode { get; init; } = GameScalingMode.None;

        /// <summary>
        /// Color that areas of the screen that haven't been drawn to will be drawn in
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Black;

        /// <summary>
        /// Engine configuration (volumes, fullscreen, etc.), call <see cref="LoadConfigAsync"/> when done editing
        /// </summary>
        public ConfigFile EngineConfig { get; set; }

        /// <summary>
        /// Default window width; also used for aspect ratio calculations with <see cref="GameScalingMode.MaintainAspectRatio"/>
        /// </summary>
        public int TargetWidth { get; set; } = 1920;
        /// <summary>
        /// Default window height; also used for aspect ratio calculations with <see cref="GameScalingMode.MaintainAspectRatio"/>
        /// </summary>
        public int TargetHeight { get; set; } = 1080;

        /// <summary>
        /// Aspect ratio the game will target with <see cref="GameScalingMode.MaintainAspectRatio"/>
        /// </summary>
        public float TargetAspectRatio => TargetWidth / TargetHeight;

        /// <summary>
        /// Screen manager. Use to change to first screen on startup
        /// </summary>
        public ScreenManager Screen { get; private set; }

        /// <summary>
        /// Project name, shown in the window title and potentionally other places
        /// </summary>
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

            LoadConfig();

            Screen = new(gameObjectManager, Window);
            Screen.UpdateScreenSize(new(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height));

            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Screen.ChangeScreen(new DefaultScreen());

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            gameObjectManager.Input.KeyDown += Input_KeyDown;
        }

        public void UpdateWindowSize(Rectangle newSize)
        {
            Graphics.PreferredBackBufferWidth = newSize.Width;
            Graphics.PreferredBackBufferHeight = newSize.Height;
            Graphics.ApplyChanges();
        }

        private async void LoadConfig() => await LoadConfigAsync(); // this is really cursed :sob:

        public async Task LoadConfigAsync()
        {
            EngineConfig = await JsonUtils.ReadAsync<ConfigFile>("Data/engineconfig.json");

            gameObjectManager.Audio.MasterVolume = EngineConfig.MasterVolume;
            gameObjectManager.Audio.MusicVolume = EngineConfig.MusicVolume;
            gameObjectManager.Audio.EffectVolume = EngineConfig.EffectVolume;
            gameObjectManager.Audio.UpdateVolumes();
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
                case Keys.OemPlus:
                    Screen.GameScale += .1f;
                    break;
                case Keys.OemMinus:
                    Screen.GameScale -= .1f;
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
