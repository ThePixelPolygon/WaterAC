using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics
{
    public class TextureCache
    {
        public Dictionary<string, Texture2D> Cache { get; private set; } = new();

        private readonly GraphicsDevice graphicsDevice;
        public TextureCache(GraphicsDevice graphicsDevice) => this.graphicsDevice = graphicsDevice;

        public void AddTexture(string path) => Cache.Add(path, Texture2D.FromFile(graphicsDevice, path));

        public Texture2D Get(string path)
        {
            Texture2D texture;
            if (!Cache.TryGetValue(path, out texture))
            {
                texture = Texture2D.FromFile(graphicsDevice, path);
                Cache.Add(path, texture);
            }
            return texture;
        }
    }
}
