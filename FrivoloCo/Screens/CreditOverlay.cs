using System.Collections.Generic;
using System.Threading;
using FrivoloCo.Arcade;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Graphics.Controls;
using Water.Graphics.Containers;

namespace FrivoloCo.Screens
{
    public class CreditOverlay : Screen
    {
        private ArcadeShim acShim;
        private TextBlock creditDisplay;
        private UniformStackContainer stackContainer;

        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Keys.F3)
            {
                int status = acShim.AcceptCoin();
                switch (status)
                {
                    case 0:
                        Game.Audio.PlayEffect("Assets/Gameplay/Customers/Logan/thankyou.wav", true);
                        break;
                    case 1:
                        Game.Audio.PlayEffect("Assets/Gameplay/kaching.wav", true);
                        break;
                }
            }
        }
        public override void Initialize()
        {
            acShim = ArcadeShim.GetInstance();
            
            Game.Input.KeyDown += Input_KeyDown;   
            
            creditDisplay = new TextBlock(new(0, 0, 100, 40),
                Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 30), "", Color.Black)
            {
                Layout = Layout.Fill,
                VerticalTextAlignment = VerticalTextAlignment.Center,
                HorizontalTextAlignment = HorizontalTextAlignment.Center
            };
            
            stackContainer = new UniformStackContainer()
            {
                RelativePosition = new Rectangle(0, 0, 1920, 40),
                Layout = Layout.AnchorBottom
            };

            Box box = new Box()
            {
                RelativePosition = new(0, 0, 250, 40),
                Color = Color.White * 0.20f,
                Layout = Layout.AnchorBottom
            };
            
            AddChild(stackContainer);
            box.AddChild(Game.AddObject(creditDisplay));
            stackContainer.AddChild(Game.AddObject(box));
            
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
        }

        public override void Update(GameTime gameTime)
        {
            int coinsPerCredit = acShim.ArcadeConfig.coinOption.coins;
            int coinsInserted = acShim.CoinsInserted;
            int credits = acShim.Credits;

            if (coinsPerCredit == 0)
            {
                creditDisplay.Text = "FREE PLAY";
            }
            else
            {
                creditDisplay.Text = $"Credits: {credits} ({coinsInserted}/{coinsPerCredit})";
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

        }
    }
}