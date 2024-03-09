namespace MonoWrapper
{
    public abstract class Curve
    {

        public Curve Flipped => new FlippedCurve(this);

        public float Evaluate(float t)
        {
            if (t == 0 || t == 1) return t;
            return Internal_Evaluate(t);
        }

        protected abstract float Internal_Evaluate(float t);
    }

    public class FlippedCurve : Curve
    {

        private readonly Curve curve;

        public FlippedCurve(Curve curve)
        {
            this.curve = curve;
        }

        protected override float Internal_Evaluate(float t)
        {
            return 1 - curve.Evaluate(1 - t);
        }
    }

    public class Linear : Curve
    {
        protected override float Internal_Evaluate(float t)
        {
            return t;
        }
    }
}