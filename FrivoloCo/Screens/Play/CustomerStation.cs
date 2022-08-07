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
                        Amount = 5
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

        private void CustomerLeaves()
        {
            Game.RemoveObject(sp);
            Game.RemoveObject(box);
            Game.RemoveObject(tb);
            sp = null;
            box = null;
            tb = null;
            var happy = true;
            if (customer.Happiness >= 0.61)
            {
                SoundEffect.FromFile(customer.ThankYouHappySound).Play();
            }
            else if (customer.Happiness <= 0.60 && customer.Happiness >= 0.45)
            {
                SoundEffect.FromFile(customer.ThankYouImpatientSound).Play();
            }
            else if (customer.Happiness <= 0.45 && customer.Happiness >= 0.20)
            {
                SoundEffect.FromFile(customer.ThankYouImpatientSound).Play();
            }
            else if (customer.Happiness <= 0.20 && customer.Happiness >= 0.01)
            {
                SoundEffect.FromFile(customer.ThankYouAngerySound).Play();
            }
            else if (customer.Happiness <= 0)
            {
                SoundEffect.FromFile(customer.ImDoneSound).Play();
                happy = false;
            }
            if (happy)
            {
                progress.Money += (decimal)(10 * customer.Happiness * progress.Day);
                SoundEffect.FromFile("Assets/Gameplay/kaching.wav").Play();
            }
            customer = null;
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonUp -= Input_PrimaryMouseButtonUp;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            Game.Input.PrimaryMouseButtonUp += Input_PrimaryMouseButtonUp;
            AddCustomer();
        }

        private GameObject scheduledObjectRemoval;

        private void Input_PrimaryMouseButtonUp(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this) && state.CurrentlyDraggedItem != null)
            {
                foreach (var entry in customer.Order)
                {
                    if (entry.Type == state.CurrentlyDraggedItem.Type)
                        entry.AmountGotten++;
                }
                scheduledObjectRemoval = state.CurrentlyDraggedItem;
                foreach (var entry in customer.Order)
                {
                    if (entry.AmountGotten < entry.Amount)
                        return;
                }
                CustomerLeaves();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (scheduledObjectRemoval is not null) Game.RemoveObject(scheduledObjectRemoval);

            if (customer is null) return;

            customer.Patience -= gameTime.ElapsedGameTime.TotalSeconds;
            if (!customer.HasBeenImpatient && customer.Happiness <= 0.60 && customer.Happiness >= 0.45)
            {
                SoundEffect.FromFile(customer.ImpatientSound).Play();
                customer.HasBeenImpatient = true;
            }
            else if (!customer.HasBeenImpatienter && customer.Happiness <= 0.45 && customer.Happiness >= 0.20)
            {
                SoundEffect.FromFile(customer.ImpatienterSound).Play();
                customer.HasBeenImpatienter = true;
            }
            else if (!customer.HasBeenAngery && customer.Happiness <= 0.20 && customer.Happiness >= 0.01)
            {
                SoundEffect.FromFile(customer.AngerySound).Play();
                customer.HasBeenAngery = true;
            }
            else if (customer.Happiness <= 0)
            {
                CustomerLeaves();
                return;
            }    

            var sb = new StringBuilder();
            sb.AppendLine("I WANT 2 ORDER");
            foreach (var x in customer.Order)
            {
                sb.AppendLine($"{x.Amount} {x.Type} (was given {x.AmountGotten})");
            }
            sb.AppendLine();
            sb.AppendLine($"{customer.Happiness * 100:F1}% happiness");
            tb.Text = sb.ToString();
        }
    }

    public class Customer
    {
        public string Name { get; set; } = "JPlexer";

        public double MaxPatience { get; set; } = 20; // in seconds

        public double Patience { get; set; } = 20; // in seconds

        public double Happiness => Patience / MaxPatience;

        public string Sprite { get; set; } = "Assets/Gameplay/Customers/JPlexer/person.png";

        public string WantToOrderSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/iwanttoorder.wav";

        public string ImpatientSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatient.wav";

        public string ImpatienterSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatienter.wav";

        public string ImDoneSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/imdone.wav";

        public string AngerySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/angery.wav";

        public string ThankYouHappySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/thankyou.wav";

        public string ThankYouImpatientSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/impatientthankyou.wav";

        public string ThankYouAngerySound { get; set; } = "Assets/Gameplay/Customers/JPlexer/angerythankyou.wav";

        public string WrongOrderSound { get; set; } = "Assets/Gameplay/Customers/JPlexer/wrongorder.wav";

        public bool HasBeenImpatient { get; set; } = false;

        public bool HasBeenImpatienter { get; set; } = false;

        public bool HasBeenAngery { get; set; } = false;

        public List<OrderItem> Order { get; set; }
    }

    public class OrderItem
    {
        public ItemType Type { get; init; }

        public int Amount { get; init; }

        public int AmountGotten { get; set; } = 0;
    }
}
