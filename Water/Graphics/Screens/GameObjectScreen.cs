using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Screens
{
    public class GameObjectScreen : GameObject
    {
        public List<Screen> Screens { get; } = new();
        public bool HasScreens { get => Screens.Count > 0; }

        private GameObjectManager gameObjectManager;
        private GameWindow window;
        public GameObjectScreen(GameObjectManager gameObjectManager, GameWindow window)
        {
            this.gameObjectManager = gameObjectManager;
            this.window = window;
        }

        private Screen InitializeScreen(Screen screen)
        {
            screen.Game = new GameObjectManager(gameObjectManager.GraphicsDevice);
            screen.ScreenManager = this;
            screen.Window = window;
            screen.ActualPosition = ActualPosition;
            screen.RelativePosition = RelativePosition;
            screen.Game.AssignRootObject(screen);
            screen.Game.AddObject(screen); 
            screen.CalculateChildrenPositions();
            screen.Initialize();
            return screen;
        }

        public void AddScreen(Screen screen)
        {
            Screens.Add(InitializeScreen(screen));
        }

        public void InsertScreen(int index, Screen screen)
        {
            Screens.Insert(index, InitializeScreen(screen));
        }

        public void RemoveScreen(Screen screen)
        {
            Screens.Remove(screen);
        }
        public void RemoveAllScreens()
        {
            Screens.Clear();
        }

        public override void AddChild(IContainer child)
        {
            base.AddChild(child);
            CalculateChildrenPositions();
        }

        public override void Initialize()
        {
        }

        public void ChangeScreen(Screen screen)
        {
            RemoveAllScreens();   
            AddScreen(screen);
        }

        public override void Update(GameTime gameTime)
        {
            if (!HasScreens) return;
            foreach (var screen in Screens)
                screen.Game.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!HasScreens) return;
            foreach (var screen in Screens)
                screen.Game.Draw(gameTime, spriteBatch, graphicsDevice);
        }

        public void UpdateScreenSize(Rectangle newSize)
        {
            ActualPosition = newSize;
            RelativePosition = newSize;
            foreach (var screen in Screens)
            {
                screen.ActualPosition = newSize;
                screen.RelativePosition = newSize;
                screen.CalculateChildrenPositions();
            }
            CalculateChildrenPositions();
        }
    }
}
