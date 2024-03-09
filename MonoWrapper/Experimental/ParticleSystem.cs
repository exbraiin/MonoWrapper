using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoWrapper.Experimental
{
    public abstract class IRange<T>
    {
        protected enum Type { Value, Random, Range }

        protected Type type;
        protected T min, max;

        public IRange(T min, T max)
        {
            Range(min, max);
        }

        /// <summary>
        ///     When using this range only this value will be chosen.
        /// </summary>
        /// <param name="value"></param>
        public void Value(T value)
        {
            min = value;
            max = value;
            type = Type.Value;
        }

        /// <summary>
        ///     When using this range the chosen value will be either min or max
        /// </summary>
        /// <param name="min">The first value</param>
        /// <param name="max">The second value</param>
        public void Random(T min, T max)
        {
            this.min = min;
            this.max = max;
            type = Type.Random;
        }

        /// <summary>
        ///     When using this range the value will be interpolated between the min and max values.
        /// </summary>
        /// <param name="min">The minimum/start value</param>
        /// <param name="max">The maximum/end value</param>
        public void Range(T min, T max)
        {
            this.min = min;
            this.max = max;
            type = Type.Range;
        }

        /// <summary>
        ///     Evaluates this range by the given t.
        /// </summary>
        /// <param name="t">The value to interpolate the range</param>
        /// <returns>The chosen value using t</returns>
        public T Evaluate(float t)
        {
            if (type == Type.Random) return t < 1f / 2 ? min : max;
            if (type == Type.Range) return IEvaluate(t);
            return min;
        }

        protected abstract T IEvaluate(float t);
    }

    public class FloatRange : IRange<float>
    {
        public FloatRange(float min, float max) : base(min, max) { }
        protected override float IEvaluate(float t) => min + (max - min) * t;
    }

    public class Vector2Range : IRange<Vector2>
    {
        public Vector2Range(Vector2 min, Vector2 max) : base(min, max) { }
        protected override Vector2 IEvaluate(float t) => Vector2.Lerp(min, max, t);
    }

    public class ColorRange : IRange<Color>
    {
        public ColorRange(Color min, Color max) : base(min, max) { }
        protected override Color IEvaluate(float t) => Color.Lerp(min, max, t);
    }

    public class ParticleSystem
    {
        public struct Particle
        {
            public bool Enable => (TimeAlive / LifeTime) < 1;
            public float Progress => (TimeAlive / LifeTime);

            // Static Configs...
            public float LifeTime;
            public Vector2 Offset;
            public Vector2 Direction;

            // Dynamic Configs...
            public float TimeAlive;
            public Vector2 Position;

            // Start Configs...
            public float StartVelocity;
            public float StartRotation;
            public Color StartColor;
            public Vector2 StartSize;
        }

        // Geral
        public bool Debug { get; set; }
        private readonly Random random = new Random();
        private readonly List<Particle> particles = new List<Particle>();

        // === EMITTER CONFIGS ===
        private int rate = 25;
        public int Rate
        {
            get => rate;
            set => rate = Mathf.Max(0, value);
        }
        public int Limit = 100;
        public float Angle = -90;
        private float spread = 180;
        public float Spread
        {
            get => spread;
            set => spread = Mathf.Clamp(value, 0, 180);
        }
        public float Interval = 1;
        public bool Emit = true;
        public bool Burst = false;
        public bool Local = false;
        public Vector2 Location = Vector2.Zero;
        public Rectangle Bounds = Rectangle.Empty;
        // =======================

        // === PARTICLE CONFIG ===
        public readonly FloatRange startLifeTime = new FloatRange(0.4f, 2);

        public readonly ColorRange startColor = new ColorRange(Color.White, Color.White);
        public readonly ColorRange overLifeTimeColor = new ColorRange(Color.White, new Color(255, 255, 255, 0));

        public readonly Vector2Range startSize = new Vector2Range(Vector2.One, Vector2.One);
        public readonly Vector2Range overLifeTimeSize = new Vector2Range(Vector2.One, Vector2.One);

        public readonly FloatRange startVelocity = new FloatRange(100, 200);
        public readonly FloatRange overLifeTimeVelocity = new FloatRange(1, 1);

        public readonly FloatRange startRotation = new FloatRange(0, 0);
        public readonly FloatRange overLifeTimeRotation = new FloatRange(0, 0);
        // =======================

        // === DEBUG =============
        public float Runtime { get; private set; }
        public int ParticlesCount => particles.Count(p => p.Enable);
        // =======================

        // Particle Config
        private readonly bool loop;
        private readonly float duration;
        private readonly Texture2D texture;
        private float nInterval = 0;

        public ParticleSystem(Texture2D texture, float duration = -1, bool loop = true)
        {
            this.loop = loop;
            this.texture = texture;
            this.duration = duration;
        }

        private Vector2 AngleToVector2(float angle)
        {
            float radians = Mathf.ToRadians(angle);
            var vector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            vector.Normalize();
            return vector;
        }

        private void AddNewParticle()
        {
            var posX = random.Next(Bounds.X, Bounds.X + Bounds.Width);
            var posY = random.Next(Bounds.Y, Bounds.Y + Bounds.Height);
            var rot = Angle + (random.NextFloat() - 0.5f) * Spread * 2;
            var direction = AngleToVector2(rot);

            particles.Add(
                new Particle()
                {
                    // Static Configs...
                    LifeTime = startLifeTime.Evaluate(random.NextFloat()),
                    Offset = Location,
                    Direction = direction,

                    // Dynamic Configs...
                    TimeAlive = 0,
                    Position = new Vector2(posX, posY),

                    // OverLifeTime Configs...
                    StartRotation = startRotation.Evaluate(random.NextFloat()),
                    StartVelocity = startVelocity.Evaluate(random.NextFloat()),
                    StartColor = startColor.Evaluate(random.NextFloat()),
                    StartSize = startSize.Evaluate(random.NextFloat()),
                }
            );
        }

        public void Update()
        {
            UpdateEmitter();
            UpdateParticles();
        }

        private void UpdateEmitter()
        {
            if (!Emit) return;
            if (Rate < 1) return;

            Runtime += Time.DeltaTime;
            if (!loop && duration >= 0 && Runtime > duration) return;

            if (Burst)
            {
                nInterval -= Time.DeltaTime;
                if (nInterval <= 0)
                {
                    nInterval = Interval;
                    particles.RemoveAll(p => !p.Enable);
                    for (int i = 0; i < Rate; ++i)
                    {
                        if (particles.Count >= Limit) break;
                        AddNewParticle();
                    }
                }
            }
            else
            {
                nInterval -= Time.DeltaTime;
                if (nInterval <= 0)
                {
                    nInterval = Interval / Rate;
                    particles.RemoveAll(p => !p.Enable);
                    var total = Mathf.CeilToInt(Time.DeltaTime / nInterval);
                    for (int i = 0; i < total; ++i)
                    {
                        if (particles.Count >= Limit) break;
                        AddNewParticle();
                    }
                }
            }
        }

        private void UpdateParticles()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                var particle = particles[i];
                if (!particle.Enable) continue;

                particle.TimeAlive += Time.DeltaTime;

                var velocity = particle.StartVelocity * overLifeTimeVelocity.Evaluate(particle.Progress);
                particle.Position += particle.Direction * velocity * Time.DeltaTime;

                particles[i] = particle;
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            if (Debug)
            {
                var rect = new Rectangle(Location.ToPoint() + Bounds.Location, Bounds.Size);
                Gizmos.DrawRectangle(batch, rect, Color.Red);

                var px = Location + AngleToVector2(Angle) * 50;
                Gizmos.DrawLine(batch, Location, px, Color.Blue);

                var px0 = Location + AngleToVector2(Angle - Spread) * 25;
                Gizmos.DrawLine(batch, Location, px0, Color.Blue);

                var px1 = Location + AngleToVector2(Angle + Spread) * 25;
                Gizmos.DrawLine(batch, Location, px1, Color.Blue);

                var org = Location;
                var vec = new Vector2(5, 5);
                Gizmos.DrawRectangle(batch, new Rectangle((org - vec / 2f).ToPoint(), vec.ToPoint()), Color.Red, 0);

                var seed = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
                var rect0 = particles.Where(p => p.Enable).Aggregate(seed, (acc, ptc) =>
                {
                    var loc = Local ? Location : ptc.Offset;
                    var pos = (loc + ptc.Position).ToPoint();
                    int minX = Mathf.Min(acc.X, pos.X);
                    int minY = Mathf.Min(acc.Y, pos.Y);
                    int maxX = Mathf.Max(acc.Width, pos.X);
                    int maxY = Mathf.Max(acc.Height, pos.Y);
                    return new Rectangle(minX, minY, maxX, maxY);
                });
                rect0.Size -= rect0.Location;
                Gizmos.DrawRectangle(batch, rect0, Color.Yellow.WithA(byte.MaxValue / 3));
            }

            for (int i = 0; i < particles.Count; ++i)
            {
                var particle = particles[i];
                if (!particle.Enable) continue;

                var loc = Local ? Location : particle.Offset;
                var pos = loc + particle.Position;

                var vectorColor0 = particle.StartColor.ToVector4();
                var vectorColor1 = overLifeTimeColor.Evaluate(particle.Progress).ToVector4();
                var color = new Color(vectorColor0 * vectorColor1);
                var center = texture.Bounds.Size.ToVector2() / 2f;
                var scale = particle.StartSize * overLifeTimeSize.Evaluate(particle.Progress);
                var rotation = particle.StartRotation + overLifeTimeRotation.Evaluate(particle.Progress);
                batch.Draw(texture, pos, null, color, rotation, center, scale, SpriteEffects.None, 0);
            }
        }
    }

    public static class ParticleSystemExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, ParticleSystem particleSystem)
        {
            particleSystem.Draw(spriteBatch);
        }
    }
}
