using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Water.Graphics.Screens;
using Water.Graphics.Controls;
using Water.Graphics.Containers;

namespace FrivoloCo.Screens;

public class CreditOverlay : Screen
{
    private arcadeShim acShim = arcadeShim.getInstance();
    private UniformStackContainer stackContainer;
    private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
    {
        if (e.Key == Keys.F3)
        {
            acShim.acceptCoin();
        }
    }
    public override void Initialize()
    {
        Game.Input.KeyDown += Input_KeyDown;
        stackContainer = new UniformStackContainer()
        {
            RelativePosition = new Rectangle(0, 0, 200, 60),
            Layout = Water.Graphics.Layout.DockBottom
        };
    }

    public override void Deinitialize()
    {
        Game.Input.KeyDown -= Input_KeyDown;
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        throw new System.NotImplementedException();
    }
}