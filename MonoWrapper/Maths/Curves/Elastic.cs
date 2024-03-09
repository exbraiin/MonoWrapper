
namespace MonoWrapper
{
    public class ElasticIn : Curve
    {

        private readonly float period;

        public ElasticIn(float period = 0.4f)
        {
            this.period = period;
        }

        protected override float Internal_Evaluate(float t)
        {
            float s = period / 4.0f;
            t = t - 1.0f;
            return -Mathf.Pow(2.0f, 10.0f * t) * Mathf.Sin((t - s) * (Mathf.PI * 2.0f) / period);
        }
    }

    public class ElasticOut : Curve
    {
        private readonly float period;

        public ElasticOut(float period = 0.4f)
        {
            this.period = period;
        }

        protected override float Internal_Evaluate(float t)
        {
            float s = period / 4.0f;
            return Mathf.Pow(2.0f, -10f * t) * Mathf.Sin((t - s) * (Mathf.PI * 2f) / period) + 1.0f;
        }
    }

    public class ElasticInOut : Curve
    {
        private readonly float period;

        public ElasticInOut(float period = 0.4f)
        {
            this.period = period;
        }

        protected override float Internal_Evaluate(float t)
        {
            float s = period / 4.0f;
            t = 2.0f * t - 1.0f;
            if (t < 0.0) return -0.5f * Mathf.Pow(2.0f, 10.0f * t) * Mathf.Sin((t - s) * (Mathf.PI * 2.0f) / period);
            else return Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t - s) * (Mathf.PI * 2.0f) / period) * 0.5f + 1.0f;
        }
    }
}