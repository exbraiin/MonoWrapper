using System;

namespace MonoWrapper
{
    public class Cubic : Curve
    {

        private readonly float a;
        private readonly float b;
        private readonly float c;
        private readonly float d;

        public Cubic(float a, float b, float c, float d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        protected override float Internal_Evaluate(float t)
        {
            const float errorBounds = 0.001f;
            float start = 0f;
            float end = 1f;
            while (true)
            {
                float middle = (start + end) / 2f;
                float estimate = EvaluateCubic(a, c, middle);
                if (Math.Abs((t - estimate)) < errorBounds) return EvaluateCubic(b, d, middle);
                if (estimate < t) start = middle;
                else end = middle;
            }
        }

        private float EvaluateCubic(float a, float b, float m)
        {
            return 3 * a * (1 - m) * (1 - m) * m + 3 * b * (1 - m) * m * m + m * m * m;
        }
    }
}