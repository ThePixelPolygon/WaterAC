using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Water.Graphics;
using Water.Graphics.Screens;
using Water.Graphics.Controls;
using Water.Graphics.Containers;

namespace FrivoloCo.Screens;

public class CreditOverlay : Screen
{
    private arcadeShim acShim = arcadeShim.getInstance();
    private UniformStackContainer stackContainer;

    private TextBlock creditDisplay;
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

        creditDisplay = new TextBlock(new(0, 0, 100, 60),
            Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 36), "", Color.Black);
    }

    public override void Deinitialize()
    {
        Game.Input.KeyDown -= Input_KeyDown;
    }

    public override void Update(GameTime gameTime)
    {
        int coinsPerCredit = acShim.CoinsPerCredit;
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
        throw new System.NotImplementedException();
    }
}