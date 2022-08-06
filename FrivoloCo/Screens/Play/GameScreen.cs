using FrivoloCo.Screens.Menu;
using FrivoloCo.Screens.Play.Items;
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
        private ProgressState state;
        public GameScreen(ProgressState state)
        {
            this.state = state;
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
            rc.Dispose();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        private TextBlock moneyTb;
        private TextBlock statusTb;
        private RenderContainer rc;

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;

            MediaPlayer.Play(Song.FromUri("gameplay-1", new("Assets/Music/gameplay-1.ogg", UriKind.Relative)));
            // Playfield
            rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Layout.Fill
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

            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.FlatWhite, State)
            {
                Layout = Layout.AnchorBottomLeft,
            }));

            co.AddChild(Game.AddObject(new CustomerStation(State, state)
            {
                RelativePosition = new(21, 98, 365, 534)
            }));
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

        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
            {
                MediaPlayer.Stop();
                ScreenManager.ChangeScreen(new MenuScreen());
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.F5)
                ScreenManager.ChangeScreen(new GameScreen(new ProgressState()));
        }

        public GameState State { get; private set; } = new();

        public override void Update(GameTime gameTime)
        {
            Game.MainGame.Window.Title = State.CurrentlyDraggedItem?.ToString() ?? "nothing";
            State.TimeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;

            moneyTb.Text = $"${state.Money:0..00}";
            statusTb.Text = $"Day {state.Day}    {State.TimeLeft} (<- placeholder)";
        }
    }
}
