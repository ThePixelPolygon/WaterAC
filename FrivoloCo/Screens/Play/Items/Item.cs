using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace FrivoloCo.Screens.Play.Items
{
    public class Item : Sprite
    {
        public bool IsBeingDragged { get; private set; } = false;

        public virtual string Name { get; set; } = "Placeholder";

        public ItemType Type { get; set; } = ItemType.Placeholder;

        private GameState state;

        public Item(string texturePath, bool isBeingDragged, string name, ItemType type, GameState state) : base(texturePath)
        {
            RelativePosition = new(0, 0, 120, 230);
            IsBeingDragged = isBeingDragged;
            Name = name;
            Type = type;
            this.state = state;
        }

        public override void Initialize()
        {
            if (Layout != Layout.Manual)
                throw new Exception("Kyaa >.< !, The only valid layout for me is manual!");

            Game.Input.PrimaryMouseButtonDown += Input_PrimaryMouseButtonDown;
            Game.Input.PrimaryMouseButtonUp += Input_PrimaryMouseButtonUp;
            base.Initialize();
        }

        private void Input_PrimaryMouseButtonDown(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this))
                IsBeingDragged = true;
        }

        private void Input_PrimaryMouseButtonUp(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this))
            {
                IsBeingDragged = false;
                state.CurrentlyDraggedItem = null;
            }
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonDown -= Input_PrimaryMouseButtonDown;
            Game.Input.PrimaryMouseButtonUp -= Input_PrimaryMouseButtonUp;
            base.Deinitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (state.Paused) return;

            if (IsBeingDragged)
            {
                state.CurrentlyDraggedItem = this;
                var pos = Game.Input.GetMousePositionRelativeTo(Parent);
                RelativePosition = new(pos.X - (RelativePosition.Width / 2), pos.Y - (RelativePosition.Height / 2), RelativePosition.Width, RelativePosition.Height);
            }
            else
            {
                if (Parent is not null)
                {
                    if (RelativePosition.Y <= Parent.ActualPosition.Height - state.TableTopOffset + RelativePosition.Height)
                        RelativePosition = new(RelativePosition.X, RelativePosition.Y + 1, RelativePosition.Width, RelativePosition.Height);
                }
            }
            base.Update(gameTime);
        }
    }
}
