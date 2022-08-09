using FrivoloCo.Screens.Menu;
using FrivoloCo.Screens.Play.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private ProgressState progressState;
        private string title;
        public GameScreen(ProgressState progressState)
        {
            this.progressState = progressState;
            State.TimeDelayBetweenCustomers = 5500 - (500 * progressState.Day); // TODO: better algo
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
            title = Game.MainGame.Window.Title;
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

            

            co.AddChild(Game.AddObject(new CustomerStation(State, progressState)
            {
                RelativePosition = new(18, 98, 365, 534)
            }));
            co.AddChild(Game.AddObject(new CustomerStation(State, progressState)
            {
                RelativePosition = new(398, 98, 365, 534)
            }));
            co.AddChild(Game.AddObject(new CustomerStation(State, progressState)
            {
                RelativePosition = new(778, 98, 365, 534)
            }));
            co.AddChild(Game.AddObject(new CustomerStation(State, progressState)
            {
                RelativePosition = new(1158, 98, 365, 534)
            }));
            co.AddChild(Game.AddObject(new CustomerStation(State, progressState)
            {
                RelativePosition = new(1538, 98, 365, 534)
            }));

            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.FlatWhite, State)
            {
                RelativePosition = new(0, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.IceTea, State)
            {
                RelativePosition = new(120, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.HotChocolate, State)
            {
                RelativePosition = new(240, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Latte, State)
            {
                RelativePosition = new(360, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Espresso, State)
            {
                RelativePosition = new(480, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Cappuccino, State)
            {
                RelativePosition = new(600, 850, 120, 230)
            }));

            co.CalculateChildrenPositions();
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
                State.Paused = !State.Paused;
                if (State.Paused)
                {
                    MediaPlayer.Pause();
                    Window.Title = "Game paused, hit ESC to continue";
                }
                else
                {
                    MediaPlayer.Resume();
                    Window.Title = title;
                } // temporary stopgap solution
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.F5)
                ScreenManager.ChangeScreen(new GameScreen(new ProgressState()));
        }

        public GameState State { get; private set; } = new();

        public override void Update(GameTime gameTime)
        {
            if (State.Paused) return;

            if (State.Strikes >= 3)
            {
                MediaPlayer.Stop();
                ScreenManager.ChangeScreen(new FailScreen(progressState));
                return;
            }

            //Game.MainGame.Window.Title = State.CurrentlyDraggedItem?.ToString() ?? "nothing";
            //Game.MainGame.Window.Title = State.TimeDelayBetweenCustomers.ToString();
            State.TimeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
            State.TimeDelayBetweenCustomers -= gameTime.ElapsedGameTime.TotalMilliseconds;

            moneyTb.Text = $"${progressState.Money:0..00}";
            statusTb.Text = $"Day {progressState.Day}    {State.TimeLeft} (<- placeholder)";
        }
    }
}
