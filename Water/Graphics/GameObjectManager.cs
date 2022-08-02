using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Water.Input;

namespace Water.Graphics
{
    public class GameObjectManager : IDrawableThing
    {
        public List<GameObject> AllObjects { get; private set; } = new();
        public GraphicsDevice GraphicsDevice { get; }
        public TextureCache Textures { get; private set; }
        public FontCache Fonts { get; private set; }
        public InputManager Input { get; private set; }
        public float GameSpeed { get; set; } = 100f;

        private readonly List<GameObject> objectsToAdd = new();
        private readonly List<GameObject> objectsToRemove = new();

        public GameObjectManager(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Textures = new TextureCache(graphicsDevice);
            Fonts = new FontCache();
            Input = new InputManager(this);
        }

        /// <summary>
        /// The objects that begin rendering
        /// </summary>
        public List<GameObject> RootObjects { get; set; } = new();

        public GameObject AddObject(GameObject obj)
        {
            obj.Game = this;
            obj.Initialize();
            objectsToAdd.Add(obj);
            return obj;
        }
        public GameObject RemoveObject(GameObject obj)
        {
            objectsToRemove.Add(obj);
            obj.Parent.RemoveChild(obj);
            return obj;
        }
        public void Update(GameTime gameTime)
        {
            Input.Update(gameTime);

            foreach (var obj in AllObjects)
                obj.Update(gameTime);

            AllObjects.AddRange(objectsToAdd);
            objectsToAdd.Clear();
            foreach (var obj in objectsToRemove)
            {
                AllObjects.Remove(obj);
            }
            objectsToRemove.Clear();
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            foreach (var rootObject in RootObjects)
            {
                rootObject.Draw(gameTime, spriteBatch, graphicsDevice);
                rootObject.DrawChildren(gameTime, spriteBatch, graphicsDevice);
            }
        }
    }
}
