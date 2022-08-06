using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Screens;

namespace Water.Graphics.Screens
{
    public class ScreenManager
    {
        public List<Screen> Screens { get; } = new();
        public bool HasScreens { get => Screens.Count > 0; }

        private GameObjectManager gameObjectManager;
        private GameWindow window;

        private Rectangle currentScreenSize;
#if DEBUG
        private DebugOverlay debugOverlay = new();
#endif
        public ScreenManager(GameObjectManager gameObjectManager, GameWindow window)
        {
            this.gameObjectManager = gameObjectManager;
            this.window = window;
        }

        private Screen InitializeScreen(Screen screen)
        {
            screen.Game = gameObjectManager;
            screen.ScreenManager = this;
            screen.Window = window;
            screen.ActualPosition = currentScreenSize;
            screen.RelativePosition = currentScreenSize;

            screen.Game.AddObject(screen); 
            screen.CalculateChildrenPositions();
            return screen;
        }

        public void AddScreen(Screen screen)
        {
            Debug.WriteLine($"Added {screen}");
            Screens.Add(InitializeScreen(screen));
#if DEBUG
            if (!Screens.Contains(debugOverlay))
            AddScreen(debugOverlay);
#endif
            gameObjectManager.RootObjects = new(Screens);
        }

        public void InsertScreen(int index, Screen screen)
        {
            Debug.WriteLine($"Inserted {screen} to {index}");
            Screens.Insert(index, InitializeScreen(screen));
#if DEBUG
            if (!Screens.Contains(debugOverlay))
                AddScreen(debugOverlay);
#endif
            gameObjectManager.RootObjects = new(Screens);
        }

        public void RemoveScreen(Screen screen)
        {
            Debug.WriteLine($"Removed {screen}");
            ClearAllObjectsFromScreen(screen);
            Screens.Remove(screen);
            gameObjectManager.RootObjects.Remove(screen);
        }
        public void RemoveAllScreens()
        {
            Debug.WriteLine("Remove all screens!");
            foreach (var screen in Screens)
            {
                ClearAllObjectsFromScreen(screen);
            }
            Screens.Clear();
            gameObjectManager.RootObjects.Clear();
        }

        public void ChangeScreen(Screen screen)
        {
            RemoveAllScreens();   
            AddScreen(screen);
        }

        public void UpdateScreenSize(Rectangle newSize)
        {
            currentScreenSize = newSize;
            foreach (var screen in Screens)
            {
                screen.ActualPosition = newSize;
                screen.RelativePosition = newSize;
                screen.CalculateChildrenPositions();
            }
        }

        private void ClearAllObjectsFromScreen(Screen screen)
        {
            var containers = ExtensionClass.FlattenWithLevel<IContainer>(screen, x => x.Children);
            
            foreach (var container in containers)
            {
                if (container.Item1 is GameObject obj)
                    gameObjectManager.RemoveObject(obj);
            }
        }
    }
}
