using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Screens;

namespace Water.Graphics.Screens
{
    public class GameObjectScreen : GameObject
    {
        public List<Screen> Screens { get; } = new();
        public bool HasScreens { get => Screens.Count > 0; }

        private GameObjectManager gameObjectManager;
        private GameWindow window;
#if DEBUG
        private DebugOverlay debugOverlay = new();
#endif
        public GameObjectScreen(GameObjectManager gameObjectManager, GameWindow window)
        {
            this.gameObjectManager = gameObjectManager;
            this.window = window;
        }

        private Screen InitializeScreen(Screen screen)
        {
            screen.Game = gameObjectManager;
            screen.ScreenManager = this;
            screen.Window = window;
            screen.ActualPosition = ActualPosition;
            screen.RelativePosition = RelativePosition;

            screen.Game.AddObject(screen); 
            screen.CalculateChildrenPositions();
            screen.Initialize();
            return screen;
        }

        public void AddScreen(Screen screen)
        {
            Screens.Add(InitializeScreen(screen));
#if DEBUG
            if (!Screens.Contains(debugOverlay))
            AddScreen(debugOverlay);
#endif
            gameObjectManager.RootObjects = new(Screens);
        }

        public void InsertScreen(int index, Screen screen)
        {
            Screens.Insert(index, InitializeScreen(screen));
#if DEBUG
            if (!Screens.Contains(debugOverlay))
                AddScreen(debugOverlay);
#endif
            gameObjectManager.RootObjects = new(Screens);
        }

        public void RemoveScreen(Screen screen)
        {
            ClearAllObjectsFromScreen(screen);
            Screens.Remove(screen);
            gameObjectManager.RootObjects.Remove(screen);
        }
        public void RemoveAllScreens()
        {
            foreach (var screen in Screens)
            {
                ClearAllObjectsFromScreen(screen);
            }
            Screens.Clear();
            gameObjectManager.RootObjects.Clear();
        }

        public override void AddChild(IContainer child)
        {
            base.AddChild(child);
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
       
        }

        public void ChangeScreen(Screen screen)
        {
            RemoveAllScreens();   
            AddScreen(screen);
        }

        public override void Update(GameTime gameTime)
        {
            //if (!HasScreens) return;
            //foreach (var screen in Screens)
            //    screen.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //if (!HasScreens) return;
            //foreach (var screen in Screens)
            //    screen.Draw(gameTime, spriteBatch, graphicsDevice);
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

        private void ClearAllObjectsFromScreen(Screen screen)
        {
            var objectsToRemove = new List<GameObject>();
            foreach (var obj in screen.Children)
            {
                if (obj is GameObject gObj) objectsToRemove.Add(gObj);
            }
            foreach (var obj in objectsToRemove)
            {
                gameObjectManager.RemoveObject(obj);
            }
            gameObjectManager.RemoveObject(screen);
        }
    }
}
