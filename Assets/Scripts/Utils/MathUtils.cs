using System;
using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static float LinearRemap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) / (fromMax - fromMin) * (toMax - toMin);
        }

        public static float Lerp(float a, float b, float r)
        {
            return a * (1 - r) + b * r;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float r)
        {
            return a * (1f - r) + b * r;
        }

        public static float ScaledTanh(float x, int k)
        {
            return (float)Math.Tanh(x / k);
        }
    }
}