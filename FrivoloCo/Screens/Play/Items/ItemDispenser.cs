using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace FrivoloCo.Screens.Play.Items
{
    public class ItemDispenser : GameObject
    {
        private ItemType type;
        private GameState state;

        public ItemDispenser(ItemType type, GameState state)
        {
            this.type = type;
            this.state = state;
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonDown -= Input_PrimaryMouseButtonDown;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            Game.Input.PrimaryMouseButtonDown += Input_PrimaryMouseButtonDown;

            var x = GetItemForItemType(type);

            RelativePosition = x.RelativePosition;

            var sp = new Sprite(x.Path)
            {
                Layout = Layout.Fill,
                RelativePosition = new(0, 0, 0, 0)
            };
            AddChild(Game.AddObject(sp));

            var labelBox = new Box()
            {
                Color = Color.Black * 0.5f,
                RelativePosition = new(0, 0, 0, 100),
                Layout = Layout.DockBottom
            };
            var tb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/Fonts/parisienne-regular.ttf", 30), x.Name, Color.White)
            {
                Layout = Layout.Fill,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Center
            };
            labelBox.AddChild(Game.AddObject(tb));
            AddChild(Game.AddObject(labelBox));
        }

        private void Input_PrimaryMouseButtonDown(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this) && !state.Paused)
                Parent.AddChild(Game.AddObject(GetItemForItemType(type)));
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private Item GetItemForItemType(ItemType type) => type switch
        {
            ItemType.FlatWhite => new Item("Assets/Gameplay/Items/flatwhite.png", true, "Flat White", ItemType.FlatWhite, state),
            ItemType.Placeholder or _ => new Item("Assets/Gameplay/Items/placeholder.png", true, "Placeholder", ItemType.Placeholder, state)
        };
    }

    public enum ItemType
    {
        FlatWhite,
        Placeholder
    }
}
