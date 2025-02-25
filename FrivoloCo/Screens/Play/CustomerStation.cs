﻿using FrivoloCo.Screens.Play.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace FrivoloCo.Screens.Play
{
    public class CustomerStation : GameObject
    {
        public float StereoPan { get; init; }

        public bool IsEmpty => customer is null;

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
            var possibleCustomers = Directory.EnumerateDirectories("Assets/Gameplay/Customers");
            var selectedCustomer = possibleCustomers.ToList()[Random.Shared.Next(0, possibleCustomers.Count())];
            var customerMeta = File.ReadAllText(Path.Combine(Path.GetFullPath(selectedCustomer), "meta.txt"));
            customer = new Customer()
            {
                Name = customerMeta,
                RootPath = Path.GetFullPath(selectedCustomer)
            };

            int patience = 50;    // TODO: reimplement these as algorithms instead of this mess lol
            switch (progress.Day)
            {
                case >= 1 and <= 2:
                    patience = 50;
                    break;
                case >= 3 and <= 4:
                    patience = 40;
                    break;
                case >= 5 and <= 6:
                    patience = 30;
                    break;
                case >= 7 and <= 9:
                    patience = 20;
                    break;
                case >= 10 and <= 11:
                    patience = 10;
                    break;
                case 12:
                    patience = 9;
                    break;
                case 13:
                    patience = 8;
                    break;
                case 14:
                    patience = 7;
                    break;
                case 15:
                    patience = 6;
                    break;
                case >= 16:
                    patience = 5;
                    break;
            }

            customer.MaxPatience = customer.Patience = patience;

            int amountOfEntries = 1;
            switch (progress.Day)
            {
                case >= 1 and <= 3:
                    amountOfEntries = 1;
                    break;
                case >= 4 and <= 6:
                    amountOfEntries = 2;
                    break;
                case >= 7 and <= 9:
                    amountOfEntries = 3;
                    break;
                case >= 10 and <= 14:
                    amountOfEntries = 4;
                    break;
                case >= 15:
                    amountOfEntries = 5;
                    break;
            }

            for (int i = 0; i < amountOfEntries; i++)
            {
                var type = (ItemType)Random.Shared.Next(0, 6);

                if (!customer.Order.Select(x => x.Type).Contains(type))
                {
                    customer.Order.Add(new()
                    {
                        Type = type,
                        Amount = 1
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
                Margins = new(5)
            };
            AddChild(Game.AddObject(box));
            box.AddChild(Game.AddObject(tb));
            Game.Audio.PlayEffect(customer.WantToOrderSound, true, 1, StereoPan);

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
            switch (customer.Happiness)
            {
                case >= 0.61:
                    Game.Audio.PlayEffect(customer.ThankYouHappySound, true, 1, StereoPan);
                    break;
                case <= 0.60 and >= 0.45:
                    Game.Audio.PlayEffect(customer.ThankYouImpatientSound, true, 1, StereoPan);
                    break;
                case <= 0.45 and >= 0.20:
                    Game.Audio.PlayEffect(customer.ThankYouImpatientSound, true, 1, StereoPan);
                    break;
                case <= 0.20 and >= 0.01:
                    Game.Audio.PlayEffect(customer.ThankYouAngerySound, true, 1, StereoPan);
                    break;
                case <= 0:
                    Game.Audio.PlayEffect(customer.ImDoneSound, true, 1, StereoPan);
                    state.Strikes++;
                    progress.TotalStrikes++;
                    happy = false;
                    break;
            }
            if (happy)
            {
                progress.Money += (decimal)(10 * customer.Happiness * progress.Day);
                progress.CustomersServed++;
                Game.Audio.PlayEffect("Assets/Gameplay/kaching.wav", true);
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
                Game.Audio.PlayEffect("Assets/Gameplay/Ian/hereyougo.wav", true);
                scheduledObjectRemoval = state.CurrentlyDraggedItem;
                if (!wasRightOrder)
                {
                    Game.Audio.PlayEffect(customer.WrongOrderSound, true, 1, StereoPan);
                    state.Strikes++;
                    progress.TotalStrikes++;
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
            var max = 15000 - (500 * progress.Day);
            if (min <= 0)
            {
                min = 1;
                max = 2;
            }
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
                    if (state.TimeDelayBetweenCustomers <= 0)
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
                    Game.Audio.PlayEffect(customer.ImpatientSound, true, 1, StereoPan);
                    customer.HasBeenImpatient = true;
                }
                else if (!customer.HasBeenImpatienter && customer.Happiness <= 0.45 && customer.Happiness >= 0.20)
                {
                    Game.Audio.PlayEffect(customer.ImpatienterSound, true, 1, StereoPan);
                    customer.HasBeenImpatienter = true;
                }
                else if (!customer.HasBeenAngery && customer.Happiness <= 0.20 && customer.Happiness >= 0.01)
                {
                    Game.Audio.PlayEffect(customer.AngerySound, true, 1, StereoPan);
                    customer.HasBeenAngery = true;
                }
                else if (customer.Happiness <= 0)
                {
                    CustomerLeaves();
                    return;
                }

                var sb = new StringBuilder();
                sb.AppendLine(customer.Name);
                foreach (var x in customer.Order)
                {
                    if (x.AmountGotten < x.Amount)
                    {
                        var name = x.Type switch
                        {
                            ItemType.IceTea => "Ice Tea",
                            ItemType.Espresso => "Espresso",
                            ItemType.FlatWhite => "Flat White",
                            ItemType.Cappuccino => "Cappucino",
                            ItemType.HotChocolate => "Hot Chocolate",
                            ItemType.Latte => "Latte",
                            _ => "something went horribly wrong"
                        };
                        sb.AppendLine($"{name}");
                    }
                }
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

        public string RootPath { get; set; }

        public string Sprite => Path.Combine(RootPath, "person.png");

        public string WantToOrderSound => Path.Combine(RootPath, "iwanttoorder.wav");

        public string ImpatientSound => Path.Combine(RootPath, "impatient.wav");

        public string ImpatienterSound => Path.Combine(RootPath, "impatienter.wav");

        public string ImDoneSound => Path.Combine(RootPath, "imdone.wav");

        public string AngerySound => Path.Combine(RootPath, "angery.wav");

        public string ThankYouHappySound => Path.Combine(RootPath, "thankyou.wav");

        public string ThankYouImpatientSound => Path.Combine(RootPath, "impatientthankyou.wav");

        public string ThankYouAngerySound => Path.Combine(RootPath, "angerythankyou.wav");

        public string WrongOrderSound => Path.Combine(RootPath, "wrongorder.wav");

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
