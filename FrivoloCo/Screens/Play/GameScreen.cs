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

        private CustomerStation s1;
        private CustomerStation s2;
        private CustomerStation s3;
        private CustomerStation s4;
        private CustomerStation s5;

        private Sprite sunriseSp;
        private Sprite daySp;
        private Sprite sunsetSp;
        private Sprite nightSp;

        private RenderContainer rc;

        public override void Initialize()
        {
            title = Game.MainGame.Window.Title;
            Game.Input.KeyDown += Input_KeyDown;

            if (progressState.SongPlayed >= 6)
                progressState.SongPlayed = 1;
            if (progressState.SongPlayed == 1)
                MediaPlayer.Play(Song.FromUri("gameplay-1", new("Assets/Music/gameplay-1.ogg", UriKind.Relative)));
            else if (progressState.SongPlayed == 2)
                MediaPlayer.Play(Song.FromUri("gameplay-2", new("Assets/Music/gameplay-2.ogg", UriKind.Relative)));
            else if (progressState.SongPlayed == 3)
                MediaPlayer.Play(Song.FromUri("gameplay-3", new("Assets/Music/gameplay-3.ogg", UriKind.Relative)));
            else if (progressState.SongPlayed == 4)
                MediaPlayer.Play(Song.FromUri("gameplay-4", new("Assets/Music/gameplay-4.ogg", UriKind.Relative)));
            else if (progressState.SongPlayed == 5)
                MediaPlayer.Play(Song.FromUri("gameplay-5", new("Assets/Music/gameplay-5.ogg", UriKind.Relative)));
            progressState.SongPlayed++;
            // Playfield
            rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Layout.Fill
            };
            //sunriseSp = new Sprite("Assets/Gameplay/gameplaysunrise.png")
            //{
            //    RelativePosition = new(0, 0, 1920, 1080),
            //    Layout = Layout.Fill
            //};
            daySp = new Sprite("Assets/Gameplay/gameplayday.png")
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Fill
            };
            //sunsetSp = new Sprite("Assets/Gameplay/gameplaysunset.png")
            //{
            //    RelativePosition = new(0, 0, 1920, 1080),
            //    Layout = Layout.Fill
            //};
            //nightSp = new Sprite("Assets/Gameplay/gameplaynight.png")
            //{
            //    RelativePosition = new(0, 0, 1920, 1080),
            //    Layout = Layout.Fill
            //};

            var co = new Container()
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Center
            };

            //co.AddChild(Game.AddObject(sunriseSp));
            co.AddChild(Game.AddObject(daySp));
            //co.AddChild(Game.AddObject(sunsetSp));
            //co.AddChild(Game.AddObject(nightSp));

            rc.AddChild(co);

            AddChild(rc);


            s1 = new CustomerStation(State, progressState)
            {
                RelativePosition = new(18, 98, 365, 534)
            };
            co.AddChild(Game.AddObject(s1));
            s2 = new CustomerStation(State, progressState)
            {
                RelativePosition = new(398, 98, 365, 534)
            };
            co.AddChild(Game.AddObject(s2));
            s3 = new CustomerStation(State, progressState)
            {
                RelativePosition = new(778, 98, 365, 534)
            };
            co.AddChild(Game.AddObject(s3));
            s4 = new CustomerStation(State, progressState)
            {
                RelativePosition = new(1158, 98, 365, 534)
            };
            co.AddChild(Game.AddObject(s4));
            s5 = new CustomerStation(State, progressState)
            {
                RelativePosition = new(1538, 98, 365, 534)
            };
            co.AddChild(Game.AddObject(s5));

            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.IceTea, State)
            {
                RelativePosition = new(550, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.FlatWhite, State)
            {
                RelativePosition = new(690, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.HotChocolate, State)
            {
                RelativePosition = new(830, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Latte, State)
            {
                RelativePosition = new(970, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Espresso, State)
            {
                RelativePosition = new(1110, 850, 120, 230)
            }));
            co.AddChild(Game.AddObject(new ItemDispenser(ItemType.Cappuccino, State)
            {
                RelativePosition = new(1250, 850, 120, 230)
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
            moneyTb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 28), "", Color.Black)
            {
                Layout = Layout.Fill,
                HorizontalTextAlignment = HorizontalTextAlignment.Left,
                VerticalTextAlignment = VerticalTextAlignment.Center,
                Margin = 2
            };
            moneyBox.AddChild(Game.AddObject(moneyTb));

            var statusBox = new Box()
            {
                RelativePosition = new(0, 0, 300, 30),
                Layout = Layout.AnchorTopLeft,
                Margin = 10,
                Color = Color.White
            };
            AddChild(Game.AddObject(statusBox));
            statusTb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 28), "", Color.Black)
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
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.F4)
                State.TimeLeft = 10;
        }

        public GameState State { get; private set; } = new();

        private bool CustomersAreNotStillPresent => s1.IsEmpty && s2.IsEmpty && s3.IsEmpty && s4.IsEmpty && s5.IsEmpty;

        //private int stage = 0;
        //private double counter = 15000;

        private double delayBeforeScreenAdvance = 3;
        private bool inactivePause = false;

        public override void Update(GameTime gameTime)
        {
            if (!Game.MainGame.IsActive && !State.Paused)
            {
                State.Paused = true;
                inactivePause = true;
                MediaPlayer.Pause();
            }
            else if (Game.MainGame.IsActive && inactivePause)
            {
                State.Paused = false;
                inactivePause = false;
                MediaPlayer.Resume();
            }

            if (State.Paused) return;

            //counter -= gameTime.ElapsedGameTime.TotalMilliseconds;
            //if (stage == 0)
            //{
            //    sunriseSp.Color = Color.White * (float)(counter / 15000);
            //    if (counter <= 0)
            //    {
            //        counter = 15000;
            //        stage++;
            //    }
            //}
            //else if (stage == 1)
            //{
            //    daySp.Color = Color.White * (float)(counter / 15000);
            //    if (counter <= 0)
            //    {
            //        counter = 15000;
            //        stage++;
            //    }
            //}
            //else if (stage == 2)
            //{
            //    sunsetSp.Color = Color.White * (float)(counter / 15000);
            //    if (counter <= 0)
            //    {
            //        counter = 15000;
            //        stage++;
            //    }
            //}
            //else if (stage == 3)
            //{
            //    nightSp.Color = Color.White * (float)(counter / 15000);
            //    if (counter <= 0)
            //    {
            //        counter = 15000;
            //        stage++;
            //    }
            //}

            if (State.Strikes >= 3)
            {
                MediaPlayer.Stop();
                ScreenManager.ChangeScreen(new FailScreen(progressState));
                return;
            }

            if (State.TimeLeft <= 0)
            {
                State.TimeDelayBetweenCustomers = 999;
                if (CustomersAreNotStillPresent)
                {
                    MediaPlayer.Stop();
                    delayBeforeScreenAdvance -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (delayBeforeScreenAdvance <= 0)
                    {
                        progressState.Day++;
                        ScreenManager.ChangeScreen(new PreGameScreen(progressState));
                        return;
                    }
                }
            }

#if DEBUG
            Game.MainGame.Window.Title = State.TimeDelayBetweenCustomers.ToString();
#endif
            State.TimeLeft -= gameTime.ElapsedGameTime.TotalMilliseconds;
            State.TimeDelayBetweenCustomers -= gameTime.ElapsedGameTime.TotalMilliseconds;

            moneyTb.Text = $"${progressState.Money:0..00}";
            statusTb.Text = $"Day {progressState.Day} | {Math.Round(TimeSpan.FromMilliseconds(State.TimeLeft).TotalSeconds)} s left | {State.Strikes}/3 strikes";
        }
    }
}
