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
        /// <summary>
        /// All screens in the game
        /// </summary>
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

        /// <summary>
        /// Adds a screen to the end of the screen stack
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(Screen screen)
        {
            Debug.WriteLine($"Added {screen}");
#if DEBUG
            if (Screens.Contains(debugOverlay))
                RemoveScreen(debugOverlay);
#endif
            Screens.Add(InitializeScreen(screen));
#if DEBUG
            if (!Screens.Contains(debugOverlay))
                AddScreen(debugOverlay);
#endif
            gameObjectManager.RootObjects = new(Screens);
        }

        /// <summary>
        /// Inserts a screen into the screen stack
        /// </summary>
        /// <param name="index"></param>
        /// <param name="screen"></param>
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

        /// <summary>
        /// Removes a screen from the stack. All of the screen's children will also be removed from the game
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(Screen screen)
        {
            Debug.WriteLine($"Removed {screen}");
            ClearAllObjectsFromScreen(screen);
            Screens.Remove(screen);
            gameObjectManager.RootObjects.Remove(screen);
        }
        /// <summary>
        /// Removes all screens from the stack
        /// </summary>
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

        /// <summary>
        /// Removes all screens, then adds the screen
        /// </summary>
        /// <param name="screen"></param>
        public void ChangeScreen(Screen screen)
        {
            RemoveAllScreens();   
            AddScreen(screen);
        }

        /// <summary>
        /// Updates all of the game screen's sizes to the new size of the screen
        /// </summary>
        /// <param name="newSize">The new size of the screen</param>
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
