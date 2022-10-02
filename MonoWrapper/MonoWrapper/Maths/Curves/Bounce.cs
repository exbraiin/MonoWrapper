namespace MonoWrapper
{
    public abstract class Bounce : Curve
    {
        protected float EvaluateBounce(float t)
        {
            if (t < 1f / 2.75f) return 7.5625f * t * t;
            if (t < 2f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }
            if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }
            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }
    }

    public class BounceIn : Bounce
    {
        protected override float Internal_Evaluate(float t)
        {
            return 1 - EvaluateBounce(1 - t);
        }
    }

    public class BounceOut : Bounce
    {
        protected override float Internal_Evaluate(float t)
        {
            return EvaluateBounce(t);
        }
    }

    public class BounceInOut : Bounce
    {
        protected override float Internal_Evaluate(float t)
        {

            if (t < 0.5f) return (1f - EvaluateBounce(1 - t * 2f)) * 0.5f;
            else return EvaluateBounce(t * 2f - 1f) * 0.5f + 0.5f;
        }
    }
}