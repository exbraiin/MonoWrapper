namespace MonoWrapper
{
    public static class Curves
    {
        public static readonly Curve Linear = new Linear();
        public static readonly Curve Ease = new Cubic(0.25f, 0.1f, 0.25f, 1.0f);

        public static readonly Curve EaseIn = new Cubic(0.42f, 0.0f, 1.0f, 1.0f);
        public static readonly Curve EaseInToLinear = new Cubic(0.67f, 0.03f, 0.65f, 0.09f);
        public static readonly Curve EaseInSine = new Cubic(0.47f, 0.0f, 0.745f, 0.715f);
        public static readonly Curve EaseInQuad = new Cubic(0.55f, 0.085f, 0.68f, 0.53f);
        public static readonly Curve EaseInCubic = new Cubic(0.55f, 0.055f, 0.675f, 0.19f);
        public static readonly Curve EaseInQuart = new Cubic(0.895f, 0.03f, 0.685f, 0.22f);
        public static readonly Curve EaseInQuint = new Cubic(0.755f, 0.05f, 0.855f, 0.06f);
        public static readonly Curve EaseInExpo = new Cubic(0.95f, 0.05f, 0.795f, 0.035f);
        public static readonly Curve EaseInCirc = new Cubic(0.6f, 0.04f, 0.98f, 0.335f);
        public static readonly Curve EaseInBack = new Cubic(0.6f, -0.28f, 0.735f, 0.045f);

        public static readonly Curve EaseOut = new Cubic(0.0f, 0.0f, 0.58f, 1.0f);
        public static readonly Curve LinearToEaseOut = new Cubic(0.35f, 0.91f, 0.33f, 0.97f);
        public static readonly Curve EaseOutSine = new Cubic(0.39f, 0.575f, 0.565f, 1.0f);
        public static readonly Curve EaseOutQuad = new Cubic(0.25f, 0.46f, 0.45f, 0.94f);
        public static readonly Curve EaseOutCubic = new Cubic(0.215f, 0.61f, 0.355f, 1.0f);
        public static readonly Curve EaseOutQuart = new Cubic(0.165f, 0.84f, 0.44f, 1.0f);
        public static readonly Curve EaseOutQuint = new Cubic(0.23f, 1.0f, 0.32f, 1.0f);
        public static readonly Curve EaseOutExpo = new Cubic(0.19f, 1.0f, 0.22f, 1.0f);
        public static readonly Curve EaseOutCirc = new Cubic(0.075f, 0.82f, 0.165f, 1.0f);
        public static readonly Curve EaseOutBack = new Cubic(0.175f, 0.885f, 0.32f, 1.275f);

        public static readonly Curve EaseInOut = new Cubic(0.42f, 0.0f, 0.58f, 1.0f);
        public static readonly Curve EaseInOutSine = new Cubic(0.445f, 0.05f, 0.55f, 0.95f);
        public static readonly Curve EaseInOutQuad = new Cubic(0.455f, 0.03f, 0.515f, 0.955f);
        public static readonly Curve EaseInOutCubic = new Cubic(0.645f, 0.045f, 0.355f, 1.0f);
        public static readonly Curve EaseInOutQuart = new Cubic(0.77f, 0.0f, 0.175f, 1.0f);
        public static readonly Curve EaseInOutQuint = new Cubic(0.86f, 0.0f, 0.07f, 1.0f);
        public static readonly Curve EaseInOutExpo = new Cubic(1.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Curve EaseInOutCirc = new Cubic(0.785f, 0.135f, 0.15f, 0.86f);
        public static readonly Curve EaseInOutBack = new Cubic(0.68f, -0.55f, 0.265f, 1.55f);

        public static readonly Curve FastOutSlowIn = new Cubic(0.4f, 0.0f, 0.2f, 1.0f);
        public static readonly Curve SlowMiddle = new Cubic(0.15f, 0.85f, 0.85f, 0.15f);

        public static readonly Curve BounceIn = new BounceIn();
        public static readonly Curve BounceOut = new BounceOut();
        public static readonly Curve BounceInOut = new BounceInOut();

        public static readonly Curve ElasticIn = new ElasticIn();
        public static readonly Curve ElasticOut = new ElasticOut();
        public static readonly Curve ElasticInOut = new ElasticInOut();

    }
}