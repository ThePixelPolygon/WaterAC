﻿using FrivoloCo.Screens.Play.Items;
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
        private double minIntervalBetweenCustomers; // in ms
        public CustomerStation(GameState state, ProgressState progress)
        {
            this.state = state;
            this.progress = progress;
            ResetMinInterval();
        }

        private Sprite sp;
        private Box box;
        private TextBlock tb;

        private void CustomerEnters()
        {
            customer = new Customer()
            {
            };

            int patience = 50;    // TODO: reimplement these as algorithms instead of this mess lol
            if (progress.Day >= 1 && progress.Day <= 2)
                patience = 50;
            else if (progress.Day >= 3 && progress.Day <= 4)
                patience = 40;
            else if (progress.Day >= 5 && progress.Day <= 6)
                patience = 30;
            else if (progress.Day >= 7)
                patience = 20;

            customer.MaxPatience = customer.Patience = patience;

            int amountOfEntries = 1;
            if (progress.Day >= 1 && progress.Day <= 3)
                amountOfEntries = 1;
            else if (progress.Day >= 4 && progress.Day <= 6)
                amountOfEntries = 2;
            else if (progress.Day >= 7 && progress.Day <= 9)
                amountOfEntries = 3;
            else if (progress.Day >= 10)
                amountOfEntries = 4;

            for (int i = 0; i < amountOfEntries; i++)
            {
                var type = (ItemType)Random.Shared.Next(0, 6);

                if (!customer.Order.Select(x => x.Type).Contains(type))
                {
                    customer.Order.Add(new()
                    {
                        Type = type,
                        Amount = Random.Shared.Next(1, 4)
                    });
                }
            }

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

            state.TimeDelayBetweenCustomers = state.TimeDelayBetweenCustomersMax;
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
                state.Strikes++;
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
        }

        private GameObject scheduledObjectRemoval;

        private void Input_PrimaryMouseButtonUp(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this) && state.CurrentlyDraggedItem != null)
            {
                if (customer is null) return;

                var wasRightOrder = false;
                foreach (var entry in customer.Order)
                {
                    if (entry.Type == state.CurrentlyDraggedItem.Type)
                    {
                        entry.AmountGotten++;
                        wasRightOrder = true;
                    }
                }
                scheduledObjectRemoval = state.CurrentlyDraggedItem;
                if (!wasRightOrder)
                {
                    SoundEffect.FromFile(customer.WrongOrderSound).Play();
                    state.Strikes++;
                    return;
                }
                foreach (var entry in customer.Order)
                {
                    if (entry.AmountGotten < entry.Amount)
                        return;
                }
                CustomerLeaves();
            }
        }

        private void ResetMinInterval()
        {
            var min = 5000 - (500 * progress.Day);
            if (min <= 0)
                min = 100;
            var max = 15000 - (500 * progress.Day);
            if (max <= 0)
                max = 1000;
            minIntervalBetweenCustomers = Random.Shared.Next(min, max);
        }

        public override void Update(GameTime gameTime)
        {
            if (state.Paused) return;

            if (scheduledObjectRemoval is not null) Game.RemoveObject(scheduledObjectRemoval);

            if (customer is null)
            {
                minIntervalBetweenCustomers -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (minIntervalBetweenCustomers <= 0)
                {
                    var diceRoll = Random.Shared.Next(0, 5000);
                    if (diceRoll == 5 && state.TimeDelayBetweenCustomers <= 0)
                    {
                        CustomerEnters();
                        ResetMinInterval();
                        return;
                    }
                }
            }
            else
            {
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
    }

    public class Customer
    {
        public string Name { get; set; } = "JPlexer";

        public double MaxPatience { get; set; } = 20; // in seconds

        public double Patience { get; set; } = 20; // in seconds

        public double Happiness => Patience / MaxPatience;

        public string Sprite { get; set; } = "Assets/Gameplay/Customers/Victor/person.png";

        public string WantToOrderSound { get; set; } = "Assets/Gameplay/Customers/Victor/iwanttoorder.wav";

        public string ImpatientSound { get; set; } = "Assets/Gameplay/Customers/Victor/impatient.wav";

        public string ImpatienterSound { get; set; } = "Assets/Gameplay/Customers/Victor/impatienter.wav";

        public string ImDoneSound { get; set; } = "Assets/Gameplay/Customers/Victor/imdone.wav";

        public string AngerySound { get; set; } = "Assets/Gameplay/Customers/Victor/angery.wav";

        public string ThankYouHappySound { get; set; } = "Assets/Gameplay/Customers/Victor/thankyou.wav";

        public string ThankYouImpatientSound { get; set; } = "Assets/Gameplay/Customers/Victor/impatientthankyou.wav";

        public string ThankYouAngerySound { get; set; } = "Assets/Gameplay/Customers/Victor/angerythankyou.wav";

        public string WrongOrderSound { get; set; } = "Assets/Gameplay/Customers/Victor/wrongorder.wav";

        public bool HasBeenImpatient { get; set; } = false;

        public bool HasBeenImpatienter { get; set; } = false;

        public bool HasBeenAngery { get; set; } = false;

        public List<OrderItem> Order { get; set; } = new();
    }

    public class OrderItem
    {
        public ItemType Type { get; init; }

        public int Amount { get; init; }

        public int AmountGotten { get; set; } = 0;
    }
}
