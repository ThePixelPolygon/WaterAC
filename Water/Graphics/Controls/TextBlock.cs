using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Controls
{
    public class TextBlock : GameObject
    {
        public TextWrapMode TextWrapping;
        public HorizontalTextAlignment HorizontalTextAlignment;
        public VerticalTextAlignment VerticalTextAlignment;
        private string text;
        public string Text
        {
            get => text.Replace("|n", "\n");
            set => text = value.Replace("\n", "|n").Replace("\r", "");
        }
        public const string LineSeparator = "|n";
        public int LineSpacing { get; set; } = 20;

        private DynamicSpriteFont font;
        public Color Color { get; set; }

        // code taken from https://github.com/redteam-os/thundershock/blob/master/src/Thundershock/Gui/Elements/TextBlock.cs

        public TextBlock(Rectangle relativePosition, DynamicSpriteFont font, string text, Color color)
        {
            RelativePosition = relativePosition;
            Color = color;
            this.font = font;
            Text = text;
            LineSpacing = (int)font.Size;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var finalText = TextWrapping switch // TODO: don't calculate this EVERY frame
            {
                TextWrapMode.LetterWrap => LetterWrap(font, text, ActualPosition.Width),
                TextWrapMode.WordWrap => WordWrap(font, text, ActualPosition.Width),
                TextWrapMode.None or _ => text
            };
            var pos = new Vector2(ActualPosition.X, ActualPosition.Y);

            var lines = finalText.Split(LineSeparator);
            int i = 0;
            foreach (var line in lines)
            {
                pos.X = ActualPosition.X;
                //pos.Y = ActualPosition.Y + (20 * i);
                var m = font.MeasureString(line.Trim());
                if (HorizontalTextAlignment != HorizontalTextAlignment.Left)
                {
                    if (HorizontalTextAlignment == HorizontalTextAlignment.Right)
                        pos.X += ActualPosition.Right - m.X;
                    else if (HorizontalTextAlignment == HorizontalTextAlignment.Center)
                        pos.X += (ActualPosition.Width - m.X) / 2;
                }
                if (VerticalTextAlignment != VerticalTextAlignment.Top)
                {
                    if (VerticalTextAlignment == VerticalTextAlignment.Bottom)
                        pos.Y = (ActualPosition.Bottom - (m.Y * ((lines.Length - i) + 1)));
                    else if (VerticalTextAlignment == VerticalTextAlignment.Center)
                        pos.Y += (ActualPosition.Height - m.Y) / 2;
                }
                spriteBatch.DrawString(font, line, pos, Color);

                pos.Y += LineSpacing;
                i++;
            }
        }

        public override void Initialize()
        {
       
        }

        public override void Deinitialize()
        {
 
        }

        public override void Update(GameTime gameTime)
        {
          
        }

        
        private string LetterWrap(DynamicSpriteFont font, string text, float wrapWidth)
        {
            if (wrapWidth <= 0)
                return text;

            var lineWidth = 0f;
            var sb = new StringBuilder();

            foreach (var ch in text)
            {
                var m = font.MeasureString(ch.ToString()).X;
                if (lineWidth + m > wrapWidth)
                {
                    sb.Append(LineSeparator);
                    lineWidth = 0;
                }

                lineWidth += m;
                sb.Append(ch);
            }

            return sb.ToString();
        }

        private string WordWrap(DynamicSpriteFont font, string text, float wrapWidth)
        {
            if (wrapWidth <= 0)
                return text;

            // first step: break words.
            var words = new List<string>();
            var w = "";
            for (var i = 0; i <= text.Length; i++)
            {
                if (i < text.Length)
                {
                    var ch = text[i];
                    w += ch;
                    if (char.IsWhiteSpace(ch))
                    {
                        words.Add(w);
                        w = "";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(w))
                    {
                        words.Add(w);
                    }
                }
            }

            // step 2: Line-wrap.
            var sb = new StringBuilder();
            var lineWidth = 0f;
            for (var i = 0; i < words.Count; i++)
            {
                var word = words[i];
                var m = font.MeasureString(word).X;
                var m2 = font.MeasureString(word.Trim()).X;

                if (lineWidth + m2 > wrapWidth && lineWidth > 0) // this makes the whole thing a lot less greedy
                {
                    sb.Append(LineSeparator);
                    lineWidth = 0;
                }

                if (m > lineWidth)
                {
                    var letterWrapped = LetterWrap(font, word, wrapWidth);
                    var lines = letterWrapped.Split(LineSeparator);
                    var last = lines.Last();

                    m = font.MeasureString(last).X;
                    word = last;

                    sb.Append(letterWrapped);
                }
                else
                {
                    sb.Append(word);
                }

                if (word.EndsWith(LineSeparator))
                    lineWidth = 0;
                else
                    lineWidth += m;
            }

            return sb.ToString();
        }
    }

    public enum TextWrapMode
    {
        None,
        LetterWrap,
        WordWrap
    }
    public enum HorizontalTextAlignment
    {
        Left,
        Center,
        Right
    }
    public enum VerticalTextAlignment
    {
        Top,
        Center,
        Bottom
    }
}
