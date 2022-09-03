using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Water.Input;
using Water.Audio;

namespace Water.Graphics
{
    /// <summary>
    /// Manages updating and drawing for the entire game and hosts all of Water's subsystems
    /// </summary>
    public class GameObjectManager : IDrawableThing
    {
        public List<GameObject> AllObjects { get; private set; } = new();
        public WaterGame MainGame { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public TextureCache Textures { get; private set; }
        public FontCache Fonts { get; private set; }
        public InputManager Input { get; private set; }
        public AudioManager Audio { get; private set; }

        private readonly List<GameObject> objectsToAdd = new();
        private readonly List<GameObject> objectsToRemove = new();

        public GameObjectManager(GraphicsDevice graphicsDevice, WaterGame waterGame)
        {
            GraphicsDevice = graphicsDevice;
            MainGame = waterGame;
            Textures = new TextureCache(graphicsDevice);
            Fonts = new FontCache();
            Input = new InputManager(this);
            Audio = new AudioManager();
            Audio.Initialize();
        }

        /// <summary>
        /// The objects that begin rendering
        /// </summary>
        public List<GameObject> RootObjects { get; set; } = new();

        /// <summary>
        /// Adds an object to the game and initializes it
        /// </summary>
        /// <param name="obj">The object to add</param>
        /// <returns>The object that was added</returns>
        public GameObject AddObject(GameObject obj)
        {
            obj.Game = this;
            obj.Initialize();
            objectsToAdd.Add(obj);
            return obj;
        }
        /// <summary>
        /// Removes an object from the game and deinitializes it. The object will also be removed from its parent.
        /// </summary>
        /// <param name="obj">The object to remove</param>
        /// <returns></returns>
        public GameObject RemoveObject(GameObject obj)
        {
            obj.Deinitialize();
            objectsToRemove.Add(obj);
            obj.Parent?.RemoveChild(obj);
            return obj;
        }
        public void Update(GameTime gameTime)
        {
            Input.Update();
            Audio.Update();

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
            spriteBatch.Begin();
            foreach (var rootObject in RootObjects)
            {
                rootObject.Draw(gameTime, spriteBatch, graphicsDevice);
                rootObject.DrawChildren(gameTime, spriteBatch, graphicsDevice);
            }
            spriteBatch.End();
        }
 
    }

    public static class ExtensionClass
    {
        public static IEnumerable<(T, int)> FlattenWithLevel<T>(this T item, Func<T, IEnumerable<T>> getChilds)
        {
            var stack = new Stack<(T, int)>();
            stack.Push(new (item, 1));

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in getChilds(current.Item1))
                    stack.Push(new (child, current.Item2 + 1));
            }
        }
    }
}
