using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Containers;
using Water.Graphics.Screens;
using Water.Graphics;
using Water.Graphics.Controls;
using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Water.Utils;

namespace FrivoloCo.Screens.Play
{
    public class FailScreen : Screen
    {
        private readonly ProgressState progress;
        public FailScreen(ProgressState progress)
        {
            this.progress = progress;
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override async void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;

            MediaPlayer.Play(Song.FromUri("failure", new("Assets/Gameplay/failure.ogg", UriKind.Relative)));

            var rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Layout.Fill,
                RelativePosition = new(0, 0, 0, 0)
            };
            var co = new Container()
            {
                Layout = Layout.Fill,
                RelativePosition = new(0, 0, 1920, 1080)
            };
            AddChild(rc);
            rc.AddChild(co);

            var box = new Box()
            {
                Layout = Layout.Fill,
                RelativePosition = new(0, 0, 0, 0),
                Color = Color.Black
            };
            co.AddChild(Game.AddObject(box));

            var tb = new TextBlock(new(0, 50, 1000, 100), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 50), "", Color.White)
            {
                Layout = Layout.HorizontalCenter,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Center
            };

            await WriteRecordAsync();
            var records = await ReadRecordsAsync();
            var sb = new StringBuilder();
            int i = 1;
            var top = records.OrderByDescending(x => x.Money);

            if (progress.Money == top.ToList().First().Money)
                sb.AppendLine("Leaderboard: New high score! :o");
            else sb.AppendLine("Leaderboard");

            foreach (var record in records.OrderByDescending(x => x.Money))
            {
                sb.AppendLine($"{i}: ${record.Money:0..00} (Day {record.Day}) {record.StartedPlaying}");
                i++;
            }

            tb.Text = 
                $"FAILED\nYour terrible performance caused people to stop going to FrivoloCo,\n" +
                $"so you were fired.\n" +
                $"(press ESC to go back)\n" +
                $"\n" +
                $"Score: ${progress.Money:0..00}\n" +
                $"Mistakes: {progress.TotalStrikes}   Customers Served: {progress.CustomersServed}    Day: {progress.Day}\n" +
                $"\n" +
                $"{sb}";
            co.AddChild(Game.AddObject(tb));
        }

        private async Task<List<ProgressState>> ReadRecordsAsync()
        {
            return await JsonUtils.ReadAsync<List<ProgressState>>("Data/records.json");
        }

        private async Task WriteRecordAsync()
        {
            var records = await ReadRecordsAsync();
            records.Add(progress);
            await JsonUtils.WriteAsync("Data/records.json", records);
        }

        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
            {
                MediaPlayer.Stop();
                ScreenManager.ChangeScreen(new MenuScreen());
            }
        }

        private double counter;
        private bool hasPlayedBeingFired = false;
        private bool hasPlayedDontFireMe = false;

        public override void Update(GameTime gameTime)
        {
            counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasPlayedBeingFired && counter >= 5 && counter <= 9.5)
            {
                hasPlayedBeingFired = true;
                SoundEffect.FromFile("Assets/Gameplay/Customers/Victor/fired.wav").Play();
            }
            if (!hasPlayedDontFireMe && counter >= 9.6)
            {
                hasPlayedDontFireMe = true;
                SoundEffect.FromFile("Assets/Gameplay/Ian/dontfiremepls.wav").Play();
            }
        }
    }
}
