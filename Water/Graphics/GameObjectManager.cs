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
    public class GameObjectManager : IDrawableThing
    {
        public List<GameObject> AllObjects { get; private set; } = new();
        public GraphicsDevice GraphicsDevice { get; }
        public TextureCache Textures { get; private set; }
        public FontCache Fonts { get; private set; }
        public InputManager Input { get; private set; }
        public AudioManager Audio { get; private set; }
        public float GameSpeed { get; set; } = 100f;

        private readonly List<GameObject> objectsToAdd = new();
        private readonly List<GameObject> objectsToRemove = new();

        public GameObjectManager(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
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

        public GameObject AddObject(GameObject obj)
        {
            obj.Game = this;
            obj.Initialize();
            objectsToAdd.Add(obj);
            return obj;
        }
        public GameObject RemoveObject(GameObject obj)
        {
            obj.Deinitialize();
            objectsToRemove.Add(obj);
            obj.Parent?.RemoveChild(obj);
            return obj;
        }
        public void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            Audio.Update(gameTime);

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
        private Rectangle scissor, previousScissor;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!WaterGame.UseExperimentalDrawingMode)
            {
                spriteBatch.Begin();
                foreach (var rootObject in RootObjects)
                {
                    rootObject.Draw(gameTime, spriteBatch, graphicsDevice);
                    rootObject.DrawChildren(gameTime, spriteBatch, graphicsDevice);
                }
                spriteBatch.End();
            }
            else
            {
                foreach (var rootObject in RootObjects)
                {
                    var objectsInOrder = ExtensionClass.FlattenWithLevel<IContainer>(rootObject, x => x.Children);

                    foreach (var y in objectsInOrder)
                    {
                        scissor = y.Item1.ActualPosition;
                        previousScissor = graphicsDevice.ScissorRectangle;
                        graphicsDevice.ScissorRectangle = scissor;

                        spriteBatch.Begin(SpriteSortMode.Deferred, rasterizerState: new() { ScissorTestEnable = true });
                        if (y.Item1 is GameObject b)
                            b.Draw(gameTime, spriteBatch, graphicsDevice);
                        spriteBatch.End();
                        graphicsDevice.ScissorRectangle = previousScissor;
                    }
                }
            }
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
