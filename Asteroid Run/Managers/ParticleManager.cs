using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Asteroid_Run
{
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;
        public float Lifespan;
        public float MaxLifespan;
    }

    public class ParticleManager
    {
        private List<Particle> _particles = new List<Particle>();
        private Texture2D _pixelTexture;
        private Random _random = new Random();

        public ParticleManager(GraphicsDevice graphicsDevice)
        {
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
        }

        public void CreateExplosion(Vector2 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)(_random.NextDouble() * Math.PI * 2);
                float speed = _random.Next(60, 250);

                Color color = Color.Red;
                int r = _random.Next(3);
                if (r == 1) color = Color.Orange;
                else if (r == 2) color = Color.Yellow;

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed),
                    Color = color,
                    Lifespan = 0f,
                    MaxLifespan = (float)(0.3f + _random.NextDouble() * 0.4f)
                });
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                var p = _particles[i];
                p.Lifespan += deltaTime;
                p.Position += p.Velocity * deltaTime;

                p.Velocity *= 0.96f;

                if (p.Lifespan >= p.MaxLifespan)
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
            {
                float progress = p.Lifespan / p.MaxLifespan;
                float alpha = 1f - progress;

                spriteBatch.Draw(_pixelTexture, new Rectangle((int)p.Position.X, (int)p.Position.Y, 3, 3), p.Color * alpha);
            }
        }

        public void Clear() => _particles.Clear();
    }
}