using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Play
{
    public class GameScreen : Screen
    {
        private GameState state;
        public GameScreen(GameState state)
        {
            this.state = state;
        }

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        private TextBlock moneyTb;
        private TextBlock statusTb;

        public override void Initialize()
        {
            MediaPlayer.Play(Song.FromUri("gameplay-1", new("Assets/Music/gameplay-1.ogg", UriKind.Relative)));
            // Playfield
            var rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Water.Graphics.Layout.Fill
            };
            var sp = new Sprite("Assets/Gameplay/gameplayday.png")
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Fill
            };
            var co = new Container()
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Center
            };
            co.AddChild(Game.AddObject(sp));

            rc.AddChild(co);

            AddChild(rc);
            // HUD
            var moneyBox = new Box()
            {
                RelativePosition = new(0, 0, 250, 30),
                Layout = Layout.AnchorTopRight,
                Margin = 10,
                Color = Color.White
            };
            AddChild(Game.AddObject(moneyBox));
            moneyTb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/Fonts/parisienne-regular.ttf", 28), "", Color.Black)
            {
                Layout = Layout.Fill,
                HorizontalTextAlignment = HorizontalTextAlignment.Left,
                VerticalTextAlignment = VerticalTextAlignment.Center,
                Margin = 2
            };
            moneyBox.AddChild(Game.AddObject(moneyTb));

            var statusBox = new Box()
            {
                RelativePosition = new(0, 0, 250, 30),
                Layout = Layout.AnchorTopLeft,
                Margin = 10,
                Color = Color.White
            };
            AddChild(Game.AddObject(statusBox));
            statusTb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/Fonts/parisienne-regular.ttf", 28), "", Color.Black)
            {
                Layout = Layout.Fill,
                HorizontalTextAlignment = HorizontalTextAlignment.Left,
                VerticalTextAlignment = VerticalTextAlignment.Center,
                Margin = 2
            };
            statusBox.AddChild(Game.AddObject(statusTb));
        }

        private double timeLeft = 120000; // 2 minutes in milliseconds

        public override void Update(GameTime gameTime)
        {
            timeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;

            moneyTb.Text = $"${state.Money:0..00}";
            statusTb.Text = $"Day {state.Day}    {timeLeft} (<- placeholder)";
        }
    }
}
