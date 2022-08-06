using FrivoloCo.Screens.Play.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace FrivoloCo.Screens.Play
{
    public class CustomerStation : GameObject
    {
        private Customer customer;

        private readonly GameState state;
        private readonly ProgressState progress;
        public CustomerStation(GameState state, ProgressState progress)
        {
            this.state = state;
            this.progress = progress;
        }

        private Sprite sp;
        private Box box;
        private TextBlock tb;

        private void AddCustomer()
        {
            customer = new Customer()
            {
                Order = new()
                {
                    new()
                    {
                        Type = ItemType.FlatWhite,
                        Amount = 1
                    }
                }
            };

            sp = new Sprite(customer.Sprite)
            {
                RelativePosition = new(0, 0, 210, 335),
                Layout = Layout.AnchorBottom
            };
            AddChild(Game.AddObject(sp));

            box = new Box()
            {
                RelativePosition = new(0, 0, 0, 188),
                Layout = Layout.DockTop,
                Color = Color.White
            };
            tb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 25), "", Color.Black)
            {
                Layout = Layout.Fill,
                Margin = 5
            };
            AddChild(Game.AddObject(box));
            box.AddChild(Game.AddObject(tb));
            SoundEffect.FromFile(customer.WantToOrderSound).Play();
        }

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            AddCustomer();
        }

        public override void Update(GameTime gameTime)
        {
            if (customer is null) return;

            var sb = new StringBuilder();
            sb.AppendLine("I WANT 2 ORDER");
            foreach (var x in customer.Order)
            {
                sb.AppendLine($"{x.Amount} {x.Type} (was given {x.AmountGotten})");
            }
            sb.AppendLine();
            sb.AppendLine($"{customer.Happiness * 100}% happiness");
            tb.Text = sb.ToString();
        }
    }

    public class Customer
    {
        public string Name { get; set; } = "JPlexer";

        public int MaxPatience { get; set; } = 20; // in seconds

        public int Patience { get; set; } = 20; // in seconds

        public double Happiness => Patience / MaxPatience;

        public string Sprite { get; set; } = "Assets/Gameplay/Customers/JPlexer/person.png";

        public string WantToOrderSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/iwanttoorder.wav";

        public string ImpatientSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatient.wav";

        public string ImpatienterSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatienter.wav";

        public string ImDoneSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/imdone.wav";

        public string AngerySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/angery.wav";

        public string ThankYouHappySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/thankyou.wav";

        public string ThankYouImpatientSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatientthankyou";

        public string ThankYouAngerySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/angerythankyou.wav";

        public string WrongOrderSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/wrongorder.wav";

        public List<OrderItem> Order { get; set; }
    }

    public class OrderItem
    {
        public ItemType Type { get; init; }

        public int Amount { get; init; }

        public int AmountGotten { get; set; } = 0;
    }
}
