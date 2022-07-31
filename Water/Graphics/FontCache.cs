using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics
{
    public class FontCache
    {
        public Dictionary<string, DynamicSpriteFont> Cache { get; private set; } = new();

        public void Add(string path, int fontSize) => Cache.Add(path, DynamicSpriteFont.FromTtf(File.ReadAllBytes(path), fontSize));

        public DynamicSpriteFont Get(string path, int fontSize)
        {
            DynamicSpriteFont font;
            if (!Cache.TryGetValue(path, out font))
            {
                Add(path, fontSize);
                font = Get(path, fontSize);
            }
            return font;
        }
    }
}
