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

        public void AddScreen(Screen screen)
        {
            Screens.Add(screen);
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
                screen.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!HasScreens) return;
            foreach (var screen in Screens)
                screen.Draw(gameTime, spriteBatch, graphicsDevice);
        }

        public void UpdateScreenSize(Rectangle newSize)
        {
            ActualPosition = newSize;
            RelativePosition = newSize;
            CalculateChildrenPositions();
        }
    }
}
