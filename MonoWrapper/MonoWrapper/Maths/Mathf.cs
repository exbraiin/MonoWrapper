using System;

namespace MonoWrapper
{
    public static class Mathf
    {
        public const float PI = 3.141593f;

        public static float ToRadians(float degrees)
        {
            return degrees * 0.01745329f;
        }

        public static float ToDegrees(float radians)
        {
            return radians * 57.29578f;
        }

        public static float Sin(float f)
        {
            return (float)Math.Sin(f);
        }

        public static float Cos(float f)
        {
            return (float)Math.Cos(f);
        }

        public static float Tan(float f)
        {
            return (float)Math.Tan(f);
        }

        public static float Asin(float f)
        {
            return (float)Math.Asin(f);
        }

        public static float Acos(float f)
        {
            return (float)Math.Acos(f);
        }

        public static float Atan(float f)
        {
            return (float)Math.Atan(f);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        public static float Abs(float t)
        {
            return Math.Abs(t);
        }

        public static int Abs(int t)
        {
            return Math.Abs(t);
        }

        public static float Min(float a, float b)
        {
            if (a < b) return a;
            return b;
        }

        public static float Min(params float[] values)
        {
            if (values.Length == 0) return 0;
            float num = values[0];
            for (int i = 0; i < values.Length; ++i)
                if (values[i] < num) num = values[i];
            return num;
        }

        public static int Min(int a, int b)
        {
            if (a < b) return a;
            return b;
        }

        public static int Min(params int[] values)
        {
            if (values.Length == 0) return 0;
            int num = values[0];
            for (int i = 0; i < values.Length; ++i)
                if (values[i] < num) num = values[i];
            return num;
        }

        public static float Max(float a, float b)
        {
            if (a > b) return a;
            return b;
        }

        public static float Max(params float[] values)
        {
            if (values.Length == 0) return 0;
            float num = values[0];
            for (int i = 0; i < values.Length; ++i)
                if (values[i] > num) num = values[i];
            return num;
        }

        public static int Max(int a, int b)
        {
            if (a > b) return a;
            return b;
        }

        public static int Max(params int[] values)
        {
            if (values.Length == 0) return 0;
            int num = values[0];
            for (int i = 0; i < values.Length; ++i)
                if (values[i] > num) num = values[i];
            return num;
        }

        public static float Pow(float t, float p)
        {
            return (float)Math.Pow(t, p);
        }

        public static float Exp(float t)
        {
            return (float)Math.Exp(t);
        }

        public static float Log(float t, float p)
        {
            return (float)Math.Log(t, p);
        }

        public static float Log(float t)
        {
            return (float)Math.Log(t);
        }

        public static float Log10(float t)
        {
            return (float)Math.Log10(t);
        }

        public static float Ceil(float t)
        {
            return (float)Math.Ceiling(t);
        }

        public static float Floor(float t)
        {
            return (float)Math.Floor(t);
        }

        public static int CeilToInt(float t)
        {
            return (int)Math.Ceiling(t);
        }

        public static int FloorToInt(float t)
        {
            return (int)Math.Floor(t);
        }

        public static int RoundToInt(float t)
        {
            return (int)Math.Round(t);
        }

        public static float Sign(float t)
        {
            return t >= 0 ? 1 : -1;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp01(float value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Repeat(float t, float length)
        {
            return t - Floor(t / length) * length;
        }

        public static float PinpPong(float t, float length)
        {
            t = Repeat(t, length * 2);
            return length - Abs(t - length);
        }

    }
}
