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

        public virtual string Name => "Placeholder";

        public Item(string texturePath, bool isBeingDragged) : base(texturePath)
        {
            IsBeingDragged = isBeingDragged;
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
                IsBeingDragged = false;
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonDown -= Input_PrimaryMouseButtonDown;
            Game.Input.PrimaryMouseButtonUp -= Input_PrimaryMouseButtonUp;
            base.Deinitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsBeingDragged)
            {
                var pos = Game.Input.GetMousePositionRelativeTo(Parent);
                RelativePosition = new(pos.X - 60, pos.Y - 115, 120, 230);
            }
            else
            {
                if (Parent is not null)
                    if (RelativePosition.Y <= Parent.ActualPosition.Height - 230)
                        RelativePosition = new(RelativePosition.X, RelativePosition.Y + 1, 120, 230);
            }
            base.Update(gameTime);
        }
    }
}
