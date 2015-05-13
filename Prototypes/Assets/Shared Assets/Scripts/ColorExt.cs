using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public static class ColorExt
    {
        public static Vector4 ToVector(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }


        public static void SetFromVector(this Color color, Vector4 vector)
        {
            color.r = vector.x;
            color.g = vector.y;
            color.b = vector.z;
            color.a = vector.w;
        }
    }
}
