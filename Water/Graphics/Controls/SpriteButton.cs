using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Controls
{
    public class SpriteButton : Sprite
    {
        private string regularPath;
        private string activatedPath;
        private Action onClicked;
        private bool hasBeenActivated = false;
        private bool spriteState = false;

        public SpriteButton(string regularPath, string activatedPath, Action onClicked) : base(regularPath)
        {
            this.regularPath = regularPath;
            this.activatedPath = activatedPath;
            this.onClicked = onClicked;
        }

        public override void Initialize()
        {
            Debug.WriteLine($"Initialized SB {this} - {regularPath} {activatedPath}");
            Game.Input.PrimaryMouseButtonDown += Input_PrimaryMouseButtonDown;
            base.Initialize();
        }

        private void Input_PrimaryMouseButtonDown(object sender, Input.MousePressEventArgs e)
        {
            if (spriteState) onClicked.Invoke();
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonDown -= Input_PrimaryMouseButtonDown;
            base.Deinitialize();
        }

        public override void Update(GameTime gameTime)
        {
            hasBeenActivated = Game.Input.IsMouseWithin(this);
            
            if (hasBeenActivated != spriteState)
            {
                if (hasBeenActivated)
                {
                    Path = activatedPath;
                    spriteState = true;
                }
                else
                {
                    Path = regularPath;
                    spriteState = false;
                }
            }

            base.Update(gameTime);
        }
    }
}
